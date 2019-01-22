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
        public int Number { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? ChapterId { get; set; }
        [ForeignKey("ChapterId")]
        public Chapter Chapter { get; set; }
        public int? ImamId { get; set; }
        [ForeignKey("ImamId")]
        public Imam Imam { get; set; }
    }
    public class Imam
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Kitab> Kitabs { get; set; }
    }
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Kitab> Kitabs { get; set; }
    }
}