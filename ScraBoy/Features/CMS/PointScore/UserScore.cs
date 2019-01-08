using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.PointScore
{
    public class UserScore
    {
        public int Id { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public string Note { get; set; }
        public string AuthorId { get; set; }
        [ForeignKey("AuthorId")]
        public virtual CMSUser Author { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}