using DataAccess.Data;
using Models.IRepositories;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class NewHotelRequestsRepository:BaseRepository<NewHotelRequests>, INewHotelRequestsRepository
    {
        private readonly ApplicationDbContext dbContext;

        public NewHotelRequestsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
