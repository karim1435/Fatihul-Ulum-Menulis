using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.Comments;
using ScraBoy.Features.CMS.Interest;
using ScraBoy.Features.CMS.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PagedList;
using ScraBoy.Features.Data;
using ScraBoy.Features.Utility;
using ScraBoy.Features.CMS.Topic;
using ScraBoy.Features.CMS.User;
using System.Data.Entity;
namespace ScraBoy.Features.CMS.HomeBlog
{
    public class BlogService
    {
        private readonly int pageSize = 15;
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commenRepository;
        private readonly IVotingRepository voteRepository;
        public readonly ITagRepostory tagRepoistory;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;
        public BlogService() : this(
            new PostRepository(),
            new CommentRepository(),
            new VotingRepository(),
            new TagRepository(),
            new CategoryRepository(),
            new UserRepository())
        {

        }
        public BlogService(
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IVotingRepository voteRepository,
            ITagRepostory tagRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.commenRepository = commentRepository;
            this.voteRepository = voteRepository;
            this.tagRepoistory = tagRepository;
            this.categoryRepository = categoryRepository;
            this.userRepository = userRepository;
        }
        private List<Voting> GetALLVoting(string userId)
        {
            var model = voteRepository.GetAllVoting();
            return model.Where(post => post.Post.AuthorId.Equals(userId) && post.UserId != userId).ToList();
        }
        public List<Comment> GetAllComment(string userId)
        {
            var model = commenRepository.GetAllComment();
            return model.Where(post => post.Post.AuthorId.Equals(userId) && post.UserId != userId).ToList();
        }
        public IEnumerable<NotificationViewModel> GetNotification(string userId)
        {
            var notifications = ((from like in GetALLVoting(userId)
                                  select new NotificationViewModel
                                  {
                                      Post = like.Post,
                                      User = like.User,
                                      PostedOn = like.PostedOn,
                                      NotificationType = NotificationType.Voting
                                  })
                                .Union
                                (from comment in GetAllComment(userId)
                                 select new NotificationViewModel
                                 {
                                     Parent = comment.Parent,
                                     Post = comment.Post,
                                     User = comment.User,
                                     NotificationType = NotificationType.Comment,
                                     PostedOn = comment.PostedOn,
                                 })).OrderByDescending(a => a.PostedOn).ToList();

            return notifications;
        }
        public IPagedList<NotificationViewModel> GetPagedListInfo(int page,string userId)
        {
            var notification = GetNotification(userId).ToList();
            return notification.ToPagedList(page,10);
        }
        public IPagedList<Post> GetPagedList(string postType,string search,string tagId,string catId,int currentPage)
        {
            var model = new List<Post>();


            model = this.postRepository.GetBlogList(search,tagId,catId);

            if(postType.Equals("latestpost") || postType.Equals(""))
            {
                model = model.OrderByDescending(a => a.Published).ToList();
            }
            else if(postType.Equals("mostpopularpost"))
            {
                model = model.Where(a => a.TotalViews > 0).
                    OrderByDescending(a => a.TotalViews).ToList();
            }
            else if(postType.Equals("mosstinterestpost"))
            {
                model = model.Where(a => a.TotalVote > 0).
                    OrderByDescending(a => a.TotalVote).ToList();
            }
            else if(postType.Equals("mostdiscusspost"))
            {
                model = model.Where(a => a.TotalComment > 0).
                    OrderByDescending(a => a.TotalComment).ToList();
            }

            return model.Where(a=>!a.Private).ToPagedList(currentPage,pageSize);
        }
        public async Task<IEnumerable<Post>> GetAllPost()
        {
            var model = await this.postRepository.GetAllAsync();
            return model.Where(a => a.Private == false);
        }
        public async Task<IEnumerable<Comment>> GetRecentCommentsAsycn()
        {
            var comments = await this.commenRepository.GetAllCommentsAsync();
            return comments.OrderByDescending(a => a.PostedOn);
        }
        public async Task<IEnumerable<Post>> RelatedPosts(string postId,int categoryId)
        {
            var post = await GetAllPost();
            return post.Where(a => a.CategoryId == categoryId && a.Id != postId);
        }
        public async Task<IEnumerable<RankingViewModel>> GetTopContributors()
        {
            var users = this.userRepository.GetAllUsersAsync();

            var topUsers = new List<RankingViewModel>();

            foreach(var user in users)
            {
                var model = new RankingViewModel();
                var posts = await postRepository.GetPostsByAuthorAsync(user.Id);

                model.User = user;
                model.TotalPublishedPost = posts.Count();
                model.TotalLikedPost = posts.Sum(post => (post.TotalVote));
                model.TotalViewedPost = posts.Sum(post => (post.TotalViews));
                model.TotalCommentPost = posts.Sum(post => (post.TotalComment));
                topUsers.Add(model);
            }
            return topUsers.Where(a => a.Point > 0).OrderByDescending(a => a.Point);
        }
        public IEnumerable<string> GetAllCategories()
        {
            var cat = this.categoryRepository.GetAllCategory();

            return cat.Where(a => a.Posts.Count() > 0).Select(a => a.Name).Distinct().ToList();
        }
    }
}