using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Review
    {
        public int Id { get; set; }
        public DateTime ReviewDate { get; set; }
        public DateTime ReviewText { get; set; }
        public string ReviewTitle { get; set;}
        public int Rate { get; set;}
        public int UserId { get; set; } 
        public int HotelId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
