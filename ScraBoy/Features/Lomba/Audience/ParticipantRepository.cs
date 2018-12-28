using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Lomba.Audience
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly CMSContext db;
        public ParticipantRepository()
        {
            db = new CMSContext();
        }

        public async Task Delete(int id)
        {
            var model = await this.GetOne(id);

            this.db.Participant.Remove(model);

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Participant>> GetAll()
        {
            return await this.db.Participant.Include("Competition").Include("Author").ToArrayAsync();
        }

        public async Task<IEnumerable<Participant>> GetByContest(string url)
        {
            var model = await GetAll();
            return model.Where(a => a.Competition.SlugUrl.Equals(url)).OrderByDescending(a=>a.ProposedOn);
        }
        public async Task<Participant> GetParticipantByAuthor(string url,string userId)
        {
            var model = await GetByContest(url);
            return model.Where(a => a.AuthorId == userId).FirstOrDefault();
        }
        public async Task<Participant> GetOne(int id)
        {
            var model = await GetAll();
            return model.Where(a => a.Id == id).FirstOrDefault();
        }

        public async Task Join(Participant model)
        {
            this.db.Participant.Add(model);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> UserHasSubmitted(string userId,string slugUrl)
        {
            var contest = this.db.Participant.Where(a => a.Competition.SlugUrl == slugUrl);
            return await contest.Where(a => a.AuthorId == userId).CountAsync() > 0 ;
        }
    }
}