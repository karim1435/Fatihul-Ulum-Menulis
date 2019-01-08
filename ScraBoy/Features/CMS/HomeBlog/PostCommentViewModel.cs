using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.HomeBlog
{
    public enum NotificationType
    {
        Comment,
        Voting
    }
    public class NotificationViewModel
    {
        public Post Post { get; set; }
        public CMSUser User { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime PostedOn { get; set; }
        public Comment Parent { get; set; }
    }

    public class RankingViewModel
    {
        public CMSUser User { get; set; }
        public int TotalLikedPost { get; set; }
        public int TotalViewedPost { get; set; }
        public int TotalPublishedPost { get; set; }
        public int TotalCommentPost { get; set; }
        public int Point
        {
            get
            {
                return ((TotalLikedPost * 1) + (TotalViewedPost * 2) +
                    (TotalPublishedPost * 10) + (TotalCommentPost * 2)) +
                    User.TotalBonus;
            }
        }
    }
}