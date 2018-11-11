using System.Collections.Generic;

namespace ScraBoy.Features.Item
{
    public class InventoryViewModel
    {
        public IEnumerable<InventoryModel> Items { get; set; }
        public double ShipmentsTotal { get; set; }
        public double ItemsTotal { get; set; }
        public double ProductsTotal { get; set; }
    }
}