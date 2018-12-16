using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Reporting
{
    [RoutePrefix("Violation")]
    [Authorize(Roles = "admin")]

    public class ViolationController :Controller
    {
        private readonly IViolationRepository violationRepository;
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;
        public ViolationController(IPostRepository postRepo,IUserRepository userRepo, IViolationRepository violationRepository)
        {
            this.userRepository = userRepo;
            this.postRepository = postRepo;
            this.violationRepository = violationRepository;
        }
        public ViolationController():this(new PostRepository(), new UserRepository(), new ViolationRepository())
        {

        }

        [Route("")]
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);

            return View("Index","",this.violationRepository.GetPagedList(currentFilter,pageNumber));
        }
        public async Task<ActionResult> Search(string search)
        {
            ViewBag.Filter = search;

            return View("Index","",this.violationRepository.GetPagedList(search,1));
        }
        [HttpGet]
        [Route("CreateReport/postId")]
        public async Task<ActionResult> CreateReport(string postId)
        {
            var post = await postRepository.GetAsync(postId);
            

            if(post==null)
            {
                return HttpNotFound();
            }
            ViewBag.post = "\"" + post.Title + "\"";
            return View();
        }
        [HttpPost]
        [Route("CreateReport/postId")]
        public async Task<ActionResult> CreateReport(Violation model,string postId)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var post = await postRepository.GetAsync(postId);


            if(post == null)
            {
                return HttpNotFound();
            }
            var user = await GetLoggedInUser();

            model.UserId = user.Id;
            model.PostId = postId;

            await violationRepository.CreateAsync(model);

            return RedirectToAction("Post","HomeBlog",new { postId = postId });
        }

        public async Task<ActionResult> Decline(int id)
        {
            var report = await this.violationRepository.GetReportAsync(id);

            if(report == null)
            {
                return HttpNotFound();
            }
            await violationRepository.Delete(report);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Accept(int id,string postId)
        {
            var post = await this.postRepository.GetAsync(postId);
            var report = await this.violationRepository.GetReportAsync(id);

            if(post==null||report==null)
            {
                return HttpNotFound();
            }

            await violationRepository.Delete(report);
            postRepository.Delete(post.Id);
            return RedirectToAction("Index");
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }
    }

}
