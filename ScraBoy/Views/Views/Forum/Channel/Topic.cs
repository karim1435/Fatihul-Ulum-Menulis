
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Forum.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Forum.Channel
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 1,ErrorMessage = "Cant be empty")]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual CMSUser Author { get; set; }
    }
    public class Topic
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 1,ErrorMessage = "Cant be empty")]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public int SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual CMSUser Author { get; set; }
    }
}