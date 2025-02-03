using InstaStay.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Models.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Models.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Capacity is required.")]
        [Range(1, 100, ErrorMessage = "Capacity must be between 1 and 100.")]
        public int Capacity { get; set; }
        [Required(ErrorMessage = "Price per night is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price per night must be a positive value.")]
        public double PricePerNight { get; set; }
        [Required(ErrorMessage = "Availability is required.")]
        public bool Availbility { get; set; }  = true;
        [Required(ErrorMessage = "Room type is required.")]
        [StringLength(50, ErrorMessage = "Room type cannot exceed 50 characters.")]
        public String RoomType { get; set; }
        [Required(ErrorMessage = "Bed type is required.")]
        [StringLength(50, ErrorMessage = "Bed type cannot exceed 50 characters.")]
        public String BedType { get; set; }
        [Required(ErrorMessage = "Hotel ID is required.")]
        public int HotelId { get; set; }
        [ValidateNever]
        public virtual Hotel Hotel { get; set; }
        [ValidateNever]
        public virtual ICollection<Booking> Bookings { get; set; }
        [ValidateNever]
        public virtual ICollection<RoomImages> RoomImages { get; set; }     

    }
}
