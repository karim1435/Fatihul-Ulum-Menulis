using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        public string NewComment { get; set; }
        public VoteViewModel Voting { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public TagViewModel SideBarTags { get; set; }
        public bool Voted { get; set; }
        public int ViewCount { get; set; }
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
            LikedUser = new List<string>();
            DislikedUser = new List<string>();
        }
        public List<string> LikedUser { get; set; }
        public List<string> DislikedUser { get; set; }
        public int TotalLike { get; set; }
        public int TotalDislike { get; set; }
    }
}