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
    [Authorize(Roles = "admin")]
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
        public async Task<ActionResult> Index()
        {
            var comments = await commentRepository.GetAllCommentsAsync();
            return View(comments);
        }
    }
}