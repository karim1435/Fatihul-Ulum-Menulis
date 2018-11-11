using ScraBoy.Features.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Game
{
    public class GameService
    {
        private CMSContext  gameRepository = new CMSContext ();

        public IQueryable<GameModel> GetAllGame(string name)
        {
            var models = this.gameRepository.Game.AsQueryable();
            if(!string.IsNullOrEmpty(name))
            {
                models = models.Where(a => a.Title.Contains(name));
            }
            return models.OrderByDescending(s => s.Id);
        }
        public List<GameModel> GetAllGame()
        {
            return this.gameRepository.Game.ToList();
        }
        public GameModel Saves(GameModel model, int creatorId)
        {
            if(!IsExist(model.Title))
            {
                model.CreatorId = creatorId;
                this.gameRepository.Game.Add(model);
                this.gameRepository.SaveChanges();
            }
            return GetOne(model.Title);
        }
        public bool IsExist(string name)
        {
            var model = this.gameRepository.Game.Where(a => a.Title.Equals(name)).Count();
            return model > 0;
        }
        public GameModel GetOne(string name)
        {
            return this.gameRepository.Game.Where(a => a.Title.Equals(name)).FirstOrDefault();
        }
    }
}