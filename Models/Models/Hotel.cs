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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Stars { get; set; }

        public string ContactInfo { get; set; }
        public string policies { get; set; }
        public string CoverImage {  get; set; }
        public virtual ICollection<Room> Rooms { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Promotion> Promotions { get; set; }
        public virtual Location Location { get; set; }
    }
}
