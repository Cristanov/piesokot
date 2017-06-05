using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using NaSpacerDo.Domain.Models;
using System.Data.Entity;
using System.Linq;
using System;
using NaSpacerDo.Domain.Enums;

namespace NaSpacerDo.Domain
{
    internal class CustomInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            base.Seed(context);

            SeedRoles(context);
            SeedUsers(context);
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            string[] roles = Enum.GetNames(typeof(Roles));
            foreach (var roleName in Enum.GetNames(typeof(Roles)))
            {
                IdentityRole role = new IdentityRole(roleName);
                context.Roles.Add(role);
            }

            context.SaveChanges();
        }

        private static void SeedUsers(ApplicationDbContext context)
        {
            if (!context.Users.Any(u => u.UserName == "admin@admin.pl"))
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser { UserName = "admin@admin.pl" };

                manager.Create(user, "1qaz!QAZ");
                manager.AddToRole(user.Id, "Admin");
            }

            context.SaveChanges();
        }
    }
}