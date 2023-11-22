namespace Webapp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Webapp.LookupModels;
    using Webapp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Webapp.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Webapp.Models.ApplicationDbContext context)
        {
            context.Roles.AddOrUpdate(x => x.Id, new IdentityRole { Name = "Manager" }, new IdentityRole { Name = "Employee" });
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            if (!context.Users.Any())
            {
                var user1 = new ApplicationUser
                {
                    FirstName = "Satoshi",
                    LastName = "Nakamoto",
                    JobTitle = "Blockchain developer",
                    IsEnabled = true,
                    Email = "satoshi@bitcoin.com",
                    UserName = "satoshi@bitcoin.com"
                };
                manager.Create(user1, "Password1");
                manager.AddToRole(user1.Id, "Manager");

                var user2 = new ApplicationUser
                {
                    FirstName = "Captain",
                    LastName = "America",
                    JobTitle = "Partime avenger",
                    IsEnabled = true,
                    Email = "steverogers@avengers.com",
                    UserName = "steverogers@avengers.com"
                };
                manager.Create(user2, "Password1");
                manager.AddToRole(user2.Id, "Manager");

                var user3 = new ApplicationUser
                {
                    FirstName = "Albus",
                    LastName = "Dumbuldore",
                    JobTitle = "Order founder",
                    IsEnabled = true,
                    Email = "albus@hogworts.com",
                    UserName = "albus@hogworts.com"
                };
                manager.Create(user3, "Password1");
                manager.AddToRole(user3.Id, "Employee");


                var user4 = new ApplicationUser
                {
                    FirstName = "Sheldon",
                    LastName = "Copper",
                    JobTitle = "Theoretical phycist",
                    IsEnabled = true,
                    Email = "sheldon@mit.com",
                    UserName = "sheldon@mit.com"
                };
                manager.Create(user3, "Password1");
                manager.AddToRole(user3.Id, "Employee");

            };
            


            if (!context.TasksStatuses.Any())
            {
                var status = new List<TasksStatus>
                {
                    new TasksStatus
                    {
                        Id = 1,
                        Description = "Not started"
                    },
                    new TasksStatus
                    {
                        Id = 2,
                        Description = "In progress"
                    },
                    new TasksStatus
                    {
                        Id = 2,
                        Description = "Completed"
                    },
                };            
                context.TasksStatuses.AddRange(status);
            }



            if (!context.Priorities.Any()){
                var priority = new List<Priority>
                {
                    new Priority
                    {
                        Id = 1,
                        Description = "Urgent"
                    },
                    new Priority
                    {
                        Id = 2,
                        Description = "Important"
                    },
                    new Priority
                    {
                        Id = 2,
                        Description = "Medium"
                    },
                    new Priority
                    {
                        Id = 3,
                        Description = "Low"
                    },
                };
                context.Priorities.AddRange(priority);
            }


            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
