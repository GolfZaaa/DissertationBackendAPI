using BackendAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
            builder.Entity<Location>(options =>
            {
                options.HasMany(a => a.locationImages).GetInfrastructure().OnDelete(DeleteBehavior.Cascade);

                options.HasOne(a => a.Category);

            });

            //builder.Entity<ReservationsOrder>(options =>
            //{
            //    //options.HasOne(a => a.Users).GetInfrastructure().OnDelete(DeleteBehavior.Cascade);


            //    //options.HasOne(a => a.Locations).GetInfrastructure().OnDelete(DeleteBehavior.Cascade);
            //});

            //builder.Entity<Location>().HasOne(a=>a.Category).WithMany(a=>a.Locations).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Name = "Student", NormalizedName = "Student", ConcurrencyStamp = "นักศึกษา"},
                new IdentityRole { Name = "Professor", NormalizedName = "Professor", ConcurrencyStamp = "อาจารย์"},
                new IdentityRole { Name = "Outsider", NormalizedName = "Outsider", ConcurrencyStamp = "บุคคลภายนอก"},
                new IdentityRole { Name = "Approver", NormalizedName = "Approver", ConcurrencyStamp = "ผู้อนุมัติ"},
                new IdentityRole { Name = "Administrator", NormalizedName = "Administrator", ConcurrencyStamp = "ผู้ดูแลระบบ"}
            );

            builder.Entity<Cart>(options =>
            {
                options.HasOne(a=>a.User).GetInfrastructure().OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<CartItem>(options =>
            {
                options.HasOne(a => a.Locations).GetInfrastructure().OnDelete(DeleteBehavior.Cascade);

            });
        }

        // ตารางเช็ค Login ผิดเกิน 3 ครั้ง
        public DbSet<LoginAttempt> LoginAttempts { get; set; }
        public DbSet<CategoryLocations> CategoryLocations { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationImages> LocationImages { get; set; }
        public DbSet<ReservationsOrder> ReservationsOrders { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<ReservationsOrderItem> ReservationsOrderItems { get; set; }
        public DbSet<Agency> Agencys { get; set; }
        public DbSet<WalkInTransaction> WalkInTransactions { get; set; }
        public DbSet<WalkInMembership> WalkInMemberships { get; set; }
        public DbSet<MembershipPrice> MembershipPrices { get; set; }


    }

}
