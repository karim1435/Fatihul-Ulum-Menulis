using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Creator
{
    public class CreatorService
    {
        private CMSContext  gameRepository = new CMSContext ();

        public List<CreatorModel> GetAllCreator()
        {
            return this.gameRepository.Creator.ToList();
        }
        public bool IsExist(string name)
        {
            var model = this.gameRepository.Creator.Where(a => a.Name.Equals(name)).Count();
            return model > 0;
        }
        public CreatorModel GetOne(string name)
        {
            return this.gameRepository.Creator.Where(a => a.Name.Equals(name)).FirstOrDefault();
        }
        public CreatorModel Saves(CreatorModel model)
        {
            if(!IsExist(model.Name))
            {
                this.gameRepository.Creator.Add(model);
                this.gameRepository.SaveChanges();     
            }
            return GetOne(model.Name);
        }
    }
}