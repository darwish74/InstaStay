using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public  class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime SentDate { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
