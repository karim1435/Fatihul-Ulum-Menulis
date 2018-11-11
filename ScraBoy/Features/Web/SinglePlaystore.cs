using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScraBoy.Features.Utility;

namespace ScraBoy.Features.Web
{
    public class SinglePlaystore : WebParent
    {
        public SinglePlaystoreModel detailModel = new SinglePlaystoreModel();
        public SinglePlaystore(string url) : base(url)
        {
            Init();
        }
        private void Init()
        {
            var node = HtmlDocument.DocumentNode.Descendants("div").
                Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("hAyfc"));

            foreach(var child in node)
            {
                GetModelProperty(child);
            }

            detailModel.Description = GetDescription();
        }

        private void GetModelProperty(HtmlNode child)
        {
            var s = child.Descendants("div").Where(d => d.Attributes.Contains("class") && 
            d.Attributes["class"]. Value.Contains("BgcNfc"));

            foreach(var childNode in s)
            {
                if(childNode.InnerText.Equals("Updated"))
                    detailModel.Update = Print(child);
                if(childNode.InnerText.Equals("Installs"))
                    detailModel.Install = Print(child);
            }
        }

        public string GetDescription()
        {
            var node = HtmlDocument.DocumentNode.Descendants("meta").Where(d =>
                   d.Attributes.Contains("itemprop") && d.Attributes["itemprop"].
                   Value.Contains("description")).FirstOrDefault();
            string result = node.Attributes["content"].Value;
            return Formatter.FormatHtml(result);

        }
        private string Print(HtmlNode child)
        {
            string hasil = "";
            var result = child.Descendants("span").
            Where(d => d.Attributes["class"].
            Value.Contains("htlgb")).FirstOrDefault();

            hasil = result.InnerText;

            return Formatter.FormatHtml(hasil); ;
        }


    }
}