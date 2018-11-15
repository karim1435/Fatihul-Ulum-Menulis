using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.User
{
    public class CMSUser:IdentityUser
    {
        [Display(Name ="About Me")]
        [AllowHtml]
        public string Description { get; set; }
    }
}