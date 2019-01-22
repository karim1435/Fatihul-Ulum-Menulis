using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ScraBoy.Features.Web
{
    public class HadisPortal : WebParent
    {
        public HadisModel model = new HadisModel();
        public HadisPortal(string url) : base(url)
        {
            Init();
        }
        private void Init()
        {
            HtmlNode titles = HtmlDocument.DocumentNode.Descendants("div").
               Where(d => d.Attributes.Contains("class") && d.Attributes["class"].
               Value.Contains("hadith")).FirstOrDefault();

            this.model.Content = RemoveNumber(GetContent(titles));
            this.model.Number = GetNumber(GetContent(titles));
        }
        private string GetContent(HtmlNode title)
        {
            string result = "";

            var node = title.Descendants("div").
                Where(d => d.Attributes["id"].
                Value.Contains("ar")).FirstOrDefault();

            result = node.InnerText;
            return result;
        }

        private static string RemoveNumber(string text)
        {
            var result = string.Join("-",text.Split('-').Skip(1));
            return result;
        }
        static int GetNumber(string text)
        {
            string number = Regex.Match(text,@"\d+").Value;
            return Convert.ToInt32(number);
        }
    }
}