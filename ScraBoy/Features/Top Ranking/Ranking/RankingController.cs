using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Ranking
{
    public class RankingController : Controller
    {
        private RankingServices rankingService;
        public RankingController()
        {
            rankingService = new RankingServices();
        }
        public ViewResult Search(string name, string creator, string start, string end)
        {
            return View("Index",this.rankingService.GetAllRanking(name,creator,start,end));
        }
       
        public ActionResult Scrap(string url)
        {
            if(!string.IsNullOrEmpty(url))
                rankingService.GetDataFromPlaystore(url);

            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            return View(rankingService.GetListRanking());
        }
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var rankingModel = rankingService.FindById(id);
            if(rankingModel == null)
            {
                return HttpNotFound();
            }
            return View(rankingModel);
        }

    }
}
