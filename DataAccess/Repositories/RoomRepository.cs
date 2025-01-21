using DataAccess.Data;
using Models;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    internal class RoomRepository : BaseRepository<Room>, IRoomRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RoomRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
