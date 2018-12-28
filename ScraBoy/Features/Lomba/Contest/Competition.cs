using Microsoft.AspNet.Identity;
using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Lomba.Audience;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Lomba.Contest
{
    public enum StatusContest
    {
        Penilaian,
        Finish,
        Open
    }
    public class Competition
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        public string Description { get; set; }
        public string SlugUrl { get; set; }
        [Required]
        [Display(Name ="Minimum Word")]
        public int MinimumWord { get; set; }
        [Required]
        [Display(Name = "Maximum Word")]
        public int MaximumWord { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public DateTime UpdatedOn { get; set; }
        [Required]
        [Display(Name = "Starts On")]
        public DateTime StartedOn { get; set; }
        [Required]
        [Display(Name = "Ends On")]
        public DateTime EndOn { get; set; }
        [Display(Name = "Status Contest")]
        public StatusContest StatusContest { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public string CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public CMSUser Creator { get; set; }
        [Display(Name = "Upload Image")]
        public string UrlImage { get; set; }
        [Required]
        [Display(Name = "Reward Contest")]
        public string Reward { get; set; }
        public virtual ICollection<Participant> Participants { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [NotMapped]
        public int TotalParticipants
        {
            get
            {
                if(Participants==null)
                {
                    return 0;
                }
                return Participants.Count();
            }
        }
        [NotMapped]
        public string UrlContest
        {
            get
            {
                return string.Format("contest/fucontest/{0}",this.SlugUrl);
            }
        }
        [NotMapped]
        public string FullUrlContest
        {
            get
            {
                return StringExtensions.getUrl() + UrlContest;
            }
        }
    }
}