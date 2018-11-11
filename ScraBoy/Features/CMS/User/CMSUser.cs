using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.User
{
    public class CMSUser:IdentityUser
    {
        [Required]
        [StringLength(50,MinimumLength = 5,ErrorMessage = "Cant be empty")]
        [Column(TypeName = "NVARCHAR")]
        public string DisplayName { get; set; }
    }
}