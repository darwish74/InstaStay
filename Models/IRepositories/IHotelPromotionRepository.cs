using Models.Models;
using Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.IRepositories
{
    internal interface IHotelPromotionRepository:IBaseRepositories<HotelPromotion>
    {
    }
}

namespace Models
{
    public interface IHotelPromotionRepository
    {
    }
}