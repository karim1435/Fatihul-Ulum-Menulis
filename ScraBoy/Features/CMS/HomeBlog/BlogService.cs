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

namespace ScraBoy.Features.CMS.HomeBlog
{
    public class BlogService
    {
        private readonly int showLimit = 5;
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
        public async Task<IEnumerable<BlogViewModel>> GetPageBlogAsync(int pageNumber,int pageSize)
        {
            var blogs = await this.postRepository.GetPageAsync(pageNumber,pageSize);

            return  GetBlogListViewModel(blogs).OrderByDescending(a=>a.Post.Published);
        }
        public async Task<IEnumerable<BlogViewModel>> GetBlogByCategoryAsync(string catName)
        {
            var blog = await this.postRepository.GetPostByCategories(catName);

            return GetBlogListViewModel(blog);
        }
        public async Task<IEnumerable<BlogViewModel>> GetBlogByTagAsync(string tagId)
        {
            var blogs = await postRepository.GetPostByTagAsync(tagId);

            return GetBlogListViewModel(blogs);
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
                var comments = db.Comment.Include("User").OrderByDescending(a => a.PostedOn).ToList();

                return GetCommentViewModel(comments).ToList().Take(showLimit);
            }
        }
        public async Task<IEnumerable<BlogViewModel>> SortByCommented()
        {
            var post = await this.postRepository.GetAllAsync();
            return GetBlogListViewModel(post).OrderByDescending(a => a.TotalComment).Take(showLimit);
        }
        public async Task<IEnumerable<BlogViewModel>> GetPopularPostByView()
        {
            var post = await this.postRepository.GetAllAsync();
            return GetBlogListViewModel(post).OrderByDescending(a=>a.ViewCount).Take(showLimit); ;
        }
        public async Task<IEnumerable<string>> GetAllCategories()
        {
            using(var db = new CMSContext())
            {
                var categories = db.Post.Include("Category").Select(a => a.Category.Name);
                return categories.ToList().Distinct();
            }
        }
        public async Task<List<string>> GetAllTags()
        {
            return tagRepoistory.GetAll().ToList();
        }

        public BlogViewModel GetBlogViewModel(Post post)
        {
            var blog = new BlogViewModel();

            blog.Post.Id = post.Id;
            blog.Post.Title= post.Title;
            blog.Post.Content= Formatter.FormatHtml(post.Content);
            blog.Post.Tags = post.Tags;
            blog.User.UserName = post.Author.UserName;
            blog.Post.Created = post.Created;
            blog.Post.Published= post.Published;
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
                blog.Post.Content= Formatter.FormatHtml(post.Content);
                blog.Post.Tags = post.Tags;
                blog.User.UserName= post.Author.UserName;
                blog.Post.Created = post.Created;
                blog.Post.Published = post.Published;
                blog.Voting.LikedUser = this.voteRepository.UserLiked(blog.Post.Id);
                blog.Voting.TotalLike = post.TotalVote;
                blog.SideBarTags.Tags = tagRepoistory.GetAll().ToList();
                blog.Post.UrlImage= post.UrlImage;
                blog.Category.Name = post.Category.Name;
                blog.ViewCount = post.TotalViews;
                blog.TotalComment = post.TotalComment;

                blogs.Add(blog);
            }
            return blogs;
        }
        public IEnumerable<CommentViewModel> GetCommentViewModel(IEnumerable<Comment> comments)
        {

            var blogComments = new List<CommentViewModel>();

            foreach(var item in comments)
            {
                var model = new CommentViewModel();

                model.Comment.PostedOn = item.PostedOn;
                model.User.UserName = item.User.UserName;
                model.Comment.Content = item.Content;
                model.Post.Id = item.Post.Id;
                blogComments.Add(model);
            }
            return blogComments;
        }
    }
}