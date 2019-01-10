using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.Following
{
    public class Follow
    {
        public int Id { get; set; }
        public string FollowerId { get; set; }
        [ForeignKey("FollowerId")]
        public virtual CMSUser Follower { get; set; }
        public string FollowedId { get; set; }
        [ForeignKey("FollowedId")]
        public virtual CMSUser Followed { get; set; }
        public DateTime FollowedOn { get; set; }
    }
}