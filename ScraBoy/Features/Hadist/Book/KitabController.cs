using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.Data;
using ScraBoy.Features.Hadist.Bab;
using ScraBoy.Features.Hadist.Hadis;
using ScraBoy.Features.Hadist.Meaning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Hadist.Book
{
    [RoutePrefix("hadis")]
    [Authorize(Roles = "admin,editor")]
    public class KitabController : Controller
    {
        private readonly ITranslationRepository translationRepository;
        private readonly IKitabRepository kitabRepository;
        private readonly IimamRepository imamRepository;
        private readonly IChapterRepository chapterRepository;
        public KitabController()
        {
            this.imamRepository = new ImamRepository();
            kitabRepository = new KitabRepository();
            chapterRepository = new ChapterRepository();
            this.translationRepository = new TranslationRepository();
        }
        [Route("byhadis/{imam}")]
        [CompressContent]
        [AllowAnonymous]
        public async Task<ActionResult> Index(string imam,int? page,string currentFilter)
        {
            var imamModel = await SetImamInfo(imam);

            if(imamModel == null)
            {
                return HttpNotFound();
            }

            int pageNumber = (page ?? 1);
            var model = await this.kitabRepository.GetPagedListKitab(imam,currentFilter,pageNumber);

            return View("Index","",model);
        }
        [CompressContent]
        [Route("SearchKitab/{imam}")]
        [AllowAnonymous]
        public async Task<ActionResult> Search(string imam,string search)
        {
            var imamModel = await SetImamInfo(imam);

            if(imamModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Filter = search;

            return View("Index","",await this.kitabRepository.GetPagedListKitab(imam,search,1));
        }
        [Route("FindByChapter/{imam}")]
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> FindByChapter(string code,int? number,string imam,string chapterId,int? page,string currentFilter)
        {
            ViewBag.Filter = currentFilter;
            ViewBag.number = number;
            var imamModel = await SetImamInfo(imam);

            var chapterModel = await SetChapterInfo(chapterId);

            if(imamModel == null || chapterModel == null)
            {
                return HttpNotFound();
            }
            await SetChapterViewBag(imam);

            int pageNumber = (page ?? 1);

            var model = await this.kitabRepository.GetPageByChapter("ID",number,imam,currentFilter,chapterId,pageNumber);

            return View("FindByChapter","",model);
        }

        [Route("SearchByChapter/{imam}")]
        [CompressContent]
        [AllowAnonymous]
        public async Task<ActionResult> SearchByChapter(string code,int? number,string imam,string chapterId,string search)
        {
            var imamModel = await SetImamInfo(imam);

            var chapterModel = await SetChapterInfo(chapterId);

            await SetChapterViewBag(imam);

            if(imamModel == null || chapterModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Filter = search;

            ViewBag.Number = number;

            var model = await this.kitabRepository.GetPageByChapter(code,number,imam,search,chapterId,1);

            return View("FindByChapter","",model);
        }
        [HttpGet]
        [Route("Scrap/{chapterId}")]
        [CompressContent]
        public async Task<ActionResult> Scrap(string chapterId,int from,int to)
        {
            var chapter = await this.chapterRepository.FindBySlug(chapterId);

            if(chapter == null)
            {
                return HttpNotFound();
            }
            await kitabRepository.GetDataFromWeb(chapter.Id,from,to);

            return await Redirect();
        }
        [HttpGet]
        [Route("create/{imam}")]
        [CompressContent]
        public async Task<ActionResult> Create(string imam)
        {
            await SetViewBag(imam);
            return View();
        }
        [HttpPost]
        [Route("create/{imam}")]
        [CompressContent]
        public async Task<ActionResult> Create(string imam,Kitab model)
        {
            await SetViewBag(imam);

            if(!ModelState.IsValid)
            {
                await SetViewBag(imam);
                return View(model);
            }
            await this.kitabRepository.Create(model);
            return await Redirect();
        }
        [HttpGet]
        [Route("edit/{imam}/{id}")]
        [CompressContent]
        public async Task<ActionResult> Edit(string imam,int id)
        {
            await SetViewBag(imam);

            var model = await this.kitabRepository.GetById(id);

            if(model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [Route("edit/{imam}/{id}")]
        [CompressContent]
        public async Task<ActionResult> Edit(string imam,int id,Kitab model)
        {
            await SetViewBag(imam);
            if(!ModelState.IsValid)
            {
                await SetViewBag(imam);
                return View(model);
            }
            await this.kitabRepository.Edit(id,model);

            return await Redirect();
        }
        [CompressContent]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.kitabRepository.GetById(id);

            if(model == null)
            {
                return HttpNotFound();
            }
            await this.kitabRepository.Delete(model);

            return await Redirect();
        }
        public async Task<ActionResult> Redirect()
        {
            ViewBag.ImamUrl = TempData["imamSlugUrl"].ToString();
            ViewBag.ChapterUrl = TempData["chapterSlugUrl"].ToString();

            return RedirectToAction("FindByChapter","Kitab",new { imam = ViewBag.ImamUrl,chapterId = ViewBag.ChapterUrl });
        }
        public async Task SetViewBag(string imam)
        {
            ViewBag.chapter = await chapterRepository.FindByImam(imam);
        }
        public async Task<Imam> SetImamInfo(string imam)
        {
            var imamModel = await this.imamRepository.FindBySlug(imam);

            ViewBag.hadisTitle = imamModel.Name;

            TempData["imamSlugUrl"] = imamModel.SlugUrl;

            ViewBag.imamName = imamModel.SlugUrl;

            return imamModel;
        }
        public async Task<Chapter> SetChapterInfo(string chapterId)
        {
            var chapterModel = await this.chapterRepository.FindBySlug(chapterId);

            ViewBag.chapterNumber = chapterModel.SlugUrl;

            TempData["chapterSlugUrl"] = chapterModel.SlugUrl;

            string chapterTitle = chapterModel.Name;
            string chapterLimit = "";
            if(chapterModel.Kitabs.Count() > 0)
                chapterLimit = chapterModel.LimitNumber;

            ViewBag.chapterName = chapterTitle + " " + chapterLimit;

            return chapterModel;
        }
        public async Task SetChapterViewBag(string imam)
        {
            var model = await this.chapterRepository.FindByImam(imam);
            var language = await this.translationRepository.GetAllBahasa();
            ViewBag.chapters = model;
            ViewBag.language = language;
        }
    }
}