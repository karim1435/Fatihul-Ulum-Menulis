using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Collections;

namespace ScraBoy.Features.CMS.PointScore
{
    public class UserScoreRepository : IUserScoreRepository
    {
        private readonly CMSContext db;

        public UserScoreRepository()
        {
            db = new CMSContext();
        }

        public async Task Create(UserScore model)
        {
            model.Created = DateTime.Now;
            model.Updated = DateTime.Now;
            this.db.UserScore.Add(model);

            await this.db.SaveChangesAsync();
        }
        public async Task<UserScore> FindOne(int id)
        {
            return await this.db.UserScore.Where(a => a.Id == id).FirstOrDefaultAsync();
        }
        public async Task Edit(UserScore model)
        {
            var score = await this.FindOne(model.Id);

            score.Updated = DateTime.Now;
            score.Score = model.Score;
            score.Note = model.Note;
            await this.db.SaveChangesAsync();
        }
        public async Task Delete(UserScore model)
        {
            this.db.UserScore.Remove(model);
            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserScore>> TrackScoreByUser(string userId)
        {
            return await this.db.UserScore.Include("Author").
                Where(a => a.AuthorId.Equals(userId)).OrderByDescending(a=>a.Created).ToArrayAsync();
        }
    }
}