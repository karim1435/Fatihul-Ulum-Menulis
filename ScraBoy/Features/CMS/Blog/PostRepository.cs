using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ScraBoy.Features.CMS.Topic;
using PagedList;
using System.Web;
using ScraBoy.Features.CMS.ModelBinders;

namespace ScraBoy.Features.CMS.Blog
{
    public class PostRepository : IPostRepository
    {
        private CMSContext db;
        private readonly int pageSize = 10;
        private string viewId;
        public PostRepository(CMSContext db)
        {
            this.db = db;

        }
        public PostRepository() : this(new CMSContext())
        {
        }
        public int CountPublished
        {
            get
            {

                return db.Post.Where(P => P.Published < DateTime.Now).Count();
            }
        }
        public async Task UpdateViewCount(string postId)
        {
            var view = await this.db.ViewPost.SingleOrDefaultAsync(a => a.ViewId == viewId &&
            a.PostId==postId);

            if(view != null)
            {
                return;
            }

            ViewPost viewPost = new ViewPost()
            {
                PostId = postId,
                ViewId = viewId,
                Count = 1,
                LastViewed = DateTime.Now

            };
            db.ViewPost.Add(viewPost);

            await this.db.SaveChangesAsync();
        }
        public void GetCookieView(HttpContextBase http)
        {
            var cookie = http.Request.Cookies.Get("ViewSinglePost");
            var cookieId = string.Empty;

            if(cookie == null || string.IsNullOrWhiteSpace(cookie.Value))
            {
                cookie = new HttpCookie("ViewSinglePost");
                cookieId = Guid.NewGuid().ToString();

                cookie.Value = cookieId;
                cookie.Expires = DateTime.Now.AddDays(365);

                http.Response.Cookies.Add(cookie);
            }
            else
            {
                cookieId = cookie.Value;
            }

            viewId = cookieId;
        }
        public async Task<IEnumerable<Post>> SortByViews()
        {
            return await this.db.Post.OrderByDescending(a => a.TotalViews).ToArrayAsync();
        }
        public async Task<Post> GetAsync(string id)
        {
            return await db.Post.Include("Author")
                .SingleOrDefaultAsync(post => post.Id == id);
        }

        public IQueryable<Post> GetBlogs(string name,string tagId,string categoryId)
        {
            if(!string.IsNullOrEmpty(name))
            {
                return this.db.Post.Where(m => m.Title.Contains(name) || 
                m.Content.Contains(name) || 
                m.Author.UserName.Contains(name));
            }
            if(!string.IsNullOrEmpty(tagId))
            {
                return GetPostByTagAsync(tagId).AsQueryable();
            }
            if(!string.IsNullOrEmpty(categoryId))
            {
                return GetPostByCategories(categoryId).AsQueryable();
            }
            return this.db.Post;
        }

        public List<Post> GetBlogList(string name,string tagId,string categoryId)
        {
            return this.GetBlogs(name,tagId,categoryId).OrderByDescending(a => a.Created).ToList();
        }

        public IQueryable<Post> GetPosts(string name)
        {
            if(!string.IsNullOrEmpty(name))
            {
                return this.db.Post.Where(m => m.Title.Contains(name));
            }
            return this.db.Post;
        }

        public List<Post> GetPostList(string name)
        {
            return this.GetPosts(name).OrderByDescending(a => a.Created).ToList();
        }
        public IPagedList<Post> GetPagedList(string search,int currentPage,string userId)
        {
            var model = new List<Post>();

            if(userId == null)
            {
                model = GetBlogList(search,"","");
            }
            else
            {
                model = GetBlogList(search,"","").Where(a => a.AuthorId == userId).ToList();
            }
            model = model.OrderByDescending(s => s.Created).ToList();
            return model.ToPagedList(currentPage,pageSize);
        }
        public void Edit(string id,Post updatedItem)
        {
            var post = db.Post.SingleOrDefault(p => p.Id == id);

            if(post == null)
            {
                throw new KeyNotFoundException("A post with the id of " + id +
                    "does not exist in the data store.");
            }

            post.Title = updatedItem.Title;
            post.Content = updatedItem.Content;
            post.Published = updatedItem.Published;
            post.Tags = updatedItem.Tags;
            post.UrlImage = updatedItem.UrlImage;
            post.CategoryId = updatedItem.CategoryId;
            post.Updated = DateTime.Now;

            db.SaveChanges();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await db.Post.Include("Author").ToArrayAsync();
        }
        public async Task Create(Post model)
        {
            var post = await db.Post.SingleOrDefaultAsync(p => p.Id == model.Id);

            if(post != null)
            {
                throw new ArgumentException("A post with the id of " + model.Id + " Already exist");
            }

            db.Post.Add(model);
            await db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(string authorId)
        {
            return await db.Post.Include("Author").Where(p => p.AuthorId == authorId).
                OrderByDescending(post => post.Created).ToArrayAsync();
        }
        public void Delete(string id)
        {
            var post = db.Post.SingleOrDefault(p => p.Id == id);

            if(post == null)
            {
                throw new KeyNotFoundException("A post with the id of " + id +
                    "does not exist in the data store.");
            }

            db.Post.Remove(post);
            db.SaveChanges();
        }

        public async Task<IEnumerable<Post>> GetPublishedPostAsync()
        {
            return await db.Post.Include("Author").
                Where(p => p.Published < DateTime.Now).OrderByDescending(p => p.Published).
                ToArrayAsync();
        }

        public IEnumerable<Post> GetPostByTagAsync(string tag)
        {
            var posts = db.Post
                .Include("Author")
                .Where(post => post.CombinedTags.Contains(tag))
                .ToList();

            return posts.Where(post =>
                post.Tags.Contains(tag,StringComparer.CurrentCultureIgnoreCase))
                .ToList();
        }

        public async Task<IEnumerable<Post>> GetPageAsync(int pageNumber,int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;

            return await db.Post.Where(p => p.Published < DateTime.Now).
                Include("Author").
                OrderByDescending(p => p.Published).
                Skip(skip).
                Take(pageSize).
                ToArrayAsync();
        }
        public  IEnumerable<Post> GetPostByCategories(string category)
        {
            return  db.Post.Where(a => a.Category.Name.Equals(category)).ToList();
        }
        public async Task<IEnumerable<string>> GetAllCategories()
        {
            return await db.Post.Select(a => a.Category.Name).ToListAsync();
        }

    }
}