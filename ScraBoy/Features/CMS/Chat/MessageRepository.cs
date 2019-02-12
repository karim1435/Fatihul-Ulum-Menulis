using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScraBoy.Features.CMS.Topic;
using System.Threading.Tasks;
using ScraBoy.Features.CMS.User;
using System.Data.Entity;

namespace ScraBoy.Features.CMS.Chat
{
    public class MessageRepository:IMessageRepository
    {
        private readonly CMSContext db;
       
        public MessageRepository(CMSContext db)
        {
            this.db = db;
        }
        public MessageRepository():this(new CMSContext())
        {
        }
        public async Task Create(Massage model)
        {
            model.Sent = DateTime.Now;
            this.db.Message.Add(model);
            await this.db.SaveChangesAsync();
        }
        public async Task<IEnumerable<Massage>> GetAllChat(string userId)
        {
            return await this.db.Message.Include("From").Include("To").Where(a => a.ToId == userId ).ToListAsync();
        }
        public async Task<IEnumerable<Massage>> GetOutcomingMassage(string fromId,string toId)
        {
            return await this.db.Message.Include("From").Include("To").Where(a => a.FromId == fromId && a.ToId == toId).ToListAsync();
        }
        public async Task<IEnumerable<Massage>> GetIncommingMassage(string fromId,string toId)
        {
            return await this.db.Message.Include("From").Include("To").Where(a => a.FromId == toId && a.ToId == fromId).ToListAsync();
        }
        public async Task<Massage> GetSingleMessage(int commentId)
        {
            return await this.db.Message.Include("From").Include("To").Where(a => a.Id == commentId).FirstOrDefaultAsync();
        }

        public async Task Delete(Massage model)
        {
            this.db.Message.Remove(model);
            await this.db.SaveChangesAsync();
        }

        public async Task Edit(int id,Massage model)
        {
            var message = await GetSingleMessage(id);

            message.Content = model.Content;
            await this.db.SaveChangesAsync();
        }
    }
}