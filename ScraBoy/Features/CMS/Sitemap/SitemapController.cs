using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScraBoy.Features.CMS.Sitemap
{
    public class SitemapController: Controller
    {
        IPostRepository postRepository = new PostRepository();
        Post _blog;
        List<ISitemapItem> _items;
        public XmlSitemapResult Post(Post blog)
        {
            _blog = blog;
            _items = new List<ISitemapItem>();
            AddEntriesPost();
            return new XmlSitemapResult(_items);
        }

        private void AddEntriesPost()
        {
            var entries = this.postRepository.GetAllPost().OrderByDescending(x => x.Updated);
            foreach(var entry in entries)
            {
                var siteMapPost = new SitemapItem(StringExtensions.getUrl() + entry.UrlPost);
                siteMapPost.LastModified = entry.Updated;
                siteMapPost.Priority = 1;
                if(!String.IsNullOrEmpty(entry.UrlImage))
                {
                    SiteMapImg img = new SiteMapImg();
                    img.img = this.getFullPhoto(entry.UrlImage);
                    img.caption = entry.Title;
                    siteMapPost.siteMapImg = img;
                }
                _items.Add(siteMapPost);
            }
        }
        public String getFullPhoto(string imgUrl)
        {
            Uri uriResult;
            bool result = Uri.TryCreate(imgUrl,UriKind.Absolute,out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if(!result)
            {
                return StringExtensions.getUrl()+imgUrl.Substring(2,imgUrl.Length-2);
            }
            return imgUrl;
        }
    }
}