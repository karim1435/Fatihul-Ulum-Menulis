using ScraBoy.Features.Hadist.Book;
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
        public string Name { get; set; }
        public virtual ICollection<Kitab> Kitabs { get; set; }
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