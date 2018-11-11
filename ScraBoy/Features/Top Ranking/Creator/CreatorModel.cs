using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Creator
{
    public class CreatorModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Company")]
        public string Name { get; set;}
    }
}