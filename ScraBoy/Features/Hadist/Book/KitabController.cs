using ScraBoy.Features.CMS.Gzip;
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
        public async Task<ActionResult> Chapter()
        {
            var model = await this.kitabRepository.GetAllChapter();
            return View(model);
        }
        [Route("FindByChapter")]
        public async Task<ActionResult> FindByChapter(int chapterId)
        {
            var model = await this.kitabRepository.FindByChapter(chapterId);
            return View(model);
        }
        [CompressContent]
        public async Task<ViewResult> Search(string search)
        {
            ViewBag.Filter = search;

            return View("Index","",await this.kitabRepository.GetPagedListKitab(search,1));
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
    }
}