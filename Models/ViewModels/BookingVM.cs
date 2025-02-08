using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class BookingVM
    {
        public int RoomId { get; set; }
        public int HotelId { get; set; }
        public string RoomType { get; set; }
        public double PricePerNight { get; set; }
        [Required]
        public DateTime CheckINDate { get; set; }
        [Required]
        public DateTime CheckOutDate { get; set; }
        public string? CouponCode { get; set; }
        public bool IsAvailable { get; set; }
    }
}
