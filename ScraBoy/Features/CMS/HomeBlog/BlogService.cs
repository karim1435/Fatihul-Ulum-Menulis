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
        public BlogService() : this(
            new PostRepository(),
            new CommentRepository(),
            new VotingRepository(),
            new TagRepository(),
            new CategoryRepository())
        {

        }
        public BlogService(
            IPostRepository postRepository,
            ICommentRepository commentRepository,
            IVotingRepository voteRepository,
            ITagRepostory tagRepository,
            ICategoryRepository categoryRepository)
        {
            this.postRepository = postRepository;
            this.commenRepository = commentRepository;
            this.voteRepository = voteRepository;
            this.tagRepoistory = tagRepository;
            this.categoryRepository = categoryRepository;
        }
        private List<Voting> GetALLVoting(string userId)
        {
            using(var db = new CMSContext())
            {
                return db.Voting.Include("User").Include("Post").Where(a => a.LikeCount == true).
                    Where(post => post.Post.AuthorId.Equals(userId) && post.UserId != userId).ToList();
            }
        }
        public List<Comment> GetAllComment(string userId)
        {
            using(var db = new CMSContext())
            {
                return db.Comment.Include("User").Include("Post").Include("Parent").
                    Where(post => post.Post.AuthorId.Equals(userId) && post.UserId != userId).ToList();
            }
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
        public IPagedList<BlogViewModel> GetPagedList(string postType,string search,string tagId,string catId,int currentPage)
        {
            var model = new List<Post>();


            model = this.postRepository.GetBlogList(search,tagId,catId);

            var posts = new List<BlogViewModel>();

            if(postType.Equals("latestpost") || postType.Equals(""))
            {
                posts = GetBlogListViewModel(model).
                    OrderByDescending(a => a.Post.Published).ToList();
            }
            else if(postType.Equals("mostpopularpost"))
            {
                posts = GetBlogListViewModel(model).Where(a => a.ViewCount > 0).
                    OrderByDescending(a => a.ViewCount).ToList();
            }
            else if(postType.Equals("mosstinterestpost"))
            {
                posts = GetBlogListViewModel(model).Where(a => a.Voting.TotalLike > 0).
                    OrderByDescending(a => a.Voting.TotalLike).ToList();
            }
            else if(postType.Equals("mostdiscusspost"))
            {
                posts = GetBlogListViewModel(model).Where(a => a.TotalComment > 0).
                    OrderByDescending(a => a.TotalComment).ToList();
            }

            return posts.ToPagedList(currentPage,pageSize);
        }

        public async Task<IEnumerable<CommentViewModel>> GetPostCommentAsync(string posId)
        {
            var comments = await commenRepository.GetCommentByPostIdAsync(posId);

            return GetCommentViewModel(comments);
        }
        public async Task<IEnumerable<CommentViewModel>> GetRecentCommentsAsycn()
        {
            using(var db = new CMSContext())
            {
                var comments = db.Comment.Include("User").Include("Parent").OrderByDescending(a => a.PostedOn).ToList();

                return GetCommentViewModel(comments).ToList();
            }
        }

        public async Task<IEnumerable<BlogViewModel>> RelatedPosts(string postId,int categoryId)
        {
            var post = await this.postRepository.GetAllAsync();
            return GetBlogListViewModel(post).Where(a => a.Post.CategoryId == categoryId && a.Post.Id != postId);
        }
        public async Task<IEnumerable<BlogViewModel>> MostNewPosts()
        {
            var post = await this.postRepository.GetAllAsync();
            return GetBlogListViewModel(post).OrderByDescending(a => a.Post.Published);
        }
        public async Task<IEnumerable<BlogViewModel>> MostLiked()
        {
            var post = await this.postRepository.GetAllAsync();
            return GetBlogListViewModel(post).Where(a => a.Voting.TotalLike >= 1).OrderByDescending(a => a.Voting.TotalLike);
        }
        public async Task<IEnumerable<BlogViewModel>> SortByCommented()
        {
            var post = await this.postRepository.GetAllAsync();
            return GetBlogListViewModel(post).Where(a => a.TotalComment >= 1).OrderByDescending(a => a.TotalComment);
        }
        public async Task<IEnumerable<BlogViewModel>> GetPopularPostByView()
        {
            var post = await this.postRepository.GetAllAsync();
            return GetBlogListViewModel(post).Where(a => a.ViewCount >= 1).OrderByDescending(a => a.ViewCount);
        }
        private IEnumerable<CMSUser> GetAllUsers()
        {
            using(var db = new CMSContext())
            {
                return db.Users.ToList();
            }
        }
        public async Task<IEnumerable<RankingViewModel>> GetTopContributors()
        {
            var users = GetAllUsers();

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
            using(var db = new CMSContext())
            {
                var categories = db.Post.Include("Category").Where(a => a.Published < DateTime.Now && !a.Private).Select(a => a.Category.Name);
                return categories.ToList().Distinct();
            }
        }
        public async Task<List<string>> GetAllTags()
        {
            using(var db = new CMSContext())
            {
                var tagsList = db.Post.Where(a => a.Published < DateTime.Now && !a.Private).Select(p => p.CombinedTags).ToList();
                return string.Join(",",tagsList).Split(',').Distinct().ToList();
            }
        }

        public BlogViewModel GetBlogViewModel(Post post)
        {
            var blog = new BlogViewModel();

            blog.Post.Id = post.Id;
            blog.Post.Title = post.Title;
            blog.Post.Content = Formatter.FormatHtml(post.Content);
            blog.Post.CategoryId = post.CategoryId;
            blog.Post.Tags = post.Tags;
            blog.FullUrl = post.FullUrlPost;
            blog.User.UserName = post.Author.UserName;
            blog.User.Id = post.Author.Id;
            blog.User.SlugUrl = post.Author.SlugUrl;
            blog.Post.Private = post.Private;
            blog.User.UrlImage = post.Author.UrlImage;
            blog.User.DisplayName = post.Author.DisplayName;
            blog.Post.Created = post.Created;
            blog.Post.Published = post.Published;
            blog.Voting.LikedUser = this.voteRepository.UserLiked(blog.Post.Id);
            blog.Voting.TotalLike = post.TotalVote;
            blog.SideBarTags.Tags = tagRepoistory.GetAll().ToList();
            blog.Category.Name = post.Category.Name;
            blog.Post.UrlImage = post.UrlImage;
            blog.ViewCount = post.TotalViews;
            blog.TotalComment = post.TotalComment;

            return blog;
        }
        public IEnumerable<BlogViewModel> GetBlogListViewModel(IEnumerable<Post> posts)
        {
            var blogs = new List<BlogViewModel>();
            foreach(var post in posts)
            {
                var blog = new BlogViewModel();

                blog.Post.Id = post.Id;
                blog.Post.Title = post.Title;
                blog.Post.Content = Formatter.FormatHtml(post.Content);
                blog.Post.Tags = post.Tags;
                blog.Post.CategoryId = post.CategoryId;
                blog.FullUrl = post.FullUrlPost;
                blog.User.UserName = post.Author.UserName;
                blog.User.DisplayName = post.Author.DisplayName;
                blog.User.Id = post.Author.Id;
                blog.Post.Private = post.Private;
                blog.User.UrlImage = post.Author.UrlImage;
                blog.User.SlugUrl = post.Author.SlugUrl;
                blog.Post.Created = post.Created;
                blog.Post.Published = post.Published;
                blog.Voting.LikedUser = this.voteRepository.UserLiked(blog.Post.Id);
                blog.Voting.TotalLike = post.TotalVote;
                blog.SideBarTags.Tags = tagRepoistory.GetAll().ToList();
                blog.Post.UrlImage = post.UrlImage;
                blog.Category.Name = post.Category.Name;
                blog.ViewCount = post.TotalViews;
                blog.TotalComment = post.TotalComment;

                blogs.Add(blog);
            }
            return blogs.Where(a => a.Post.Published < DateTime.Now && !a.Post.Private);
        }
        public IEnumerable<CommentViewModel> GetCommentViewModel(IEnumerable<Comment> comments)
        {
            var blogComments = new List<CommentViewModel>();

            foreach(var item in comments)
            {
                var model = new CommentViewModel();
                model.Comment.Id = item.Id;
                model.Comment.PostedOn = item.PostedOn;
                model.Post.Title = item.Post.Title;
                model.Post.AuthorId = item.Post.AuthorId;

                if(item.ParentId.HasValue)
                {
                    model.Comment.Parent = commenRepository.GetParentComment(item.ParentId.Value);

                }
                model.Comment.UserId = item.UserId;
                model.User.UserName = item.User.UserName;
                model.User.Id = item.User.Id;
                model.User.UrlImage = item.User.UrlImage;
                model.User.SlugUrl = item.User.SlugUrl;
                model.User.DisplayName = item.User.DisplayName;
                model.Comment.Content = item.Content;
                model.Post.Id = item.Post.Id;
                blogComments.Add(model);
            }
            return blogComments.OrderBy(a => a.Comment.PostedOn);
        }
    }
}