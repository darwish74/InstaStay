using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Amentities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        [ValidateNever]
        public string Image { get; set; }
        public int HotelId { get; set; }
        [ValidateNever]
        public Hotel Hotel { get; set; }
    }
}
