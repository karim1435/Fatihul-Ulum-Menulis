using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Forum.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Forum.Warning
{
    public enum AlertType
    {
        Spam,
        Abusive,
        Duplicate,
        Innappropriate,
        Violation
    }
    public class Alert
    {
        public int Id { get; set; }
        [Required]
        public AlertType ReportType { get; set; }
        [Required]
        public string Reason { get; set; }
        public string ThreadId { get; set; }
        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual CMSUser User { get; set; }
        public DateTime ReportedOn { get; set; }
    }
}