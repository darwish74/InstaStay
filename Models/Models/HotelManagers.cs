using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class HotelManagers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
