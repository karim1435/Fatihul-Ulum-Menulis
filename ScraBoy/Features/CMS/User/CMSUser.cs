using Microsoft.AspNet.Identity.EntityFramework;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.Nws;
using ScraBoy.Features.CMS.PointScore;
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
        public virtual ICollection<UserScore> UserScores { get; set; }
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
        [NotMapped]
        public int TotalScore
        {
            get
            {
                return (TotalLike * 1) + 
                    (TotalView * 2) + 
                    (TotalPost * 10) + 
                    (TotalComment * 2) + 
                    TotalBonus;
            }
        }
        [NotMapped]
        public int TotalBonus
        {
            get
            {
                if(UserScores==null)
                    return 0;
                return UserScores.Sum(a => a.Score);
            }
        }

        [NotMapped]
        public int TotalPoint
        {
            get
            {
                if(Posts == null)
                    return 0;

                return Posts.Sum(a=>a.TotalViews) + Posts.Sum(a => a.TotalComment) +
                    Posts.Sum(a => a.TotalVote);
            }
        }
        [NotMapped]
        public int TotalComment
        {
            get
            {
                if(Posts == null)
                    return 0;
                return Posts.Sum(a=>a.Comments.Count());
            }
        }
        [NotMapped]
        public int TotalPost
        {
            get
            {
                if(Posts == null)
                    return 0;
                return Posts.Where(a=>!a.Private).Count();
            }
        }
        [NotMapped]
        public int TotalLike
        {
            get
            {
                if(Posts == null)
                    return 0;
                return Posts.Sum(a => a.Votings.Count());
            }
        }
        [NotMapped]
        public int TotalView
        {
            get
            {
                if(Posts == null)
                    return 0;
                return Posts.Sum(a => a.ViewPosts.Count());
            }
        }
    }

}