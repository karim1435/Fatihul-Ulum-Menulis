using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScraBoy.Features.Data;
using ScraBoy.Features.Shop;
using System.Threading.Tasks;
using ScraBoy.Features.Data;

namespace ScraBoy.Features.Inventory.Shop
{
    [Authorize(Roles = "Admin")]
    public class ShopController : Controller
    {
        private CMSContext  db = new CMSContext ();

        private readonly ShopService shopService;
        public ShopController(ShopService service)
        {
            this.shopService = service;
        }
        public ShopController() : this(new ShopService()) { }
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var shops = await shopService.GetShopsAsync();
            return View(shops);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopModel shopModel = db.Shop.Find(id);
            if (shopModel == null)
            {
                return HttpNotFound();
            }
            return View(shopModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( ShopModel shopModel)
        {
            if (ModelState.IsValid)
            {
                db.Shop.Add(shopModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(shopModel);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopModel shopModel = db.Shop.Find(id);
            if (shopModel == null)
            {
                return HttpNotFound();
            }
            return View(shopModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ShopModel shopModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shopModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(shopModel);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopModel shopModel = db.Shop.Find(id);
            if (shopModel == null)
            {
                return HttpNotFound();
            }
            return View(shopModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopModel shopModel = db.Shop.Find(id);
            db.Shop.Remove(shopModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
