using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using ScraBoy.Features.Data;

namespace ScraBoy.Features.Item
{
    public class InventoryService
    {
        private readonly CMSContext  _db;
        public InventoryService(CMSContext  context)
        {
            this._db = context;
        }
        public InventoryService() : this(new CMSContext ()) { }
        public async Task<IEnumerable<InventoryModel>> GetInventoriesAsync()
        {
            return await this._db.Inventory.Include("Product").ToArrayAsync();
        }
        public async Task<InventoryViewModel> GetInventoryAsync()
        {
            var inventories = await this.GetInventoriesAsync();
            return GetInventoryViewModel(inventories);
        }

        private InventoryViewModel GetInventoryViewModel(IEnumerable<InventoryModel> inventories)
        {
            var inventoryModel = new InventoryViewModel();

            foreach(var inventory in inventories)
            {
                inventoryModel.Items = inventories;
                inventoryModel.ItemsTotal += inventory.TotalProductPrice;
                inventoryModel.ShipmentsTotal += inventory.TotalShipmentPrice;
                inventoryModel.ProductsTotal = inventoryModel.ItemsTotal + inventoryModel.ShipmentsTotal;           
            }

            return inventoryModel;
        }

        public async Task AddToInventoryAsync(int productId)
        {
            var product = await this._db.Product.FirstOrDefaultAsync(p => p.ProductId == productId);

            if(product == null)
                return;

            var inventory = await _db.Inventory.FirstOrDefaultAsync(c => c.ProductId == productId);

            if(inventory != null)
            {
                inventory.Quantity++;
            }
            else
            {
                inventory = new InventoryModel
                {
                    ProductId = productId,
                    Quantity = 1,
                    CreatedOn = DateTime.Now
                };
                
                _db.Inventory.Add(inventory);
            }

            SetTotalPrice(inventory);

            await _db.SaveChangesAsync();
        }
        public void SetTotalPrice(InventoryModel model)
        {
            model.TotalProductPrice = (model.Product.UnitPrice * model.Quantity);
            model.TotalShipmentPrice = (model.Product.ShipmentPrice * model.Quantity);
        }
        public async Task<int> RemoveFromInventoryAsync(int productId)
        {
            var inventory = await _db.Inventory.FirstOrDefaultAsync(c => c.ProductId == productId);

            var inventoryCount = 0;

            if(inventory == null)
                return inventoryCount;

            if(inventory.Quantity > 1)
            {
                inventory.Quantity--;
                inventoryCount = inventory.Quantity;
            }
            else
            {
                _db.Inventory.Remove(inventory);
            }

            await _db.SaveChangesAsync();

            return inventoryCount;
        }
    }
}