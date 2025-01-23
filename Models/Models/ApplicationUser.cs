using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country  { get; set; }
        public string City  { get; set; }
        public string State { get; set; }
        public int  PostalCode { get; set; }
        public string Photo { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Room> Wishlist { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public bool? ISBlocked { get; set; } = false;
    }
}
