using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class UserPaginationViewModel
    { 
       public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
       public int CurrentPage { get; set; }
       public int PageSize { get; set; }
       public int TotalUsers { get; set; }
       public int TotalPages => (int)Math.Ceiling((double)TotalUsers / PageSize);
    }
}
