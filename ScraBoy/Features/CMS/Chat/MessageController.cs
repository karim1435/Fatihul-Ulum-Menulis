using Microsoft.AspNet.Identity;
using ScraBoy.Features.CMS.Gzip;
using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Chat
{
    [RoutePrefix("message")]
    [Authorize]
    public class MessageController : Controller
    {
        MessageServices messageService;
        IUserRepository userRepository;
        IMessageRepository messageRepository;
        public MessageController()
        {
            this.userRepository = new UserRepository();
            this.messageRepository = new MessageRepository();
            this.messageService = new MessageServices(ModelState,messageRepository,userRepository);
        }
        [CompressContent]
        [Route("")]
        public async Task<ActionResult> Index()
        {
            var user = await this.messageService.GetAllUser();
            var model = user.Where(a => !a.Id.Equals(UserLoginId));
            return View(model.ToList());
        }
        [Route("Chatting/{username}")]
        [CompressContent]
        public async Task<ActionResult> ChatIndex(string username)
        {
            var user = await this.userRepository.GetUserByNameAsync(username);

            ViewBag.username = username;
            ViewBag.info = user.DisplayName;
            await SetViewBag();

            return View();
        }
        [HttpGet]
        [CompressContent]
        public async Task<ActionResult> History(string username)
        {
            var chats = await this.messageRepository.GetAllChat(UserLoginId);

            return PartialView("History",chats);
        }
        [HttpGet]
        [CompressContent]
        public async Task<ActionResult> Chat(string username)
        {
            var user = await this.userRepository.GetUserByNameAsync(username);

            var chats = await this.messageService.GetAllChat(UserLoginId,user.Id);

            return PartialView("Chat",chats);
        }
        [Route("Send/{username}")]
        [CompressContent]
        public async Task<ActionResult> Send(string username)
        {
            var user = await this.userRepository.GetUserByNameAsync(username);

            if(user == null)
            {
                return HttpNotFound();
            }
            return View();
        }
        [Route("Send/{username}")]
        [Authorize]
        [HttpPost]
        [CompressContent]
        public async Task<ActionResult> Send(string username,MassageViewModel model)
        {
            var user = await this.userRepository.GetUserByNameAsync(username);

            model.NewMessage.FromId = UserLoginId;
            model.NewMessage.ToId = user.Id;

            var result = await this.messageService.SendMessage(user.Id,model.NewMessage);

            if(!result)
            {
                return RedirectToAction("ChatIndex","Message",new { username = username });
            }

            MessageHub.NotifyCurrentEmployeeInformationToAllClients();

            return RedirectToAction("ChatIndex","Message",new { username = username });
        }
        [Route("Remove/{id}")]
        [CompressContent]
        public async Task<ActionResult> Remove(int id)
        {
            var message = await messageRepository.GetSingleMessage(id);
            if(message == null)
            {
                return HttpNotFound();
            }
            string username = message.To.UserName;

            await this.messageRepository.Delete(message);

            MessageHub.NotifyCurrentEmployeeInformationToAllClients();

            return RedirectToAction("ChatIndex","Message",new { username = username });
        }
        public async Task SetViewBag()
        {
            ViewBag.user = await this.messageRepository.GetAllChat(UserLoginId);
            
        }
        public string UserLoginId
        {
            get { return User.Identity.GetUserId(); }
        }


    }
}
