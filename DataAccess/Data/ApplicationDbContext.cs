using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Hotel> hotels { get; set; }
        public DbSet<Room> rooms { get; set; }
        public DbSet<Booking> bookings { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<Location> locations { get; set; }
        public DbSet<Review> reviews { get; set; }  
        public DbSet<HotelPromotion> hotelpromotions { get; set;}
        public DbSet<Payment> payments { get; set; }
        public DbSet<ApplicationUser>applicationUsers { get; set; }
        public DbSet<ProblemReport> ProblemReports { get; set; }
        public DbSet<HotelManagerRequests> HotelManagerRequests { get; set; }
        public DbSet<NewHotelRequests> NewHotelRequests { get; set; }
        public DbSet<HotelManager> HotelManagers { get; set; }

  
    }
}
