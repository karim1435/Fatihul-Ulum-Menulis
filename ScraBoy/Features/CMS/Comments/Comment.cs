using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Comments
{
    public class Comment
    {
        public int Id { get; set; }

        [Display(Name = "Post Content")]
        [Required(ErrorMessage = "Please Fill The Content")]
        [StringLength(2000,MinimumLength = 5)]
        [AllowHtml]
        public string Content { get; set; }
        public DateTime PostedOn { get; set; }
        public string PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual CMSUser User { get; set; }

        public int? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public virtual Comment Parent { get; set; }

        public virtual ICollection<Comment> Respond { get; set; }
    }
}