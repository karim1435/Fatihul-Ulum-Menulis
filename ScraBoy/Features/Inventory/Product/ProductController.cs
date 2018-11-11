using ScraBoy.Features.Inventory.Data;
using ScraBoy.Features.Inventory.Type;
using ScraBoy.Features.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Product
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ProductService productService;

        public ProductController(ProductService service)
        {
            this.productService = service;
        }
        public ProductController() : this(new ProductService()) { }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var products = await this.productService.GetProductsAsync();
            return View(products);
        }
        [AllowAnonymous]
        public async Task<ActionResult> Browse(string type)
        {
            var products = await productService.GetProductsByTypeAsync(type);

            if(!products.Any())
                return HttpNotFound();

            return View("Index",products);
        }
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if(!id.HasValue)
            {
                return HttpNotFound();
            }

            var product = await productService.GetProductByIdAync(id);
            if(product == null)
            {
                return HttpNotFound();
            }

            return View(product);

        }

        public async Task SetViewBag()
        {
            ShopService shopService = new ShopService();
            TypeService typeService = new TypeService();

            ViewBag.Shops = await shopService.GetShopsAsync();
            ViewBag.Types = await typeService.GetTypesAsync();
        }
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            await SetViewBag();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            await this.productService.AddProductAsync(model);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Edit(int? id)
        {
            if(!id.HasValue)
                return HttpNotFound();

            var product = await this.productService.GetProductByIdAync(id);

            if(product == null)
                return HttpNotFound();

            await SetViewBag();

            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Edit(ProductModel model)
        {
            await SetViewBag();
            if(!ModelState.IsValid)
                return View(model);

            await this.productService.UpdateProductAsync(model);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var product = await this.productService.GetProductByIdAync(id);
            if(product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        { 
            await this.productService.DeleteProductAsync(id);
            return RedirectToAction("Index");
        }
    }

}