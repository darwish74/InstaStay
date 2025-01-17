using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class HotelPromotion
    {
        public int Id { get; set; } 
        public int PromotionId { get; set; }
        public int HotelId { get; set; }
        public string ApplicableHotelsAndRooms { get; set; }
        public virtual Promotion Promotion { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
