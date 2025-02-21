using Models.Models;

namespace Models.ViewModels
{
    public class AdminDashboardVM
    {
        public int TotalUsers { get; set; }
        public int TotalBookings { get; set; }
        public int PendingPayments { get; set; }
        public List<ActivityLog> RecentActivities { get; set; } 
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
