using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Lomba.Contest
{
    public class CompetitionRepository : ICompetitionRepositroy
    {
        private readonly CMSContext db;

        public CompetitionRepository()
        {
            db = new CMSContext();
        }
        public async Task Create(Competition model)
        {
            model.CreatedOn = DateTime.Now;
            model.UpdatedOn = DateTime.Now;

            model.SlugUrl = DateTime.Now.ToString("yymmddss")+"-"+StringExtensions.MakeUrlFriednly(model.Title);

            this.db.Competiton.Add(model);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(string slugIrl)
        {
            var contest = await GetByUrl(slugIrl);
            this.db.Competiton.Remove(contest);
            await this.db.SaveChangesAsync();
        }

        public async Task Edit(string slugurl,Competition model)
        {
            var contest = await this.GetByUrl(slugurl);

            contest.UrlImage = model.UrlImage;
            contest.Description = model.Description;
            contest.MaximumWord = model.MaximumWord;
            contest.MinimumWord = model.MinimumWord;
            contest.Title = model.Title;
            contest.Reward = model.Reward;
            contest.StartedOn = model.StartedOn;
            contest.EndOn = model.EndOn;
            contest.StatusContest = model.StatusContest;
            contest.CategoryId = model.CategoryId;
            contest.SlugUrl = StringExtensions.MakeUrlFriednly(model.Title);

            await this.db.SaveChangesAsync();
        }
        //https://docs.microsoft.com/en-us/ef/ef6/querying/related-data
        public async Task<IEnumerable<Competition>> GetAll()
        {
            return await this.db.Competiton.Include(a => a.Participants.Select(post=> post.Author)).Include("Category").Include("Creator").ToArrayAsync();
        }

        public async Task<Competition> GetByUrl(string slugUrl)
        {
            var model = await this.GetAll();
            return model.Where(a => a.SlugUrl.Equals(slugUrl)).FirstOrDefault();
        }
    }
}