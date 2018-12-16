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
    public class BlogViewModel
    {
        public BlogViewModel()
        {
            Comments = new List<CommentViewModel>();
            Voting = new VoteViewModel();
            SideBarTags = new TagViewModel();
            Post = new Post();
            User = new CMSUser();
            Category = new Category();
        }
        public Post Post { get; set; }

        public CMSUser User { get; set; }
        public Category Category { get; set; }
        [AllowHtml]
        [Required(ErrorMessage ="Content Can't be Empty")]
        public Comment NewComment { get; set; }
        public VoteViewModel Voting { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public TagViewModel SideBarTags { get; set; }
        public bool Voted { get; set; }
        public int ViewCount { get; set; }
        public int TotalComment { get; set; }
        public string FullUrl { get; set; }
    }
    public class TagViewModel
    {
        public IList<string> Tags { get; set; }
    }
    public class CategoryViewModel
    {
        public string Names { get; set; }
    }
    public class CommentViewModel
    {
        public CommentViewModel()
        {
            User = new CMSUser();
            Post = new Post();
            Comment = new Comment();
        }
        public Comment Comment { get; set; }
        public CMSUser User { get; set; }
        public Post Post { get; set; }
    }
    public class VoteViewModel
    {
        public VoteViewModel()
        {
            LikedUser = new List<Voting>();
        }
        public List<Voting> LikedUser { get; set; }
        public int TotalLike { get; set; }
    }
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
                return (TotalLikedPost *1) + (TotalViewedPost * 2) + (TotalPublishedPost * 10) + (TotalCommentPost * 2);
            }
        }
    }
}