using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.Data;
using ScraBoy.Features.Hadist.Bab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Hadist.Book
{
    [RoutePrefix("hadis")]
    public class KitabController : Controller
    {
        private readonly KitabRepository kitabRepository;
        public KitabController()
        {
            kitabRepository = new KitabRepository();
        }
        [Route("")]
        [CompressContent]
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);
            var model = await this.kitabRepository.GetPagedListKitab(currentFilter,pageNumber);
            return View("Index","",model);
        }
        [CompressContent]
        public async Task<ActionResult> Search(string search)
        {
            ViewBag.Filter = search;

            return View("Index","",await this.kitabRepository.GetPagedListKitab(search,1));
        }
        [Route("FindByChapter/{chapterId}")]
        [CompressContent]
        public async Task<ActionResult> FindByChapter(int chapterId,int? page,string currentFilter)
        {
            ViewBag.chapterNumber = chapterId;

            int pageNumber = (page ?? 1);

            var model = await this.kitabRepository.GetPageByChapter(currentFilter,chapterId,pageNumber);

            return View("Chapter","",model);
        }
        [Route("SearchByChapter/{chapterId}")]
        [CompressContent]
        public async Task<ActionResult> SearchByChapter(int chapterId,string search)
        {
            ViewBag.Filter = search;

            ViewBag.chapterNumber = chapterId;

            var model = await this.kitabRepository.GetPageByChapter(search,chapterId,1);

            return View("Chapter","",model);
        }
        
        [CompressContent]
        [Authorize]
        public async Task<ActionResult> Scrap(string url)
        {
            if(!string.IsNullOrEmpty(url))
            {
                await kitabRepository.GetDataFromWeb(url);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("create")]
        [CompressContent]
        public async Task<ActionResult> Create()
        {
            await SetViewBag();
            return View(new Kitab());
        }
        [HttpPost]
        [Route("create")]
        [CompressContent]
        public async Task<ActionResult> Create(Kitab model)
        {
            await SetViewBag();

            if(!ModelState.IsValid)
            {
                await SetViewBag();
                return View(model);
            }
            await this.kitabRepository.Create(model);
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("edit/{id}")]
        [CompressContent]
        public async Task<ActionResult> Edit(int id)
        {
            await SetViewBag(); 
            var model = await this.kitabRepository.GetById(id);

            if(model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [Route("edit/{id}")]
        [CompressContent]
        public async Task<ActionResult> Edit(int id,Kitab model)
        {
            await SetViewBag();
            if(!ModelState.IsValid)
            {
                await SetViewBag();
                return View(model);
            }
            await this.kitabRepository.Edit(id,model);

            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.kitabRepository.GetById(id);

            if(model == null)
            {
                return HttpNotFound();
            }

            await this.kitabRepository.Delete(model);
            return RedirectToAction("Index");
        }
        public async Task SetViewBag()
        {
            IChapterRepository chapterRepo = new ChapterRepository();
            ViewBag.chapter = await chapterRepo.GetAll();

            using(var db = new CMSContext())
            {
                ViewBag.imam = db.Imam.ToList();
            }

        }
    }
}