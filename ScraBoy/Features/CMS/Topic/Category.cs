using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Lomba.Contest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.Topic
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 1,ErrorMessage = "Cant be empty")]
        [Column(TypeName = "NVARCHAR")]
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Competition> Competitions { get; set; }
    }
}