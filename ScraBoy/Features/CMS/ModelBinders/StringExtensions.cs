using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ScraBoy.Features.CMS.ModelBinders
{
    public static class StringExtensions
    {
        public static string MakeUrlFriednly(this string value)
        {
            value = value.ToLowerInvariant().Replace(","," ");
            value = Regex.Replace(value,@"[^0-9a-z-]",string.Empty);
            return value;
        }
    }
}