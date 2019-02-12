using ScraBoy.Features.CMS.Gzip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Hadist.Hadis
{
    [RoutePrefix("imam")]
    [Authorize(Roles = "admin,editor")]
    public class ImamController : Controller
    {
        private IimamRepository imamRepository;
        public ImamController()
        {
            imamRepository = new ImamRepository();
        }
        [Route("")]
        [CompressContent]
        [AllowAnonymous]
        public async Task<ActionResult> Index(int? page, string currentFilter)
        {
            int pageNumber = page ?? 1;

            var model = await this.imamRepository.GetPageImam(currentFilter,pageNumber);
            return View(model);
        }
        [CompressContent]
        [AllowAnonymous]
        public async Task<ActionResult> Search(string search)
        {
            ViewBag.Filter = search;

            return View("Index","",await this.imamRepository.GetPageImam(search,1));
        }
        [CompressContent]
        [HttpGet]
        [Route("create")]
        public async Task<ActionResult> Create()
        {
            return View(new Imam());
        }
        [CompressContent]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(Imam model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            await this.imamRepository.Create(model);
            return RedirectToAction("Index");
        }
        [CompressContent]
        [HttpGet]
        [Route("edit/id")]
        public async Task<ActionResult> Edit(int id)
        {
            var model = await this.imamRepository.FindById(id);

            if(model==null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [CompressContent]
        [HttpPost]
        [Route("edit/id")]
        public async Task<ActionResult> Edit(int id, Imam model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            await this.imamRepository.Edit(model,id);

            return RedirectToAction("Index");
        }
        [CompressContent]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.imamRepository.FindById(id);

            if(model == null)
            {
                return HttpNotFound();
            }

            await this.imamRepository.Delete(model);
            return RedirectToAction("Index");
        }
    }
}