using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int Capacity{ get; set; }
        public double PricePerNight { get; set; }
        public bool Availbility { get; set; }
        public string RoomType { get; set; }
        public string BedType { get; set; }
        public int HotelId { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
