using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class HotelManagerRequests
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RequestMesssage { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
