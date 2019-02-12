using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Forum.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Forum.Favorite
{
    public class ThreadStar
    {
        public int Id { get; set; }
        public bool Like { get; set; }
        public string ThreadId { get; set; }
        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual CMSUser User { get; set; }
    }
}