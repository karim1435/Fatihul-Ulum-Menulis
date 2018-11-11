using ScraBoy.Features.Game;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Ranking
{
    public class RankingModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Rank")]
        public int Rank { get; set; }
        public int GameId { get; set; }
        [ForeignKey("GameId")]
        public virtual GameModel Game { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        [Display(Name = "Date")]
        public DateTime RankedOn { get; set; }
        [Display(Name = "Image")]
        public string Image { get; set; }
        [Display(Name = "Updated On")]
        public string Update { get; set; }
        [Display(Name = "Donwload")]
        public string TotalInstall { get; set; }
    }
}