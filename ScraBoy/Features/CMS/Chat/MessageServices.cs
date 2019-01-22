using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Chat
{
    public class MessageServices
    {
        private readonly IMessageRepository messageRepository;
        private readonly IUserRepository userRepository;
        private readonly ModelStateDictionary modelState;
        public MessageServices(ModelStateDictionary modelState,
            IMessageRepository messageRepository,
            IUserRepository userRepository)
        {
            this.messageRepository = messageRepository;
            this.userRepository = userRepository;
            this.modelState = modelState;
        }
        public async Task<IEnumerable<CMSUser>> GetAllUser()
        {
            using(var db = new CMSContext())
            {
                return await db.Users.ToListAsync();
            }
        }
        public async Task<IEnumerable<Massage>>  GetMessages()
        {
            using(var db = new CMSContext())
            {
                return await db.Message.ToListAsync();
            }
        }
        public async Task<CMSUser> FindUser(string userId)
        {
            return await this.userRepository.GetUserById(userId);
        }
        public async Task<MassageViewModel> GetAllChat(string fromId,string toId)
        {
            var incommingMessage = await this.messageRepository.GetIncommingMassage(fromId,toId);
            var outcommingmassage = await this.messageRepository.GetOutcomingMassage(fromId,toId);
            return GetMessageModel(outcommingmassage,incommingMessage);
        }
        public MassageViewModel GetMessageModel(IEnumerable<Massage> income,IEnumerable<Massage> outcome)
        {
            MassageViewModel models = new MassageViewModel();

            foreach(var item in income)
            {
                var model = new Massage();
                model = item;
                model.MassageType = MessageType.Income;
                models.Massages.Add(model);
            }

            foreach(var item in outcome)
            {
                var model = new Massage();
                model = item;
                model.MassageType = MessageType.Outcome;
                models.Massages.Add(model);
            }

            return models;
        }
        public async Task<bool> SendMessage(string userId, Massage model)
        {
            if(!modelState.IsValid)
            {
                return false;
            }
            var user = await this.FindUser(userId);

            if(user==null)
            {
                modelState.AddModelError(string.Empty,"can't find user");
                return false;
            }

            if(string.IsNullOrWhiteSpace(model.Content))
            {
                modelState.AddModelError(string.Empty,"massage can't be empty");
                return false;
            }

            model.Sent = DateTime.Now;
      
            await this.messageRepository.Create(model);
            return true;
        } 
    }
}