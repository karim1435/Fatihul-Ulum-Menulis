using ScraBoy.Features.Hadist.Book;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Hadist.Meaning
{
    public class Translation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime TranslatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }
        public int KitabId { get; set; }
        [ForeignKey("KitabId")]
        public Kitab Kitab { get; set; }
    }
    public class Language
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string KeyCode { get; set; }
    }
}