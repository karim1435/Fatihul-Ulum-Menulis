using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Lomba.Contest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Lomba.Audience
{
    public class Participant
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [AllowHtml]
        public string Content { get; set; }
        [Required]
        public DateTime ProposedOn { get; set; }
        public int Score { get; set; }
        [AllowHtml]
        public string Message { get; set; }
        public int CompetitionId { get; set; }
        [ForeignKey("CompetitionId")]
        public Competition Competition { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public CMSUser Author { get; set; }
    }
}