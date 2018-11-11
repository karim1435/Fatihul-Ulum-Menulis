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
            PostTags = new TagViewModel();
            SideBarCategory = new CategoryViewModel();
            PostCategories = new CategoryViewModel();
            SideBarTags = new TagViewModel();
        }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public TagViewModel PostTags { get; set; }
        public CategoryViewModel PostCategories { get; set; }
        public string NewComment { get; set; }
        public string UrlImage { get; set; }
        public string PostId { get; set; }
        public string UserId { get; set; }
        public VoteViewModel Voting { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public IEnumerable<CommentViewModel> RecentComments { get; set; }
        public CategoryViewModel SideBarCategory { get; set; }
        public List<CategoryViewModel> SideBarComments { get; set; }
        public TagViewModel SideBarTags { get; set; }
        public DateTime Created { get; internal set; }
        public DateTime? Published { get; internal set; }
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
        public DateTime PostedOn { get; set; }
        public string Post { get; set; }
        public string Author { get; set; }
        public string Comment { get; set; }
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
    public class LikeViewModel
    {
        public string Author { get; set; }
    }
}