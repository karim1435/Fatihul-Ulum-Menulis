using ScraBoy.Features.Hadist.Book;
using ScraBoy.Features.Hadist.Hadis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Hadist.Bab
{
    public class Chapter
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        public string SlugUrl { get; set; }
        [Required]
        public string Name { get; set; }
        public int ImamId { get; set; }
        [ForeignKey("ImamId")]
        public Imam Imam { get; set; }
        public virtual ICollection<Kitab> Kitabs { get; set; }
        [NotMapped]
        public int TotalHadist
        {
            get
            {
                if(Kitabs==null)
                {
                    return 0;
                }
                return Kitabs.Count();
            }
        }
        [NotMapped]
        public int FirstNumber
        {
            get
            {
                if(Kitabs==null)
                {
                    return 0;
                }
                return Kitabs.FirstOrDefault().Number;
            }
        }
        [NotMapped]
        public int LastNumber
        {
            get
            {
                if(Kitabs==null)
                {
                    return 0;
                }
                return Kitabs.OrderByDescending(a => a.Number).FirstOrDefault().Number;
               
            }
        }
        [NotMapped]
        public string LimitNumber
        {
            get
            {
                if(Kitabs==null)
                {
                    return "";
                }
                if(FirstNumber == LastNumber)
                {
                    return "(" + FirstNumber + ")";
                }
                return "(" + FirstNumber + "-" + LastNumber + ")";
            }
        }
    }
}