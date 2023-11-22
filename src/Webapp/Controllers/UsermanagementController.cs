using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using System.Security.Policy;
using System.Threading.Tasks;
using Webapp.Dtos;
using Webapp.Models;
using System.Data.Entity;

namespace Webapp.Controllers
{
    [Authorize(Roles = "Manager")]
    public class UsermanagementController : Controller
    {
        //private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext _context;

        public UsermanagementController()
        {
            
        }
        //public UsermanagementController(ApplicationUserManager userManager)
        //{
        //    UserManager = userManager;
        //}

        public UsermanagementController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        _userManager = value;
        //    }
        //}
        // GET: Usermanagement
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult LoadUsers()
        {
 
            var data = (from user in _context.Users
                                  select new
                                  {
                                      UserId = user.Id,
                                      user.Email,
                                      user.FirstName, user.LastName,user.IsEnabled,user.JobTitle,user.EmailConfirmed,
                                      RoleNames = (from userRole in user.Roles
                                                   join role in _context.Roles on userRole.RoleId
                                                   equals role.Id
                                                   select role.Name).ToList()
                                  }).ToList().Select(p => new UsersDto()
                                  {
                                      UserId = p.UserId,
                                      Email = p.Email,
                                      IsEnabled = p.IsEnabled,
                                      FullName = p.FirstName + " " + p.LastName,
                                      RoleName = string.Join(",", p.RoleNames),
                                      JobTitle = p.JobTitle,
                                      EmailConfirmed = p.EmailConfirmed
                                  });
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> DisableUser(string Id)
        {
            if (Id == null)
            {
                return Json(new { success = false, message = "Invalid link" }, JsonRequestBehavior.AllowGet);
            }
            var user = await _context.Users.Where(x => x.Id == Id).SingleOrDefaultAsync();
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" }, JsonRequestBehavior.AllowGet);
            }
            if(user.Id == User.Identity.GetUserId())
            {
                return Json(new { success = false, message = "You cannot disable you own account" }, JsonRequestBehavior.AllowGet);

            }
            user.IsEnabled = false;
            _context.Entry(user).State = EntityState.Modified;
            var success = await _context.SaveChangesAsync() > 0;
            if(success)
                return Json(new {success = true, message = "Operation successful"}, JsonRequestBehavior.AllowGet);
            else
                return Json(new { success = false, message = "Operation failed" }, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> Edit(string userId)
        {
            //Not implemented on purpose. Focusing on the core features of the applcation
            return View();
        }
    }
}