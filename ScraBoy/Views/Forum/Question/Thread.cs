using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Forum.Channel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Forum.Question
{
    public class Thread
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
        [StringLength(int.MaxValue,MinimumLength = 400)]
        public string Content { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }

        private IList<string> _labels = new List<String>();

        public IList<string> Labels
        {
            get { return _labels; }
            set { _labels = value; }
        }
        [Required(ErrorMessage = "Labels can't be empty")]
        public string CombinedLabels
        {
            get
            {
                return string.Join(",",_labels).TrimEnd(',');
            }
            set
            {
                _labels = value.Split(',').Select(s => s.Trim()).ToList();
            }
        }

        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual CMSUser Author { get; set; }
        public int TopicId { get; set; }
        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
    }
}