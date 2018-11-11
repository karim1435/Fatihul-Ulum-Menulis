using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScraBoy.Features.Data;

namespace ScraBoy.Features.Creator
{
    public class CreatorController : Controller
    {
        private CreatorService creatorService;
        public CreatorController()
        {
            creatorService = new CreatorService();
        }
        public ActionResult Index()
        {
            return View(creatorService.GetAllCreator());
        }
    }
}
