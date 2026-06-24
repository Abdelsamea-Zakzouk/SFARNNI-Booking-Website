using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myShop.Entities.Models;
using myShop.Entities.ViewModels;

namespace myShop.DataAccess.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categorys { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<FlightProduct> FlightProducts { get; set; }
        public DbSet<ApplicationUsers> ApplicationUsers { get; set; }
         public DbSet<HostHotel> HostHotels { get; set; }
         public DbSet<HostFlight> hostFlights { get; set; }
         public DbSet<OrderDetail> OrderDetailHotels { get; set; }
         public DbSet<OrderHeader> OrderHeaders{ get; set; }

        public DbSet<OrderDetailFlight> OrderDetailFlights { get; set; }

    }
}


