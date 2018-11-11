using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;

namespace ScraBoy.Features.Utility
{
    public class Formatter
    {
        public static string FormatHtml(string url)
        {
            return WebUtility.HtmlDecode(url);
        }
        public static DateTime? FormatDate(string dateToFormat)
        {
            DateTime? date = DateTime.ParseExact(dateToFormat,"dd/MM/yyyy",CultureInfo.InvariantCulture);
            return date;
        }
    }
}