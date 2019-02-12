using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.Data;
using ScraBoy.Features.Hadist.Book;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Hadist.Meaning
{
    [Route("Translation")]
    [Authorize(Roles = "admin,editor")]
    public class TranslationController : Controller
    {
        private readonly ITranslationRepository translationRepository;
        private readonly IKitabRepository kitabRepository;
        private readonly TranslationService translationService;
        public TranslationController()
        {
            this.translationRepository = new TranslationRepository();
            this.kitabRepository = new KitabRepository();
            this.translationService = new TranslationService(ModelState,translationRepository);
        }
        [Route("TranslationList/{imam}/{hadistNumber}")]
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> Index(string imam,int hadistNumber)
        {
            var hadist = await this.kitabRepository.FindByNumberImam(imam,hadistNumber);

            ViewBag.imamName = hadist.Chapter.Imam.SlugUrl;
            ViewBag.hadisTitle = hadist.Chapter.Imam.Name;
            ViewBag.chapterName = hadist.Chapter.Name;
            ViewBag.original = hadist;

            var model = await this.translationService.GetTranslationByHadist(hadist.Id,imam);

            if(hadist == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpGet]
        [CompressContent]
        [Route("AddTranslation/{imam}/{hadistNumber}")]
        public async Task<ActionResult> AddTranslation(string imam,int hadistNumber)
        {
            await SetViewBag();

            var hadist = await this.kitabRepository.FindByNumberImam(imam,hadistNumber);

            if(hadist == null)
            {
                return HttpNotFound();
            }
            ViewBag.original = hadist;

            return View();
        }
        [HttpPost]
        [CompressContent]
        [Route("AddTranslation/{imam}/{hadistNumber}")]
        public async Task<ActionResult> AddTranslation(string imam,int hadistNumber,Translation model)
        {
            await SetViewBag();

            var hadist = await this.kitabRepository.FindByNumberImam(imam,hadistNumber);

            model.KitabId = hadist.Id;

            ViewBag.original = hadist;

            var result = await this.translationService.AddTranslation(model);

            if(!result)
            {
                await SetViewBag();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("EditTranslation/{imam}/{Id}")]
        [CompressContent]
        public async Task<ActionResult> EditTranslation(string imam,int id)
        {
            await SetViewBag();

            var model = await this.translationRepository.FindById(id);

            var hadist = await this.kitabRepository.FindByNumberImam(imam,model.Kitab.Number);

            ViewBag.original = hadist;

            if(model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }
        [HttpPost]
        [Route("EditTranslation/{imam}/{Id}")]
        [CompressContent]
        public async Task<ActionResult> EditTranslation(string imam,int id,Translation model)
        {
            await SetViewBag();

            var translation = await this.translationRepository.FindById(id);

            var hadist = await this.kitabRepository.FindByNumberImam(imam,translation.Kitab.Number);

            ViewBag.original = hadist;

            var result = await this.translationService.UpdateTranslation(model,id);

            if(!result)
            {
                await SetViewBag();
                return View(model);
            }
            return RedirectToAction("Index",new { hadistNumber = translation.Kitab.Number });
        }
        [Route("Delete/{imam}/{id}")]
        [CompressContent]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await this.translationRepository.FindById(id);

            int kitabId = model.Kitab.Number;

            if(model == null)
            {
                return HttpNotFound();
            }
            var result = await this.translationService.RemoveTranslation(model);

            if(!result)
            {
                ModelState.AddModelError(string.Empty,"Something Wrong Happened");
            }
            return RedirectToAction("Index",new { hadistNumber = kitabId });
        }
        public async Task SetViewBag()
        {
            using(var db = new CMSContext())
            {
                ViewBag.languages = db.Language.ToList();
            }
        }
    }
}