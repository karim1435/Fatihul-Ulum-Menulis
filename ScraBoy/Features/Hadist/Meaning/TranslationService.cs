using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using ScraBoy.Features.CMS.Role;
using ScraBoy.Features.CMS.Admin;
using ScraBoy.Features.Utility;
using ScraBoy.Features.CMS.HomeBlog;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.Data;
using System.Data.Entity;
using PagedList;

namespace ScraBoy.Features.Hadist.Meaning
{
    public class TranslationService
    {
        private readonly ITranslationRepository translationRepository;
        private readonly ModelStateDictionary modelState;
        public TranslationService(ModelStateDictionary modelState,
            ITranslationRepository translationRepository)
        {
            this.translationRepository = translationRepository;
            this.modelState = modelState;
        }
        public async Task<Boolean> CheckAvailableTranslation(int hadistId,int languangeId)
        {
            var model = await this.translationRepository.GetAll();
            return model.Where(a => a.KitabId == hadistId && a.LanguageId == languangeId).Count() > 0;
        }
        public async Task<Boolean> AddTranslation(Translation model)
        {
            if(!modelState.IsValid)
            {
                return false;
            }
            var existingTranslation = await CheckAvailableTranslation(model.KitabId,model.LanguageId);

            if(existingTranslation)
            {
                modelState.AddModelError(string.Empty,"The translation in this languange already Exists");
                return false;
            }

            if(string.IsNullOrWhiteSpace(model.Content))
            {
                modelState.AddModelError(string.Empty,"Translation content is empty");
                return false;
            }

            await this.translationRepository.Create(model);

            return true;
        }
        public async Task<Boolean> UpdateTranslation(Translation model,int id)
        {
            var hadist = await this.translationRepository.FindById(id);

            if(!modelState.IsValid)
            {
                return false;
            }
            await this.translationRepository.Edit(model,id);

            return true;
        }
        public async Task<Boolean> RemoveTranslation(Translation model)
        {
            await this.translationRepository.Delete(model);
            return true;
        }
        public async Task<IEnumerable<Translation>> GetTranslationByHadist(int hadistId,string slugUrl)
        {
            var model = await this.translationRepository.GetAll();
            return model.Where(a => a.KitabId == hadistId && a.Kitab.Chapter.Imam.SlugUrl.Equals(slugUrl)).ToList();
        }
    }
}