using System;
using System.ComponentModel.DataAnnotations;
namespace Models.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Required]
        public DateTime ReviewDate { get; set; } = DateTime.Now;
        [Required]
        public string ReviewText { get; set; }  
        [Required]
        public string ReviewTitle { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rate { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int HotelId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
