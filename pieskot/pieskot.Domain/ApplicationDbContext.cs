using Microsoft.AspNet.Identity.EntityFramework;
using NaSpacerDo.Domain.Models;
using System.Data.Entity;

namespace NaSpacerDo.Domain
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("LocalDbConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new CustomInitializer());
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Address> Adresses { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasRequired(x => x.Owner).WithMany(x => x.Companies).HasForeignKey(x => x.OwnerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
