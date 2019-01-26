using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.Data;
using ScraBoy.Features.Hadist.Bab;
using ScraBoy.Features.Hadist.Hadis;
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
        private readonly IKitabRepository kitabRepository;
        private readonly ImamRerpository imamRepository;
        private readonly IChapterRepository chapterRepository;
        public KitabController()
        {
            this.imamRepository = new ImamRepository();
            kitabRepository = new KitabRepository();
            chapterRepository = new ChapterRepository();
        }
        [Route("byhadis/{imam}")]
        [CompressContent]
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
        [Route("FindByChapter/{imam}/{chapterId}")]
        [CompressContent]
        public async Task<ActionResult> FindByChapter(string imam,string chapterId,int? page,string currentFilter)
        {
            var imamModel = await SetImamInfo(imam);

            var chapterModel = await SetChapterInfo(chapterId);

            if(imamModel == null || chapterModel == null)
            {
                return HttpNotFound();
            }

            int pageNumber = (page ?? 1);

            var model = await this.kitabRepository.GetPageByChapter(imam,currentFilter,chapterId,pageNumber);

            return View("Chapter","",model);
        }

        [Route("SearchByChapter/{imam}/{chapterId}")]
        [CompressContent]
        public async Task<ActionResult> SearchByChapter(string imam,string chapterId,string search)
        {
            var imamModel = await SetImamInfo(imam);

            var chapterModel = await SetChapterInfo(chapterId);

            if(imamModel == null || chapterModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Filter = search;

            var model = await this.kitabRepository.GetPageByChapter(imam,search,chapterId,1);

            return View("Chapter","",model);
        }

        [Authorize]
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
            return await Redirect();
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
            ViewBag.ChapterUrl= TempData["chapterSlugUrl"].ToString();

            return RedirectToAction("FindByChapter","Kitab",new { imam = ViewBag.ImamUrl,chapterId = ViewBag.ChapterUrl });
        }
        public async Task SetViewBag()
        {
            ViewBag.chapter = await chapterRepository.GetAll();
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

            ViewBag.chapterName = chapterModel.Name;

            return chapterModel;
        }
    }
}