using ScraBoy.Features.Shop;
using ScraBoy.Features.Type;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Product
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        [Required]
        [Display(Name = "Price Per Unit")]
        public double UnitPrice { get; set; }
        [Required]
        [Display(Name = "Shipmet Price")]
        public double ShipmentPrice { get; set; }

        [Required]
        [Display(Name = "Shop")]
        public int ShopId { get; set; }
        
        [ForeignKey("ShopId")]
        public virtual ShopModel Shop { get; set; }

        [Required]
        [Display(Name = "Type")]
        public int TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual TypeModel Type { get; set; }

        [Required]
        [Display(Name = "Guarantee End On")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ExpiresOn { get; set; }
        [Required]
        [Display(Name = "Url Product")]
        public string Url { get; set; }

    }
}