using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public string UserId { get; set; } 
        public int HotelId { get; set; }
        public int RoomId { get; set; }
        public string BookingStatus { get; set; }
        public double TotalAmount { get; set;}
        public DateTime CheckINDate { get; set;}
        public DateTime CheckOutDate { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual Room Room { get; set; }
    }
}
