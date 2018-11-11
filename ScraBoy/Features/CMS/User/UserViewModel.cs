using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.User
{
    public class UserViewModel
    {
        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }
        [Required]
        [Display(Name = " Display Name")]
        public string DisplayName { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("ConfirmPassword",ErrorMessage ="The new password and confirmation password don't match")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Display(Name ="Role")]
        public string SelectedRole { get; set; }
        private readonly List<String> roles = new List<string>();
        public IEnumerable<SelectListItem> Roles
        {
            get { return new SelectList(roles); }
        }
        
        public void LoadUserRoles(IEnumerable<IdentityRole> roleNames)
        {
            this.roles.AddRange(roleNames.Select(r => r.Name));
        }
    }
}