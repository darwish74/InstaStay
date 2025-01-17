using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string country { get; set; }
        public string State{ get; set; }
        public string City{ get; set; }
        public int ZipCode { get; set; }
        public string NearbyAttractions  { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
