using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.Interest
{
    public class Voting
    {
        public int Id { get; set; }
        public bool LikeCount { get; set; }
        public bool DislikeCount { get; set; }
        public DateTime PostedOn { get; set; }
        public string PostId { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual CMSUser User { get; set; }
    }
}