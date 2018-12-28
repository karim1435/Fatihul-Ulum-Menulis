using Microsoft.AspNet.Identity;
using PagedList;
using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.Lomba.Contest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.Lomba.Audience
{
    public class ParticipantService
    {
        private readonly int pageSize = 10;
        private readonly IParticipantRepository participantRepository;
        private readonly ICompetitionRepositroy competitionRepositroy;
        private readonly ModelStateDictionary modelState;
        public ParticipantService(IParticipantRepository repo,ModelStateDictionary modelState)
        {
            competitionRepositroy = new CompetitionRepository();
            this.participantRepository = repo;
            this.modelState = modelState;
            
        }
        
        public async Task<IEnumerable<Participant>> GetParticipantList(string slugUrl, string name)
        {
            var model =await this.participantRepository.GetByContest(slugUrl);

            if(!string.IsNullOrEmpty(name))
            {
                return model.Where(a => a.Title.ToLower().Contains(name.ToLower()));
            }

            return model;
        }
        public async Task<IPagedList<Participant>> GetParticipantPagedList(string slugUrl, string name,int currentPage)
        {
            var model = await this.GetParticipantList(slugUrl,name);

            return model.ToPagedList(currentPage,pageSize);
        }
        public async Task<bool> JoinContest(string slugUrl,Participant model)
        {
            var contest = await competitionRepositroy.GetByUrl(slugUrl);

            if(!modelState.IsValid)
                return false;

            var userHasJoin = await this.participantRepository.UserHasSubmitted(model.AuthorId,slugUrl);

            if(userHasJoin)
            {
                modelState.AddModelError(string.Empty,"Sorry, Kamu tidak bisa submit lebih dari satu kali");
                return false;
            }
            if(contest.StartedOn>DateTime.Now)
            {
                modelState.AddModelError(string.Empty,"Sorry, Contest ini belum dimulai");
                return false;
            }
            if(contest.EndOn<=DateTime.Now)
            {
                modelState.AddModelError(string.Empty,"Sorry, Contest ini sudah Berakhir");
                return false;
            }
            if(!contest.StatusContest.Equals(StatusContest.Open))
            {
                modelState.AddModelError(string.Empty,"Sorry, Contest ini sudah Berakhir");
                return false;
            }
            var content = model.Content.ReadMore(model.Content.Length);

            int contentLenth = content.CountTotalWords();

            if(contentLenth < contest.MinimumWord)
            {
                modelState.AddModelError(string.Empty,"Conten harus lebih dari " + contest.MinimumWord + " Kata");
                return false;
            }
            if(contentLenth > contest.MaximumWord)
            {
                modelState.AddModelError(string.Empty,"Content harus kurang dari " + contest.MaximumWord + " Kata");
                return false;
            }

            model.CompetitionId = contest.Id;
            model.ProposedOn = DateTime.Now;
            await this.participantRepository.Join(model);
            return true;
        }
        public async Task<bool> Review(int id,Participant model)
        {
            var post = await participantRepository.GetOne(id);

            if(model.Score < 0)
            {
                modelState.AddModelError(string.Empty,"Score must be greater than zero");
                return false;
            }
            if(string.IsNullOrWhiteSpace(model.Message))
            {
                modelState.AddModelError(string.Empty,"Please Fill the message");
                return false;
            }
            post.Score = model.Score;
            post.Message = model.Message;
            await this.participantRepository.SaveChanges();
            return true;
        }
    }
}