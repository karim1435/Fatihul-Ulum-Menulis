using ScraBoy.Features.Hadist.Bab;
using ScraBoy.Features.Hadist.Hadis;
using ScraBoy.Features.Hadist.Meaning;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Hadist.Book
{
    public class Kitab
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public Chapter Chapter { get; set; }
        public virtual ICollection<Translation> Translations { get; set; }
        [NotMapped]
        public Translation CurrentTranslation { get; set; }
    }


}