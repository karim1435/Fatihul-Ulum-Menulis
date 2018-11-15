using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Comments
{

    [RoutePrefix("comment")]
    [Authorize]
    public class CommentController : Controller
    {
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IUserRepository userRepository;
        public CommentController(IPostRepository postRepo, ICommentRepository commentRepo,IUserRepository userRepo)
        {
            this.userRepository = userRepo;
            this.postRepository = postRepo;
            this.commentRepository = commentRepo;
        }
        public CommentController():this(new PostRepository(), new CommentRepository(), new UserRepository())
        {

        }


        [Route("")]
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);

            if(!User.IsInRole("author"))
            {
                return View("Index","",this.commentRepository.GetPagedList(currentFilter,pageNumber,null));
            }

            var user = await GetLoggedInUser();

            return View("Index","",this.commentRepository.GetPagedList(currentFilter,pageNumber,user.Id));

        }
        public async Task<ViewResult> Search(string search)
        {
            ViewBag.Filter = search;

            if(!User.IsInRole("author"))
            {
                return View("Index","",this.commentRepository.GetPagedList(search,1,null));
            }

            var user = await GetLoggedInUser();

            return View("Index","",this.commentRepository.GetPagedList(search,1,user.Id));
        }
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var comment = await commentRepository.GetCommentById(id);
            if(comment==null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        [HttpPost]
        [Route("delete/{id}")]
        //test is fake
        public async Task<ActionResult> Delete(int id,string test)
        {
            var comment = await commentRepository.GetCommentById(id);

            try
            {
                await this.commentRepository.DeleteCommentAsync(comment);
                return RedirectToAction("Index");
            }
            catch(KeyNotFoundException e)
            {
                return HttpNotFound();
            }
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }

    }
}