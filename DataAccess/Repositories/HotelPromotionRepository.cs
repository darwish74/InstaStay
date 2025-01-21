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
    internal class HotelPromotionRepository:BaseRepository<HotelPromotion>, IHotelPromotionRepository
    {
        private readonly ApplicationDbContext dbContext;

        public HotelPromotionRepository(ApplicationDbContext dbContext):base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
