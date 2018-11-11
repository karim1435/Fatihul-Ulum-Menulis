using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;

namespace ScraBoy.Features.Shop
{
    public class ShopModel
    {
        [Key]
        public int ShopId { get; set; }
        [Required]
        [Display(Name = "Shop Name")]
        public string ShopName { get; set; }
        [Required]
        [Display(Name = "Store Link")]
        public string ShopLink { get; set; }

    }
}