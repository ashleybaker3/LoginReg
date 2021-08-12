using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginReg.Models
{
    public class LoginUser
    {
        public int LoginUserID { get; set; }

        [Required(ErrorMessage = "Required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string LogEmail { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required")]
        [MinLength(8, ErrorMessage = "Must be at least 8 characters.")]
        [Display(Name = "Password")]
        public string LogPassword { get; set; }
    }
}