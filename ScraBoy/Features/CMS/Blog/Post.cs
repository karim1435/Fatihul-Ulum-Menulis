using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.Reporting;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Blog
{
    public class Post
    {
        [Display(Name = "Slug")]
        public string Id { get; set; }

        [Display(Name = "Title")]
        [Required]
        [StringLength(80,MinimumLength = 3,ErrorMessage = "Cant be empty")]
        public string Title { get; set; }

        [AllowHtml]
        [Display(Name = "Post Content")]
        [Required]
        public string Content { get; set; } 
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }

        public DateTime? Published { get; set; }
        private IList<string> _tags = new List<String>();

        public IList<string> Tags
        {
            get { return _tags; }
            set { _tags = value; }
        }
        [Required(ErrorMessage = "Tag can't be empty")]
        public string CombinedTags
        {
            get
            {
                return string.Join(",",_tags).TrimEnd(',');
            }
            set
            {
                _tags = value.Split(',').Select(s => s.Trim()).ToList();
            }
        }

        public bool Private { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual CMSUser Author { get; set; }

        [Required(ErrorMessage = "Please Select Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Voting> Votings { get; set; }
        public virtual ICollection<ViewPost> ViewPosts { get; set; }
        public virtual ICollection<Violation> Violations { get; set; }
        [Display(Name = "Upload Image")]
        public string UrlImage { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        [NotMapped]
        public int TotalViews
        {
            get
            {
                if(ViewPosts == null)
                {
                    return 0;
                }
                return ViewPosts.Sum(a => a.Count);
            }
        }
        [NotMapped]
        public int TotalVote
        {
            get
            {
                if(Votings == null)
                {
                    return 0;
                }
                return Votings.Where(p => p.LikeCount == true).Count();
            }
        }
        [NotMapped]
        public int TotalComment
        {
            get
            {
                if(Comments == null)
                {
                    return 0;
                }
                return Comments.Count();
            }
        }
        [NotMapped]
        public string UrlPost
        {
            get
            {
                return string.Format("posts/{0}",this.Id);
            }
        }
        [NotMapped]
        public string FullUrlPost
        {
            get
            {
                return StringExtensions.getUrl() + UrlPost;
            }
        }  
    }
    public class ViewPost
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string ViewId { get; set; }
        public int Count { get; set; }
        public string PostId  { get; set; }
        [ForeignKey("PostId")]
        public virtual Post Post { get; set; }
        public DateTime LastViewed { get; set; }
    }
}