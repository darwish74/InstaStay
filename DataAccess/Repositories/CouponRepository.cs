using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
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
        private readonly ApplicationDbContext context;

        public CouponRepository(ApplicationDbContext _context) : base(_context)
        {
            context = _context;
        }
        public IEnumerable<Coupon> GetCouponsByManager(string managerId)
        {
            return context.Coupons.Where(c => c.HotelManagerId == managerId).ToList();
        }

    }
}
