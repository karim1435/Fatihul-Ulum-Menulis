using ScraBoy.Features.CMS.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Tag
{
    //
    [RoutePrefix("tag")]
    [Authorize]
    public class TagController : Controller
    {
        public TagController():this(new TagRepository()) { }

        private readonly ITagRepostory tagRepository;
        public TagController(ITagRepostory repository)
        {
            this.tagRepository = repository;
        }
        [Route("")]
        public ActionResult Index()
        {
            var tags = tagRepository.GetAll();
            if(Request.AcceptTypes.Contains("application/json"))
            {
                return Json(tags,JsonRequestBehavior.AllowGet);
            }

            if(User.IsInRole("author"))
            {
                return new HttpUnauthorizedResult();
            }
            return View(model:tags);
        }
        [Route("edit/{tag}")]
        [HttpGet]
        [Authorize(Roles = "admin, editor")]
        public ActionResult Edit(String tag)
        {
            try
            {
                var model = tagRepository.Get(tag);
                return View(model:model);
            }
            catch(KeyNotFoundException e)
            {
                return HttpNotFound();
                throw;
            }
        }
        [Route("edit/{tag}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, editor")]
        public ActionResult Edit(String tag,string newTag)
        {
            var tags = tagRepository.GetAll();

            if(!tags.Contains(tag))
            {
                return HttpNotFound();
            }

            if(tag.Contains(newTag))
            {
                return RedirectToAction("Index");
            }

            if(string.IsNullOrWhiteSpace(newTag))
            {
                ModelState.AddModelError("key","New tag value cannot be empty");
                return View(model: tag);
            }

            tagRepository.Edit(tag,newTag);

            return RedirectToAction("Index");
        }
        [Route("delete/{tag}")]
        [HttpGet]
        [Authorize(Roles = "admin, editor")]
        public ActionResult Delete(string tag)
        {
            try
            {
                var model = tagRepository.Get(tag);
                return View(model: model);
            }
            catch(KeyNotFoundException e)
            {
                return HttpNotFound();
            } 
        }
        [Route("delete/{tag}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, editor")]
        public ActionResult Delete(String tag,string foo)
        {
            try
            {
                tagRepository.Delete(tag);

                return RedirectToAction("Index");
            }
            catch(KeyNotFoundException)
            {
                return HttpNotFound();
            }
            
        }

    }
}