using PagedList;
using ScraBoy.Features.Creator;
using ScraBoy.Features.Data;
using ScraBoy.Features.Game;
using ScraBoy.Features.Utility;
using ScraBoy.Features.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Ranking
{
    public class RankingServices
    {
        private int pageSize { get; set; }

        private CMSContext  gameRepository;
        public RankingServices()
        {
            this.pageSize = pageSize;
            gameRepository = new CMSContext ();
        }
        public IQueryable<RankingModel> GetAllRanking(string search,string creator,string start,string end)
        {
            var models = this.GetModel();
            if(!string.IsNullOrEmpty(search))
            {
                models = models.Where(a => a.Game.Title.Contains(search));
            }
            if(!string.IsNullOrEmpty(creator))
            {
                models = models.Where(a => a.Game.Creator.Name.Contains(creator));
            }
            if(!string.IsNullOrEmpty(start))
            {
                DateTime? from = Formatter.FormatDate(start);

                models = from p in models where DbFunctions.TruncateTime(p.RankedOn) >=
                    DbFunctions.TruncateTime(@from.Value) select p;
            }

            if(!string.IsNullOrEmpty(end))
            {
                DateTime? to = Formatter.FormatDate(end);

                models = from p in models where DbFunctions.TruncateTime(p.RankedOn) <= 
                         DbFunctions.TruncateTime(@to.Value)select p;
            }

            return models.OrderBy(a => a.RankedOn);
        }
        IQueryable<RankingModel> GetModel()
        {
            var modes = from s in this.gameRepository.Ranking select s;

            return modes.OrderBy(a => a.RankedOn).AsQueryable();
        }

        public IQueryable<RankingModel> GetListRanking()
        {
            var model = from p in GetModel() where DbFunctions.TruncateTime(p.RankedOn) ==
                        DbFunctions.TruncateTime(DateTime.Now) select p;

            return model;
        }
        public RankingModel FindById(int? id)
        {
            return this.gameRepository.Ranking.Find(id);
        }
        public bool BeginScrap()
        {
            if(this.gameRepository.Game.Count() <= 0)
                return false;
            var model = this.gameRepository.Ranking.OrderByDescending(a => a.Id).FirstOrDefault();

            return model.RankedOn.Date == DateTime.Now.Date;

        }
        public void GetDataFromPlaystore(string url)
        {
            if(BeginScrap())
                return;

            Playstore playtoreScrapper = new Playstore(url);

            GameService gameService = new GameService();
            CreatorService creatorService = new CreatorService();


            int Position = 1;

            foreach(var data in playtoreScrapper.PlaystoreData)
            {
                CreatorModel creatorModel = new CreatorModel();
                creatorModel.Name = data.Creator;
                var creator = creatorService.Saves(creatorModel);

                GameModel gameModel = new GameModel();
                gameModel.Title = data.Title;
                gameModel.Subtitle = data.SubTitle;
                gameModel.UrlGame=data.Link;
                var game = gameService.Saves(gameModel,creator.Id);

                RankingModel rankingModel = new RankingModel();
                rankingModel.Rank = Position;
                rankingModel.Image = data.Image;
                rankingModel.TotalInstall = data.Installs;
                rankingModel.Update = data.Updated;
                Saves(rankingModel,game.Id);
                Position++;
            }
        }
        public void Saves(RankingModel model,int gameId)
        {
            model.GameId = gameId;
            model.RankedOn = DateTime.Now;
            this.gameRepository.Ranking.Add(model);
            this.gameRepository.SaveChanges();
        }
    }
}