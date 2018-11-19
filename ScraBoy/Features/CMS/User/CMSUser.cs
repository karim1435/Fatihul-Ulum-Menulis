using Microsoft.AspNet.Identity.EntityFramework;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.Nws;
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
        public string SlugUrl { get; set; }
        public string DisplayName { get; set; }
        [Display(Name ="Birth Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",ApplyFormatInEditMode = true)]
        public DateTime? Born { get; set; }
        [AllowHtml]
        [StringLength(1000,MinimumLength =10)]
        public string Description { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        [NotMapped]
        public string CurrentRole { get; set; }
    }
}