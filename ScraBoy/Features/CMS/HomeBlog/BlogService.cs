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
        public async Task<IEnumerable<BlogViewModel>> GetBlogs()
        {
            var posts = await this.postRepository.GetAllAsync();

            return await GetBlogListViewModel(posts);

        }
        public async Task<IEnumerable<BlogViewModel>> GetPageBlogAsync(int pageNumber,int pageSize)
        {
            var blogs = await this.postRepository.GetPageAsync(pageNumber,pageSize);

            return await GetBlogListViewModel(blogs);
        }
        public async Task<IEnumerable<BlogViewModel>> GetBlogByCategoryAsync(string catName)
        {
            var blog = await this.postRepository.GetPostByCategories(catName);

            return await GetBlogListViewModel(blog);
        }
        public async Task<BlogViewModel> GetBlogAsync(string postId)
        {
            var blog = await postRepository.GetAsync(postId);

            return await GetBlogViewModel(blog);
        }
        public async Task<IEnumerable<BlogViewModel>> GetBlogByTagAsync(string tagId)
        {
            var blogs = await postRepository.GetPostByTagAsync(tagId);

            return await GetBlogListViewModel(blogs);
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
                var comments = db.Comment.Include("User").OrderByDescending(a=>a.PostedOn).ToList();

                return GetCommentViewModel(comments).ToList();
            }
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
 
        public async Task<BlogViewModel> GetBlogViewModel(Post post)
        {
            var blog = new BlogViewModel();

            blog.PostId = post.Id;
            blog.Title = post.Title;
            blog.Content = Formatter.FormatHtml(post.Content);
            blog.PostTags.Tags = post.Tags;
            blog.Author = post.Author.DisplayName;
            blog.Created = post.Created;
            blog.Published = post.Published;
            blog.Voting = await this.voteRepository.GetVotedPostUser(blog.PostId);
            blog.SideBarTags.Tags = tagRepoistory.GetAll().ToList();
            blog.PostCategories.Names = post.Category.Name;
            blog.UrlImage = post.UrlImage;
            return blog;
        }

        public async Task<IEnumerable<BlogViewModel>> GetBlogListViewModel(IEnumerable<Post> posts)
        {
            var blogs = new List<BlogViewModel>();
            foreach(var post in posts)
            {
                var blog = new BlogViewModel();

                blog.PostId = post.Id;
                blog.Title = post.Title;
                blog.Content = Formatter.FormatHtml(post.Content);
                blog.PostTags.Tags = post.Tags;
                blog.Author = post.Author.DisplayName;
                blog.Created = post.Created;
                blog.Published = post.Published;
                blog.Voting = await this.voteRepository.GetVotedPostUser(blog.PostId);
                blog.SideBarTags.Tags = tagRepoistory.GetAll().ToList();
                blog.UrlImage = post.UrlImage;
                blog.PostCategories.Names = post.Category.Name;
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

                model.PostedOn = item.PostedOn;
                model.Author = item.User.DisplayName;
                model.Comment = item.Content;
                model.Post = item.Post.Id;
                blogComments.Add(model);
            }
            return blogComments;
        }
    }
}