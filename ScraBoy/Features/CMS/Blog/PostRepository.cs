using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using ScraBoy.Features.CMS.Topic;

namespace ScraBoy.Features.CMS.Blog
{
    public class PostRepository : IPostRepository
    {
        private CMSContext db;

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

        public async Task<Post> GetAsync(string id)
        {
            return await db.Post.Include("Author")
                .SingleOrDefaultAsync(post => post.Id == id);
        }


        public void Edit(string id,Post updatedItem)
        {
            var post = db.Post.SingleOrDefault(p => p.Id == id);

            if(post == null)
            {
                throw new KeyNotFoundException("A post with the id of " + id +
                    "does not exist in the data store.");
            }

            post.Id = updatedItem.Id;
            post.Title = updatedItem.Title;
            post.Content = updatedItem.Content;
            post.Published = updatedItem.Published;
            post.Tags = updatedItem.Tags;
            db.SaveChanges();
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await db.Post.Include("Author").OrderByDescending(post => post.Created).ToArrayAsync();
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

        public async Task<IEnumerable<Post>> GetPostByTagAsync(string tag)
        {
            var posts = await db.Post
                .Include("Author")
                .Where(post => post.CombinedTags.Contains(tag))
                .ToListAsync();

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
        public async Task<IEnumerable<Post>> GetPostByCategories(string category)
        {
            return await db.Post.Where(a => a.Category.Name.Equals(category)).ToArrayAsync();
        }
        public async Task<IEnumerable<string>> GetAllCategories()
        {
            return await db.Post.Select(a => a.Category.Name).ToListAsync();
        }
    }
}