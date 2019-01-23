using ScraBoy.Features.CMS.Gzip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Hadist.Bab
{
    [RoutePrefix("chapterhadist")]
    public class ChapterController : Controller
    {
        private readonly IChapterRepository chapterRepository;
        public ChapterController(IChapterRepository chapterRepository)
        {
            this.chapterRepository = chapterRepository;
        }
        public ChapterController() : this(new ChapterRepository())
        {

        }
        [Route("")]
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = page ?? 1;
            var model = await this.chapterRepository.GetPageChapter(currentFilter,pageNumber);
            return View(model);
        }
        [CompressContent]
        public async Task<ActionResult> Search(string search)
        {
            ViewBag.Filter = search;

            return View("Index","",await this.chapterRepository.GetPageChapter(search,1));
        }
        [Route("create")]
        [HttpGet]
        [CompressContent]
        public async Task<ActionResult> Create()
        {
            return View(new Chapter());
        }
        [Route("create")]
        [HttpPost]
        [CompressContent]
        public async Task<ActionResult> Create(Chapter model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            await this.chapterRepository.Create(model);
            return RedirectToAction("Index");
        }
        [Route("edit/{id}")]
        [HttpGet]
        [CompressContent]
        public async Task<ActionResult> Edit(int id)
        {
            var model = await this.chapterRepository.GetOne(id);

            if(model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [Route("edit/{id}")]
        [HttpPost]
        [CompressContent]
        public async Task<ActionResult> Edit(int id,Chapter model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            await this.chapterRepository.Edit(model,id);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.chapterRepository.GetOne(id);

            if(model == null)
            {
                return HttpNotFound();
            }
            await this.chapterRepository.Delete(model);
            return RedirectToAction("Index");
        }
    }
}