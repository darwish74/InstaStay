using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class RoomImages
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
    }
}
