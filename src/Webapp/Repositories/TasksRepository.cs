using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Webapp.Dtos;
using Webapp.Helpers;
using Webapp.Interfaces;
using Webapp.Models;
using Webapp.ViewModels;

namespace Webapp.Repositories
{
    public class TasksRepository : ITasks
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TasksRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public void AddTasks(TasksViewModel task)
        {
            var data = _mapper.Map<Tasks>(task);
            data.TaskStatusId = 1;
            data.DateAssigned = DateTime.Now;
            data.Id = Guid.NewGuid();
            _context.Tasks.Add(data);
        }

        public async Task<Result<List<TasksViewModel>>> GetAllTasksAsync(string userId)
        {
            var user = await _context.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Id == userId);
            var tasks = _context.Tasks.AsQueryable();
            if(user != null)
            {
                var role = user.Roles.FirstOrDefault().RoleId;
                var userRole = await _context.Roles.SingleOrDefaultAsync(x => x.Id == role);
                switch (userRole.Name)
                {
                    case "Employee":
                        tasks = tasks.Where(x => x.AssignedToUserId == userId);
                        break;
                    case "Manager":
                        tasks = tasks.Where(x => x.AssigningUserId == userId);
                        break;
                    default:
                        break;                    
                }
                var data = await tasks.ToListAsync();
                var results = _mapper.Map<List<TasksViewModel>>(data);
                return Result<List<TasksViewModel>>.Success(results);
            }

            return Result<List<TasksViewModel>>.Failure("User not found");
        }

        public async Task<Result<TasksViewModel>> GetTaskAsync(Guid taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if(task != null)
            {
                var result = _mapper.Map<TasksViewModel>(task);
                return Result<TasksViewModel>.Success(result);
            }
            return Result<TasksViewModel>.Failure("Task not found");
        }

        public async Task<Result<Tasks>> GetTaskEntityAsync(Guid taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
            {
                return Result<Tasks>.Failure("Cannot find task");
            }
            return Result<Tasks>.Success(task);
        }

        public async Task<Result<List<TaskIndexDto>>> GetTaskIndexAsync(string userId)
        {
            var user = await _context.Users.Include(x => x.Roles).SingleOrDefaultAsync(x => x.Id == userId);
            var tasks = _context.Tasks.AsQueryable();
            if (user != null)
            {
                var role = user.Roles.FirstOrDefault().RoleId;
                var userRole = await _context.Roles.SingleOrDefaultAsync(x => x.Id == role);
                switch (userRole.Name)
                {
                    case "Employee":
                        tasks = tasks.Where(x => x.AssignedToUserId == userId);
                        break;
                    case "Manager":
                        tasks = tasks.Where(x => x.AssigningUserId == userId);
                        break;
                    default:
                        break;
                }
                var data = await tasks.Include(x => x.AssignedToUser).Select(x => new TaskIndexDto
                {
                    Id = x.Id,
                    Subject = x.Subject,
                    AssignedTo = x.AssignedToUser.FirstName + " " + x.AssignedToUser.LastName,
                    Status = x.TaskStatusId,
                    Priority = x.PriorityId,
                    CompletedYN = x.DateCompleted != null,
                    DateDue = x.DateDue,
                }).ToListAsync();
                return Result<List<TaskIndexDto>>.Success(data);
            }
            return Result<List<TaskIndexDto>>.Failure("Operation failed");
        }
        public async Task<Result<List<AutocompleteDto>>> GetUsers(string FirstName)
        {
            var role = _context.Roles.SingleOrDefault(m => m.Name == "Manager");

            var users = await _context.Users
                .Include(x => x.Roles)
                .Where(x => x.Roles.All(o => o.RoleId != role.Id))
                .Where(x => x.FirstName.Contains(FirstName)).Select(x => new AutocompleteDto
            {
                UserId = x.Id,
                FullName = x.FirstName + " " + x.LastName,
            }).ToListAsync();
            return Result<List<AutocompleteDto>>.Success(users);
        }

        public void Populatedropdown(TasksViewModel data)
        {

            data.Status = _context.TasksStatuses.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = x.Id.ToString()
            }).ToList();
            data.Priority = _context.Priorities.Select(x => new SelectListItem
            {
                Text = x.Description,
                Value = x.Id.ToString()
            }).ToList();
            if(data.AssignedToUserId != null)
            {
                var user = _context.Users.Find(data.AssignedToUserId);
                data.AutoCompletebox = user.FirstName + " " + user.LastName;
            }
            //return Result<TasksViewModel>.Success(data);
        }

        public void RemoveTasks(Tasks tasks)
        {
            _context.Tasks.Remove(tasks);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateTasks(Tasks task)
        {
            _context.Entry(task).State = EntityState.Modified;
        }
    }
}