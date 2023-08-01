using Domin.Entity;
using Domin.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Infrastructure
{
    public class ElectroBookDbContext : IdentityDbContext<AppUserModel>
    {
        public ElectroBookDbContext(DbContextOptions<ElectroBookDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<IdentityUser>().ToTable("Users", "Identity");
            //modelBuilder.Entity<IdentityRole>().ToTable("Roles", "Identity");
            //modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "Identity");
            //modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "Identity");
            //modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "Identity");
            //modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim", "Identity");
            //modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserToken", "Identity");
            modelBuilder.Entity<Category>().Property(n => n.Id).HasDefaultValueSql("(newid())");

            modelBuilder.Entity<Book>().Property(n => n.Id).HasDefaultValueSql("(newid())");
            modelBuilder.Entity<LogCategory>().Property(n => n.Id).HasDefaultValueSql("(newid())");

            modelBuilder.Entity<LogBook>().Property(n => n.Id).HasDefaultValueSql("(newid())");
            modelBuilder.Entity<UsersView>().HasNoKey().ToView("UsersView");
        //    modelBuilder.Entity<NewCategory>().Property(i => i.Id).HasDefaultValue("1");
            modelBuilder.Entity<LogCategory>()
              .HasOne(lc => lc.Category)
              .WithMany()
              .HasForeignKey(lc => lc.CategoryId)
              .OnDelete(DeleteBehavior.Restrict);


        }
   
        public DbSet<Category> Categorys { get; set; }
        public DbSet<LogCategory> LogCategorys { get; set; }
    
          public DbSet<UsersView> UsersView { get; set; }
        public static implicit operator UserManager<object>(ElectroBookDbContext v)
        {
            throw new NotImplementedException();
        }
        
    }

}
