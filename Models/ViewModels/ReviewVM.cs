using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ReviewVM
    {
        public int HotelId { get; set; }
        [Required]
        public string ReviewTitle { get; set; }
        [Required]
        public string ReviewText { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rate { get; set; }
    }
}
