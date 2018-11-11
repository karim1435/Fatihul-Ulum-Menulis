using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ScraBoy.Features.Data;

namespace ScraBoy.Features.Game
{
    public class GameController : Controller
    {
        private GameService gameService;
        public GameController()
        {
            gameService = new GameService();
        }
        public ActionResult Index()
        {
            return View(gameService.GetAllGame());
        }

    }
}
