﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ApplicationUserVM
    {
        [Required]
        public string UserName { get; set; }
        [ValidateNever]
        public string FirstName { get; set; }
        [ValidateNever]
        public string LastName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [ValidateNever]
        public string Photo { get; set; }
        [ValidateNever]
        public IFormFile ProfilePhoto { get; set; }
    }
}
