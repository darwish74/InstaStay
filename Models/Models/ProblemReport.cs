using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ProblemReport
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
