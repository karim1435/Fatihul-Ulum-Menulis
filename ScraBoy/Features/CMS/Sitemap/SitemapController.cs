using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.CMS.ModelBinders;
using ScraBoy.Features.CMS.User;
using ScraBoy.Features.Lomba.Contest;
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
        IUserRepository userRepository = new UserRepository();
        ICompetitionRepositroy contestRepository = new CompetitionRepository();
        Post _blog;
        CMSUser user;
        Competition contest;
        List<ISitemapItem> _items;
        public XmlSitemapResult Author(CMSUser author)
        {
            user = author;
            _items = new List<ISitemapItem>();
            AddEntriesUser();
            return new XmlSitemapResult(_items);
        }
        public XmlSitemapResult Post(Post blog)
        {
            _blog = blog;
            _items = new List<ISitemapItem>();
            AddEntriesPost();
            return new XmlSitemapResult(_items);
        }
       public XmlSitemapResult Contest(Competition contest)
        {
            this.contest = contest;
            _items = new List<ISitemapItem>();
            AddEntriesContest();
            return new XmlSitemapResult(_items);
        }
        private void AddEntriesContest()
        {
            var entries = this.contestRepository.GetContest();
            foreach(var entry in entries)
            {
                var siteMapPost = new SitemapItem(entry.FullUrlContest);
                siteMapPost.LastModified = entry.UpdatedOn;
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
        private void AddEntriesUser()
        {
            var entries =this.userRepository.GetAllUsersAsync();
            foreach(var entry in entries)
            {
                var siteMapPost = new SitemapItem(entry.FullUrlUser);
                siteMapPost.LastModified = entry.RegistrationDate;
                siteMapPost.Priority = 1;
                if(!String.IsNullOrEmpty(entry.UrlImage))
                {
                    SiteMapImg img = new SiteMapImg();
                    img.img = this.getFullPhoto(entry.UrlImage);
                    img.caption = entry.DisplayName;

                    siteMapPost.siteMapImg = img;
                }
                _items.Add(siteMapPost);
            }
        }

        private void AddEntriesPost()
        {
            var entries = this.postRepository.GetAllPost().OrderByDescending(x => x.Updated);
            foreach(var entry in entries)
            {
                var siteMapPost = new SitemapItem(entry.FullUrlPost);
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