using HtmlAgilityPack;
using ScraBoy.Features.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ScraBoy.Features.CMS.ModelBinders
{
    public static class StringExtensions
    {
        public static String ReadMore(this string content,int length=300,string ommission = "...")
        {
            HtmlDocument doc = new HtmlDocument();

            string test = Formatter.FormatHtml(content);
            doc.LoadHtml(test);

            string s = "";
            StringBuilder sb = new StringBuilder();
            foreach(HtmlTextNode node in doc.DocumentNode.SelectNodes("//text()"))
            {
                sb.AppendLine(node.InnerText.TrimEnd() + " ");
            }
            s += sb;

            return s.Replace(System.Environment.NewLine,string.Empty)
                .Replace("\t",string.Empty)
                .TruncateHtml(length,ommission);
        }
        /// <summary>
        /// remove html 
        /// </summary>
        public static string TruncateHtml(this string input,int length = 300,
                                           string ommission = "...")
        {
            // s.Substring(0, Math.Min(150, s.Length - 1))
            if(input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ",length);
            return string.Format("{0}" + ommission,input.Substring(0,(iNextSpace > 0) ?
                                                                  iNextSpace : length).Trim());
        }
        public static string MakeUrlFriednly(this string value)
        {
            value = value.ToLowerInvariant().Replace(","," ");
            value = Regex.Replace(value,@"[^0-9a-z-]",string.Empty);
            return value;
        }
        public static string FormatDate(this DateTime datetime)
        {
            return datetime.ToString("dd MMMMM yyyy");
        }
        public static string TimeAgo(this DateTime dateTime)
        {
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(dateTime);

            if(timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago",timeSpan.Seconds);
            }
            else if(timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago",timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if(timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago",timeSpan.Hours) :
                    "about an hour ago";
            }
            else if(timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago",timeSpan.Days) :
                    "yesterday";
            }
            else if(timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago",timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago",timeSpan.Days / 365) :
                    "about a year ago";
            }

            return result;
        }
    }
}