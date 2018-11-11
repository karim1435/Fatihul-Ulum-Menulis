using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScraBoy.Features.Data;
using ScraBoy.Features.Type;
using System.Threading.Tasks;
using ScraBoy.Features.Product;
using ScraBoy.Features.Data;

namespace ScraBoy.Features.Inventory.Type
{
    [Authorize(Roles = "Admin")]
    public class TypeController : Controller
    {
        private CMSContext  db = new CMSContext ();

        private readonly TypeService typeService;
        public TypeController(TypeService service)
        {
            this.typeService = service;
        }
        public TypeController() : this(new TypeService()) { }
        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            var types = await this.typeService.GetTypesAsync();
            return View(types);
        }
        public ActionResult Create()
        {
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TypeModel typeModel)
        {
            if (ModelState.IsValid)
            {
                db.Type.Add(typeModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(typeModel);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeModel typeModel = db.Type.Find(id);
            if (typeModel == null)
            {
                return HttpNotFound();
            }
            return View(typeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( TypeModel typeModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(typeModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(typeModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TypeModel typeModel = db.Type.Find(id);
            if (typeModel == null)
            {
                return HttpNotFound();
            }
            return View(typeModel);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TypeModel typeModel = db.Type.Find(id);
            db.Type.Remove(typeModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
