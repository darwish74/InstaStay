using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.IRepositories;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccess.Repositories
{
    public class BookingRepository:BaseRepository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext dbContext;

        public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }

   
}
