using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.Web
{
    public class WebParent
    {  
        protected string playstoreUrl { get; set; }
        protected HtmlWeb HtmlWeb { get; set; }
        protected HtmlDocument HtmlDocument { get; set; }

        public WebParent(string url)
        {
            this.playstoreUrl = url;
            HtmlWeb = new HtmlWeb();
            HtmlDocument = HtmlWeb.Load(playstoreUrl);
        }
    }
}