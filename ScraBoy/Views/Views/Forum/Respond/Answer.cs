using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Forum.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Forum.Respond
{
    public class Answer
    {
        public int Id { get; set; }
        [Required]
        [AllowHtml]
        [Column(TypeName = "NVARCHAR")]
        public string Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ThreadId { get; set; }
        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual CMSUser User { get; set; }
    }
}