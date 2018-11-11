using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Type
{
    public class TypeModel
    {
        [Key]
        public int TypeId { get; set; }
        [Required]
        [Display(Name = "Type Name")]
        public string TypeName { get; set; }
    }
}