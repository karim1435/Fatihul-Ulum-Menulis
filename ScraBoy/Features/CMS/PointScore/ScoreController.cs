using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.PointScore
{
    [RoutePrefix("Score")]
    [Authorize(Roles = "admin")]
    public class ScoreController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IUserScoreRepository userScoreRepository;
        public ScoreController(IUserRepository userRepository,IUserScoreRepository userScoreRepostory)
        {
            this.userRepository = userRepository;
            this.userScoreRepository = userScoreRepostory;
        }
        public ScoreController() : this(new UserRepository(),new UserScoreRepository())
        {
        }

        [Route("history/{userId}")]
        [CompressContent]
        public async Task<ActionResult> History(string userId)
        {
            var user = await userRepository.GetUserById(userId);

            if(user == null)
            {
                return HttpNotFound();
            }

            var trackScore = await userScoreRepository.TrackScoreByUser(user.Id);

            return View(trackScore);
        }

        [HttpGet]
        [Route("AddScore/{userId}")]
        [CompressContent]
        public async Task<ActionResult> AddScore(string userId)
        {
            var user = await userRepository.GetUserById(userId);

            if(user == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        [Route("AddScore/{userId}")]
        [CompressContent]
        public async Task<ActionResult> AddScore(string userId,UserScore model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userRepository.GetUserById(userId);

            model.AuthorId = user.Id;

            await this.userScoreRepository.Create(model);

            return RedirectToAction("History","Score",new { userId = user.Id });
        }

        [HttpGet]
        [Route("Edit/{id}")]
        [CompressContent]
        public async Task<ActionResult> Edit(int id)
        {
            var score =await this.userScoreRepository.FindOne(id);

            if(score == null)
            {
                return HttpNotFound();
            }
            return View(score);
        }
        [HttpPost]
        [Route("Edit/{id}")]
        [CompressContent]
        public async Task<ActionResult> Edit(int id,UserScore model)
        {
            var score =await this.userScoreRepository.FindOne(id);

            if(!ModelState.IsValid)
            {
                return View(model);
            }

            await this.userScoreRepository.Edit(model);

            return RedirectToAction("History","Score",new { userId = score.AuthorId });

        }

        [HttpGet]
        [Route("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var score = await this.userScoreRepository.FindOne(id);

            if(score == null)
            {
                return HttpNotFound();
            }

            return View(score);
        }

        [HttpPost]
        [Route("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id,string foo)
        {
            var score = await this.userScoreRepository.FindOne(id);

            await this.userScoreRepository.Delete(score);

            return RedirectToAction("Index");
        }
    }
}