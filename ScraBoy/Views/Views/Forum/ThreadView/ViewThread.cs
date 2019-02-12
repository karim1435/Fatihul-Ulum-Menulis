using ScraBoy.Features.Forum.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Forum.ThreadView
{
    public class ThreadView
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string ViewId { get; set; }
        public int Count { get; set; }
        public string ThreadId { get; set; }
        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }
        public DateTime LastViewed { get; set; }
    }
}