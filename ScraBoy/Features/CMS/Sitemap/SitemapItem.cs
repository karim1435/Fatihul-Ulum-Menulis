using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScraBoy.Features.CMS.Sitemap
{
    public class SiteMapImg
    {
        public String img { get; set; }
        public String caption { get; set; }
    }
    public class SitemapItem: ISitemapItem
    {
        public SitemapItem(string url)
        {
            Url = url;
        }

        public string Url { get; set; }

        public DateTime? LastModified { get; set; }

        public ChangeFrequency? ChangeFrequency { get; set; }

        public float? Priority { get; set; }

        public SiteMapImg siteMapImg { get; set; }
    }
}