using ScraBoy.Features.CMS.HomeBlog;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using PagedList;

namespace ScraBoy.Features.CMS.Comments
{
    public class CommentRepository : ICommentRepository
    {
        private readonly int pageSize = 10;
        private readonly CMSContext db;
        public CommentRepository()
        {
            db = new CMSContext();
        }
        public async Task<IEnumerable<Comment>> GetAllCommentsAsync()
        {
            using(var db = new CMSContext())
            {
                return await db.Comment.Include("Post").OrderByDescending(a=>a.PostedOn).ToArrayAsync();
            }
        }
        public async Task ReplyAsync(Comment model)
        {
            using(var db = new CMSContext())
            {
                Comment comment = new Comment();

                comment.Content = model.Content;
                comment.UserId = model.UserId;
                comment.PostId = model.PostId;
                comment.PostedOn = DateTime.Now;
                comment.ParentId = model.ParentId;

                db.Comment.Add(comment);
                await db.SaveChangesAsync();
            }
        }
        public async Task CreateAsync(Comment model)
        {
            using(var db = new CMSContext())
            {
                model.PostedOn = DateTime.Now;

                db.Comment.Add(model);
                await db.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Comment>> GetCommentByPostIdAsync(string postId)
        {
            return await db.Comment.Where(a => a.PostId == postId).
                OrderByDescending(a=>a.PostedOn).
                ToArrayAsync();
        }
        public Comment GetParentComment(int id)
        {
            return db.Comment.Where(a => a.Id == id).FirstOrDefault();
        }
        public async Task<Comment> GetCommentById(int id)
        {
            return await db.Comment.Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        public IQueryable<Comment> GetComments(string name)
        {
            if(!string.IsNullOrWhiteSpace(name))
            {
                return this.db.Comment.Include("User").Include("Post").Where(a => a.User.UserName.Contains(name) || 
                a.Post.Title.Contains(name));
            }
            return this.db.Comment.Include("User").Include("Post");
        }
        public List<Comment> GetCommentList(string name)
        {
            return GetComments(name).OrderByDescending(a => a.PostedOn).ToList();
        }
        
        public async Task DeleteCommentAsync(Comment comment)
        {
            using(var db = new CMSContext())
            {
                if(comment.Respond.Count()>0)
                {
                    foreach(var item in comment.Respond.ToList())
                    {
                        await DeleteCommentAsync(item);
                    }
                }
                else
                {
                    this.db.Comment.Remove(comment);
                }
                this.db.Comment.Remove(comment);
            }
            
            await this.db.SaveChangesAsync();
        }
        public async Task EditAsync(Comment model,int commentId)
        {
            var comment = await GetCommentById(commentId);

            comment.Content = model.Content;

            await db.SaveChangesAsync();
        }
        public IPagedList<Comment> GetPagedList(string search,int page, string userId)
        {
            var model = new List<Comment>();

            if(userId==null)
            {
                model = GetCommentList(search).ToList();
            }
            else
            {
                model = GetCommentList(search).Where(post => post.Post.AuthorId.Equals(userId)).ToList();
            }

            return model.ToPagedList(page,pageSize);
        }
    }

}