using BackendAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BackendAPI.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=DESKTOP-DTGB06O\\SQLEXPRESS; Database=Dissertation; Trusted_connection=true; TrustServerCertificate=true");

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Name = "Student", NormalizedName = "student" },
                new IdentityRole { Name = "Professor", NormalizedName = "Professor" },
                new IdentityRole { Name = "Outsider", NormalizedName = "Outsider" },
                new IdentityRole { Name = "Approver", NormalizedName = "Approver" },
                new IdentityRole { Name = "Administrator", NormalizedName = "Administrator" }
            );

        }

        // ตารางเช็ค Login ผิดเกิน 3 ครั้ง
        //public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<CategoryRoom> CategoryRooms { get; set; }
        public DbSet<Location> Locations { get; set; }


    }

}
