using Microsoft.AspNet.Identity;
using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.Lomba.Contest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Lomba.Audience
{
    [RoutePrefix("joincontest")]
    [Authorize]
    public class ParticipantController : Controller
    {
        private ICompetitionRepositroy competitionRepository;
        private IParticipantRepository participantRepository;
        private readonly ParticipantService participantService;
        public ParticipantController()
        {
            this.participantRepository = new ParticipantRepository();
            competitionRepository = new CompetitionRepository();
            this.participantService = new ParticipantService(participantRepository,ModelState);
        }
        [Authorize(Roles = "admin")]
        [Route("contestParticipants/{slugUrl}")]
        [CompressContent]
        public async Task<ActionResult> Index(string slugUrl,int? page,string currentFilter)
        {
            ViewBag.slugUrl = slugUrl;

            var pageNumber = (page ?? 1);

            var model = await this.participantService.GetParticipantPagedList(slugUrl,currentFilter,pageNumber);

            return View("Index","",model);
        }
        [Route("Search/{slugUrl}")]
        [CompressContent]
        public async Task<ViewResult> Search(string slugUrl, string search)
        {
            ViewBag.Filter = search;
            ViewBag.slugUrl = slugUrl;

            return View("Index","",await this.participantService.GetParticipantPagedList(slugUrl,search,1));
        }
        [HttpGet]
        [Route("join/{slugUrl}")]
        [CompressContent]
        public async Task<ActionResult> Join(string slugUrl)
        {
            var contest = await this.competitionRepository.GetByUrl(slugUrl);

            if(contest == null)
            {
                return HttpNotFound();
            }

            ViewBag.contest = contest;

            return View();
        }
        [HttpPost]
        [Route("join/{slugUrl}")]
        [CompressContent]
        public async Task<ActionResult> Join(Participant model,string slugUrl)
        {
            var contest = await this.competitionRepository.GetByUrl(slugUrl);

            ViewBag.contest = contest;

            model.AuthorId = UserId;

            var result = await this.participantService.JoinContest(slugUrl,model);

            if(!result)
            {
                return View(model);
            }
            return RedirectToAction("MyContest","Competition");
        }
        [HttpGet]
        [Route("Review/{Id}")]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> Review(int id)
        {
            var model = await participantRepository.GetOne(id);

            if(model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        [HttpPost]
        [Route("Review/{Id}")]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> Review(int id,Participant model)
        {
            var participant = await participantRepository.GetOne(id);

            var result = await this.participantService.Review(id,model);

            if(!result)
            {
                return View(participant);
            }
            return RedirectToAction("Index","Participant",new { slugUrl = participant.Competition.SlugUrl });
        }
        [HttpGet]
        [Route("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await participantRepository.GetOne(id);

            if(model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Route("delete/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(int id,string foo)
        {
            var model = await participantRepository.GetOne(id);

            await participantRepository.Delete(id);

            return RedirectToAction("Index");

        }
        [HttpGet]
        [Route("ViewContest/{slugUrl}")]
        public async Task<ActionResult> ViewContest(string slugUrl)
        {
            var contest = await this.competitionRepository.GetByUrl(slugUrl);

            ViewBag.mycontest = contest;

            var model = await this.participantRepository.GetParticipantByAuthor(slugUrl,UserId);
            if(model==null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        public string UserId
        {
            get { return User.Identity.GetUserId(); }
        }
    }
}