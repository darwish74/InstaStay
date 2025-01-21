using DataAccess.Data;
using Microsoft.EntityFrameworkCore.Metadata;
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
    public class HotelRepository:BaseRepository<Hotel>,IHotelRepository
    {
        private readonly ApplicationDbContext dbContext;

        public HotelRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
