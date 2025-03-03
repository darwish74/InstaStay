﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
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
        public DbSet<HotelManagers> HotelManagers { get; set; }
        public DbSet<RoomImages> roomImages { get; set; }   
        public DbSet<HotelImages> hotelImages { get; set; } 
        public DbSet<Amentities> Amentities { get; set; }
        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<ActivityLog>ActivityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Room>()
           .HasOne(r => r.Hotel)
           .WithMany(h => h.Rooms)
           .HasForeignKey(r => r.HotelId)
           .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
