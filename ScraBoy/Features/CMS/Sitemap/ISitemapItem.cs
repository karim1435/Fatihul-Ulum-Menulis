using System;

namespace ScraBoy.Features.CMS.Sitemap
{
    public interface ISitemapItem
    {
        string Url { get; }

        DateTime? LastModified { get; }

        ChangeFrequency? ChangeFrequency { get; }

        float? Priority { get; }
        SiteMapImg siteMapImg { get; }
    }
    public enum ChangeFrequency
    {
        Always,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Never
    }
}