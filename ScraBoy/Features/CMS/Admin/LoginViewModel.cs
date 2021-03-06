﻿using System.ComponentModel.DataAnnotations;

namespace ScraBoy.Features.CMS.Admin
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name ="Username")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Pasword { get; set; }
        [Display(Name ="Remember Me")]
        public bool RememberMe { get; set; }
    }
    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [StringLength(100,ErrorMessage = "The {0} must be at least {2} characters long.",MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please Fill Your Security Question")]
        [Display(Name = "Your Security Question")]
        public string Security { get; set; }

    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Required(ErrorMessage ="Please Fill Your Security Question")]
        [Display(Name = "Security Question")]
        public string Security { get; set; }
        public string UrlImage { get; set; }
        [Required]
        [StringLength(100,ErrorMessage = "The {0} must be at least {2} characters long.",MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}