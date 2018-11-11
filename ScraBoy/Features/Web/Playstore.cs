using HtmlAgilityPack;
using ScraBoy.Features.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScraBoy.Features.Creator;
using ScraBoy.Features.Utility;

namespace ScraBoy.Features.Web
{
    public class Playstore : WebParent
    {
        public List<PlaystoreModel> PlaystoreData = new List<PlaystoreModel>();

        public Playstore(string url) : base(url)
        {

            Init();
        }
        private void Init()
        {
            IEnumerable<HtmlNode> titles = HtmlDocument.DocumentNode.Descendants("div").
               Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("card no-rationale square-cover apps small"));

            foreach(var item in titles)
            {
                AddGameData(item);
            }
        }

        private void AddGameData(HtmlNode item)
        {
            PlaystoreModel model = new PlaystoreModel();
            string urlImage = string.Format("https:" + SetImageUrl(item));
            model.Image = urlImage;

            string urlGame = String.Format("https://play.google.com" + SetUrlLink(item) + "&hl=en");
            model.Link = urlGame;

            GetSingleGame(model);

            var detail = item.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("details"));

            foreach(var test in detail)
            {
                model.Title = SetTitle(test);
                model.SubTitle = SetDescription(test);
                model.Creator = SetCreator(test);

            }
            PlaystoreData.Add(model);
        }

        private void GetSingleGame(PlaystoreModel model)
        {
            SinglePlaystore sp = new SinglePlaystore(model.Link);
            model.Updated = sp.detailModel.Update;
            model.Installs = sp.detailModel.Install;
            model.Description = sp.detailModel.Description;
        }

        public string SetHeading()
        {
            var node = this.HtmlDocument.DocumentNode.Descendants("div").Where(d =>
            d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("cluster-heading")).FirstOrDefault();

            return node.InnerText;
        }
        private string SetDescription(HtmlNode title)
        {
            string result = "";
            var node = title.Descendants("div").Where(d => d.Attributes
            ["class"].Value.Contains("description")).FirstOrDefault();

            result = node.InnerText.Trim();

            return Formatter.FormatHtml(result);
        }
        private string SetCreator(HtmlNode title)
        {
            string result = "";
            var node = title.Descendants("a").
                Where(d => d.Attributes["class"].Value.Contains("subtitle"))
                .FirstOrDefault();

            result = node.InnerText;

            return Formatter.FormatHtml(result);
        }
        private string SetTitle(HtmlNode title)
        {
            string result = "";
            var node = title.Descendants("a").
                Where(d => d.Attributes["class"].
                Value.Contains("title")).FirstOrDefault();

            result = node.Attributes["title"].Value;

            return Formatter.FormatHtml(result);
        }
        private string SetImageUrl(HtmlNode node)
        {
            string result = "";
            var titles = node.Descendants("img").
                Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("cover-image")).FirstOrDefault();

            result = titles.GetAttributeValue("data-cover-large","");

            return result;
        }
        public string SetUrlLink(HtmlNode node)
        {
            string result = "";
            var titles = node.Descendants("a").
                Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("card-click-target")).FirstOrDefault();

            result = titles.GetAttributeValue("href","");

            return result;
        }
    }
}