using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class ProblemReport
    {
        public int Id { get; set; }
        [ValidateNever]
        public string UserEmail { get; set; }
        [Required]
        public RequestType Request { get; set; }
        [ValidateNever]
        public string UserName { get; set; }
        [Required]
        public string ProblemDescription { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; } =null;
        public string? UserImgRequest { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; }
        
    }
    public enum RequestType
    {
        Complaint,
        Suggestion
    }
}
