using Models.Models;
using Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IRepositories
{
    public interface ICouponRepository:IBaseRepositories<Coupon>
    {
        public IEnumerable<Coupon> GetCouponsByManager(string managerId);
    }
}
