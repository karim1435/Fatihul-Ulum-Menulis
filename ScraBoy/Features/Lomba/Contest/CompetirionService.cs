using PagedList;
using ScraBoy.Features.CMS.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Lomba.Contest
{
    public class CompetitionService
    {
        private readonly int pageSize = 10;
        private readonly ICompetitionRepositroy competitionRepositroy;
        private readonly ModelStateDictionary modelState;
        public CompetitionService(ICompetitionRepositroy repo,ModelStateDictionary modelState)
        {
            this.competitionRepositroy = repo;
            this.modelState = modelState;
        }
        public async Task<IEnumerable<Competition>> GetAllContest()
        {
            return await this.competitionRepositroy.GetAll();
        }
        public async Task<IEnumerable<Competition>> GetCompetition(string name)
        {
            var model = await this.competitionRepositroy.GetAll();

            if(!string.IsNullOrEmpty(name))
            {
                return model.Where(a => a.Title.ToLower().Contains(name.ToLower()));
            }
            return model;  
        }
        public async Task<IPagedList<Competition>> GetPagedListContest(string name, int currentPage)
        {
            var model = await GetCompetition(name);

            return model.OrderByDescending(a=>a.StartedOn).ToPagedList(currentPage,pageSize);       
        }
        public async Task<IEnumerable<Competition>> GetContestHomePage()
        {
            var model = await this.competitionRepositroy.GetAll();
            return model.OrderByDescending(a => a.StartedOn);
        }
        public async Task<IEnumerable<Competition>> GetContestByAuthor(string id)
        {
            var model = await this.competitionRepositroy.GetAll();

            var query = from article in model
                        where article.Participants.Any(c => c.AuthorId == id)
                        select article;

            return query.OrderByDescending(a=>a.StartedOn);
        }
        public async Task<IPagedList<Competition>> GetPagedListByAuthor(string userId,int currentPage)
        {
            var model = await this.GetContestByAuthor(userId);
            return model.ToPagedList(currentPage,pageSize);
        }
        public async Task<IEnumerable<Competition>> GetPopularContest()
        {
            var model = await this.competitionRepositroy.GetAll();
            return model.Where(c => c.TotalParticipants > 0 && c.StatusContest.Equals(StatusContest.Open)).OrderByDescending(a => a.TotalParticipants);
        }
        public async Task<IEnumerable<Competition>> GetRecentContest()
        {
            var model = await this.competitionRepositroy.GetAll();
            return model.Where(a=>a.StatusContest.Equals(StatusContest.Open)).OrderByDescending(a => a.StartedOn);
        }
        public async Task<bool> CreateContest(Competition model)
        {
            if(!modelState.IsValid)
                return false;
            if(model.MaximumWord < model.MinimumWord)
            {
                modelState.AddModelError(string.Empty,"Maximum can't less than minumum word");
                return false;
            }
            if(model.EndOn < model.StartedOn)
            {
                modelState.AddModelError(string.Empty,"end date can't less than start date");
                return false;
            }

            await this.competitionRepositroy.Create(model);
            return true;
        }
        public async Task<bool> EditContest(string slugUrl,Competition model)
        {
            var contest = await this.competitionRepositroy.GetByUrl(slugUrl);

            if(!modelState.IsValid)
                return false;

            if(model.MaximumWord < model.MinimumWord)
            {
                modelState.AddModelError(string.Empty,"Maximum can't less than minumum word");
                return false;
            }
            if(model.EndOn < model.StartedOn)
            {
                modelState.AddModelError(string.Empty,"end date can't less than start date");
                return false;
            }

            await this.competitionRepositroy.Edit(slugUrl,model);

            return true;
        }
    }
}