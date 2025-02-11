using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class HotelManagerDashboardVM
    {
        public int TotalBookings { get; set; }
        public int CheckedInGuests { get; set; }
        public int PendingRequests { get; set; }
        public List<RoomStatusVM> Rooms { get; set; }
    }
}
