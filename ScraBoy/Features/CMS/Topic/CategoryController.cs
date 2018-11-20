using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Topic
{
    [RoutePrefix("category")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly CategoryService categoryService;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;
        public CategoryController()
        {
            this.userRepository = new UserRepository();
            this.categoryRepository = new CategoryRepository();
            this.categoryService = new CategoryService(categoryRepository,ModelState);
        }

        [Route("")]
        public async Task<ActionResult> Index(int? page,string currentFilter)
        {
            int pageNumber = (page ?? 1);

            if(!User.IsInRole("author"))
            {
                return View("Index","",this.categoryService.GetPagedList(currentFilter,pageNumber,null));
            }

            var user = await GetLoggedInUser();

            return View("Index","",this.categoryService.GetPagedList(currentFilter,pageNumber,user.Id));
        }
        public async Task<ViewResult> Search(string search)
        {
            ViewBag.Filter = search;

            if(!User.IsInRole("author"))
            {
                return View("Index",this.categoryService.GetPagedList(search,1,null));
            }

            var user = await GetLoggedInUser();

            return View("Index","",this.categoryService.GetPagedList(search,1,user.Id));
        }

        [HttpGet]
        [Route("create")]
        public async Task<ActionResult> Create()
        {
            await SetViewBag();

            return View();
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create(Category model)
        {
            if(!ModelState.IsValid)
            {
                await SetViewBag();
                return View(model);
            }
            var user = await GetLoggedInUser();

            if(categoryService.GetCategoryByName(model.Name,user.Id))
            {
                ModelState.AddModelError(string.Empty,"Category Already Exists");
                return View(model);
            }

            model.AuthorId = user.Id;

            var success= await this.categoryService.AddAsync(model);
            if(!success)
            {
                await SetViewBag();
                return View(model);
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        [Route("edit/{catId}")]
        public async Task<ActionResult> Edit(int catId)
        {
            await SetViewBag();

            var category = await this.categoryService.GetCategory(catId);

            if(category == null)
            {
                return HttpNotFound();
            }

            if(User.IsInRole("author"))
            {
                var user = await GetLoggedInUser();

                if(category.AuthorId != user.Id)
                {
                    return new HttpUnauthorizedResult();
                }
            }
            return View(category);
        }
        [HttpPost]
        [Route("edit/{catId}")]
        public async Task<ActionResult> Edit(Category model,int catId)
        {

            var category = await this.categoryService.GetCategory(catId);
            var user = await GetLoggedInUser();

            if(categoryService.GetExistingCategory(model.Name,user.Id,category.Id))
            {
                ModelState.AddModelError(string.Empty,"Category Already Exists");
                return View(model);
            }


            if(User.IsInRole("author"))
            {

                try
                {
                    if(category.AuthorId != user.Id)
                    {

                        return new HttpUnauthorizedResult();
                    }
                }
                catch { }

            }

            bool updated = await categoryService.UpdateAsync(model, catId);
            if(!updated)
            {
                await SetViewBag();
                return View(model);
            }
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Delete(int id)
        {
            var category = this.categoryService.GetCategory(id);

            if(category==null)
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
                var category =await  this.categoryService.GetCategory(id);

                await categoryService.DeleteCategory(category);

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
       
        public async Task SetViewBag()
        {
            var user = await GetLoggedInUser();

            ViewBag.Categories = await categoryService.GetByUserId(user.Id);
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }
    }
}