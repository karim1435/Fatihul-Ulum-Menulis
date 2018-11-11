using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Topic
{
    [RoutePrefix("category")]
    public class CategoryController : Controller
    {
        private readonly CategoryService categoryService;
        private readonly ICategoryRepository categoryRepository;

        public CategoryController()
        {
            this.categoryRepository = new CategoryRepository();
            this.categoryService = new CategoryService(categoryRepository,ModelState);
        }
        [Route("")]
        public async Task<ActionResult> Index()
        {
            var cats = await categoryService.GetAllAsync();

            return View(cats);
        }
        public async Task SetViewBag()
        {
            var repository = new CategoryRepository();
            ViewBag.Categories = await categoryRepository.GetAllCategoriesAsync();
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

            var category = await this.categoryRepository.GetByIdAsync(catId);

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
            bool updated = await categoryService.UpdateAsync(model);

            if(!updated)
            {
                await SetViewBag();
                return View(model);
            }
            return RedirectToAction("Index");
        }


    }
}