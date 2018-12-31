using Microsoft.AspNet.Identity.EntityFramework;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.Nws;
using ScraBoy.Features.CMS.Reporting;
using ScraBoy.Features.Lomba.Audience;
using ScraBoy.Features.Lomba.Contest;
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
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        [Display(Name ="Birth Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}",ApplyFormatInEditMode = true)]
        public DateTime? Born { get; set; }
        public int Bonus { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        [Required]
        public string Security { get; set; }
        public string FbProfile { get; set; }
        public string InstagramProfile { get; set; }
        public string TwitterProfile { get; set; }
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual DateTime? RegistrationDate { get; set; }
        public string UrlImage { get; set; }    
        public virtual ICollection<Competition> Competitions { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Violation> Violations { get; set; }
        [NotMapped]
        public string CurrentRole { get; set; }

        [NotMapped]
        public string UrlUser
        {
            get
            {
                return string.Format("user/profile/{0}",this.SlugUrl);
            }
        }
        [NotMapped]
        public string FullUrlUser
        {
            get
            {
                return StringExtensions.getUrl() + UrlUser;
            }
        }
    }

}