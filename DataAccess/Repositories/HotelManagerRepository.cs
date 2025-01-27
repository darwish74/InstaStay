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
    public class HotelManagerRepository : BaseRepository<HotelManager>, IHotelManager
    {
        public HotelManagerRepository(ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
