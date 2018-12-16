using ScraBoy.Features.CMS.User;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Nws
{
    [RoutePrefix("Report")]
    [Authorize]
    public class ReportController : Controller
    {
        IReportRepoitory reportRepsitory;
        IUserRepository userRepository;
        public ReportController(IReportRepoitory reportRepository, IUserRepository userRepository)
        {
            this.reportRepsitory = reportRepository;
            this.userRepository = userRepository;
        }
        public ReportController():this(new ReportRepoitory(), new UserRepository())
        {

        }

        [Route("")]
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);

            return View("Index","",this.reportRepsitory.GetPagedList(currentFilter,pageNumber));
        }
        public async Task<ViewResult> Search(string search)
        {
            ViewBag.Filter = search;

            return View("Index","",this.reportRepsitory.GetPagedList(search,1));
        }
        [Route("create")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(Report model)
        {
           if(!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await GetLoggedInUser();
            model.UserId = user.Id;
            await this.reportRepsitory.CreateReportAsync(model);

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("edit{id}")]
        public async Task<ActionResult> Edit(int id)
        {
            var report = await this.reportRepsitory.GetByIdAsync(id);
            if(report==null)
            {
                return HttpNotFound();
            }
            return View(report);
        }
        [HttpPost]
        [Route("edit{id}")]
        public async Task<ActionResult> Edit(Report model, int id)
        {
            var report = await this.reportRepsitory.GetByIdAsync(id);

            if(report == null)
            {
                return HttpNotFound();
            }
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            

            await this.reportRepsitory.UpdateReportAsync(model,id);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int id)
        {
            var category = await this.reportRepsitory.GetByIdAsync(id);

            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [Route("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id,string foo)
        {
            try
            {
                var category = await this.reportRepsitory.GetByIdAsync(id);

                await reportRepsitory.DeleteReportAsync(category);

                return RedirectToAction("Index");
            }
            catch(KeyNotFoundException e)
            {
                return HttpNotFound();
            }
            catch(SqlException ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return RedirectToAction("Index");
            }
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }

    }
}
