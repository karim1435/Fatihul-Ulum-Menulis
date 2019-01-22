using Microsoft.AspNet.Identity;
using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.Upload;
using ScraBoy.Features.Data;
using ScraBoy.Features.Lomba.Audience;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Lomba.Contest
{
    [RoutePrefix("Contest")]

    public class CompetitionController : UploadController
    {
        private readonly string pathFolder = "~/Image/contest/";
        CompetitionService competitionService;
        ICompetitionRepositroy competititionRepository;
        ICategoryRepository categoryRepository;
        private IParticipantRepository participantRepository;
        public CompetitionController()
        {
            participantRepository = new ParticipantRepository();
            categoryRepository = new CategoryRepository();
            competititionRepository = new CompetitionRepository();
            competitionService = new CompetitionService(competititionRepository,ModelState);
        }
        [Route("")]
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);
            var contests = await this.competitionService.GetPagedListContest(currentFilter,pageNumber);

            return View("Index","",contests);
        }
        [AllowAnonymous]
        [CompressContent]
        public async Task<ViewResult> Search(string search)
        {
            ViewBag.Filter = search;

            return View("Index","",await this.competitionService.GetPagedListContest(search,1));
        }
        [Route("ContestList")]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> ContestList(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);
            var contests = await this.competitionService.GetPagedListContest(currentFilter,pageNumber);

            return View("ContestList","",contests);
        }
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ViewResult> SearchContest(string search)
        {
            ViewBag.Filter = search;

            return View("ContestList","",await this.competitionService.GetPagedListContest(search,1));
        }
        [Route("mycontest")]
        [Authorize]
        [CompressContent]
        public async Task<ActionResult> MyContest(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);

            var contest = await this.competitionService.GetPagedListByAuthor(UserId,pageNumber);

            return View("MyContest","",contest);
        }
        [HttpGet]
        [Route("newcontest")]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> Create()
        {
            await SetViewBag();
            return View(new Competition());
        }
        [HttpPost]
        [Route("newcontest")]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> Create(Competition model)
        {
            await SetViewBag();

            if(model.ImageFile == null)
            {
                ModelState.AddModelError(String.Empty,"Please Upload image to continue");
                await SetViewBag();
                return View(model);
            }

            var filePath = GetFullFile(model.ImageFile.FileName);

            if(!CheckFileType(filePath))
            {
                ModelState.AddModelError(string.Empty,"Upload Image with JPEG, JPG OR PNG Extension");
                await SetViewBag();
                return View(model);
            }

            model.UrlImage = pathFolder + filePath;
            model.CreatorId = UserId;

            var result = await this.competitionService.CreateContest(model);

            if(!result)
            {
                await SetViewBag();
                return View(model);
            }

            SaveImage(model.ImageFile,pathFolder,filePath);
            return RedirectToAction("ContestList");
        }
        [HttpGet]
        [Route("editcontest/{slugUrl}")]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> Edit(string slugUrl)
        {
            await SetViewBag();
            var model = await this.competititionRepository.GetByUrl(slugUrl);

            if(model == null)
                return HttpNotFound();

            return View(model);
        }
        [HttpPost]
        [Route("editcontest/{slugUrl}")]
        [Authorize(Roles = "admin")]
        [CompressContent]
        public async Task<ActionResult> Edit(string slugUrl,Competition model)
        {
            var contest = await this.competititionRepository.GetByUrl(slugUrl);

            if(contest == null)
                return HttpNotFound();

            model.CreatorId = UserId;

            if(model.ImageFile != null)
            {
                DeleteOldImage(contest.UrlImage);

                var filePath = GetFullFile(model.ImageFile.FileName);

                if(!CheckFileType(filePath))
                {
                    ModelState.AddModelError(string.Empty,"Upload Image with JPEG, JPG OR PNG Extension");
                    await SetViewBag();
                    return View(model);
                }

                SaveImage(model.ImageFile,pathFolder,filePath);

                model.UrlImage = pathFolder + filePath;
            }
            else
            {
                model.UrlImage = contest.UrlImage;
            }

            var result = await this.competitionService.EditContest(slugUrl,model);
            if(!result)
            {
                await SetViewBag();
                return View(model);
            }

            return RedirectToAction("ContestList");
        }

        [HttpGet]
        [Route("fucontest/{slugUrl}")]
        [AllowAnonymous]
        [CompressContent]
        public async Task<ActionResult> View(string slugUrl)
        {
            await SetViewBag();

            ViewBag.submit = await this.participantRepository.UserHasSubmitted(UserId,slugUrl);

            var model = await this.competititionRepository.GetByUrl(slugUrl);

            if(model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }


        [HttpGet]
        [Route("delete/{slugUrl}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string slugUrl)
        {
            var contest = await competititionRepository.GetByUrl(slugUrl);

            if(contest == null)
            {
                return HttpNotFound();
            }

            return View(contest);
        }

        [HttpPost]
        [Route("delete/{slugUrl}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Delete(string slugUrl,string foo)
        {
            var contest = await competititionRepository.GetByUrl(slugUrl);
            if(contest == null)
            {
                return HttpNotFound();
            }

            DeleteOldImage(contest.UrlImage);

            await competititionRepository.Delete(slugUrl);

            return RedirectToAction("Index");
        }
        public string UserId
        {
            get { return User.Identity.GetUserId(); }
        }

        public async Task SetViewBag()
        {
            ViewBag.popular = await this.competitionService.GetPopularContest();
            ViewBag.recent = await this.competitionService.GetRecentContest();
            ViewBag.events = await categoryRepository.GetAllCategoriesAsync();
        }
    }
}