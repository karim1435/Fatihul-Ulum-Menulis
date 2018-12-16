using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Reporting
{
    public enum ViolationType
    {
        Offensive,
        Promosi,
        Sampah,
        Kekerasan,
        Hoax,
        Menyesatkan,
        Duplikat,
        Penistaan

    }
    public class Violation
    {
        public int Id { get; set; }

        [Display(Name = "Post Content")]
        [Required(ErrorMessage = "Please Fill The Reason")]
        [StringLength(2000,MinimumLength = 5)]
        [AllowHtml]
        public string Reason { get; set; }
        public ViolationType ViolationType { get; set; }
        public DateTime ReportedOn { get; set; }
        public string PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual CMSUser User { get; set; }

    }
}