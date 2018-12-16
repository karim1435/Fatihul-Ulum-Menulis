using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace ScraBoy.Features.CMS.Sitemap
{
    public class XmlSitemapResult : ActionResult
    {
        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        XNamespace nsImage = "http://www.google.com/schemas/sitemap-image/1.1";

        private IEnumerable<ISitemapItem> _items;

        public XmlSitemapResult(IEnumerable<ISitemapItem> items)
        {
            _items = items;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string encoding = context.HttpContext.Response.ContentEncoding.WebName;
            XNamespace xsi = "http://www.w3.org/2001/XMLScema-instance";
            var XElemens = new XElement(ns + "urlset",
                new XAttribute(XNamespace.Xmlns + "xsi",("http://www.w3.org/2001/XMLSchema-instance")),
                new XAttribute(XNamespace.Xmlns + "image",("http://www.google.com/schemas/sitemap-image/1.1")));
            foreach(var item in _items)
            {
                XElemens.Add(CreateItemElement(item));
            }
            XDocument sitemap = new XDocument(new XDeclaration("1.0",encoding,"yes"),XElemens);
            context.HttpContext.Response.ContentType = "application/rss+xml";
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Write(sitemap.Declaration + sitemap.ToString());
        }
        public XmlDocument RemoveXmlns(String xml)
        {
            XDocument d = XDocument.Parse(xml);
            d.Root.Descendants().Attributes().Where(x => x.IsNamespaceDeclaration).Remove();

            foreach(var elem in d.Descendants())
                elem.Name = elem.Name.LocalName;

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(d.CreateReader());

            return xmlDocument;
        }
        private XElement CreateItemElement(ISitemapItem item)
        {
            XElement itemElement = new XElement(ns + "url",new XElement(ns + "loc",item.Url.ToLower()));

            if(item.LastModified.HasValue)
                itemElement.Add(new XElement(ns + "lastmod",item.LastModified.Value.ToString("yyyy-MM-dd")));

            if(item.ChangeFrequency.HasValue)
                itemElement.Add(new XElement(ns + "changefreq",item.ChangeFrequency.Value.ToString().ToLower()));

            if(item.Priority.HasValue)
                itemElement.Add(new XElement(ns + "priority",item.Priority.Value.ToString(CultureInfo.InvariantCulture)));
            if(item.siteMapImg != null)
            {
                XElement imgEle = new XElement(nsImage + "image");
                imgEle.Add(new XElement(nsImage + "loc",item.siteMapImg.img));
                imgEle.Add(new XElement(nsImage + "caption",@"<![CDATA[" + WebUtility.HtmlDecode(item.siteMapImg.caption + "]]")));
                itemElement.Add(imgEle);

            }

            return itemElement;
        }
    }
}