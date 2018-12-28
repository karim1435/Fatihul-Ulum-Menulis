using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.Lomba.Audience
{
    public interface IParticipantRepository
    {
        Task<IEnumerable<Participant>> GetAll();
        Task<IEnumerable<Participant>> GetByContest(string url);
        Task Join(Participant model);
        Task<bool> UserHasSubmitted(string userId,string contestUrl);
        Task<Participant> GetOne(int id);
        Task SaveChanges();
        Task Delete(int id);
        Task<Participant> GetParticipantByAuthor(string url,string userId);
    }
}