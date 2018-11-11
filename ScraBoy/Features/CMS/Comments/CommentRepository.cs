using ScraBoy.Features.CMS.HomeBlog;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Comments
{
    public class CommentRepository : ICommentRepository
    {
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
        public async Task CreateAsync(BlogViewModel model)
        {
            using(var db = new CMSContext())
            {
                Comment comment = new Comment();

                comment.Content = model.NewComment;
                comment.UserId = model.UserId;
                comment.PostId = model.PostId;
                comment.PostedOn = DateTime.Now;

                db.Comment.Add(comment);
                await db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Comment>> GetCommentByPostIdAsync(string postId)
        {
            return await db.Comment.Where(a => a.PostId == postId).
                OrderByDescending(a=>a.PostedOn).
                ToArrayAsync();
        }
    }

}