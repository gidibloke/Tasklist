using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Unity;
using Webapp.Dtos;
using Webapp.Interfaces;
using Webapp.ViewModels;

namespace Webapp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ITasks _tasks;

        [InjectionConstructor]
        public DashboardController(ITasks tasks)
        {
            _tasks = tasks;
        }
        // GET: Dashboard
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var tasks = await _tasks.GetAllTasksAsync(userId);
            var result = new DashboardDto();
            if(tasks.Data.Count > 0)
            {
                result.AssociatedTasks = tasks.Data.Count();
                result.CompletedTasks = tasks.Data.Where(x => x.TaskStatusId == 3).Count();
                result.InprogressTasks = tasks.Data.Where(x => x.TaskStatusId == 2).Count();
                result.NotStarted = tasks.Data.Where(x => x.TaskStatusId == 1).Count();
            }
            ViewBag.ManagerYN = User.IsInRole("Manager");
            return View(result);
        }

        public async Task<ActionResult> LoadTasks()
        {
            var userId = User.Identity.GetUserId();
            var tasks = await _tasks.GetTaskIndexAsync(userId);
            
            return Json(new { data = tasks.Data }, JsonRequestBehavior.AllowGet);
        }

      
        public async Task<ActionResult> AutoComplete(string term)
        {
            var result = await _tasks.GetUsers(term);
            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            var userId = User.Identity.GetUserId();
            var model = new TasksViewModel
            {
                AssigningUserId = userId,
            };
            _tasks.Populatedropdown(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TasksViewModel tasks)
        {
            if (!ModelState.IsValid)
            {
                _tasks.Populatedropdown(tasks);
                return View(tasks);
            }
            _tasks.AddTasks(tasks);
            var sucess = await _tasks.SaveChangesAsync();
            if (sucess)
            {
                TempData["success"] = "Task successfully assigned";
                return RedirectToAction(nameof(Index));
            }
            TempData["failure"] = "Problem adding tasks. Please try again"; ;

            _tasks.Populatedropdown(tasks);
            return View(tasks);

        }

        [Authorize(Roles ="Manager")]
        public async Task<ActionResult> Edit(string id)
        {
            var guid = Guid.TryParse(id, out var tasksId);
            if (!guid)
            {
                TempData["failure"] = "Invalid link";
                return RedirectToAction(nameof(Index));
            }
            var model = await _tasks.GetTaskAsync(tasksId);
            if (!model.IsSuccess)
            {
                TempData["failure"] = model.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            if (model.Data.AssigningUserId != User.Identity.GetUserId())
            {
                TempData["failure"] = "You do not have access to edit this task";
                return RedirectToAction(nameof(Index));

            }
            _tasks.Populatedropdown(model.Data);
            return View(model.Data);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TasksViewModel model)
        {
            if (!ModelState.IsValid)
            {
                _tasks.Populatedropdown(model);
                return View(model);
            }
            var result = await _tasks.GetTaskEntityAsync(model.Id);
            result.Data.AssignedToUserId = model.AssignedToUserId ?? result.Data.AssignedToUserId;
            result.Data.Subject = model.Subject ?? result.Data.Subject;
            result.Data.Description = model.Description ?? result.Data.Description;
            result.Data.PriorityId = model.PriorityId;
            result.Data.DateDue = model.DateDue;
            _tasks.UpdateTasks(result.Data);
            var response = await _tasks.SaveChangesAsync();
            if (response)
            {
                TempData["success"] = "Task successfully updated";
                return RedirectToAction(nameof(Index));
            }
            TempData["failure"] = "Operation failed. please try again later";
            _tasks.Populatedropdown(model);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="Manager")]
        public async Task<ActionResult> Delete(string id)
        {
            var guid = Guid.TryParse(id, out var tasksId);
            if (!guid)
            {
                return Json(new {success = false, message = "Invalid link", JsonRequestBehavior.AllowGet });
            }
            var model = await _tasks.GetTaskEntityAsync(tasksId);
            if (!model.IsSuccess)
            {
                return Json(new { success = false, message = model.ErrorMessage, JsonRequestBehavior.AllowGet });

            }
            if (model.Data.AssigningUserId != User.Identity.GetUserId())
            {
                return Json(new { success = false, message = "Only assigned user can delete tasks", JsonRequestBehavior.AllowGet });
            }
            _tasks.RemoveTasks(model.Data);
            var response = await _tasks.SaveChangesAsync();
            if (response)
            {
                return Json(new { success = true, message = "Task successfully deleted", JsonRequestBehavior.AllowGet });

            }
            return Json(new { success = false, message = "Invalid request, please try again later", JsonRequestBehavior.AllowGet });

        }

        [HttpPost]
        public async Task<ActionResult> MarkCompleted(MarkCompleted completed)
        {
            var task = await _tasks.GetTaskEntityAsync(completed.TaskId);
            if (!task.IsSuccess)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            var model = task.Data;
            model.DateCompleted = completed.CompletedDate;
            model.TaskStatusId = 3;
            _tasks.UpdateTasks(model);
            var success = await _tasks.SaveChangesAsync();
            return Json(new {success = success}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> ChangeToInprogress(string id)
        {
            var guid = Guid.TryParse(id, out var tasksId);
            if (!guid)
            {
                return Json(new { success = false, message="Invalid link" }, JsonRequestBehavior.AllowGet);
            }
            var task = await _tasks.GetTaskEntityAsync(tasksId);
            if (!task.IsSuccess)
            {
                return Json(new { success = false, message = "Invalid link" }, JsonRequestBehavior.AllowGet);
            }
            var result = task.Data;
            result.TaskStatusId = 2;
            _tasks.UpdateTasks(result);
            var success = await _tasks.SaveChangesAsync();
            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> Details(string id)
        {
            var guid = Guid.TryParse(id, out var tasksId);
            if (!guid)
            {
                TempData["failure"] = "Invalid link";
                return RedirectToAction(nameof(Index));
            }
            var model = await _tasks.GetTaskAsync(tasksId);
            if (!model.IsSuccess)
            {
                TempData["failure"] = model.ErrorMessage;
                return RedirectToAction(nameof(Index));
            }
            _tasks.Populatedropdown(model.Data);
            return View(model.Data);
        }
    }
}

