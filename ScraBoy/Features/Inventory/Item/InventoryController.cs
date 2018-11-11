using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Item
{
    [Authorize(Roles = "Admin")]
    public class InventoryController : Controller
    {
        private readonly InventoryService inventoryService;
        public InventoryController(InventoryService service)
        {
            this.inventoryService = service;
        }
        public InventoryController():this(new InventoryService()) { }
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var inventoryViewModel = await this.inventoryService.GetInventoryAsync();
            if(inventoryViewModel == null)
                return RedirectToAction("Index","Product");

            return View(inventoryViewModel);
        }
        
        public async Task<ActionResult> AddToInventory(int id)
        {
            await this.inventoryService.AddToInventoryAsync(id);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> RemoveFromInventory(int id)
        {
            await this.inventoryService.RemoveFromInventoryAsync(id);

            return RedirectToAction("Index");
        }
    }
}