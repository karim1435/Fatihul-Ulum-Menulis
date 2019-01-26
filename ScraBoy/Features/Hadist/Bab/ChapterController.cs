using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.Hadist.Hadis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Hadist.Bab
{
    [RoutePrefix("chapter")]
    public class ChapterController : Controller
    {
        private readonly IChapterRepository chapterRepository;
        private readonly ImamRerpository imamRepository;
        public ChapterController(IChapterRepository chapterRepository,ImamRerpository imamRepository)
        {
            this.chapterRepository = chapterRepository;
            this.imamRepository = imamRepository;
        }
        public ChapterController() : this(new ChapterRepository(),new ImamRepository())
        {

        }
        [Route("hadist/{imam}")]
        [CompressContent]
        public async Task<ActionResult> Index(string imam,int? page,string currentFilter)
        {
            var imamModel = await SetInfo(imam);

            if(imamModel == null)
            {
                return HttpNotFound();
            }

            int pageNumber = (page ?? 1);

            var model = await this.chapterRepository.GetPageChapter(currentFilter,imam,pageNumber);

            return View("Index","",model);
        }
   
        [Route("SearchByImam/{imam}")]
        [CompressContent]
        public async Task<ActionResult> Search(string imam,string search)
        {
            var imamModel = await SetInfo(imam);

            if(imamModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Filter = search;

            return View("Index","",await this.chapterRepository.GetPageChapter(search,imam,1));
        }
        [Route("create")]
        [HttpGet]
        [CompressContent]
        public async Task<ActionResult> Create()
        {
            await SetViewBag();

            return View(new Chapter());
        }
        [Route("create")]
        [HttpPost]
        [CompressContent]
        public async Task<ActionResult> Create(Chapter model)
        {
            await SetViewBag();

            if(!ModelState.IsValid)
            {
                await SetViewBag();
                return View(model);
            }

            await this.chapterRepository.Create(model);

            return await Redirect();
        }
        [Route("edit/{id}")]
        [HttpGet]
        [CompressContent]
        public async Task<ActionResult> Edit(int id)
        {
            await SetViewBag();

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
            await SetViewBag();
            if(!ModelState.IsValid)
            {
                await SetViewBag();
                return View(model);
            }
            await this.chapterRepository.Edit(model,id);

            return await Redirect();
        }
        [CompressContent]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.chapterRepository.GetOne(id);

            if(model == null)
            {
                return HttpNotFound();
            }
            await this.chapterRepository.Delete(model);

            return await Redirect();
        }
        public async Task<ActionResult> Redirect()
        {
            ViewBag.Message = TempData["shortMessage"].ToString();
            return RedirectToAction("Index","Chapter",new { imam = ViewBag.Message });
        }
        public async Task<Imam> SetInfo(string imam)
        {
            var imamModel = await this.imamRepository.FindBySlug(imam);

            ViewBag.hadisTitle = imamModel.Name;

            TempData["shortMessage"] = imamModel.SlugUrl;

            ViewBag.imamName = imamModel.SlugUrl;

            return imamModel;
        }
        public async Task SetViewBag()
        {
            ImamRerpository imamRepo = new ImamRepository();
            var imams = await imamRepo.GetAll();

            ViewBag.imam = imams.ToList();
        }
    }
}