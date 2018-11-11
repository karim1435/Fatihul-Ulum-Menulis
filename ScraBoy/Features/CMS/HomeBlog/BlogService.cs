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

namespace ScraBoy.Features.CMS.HomeBlog
{
    public class BlogService
    {
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commenRepository;
        private readonly IVotingRepository voteRepository;
        public readonly ITagRepostory tagRepoistory;
        public BlogService():this(new PostRepository(), 
            new CommentRepository(), 
            new VotingRepository(), 
            new TagRepository())
        {

        }
        public BlogService(IPostRepository postRepository, 
            ICommentRepository commentRepository, 
            IVotingRepository voteRepository,ITagRepostory tagRepository)
        {
            this.postRepository = postRepository;
            this.commenRepository = commentRepository;
            this.voteRepository = voteRepository;
            this.tagRepoistory = tagRepository;
        }
    
    
        public async Task<IEnumerable<BlogViewModel>> GetBlogs()
        {
            var posts = await this.postRepository.GetAllAsync();
            
            return  await GetBlogListViewModel(posts);
            
        }
        public async Task<IEnumerable<BlogViewModel>> GetPageBlogAsync(int pageNumber, int pageSize)
        {
            var blogs = await this.postRepository.GetPageAsync(pageNumber,pageSize);

            return await GetBlogListViewModel(blogs);
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
        public IEnumerable<CommentViewModel> GetCommentViewModel(IEnumerable<Comment> comments)
        {
            var blogComments = new List<CommentViewModel>();

            foreach(var item in comments)
            {
                var model = new CommentViewModel();

                model.Author = item.User.DisplayName;
                model.Comment = item.Content;
                blogComments.Add(model);
            }
            return blogComments;
        }
        public async Task<List<string>> GetAllTags ()
        {
            return tagRepoistory.GetAll().ToList();

        }
        public async Task<BlogViewModel> GetBlogViewModel(Post post)
        {
            var blog = new BlogViewModel();

            blog.PostId = post.Id;
            blog.Title = post.Title;
            blog.Content = post.Content;
            blog.PostTags.Tags = post.Tags;
            blog.Author = post.Author.DisplayName;
            blog.Created = post.Created;
            blog.Published = post.Published;
            blog.Voting= await this.voteRepository.GetVotedPostUser(blog.PostId);
            blog.SideBarTags.Tags = tagRepoistory.GetAll().ToList();
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
                blog.Content = post.Content;
                blog.PostTags.Tags = post.Tags;
                blog.Author = post.Author.DisplayName;
                blog.Created = post.Created;
                blog.Published = post.Published;
                blog.Voting = await this.voteRepository.GetVotedPostUser(blog.PostId);
                blog.SideBarTags.Tags = tagRepoistory.GetAll().ToList();
                blog.UrlImage = post.UrlImage;
                blogs.Add(blog);
            }
            return blogs;
        }
        
    }
}