using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Nws
{
    public enum ReportType
    {
        Update,
        Bug
    }
    public class Report
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTime ReportedOn { get; set; }
        [Display(Name ="Report Type")]
        public ReportType ReportType { get; set; }
        public bool Fixed { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual CMSUser User { get; set; }
    }
}