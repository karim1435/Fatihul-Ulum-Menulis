using ScraBoy.Features.Creator;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Game
{
    public class GameModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Title { get; set; }
        public int CreatorId { get; set; }
        [ForeignKey("CreatorId")]
        public virtual CreatorModel Creator { get; set; }
        [Display(Name = "Subtitle")]
        public string Subtitle { get; set; }
        [Display(Name ="Url")]
        public string UrlGame { get; set; }
    }
}