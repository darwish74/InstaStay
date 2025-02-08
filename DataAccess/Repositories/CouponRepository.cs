using DataAccess.Data;
using Models.IRepositories;
using Models.Models;
using Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccess.Repositories
{
    public class CouponRepository : BaseRepository<Coupon>,ICouponRepository
    {
        public CouponRepository(ApplicationDbContext _context) : base(_context)
        {
        }
    }
}
