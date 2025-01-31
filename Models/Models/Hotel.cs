using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Hotel
    {
        public object Price;
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Stars { get; set; }
        public string? CoverImage { get; set; }
        public string? ContactInfo { get; set; }
        public int? HotelManagerId { get; set; } 
        public virtual HotelManagers? HotelManager { get; set; }
        [ValidateNever]
        public virtual ICollection<Room> Rooms { get; set; }
        [ValidateNever]
        public virtual ICollection<Review> Reviews { get; set; }
        [ValidateNever]
        public virtual ICollection<Promotion> Promotions { get; set; }
        [ValidateNever]
        public virtual ICollection<HotelImages>HotelImages { get; set; }
        [ValidateNever]
        public virtual ICollection<Amentities> Amentities { get; set; } = null;
    }
}
