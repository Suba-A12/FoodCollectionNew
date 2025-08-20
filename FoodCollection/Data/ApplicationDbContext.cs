using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FoodCollection.Models;

namespace FoodCollection.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FoodCollection.Models.BookPickupDetail> BookPickupDetail { get; set; } = default!;
        public DbSet<FoodCollection.Models.Customer> Customer { get; set; } = default!;
        public DbSet<FoodCollection.Models.Staff> Staff { get; set; } = default!;
        public DbSet<FoodCollection.Models.BookPickup> BookPickup { get; set; } = default!;
        public DbSet<FoodCollection.Models.Appointment> Appointment { get; set; } = default!;
        public DbSet<FoodCollection.Models.FoodItem> FoodItem { get; set; } = default!;
        public DbSet<FoodCollection.Models.Payment> Payment { get; set; } = default!;
        public DbSet<FoodCollection.Models.Pickup> Pickup { get; set; } = default!;
        public DbSet<FoodCollection.Models.Organization> Organization { get; set; } = default!;
        public DbSet<FoodCollection.Models.DeliveryInfo> DeliveryInfo { get; set; } = default!;
        public DbSet<FoodCollection.Models.Report> Report { get; set; } = default!;
    }
}
