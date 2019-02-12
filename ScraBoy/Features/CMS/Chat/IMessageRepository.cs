using ScraBoy.Features.CMS.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ScraBoy.Features.CMS.Chat
{
    public interface IMessageRepository
    {
        Task Create(Massage model);
        Task Edit(int id,Massage model);
        Task<IEnumerable<Massage>> GetAllChat(string fromId);
        Task<Massage> GetSingleMessage(int commentId);
        Task Delete(Massage model);
        Task<IEnumerable<Massage>> GetOutcomingMassage(string fromId,string toId);
        Task<IEnumerable<Massage>> GetIncommingMassage(string fromId,string toId);
    }
}