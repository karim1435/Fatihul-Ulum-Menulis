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

            return View("Index","",this.categoryService.GetPagedList(currentFilter,pageNumber));
        }
        public async Task<ViewResult> Search(string search)
        {
            ViewBag.Filter = search;

            return View("Index","",this.categoryService.GetPagedList(search,1));
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

            if(categoryService.GetCategoryByName(model.Name))
            {
                ModelState.AddModelError(string.Empty,"Category Already Exists");
                return View(model);
            }

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

            return View(category);
        }
        [HttpPost]
        [Route("edit/{catId}")]
        public async Task<ActionResult> Edit(Category model,int catId)
        {

            var category = await this.categoryService.GetCategory(catId);

            if(categoryService.GetExistingCategory(model.Name,category.Id))
            {
                ModelState.AddModelError(string.Empty,"Category Already Exists");
                return View(model);
            }

            bool updated = await categoryService.UpdateAsync(model, catId);

            if(!updated)
            {
                await SetViewBag();
                return View(model);
            }

            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin, editor")]
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
            ViewBag.Categories = categoryRepository.GetAllCategory();
        }
        private async Task<CMSUser> GetLoggedInUser()
        {
            return await userRepository.GetUserByNameAsync(User.Identity.Name);
        }
    }
}