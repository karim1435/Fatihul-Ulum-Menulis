using ScraBoy.Features.Hadist.Bab;
using ScraBoy.Features.Hadist.Book;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Hadist.Hadis
{
    public class Imam
    {
        [Key]
        public int Id { get; set; }
        public string SlugUrl { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }

        [NotMapped]
        public int FirstNumber
        {
            get
            {
                if(Chapters == null)
                {
                    return 0;
                }
                return 1;
            }
        }
        [NotMapped]
        public int LastNumber
        {
            get
            {
                if(Chapters == null)
                {
                    return 0;
                }
                return Chapters.Sum(a => a.TotalHadist);

            }
        }
        [NotMapped]
        public string LimitNumber
        {
            get
            {
                if(Chapters == null)
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