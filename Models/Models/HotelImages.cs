using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class HotelImages
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }
        public int HotelId { get; set; }
        [ValidateNever]
        public Hotel Hotel { get; set; }
    }
}
