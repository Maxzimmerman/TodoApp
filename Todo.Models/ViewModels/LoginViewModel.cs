﻿using System.ComponentModel.DataAnnotations;

namespace Todo.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email is reqzired")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
