using ScraBoy.Features.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace ScraBoy.Features.Item
{
    public class InventoryModel
    {       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double TotalProductPrice { get; set; }
        public double TotalShipmentPrice { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}