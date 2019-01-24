using HtmlAgilityPack;
using ScraBoy.Features.CMS.Blog;
using ScraBoy.Features.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ScraBoy.Features.CMS.ModelBinders
{

    public static class Utils
    {
        // update : 5/April/2018
        static Regex MobileCheck = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino",RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        static Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-",RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        public static bool fBrowserIsMobile()
        {
            if(HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                var u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();

                if(u.Length < 4)
                    return false;

                if(MobileCheck.IsMatch(u) || MobileVersionCheck.IsMatch(u.Substring(0,4)))
                    return true;
            }

            return false;
        }
    }

    public static class StringExtensions
    {
        public static bool CheckHarokah(this string text)
        {
            char[] tashkeel = new char[] { 'ِ','ُ','ٓ','ٰ','ْ','ٌ','ٍ','ً','ّ','َ' };

            foreach(var c in tashkeel)
            {
                if(text.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
        public static string RemoveHarokah(this string text)
        {
            // to be replaced characters
            char[] tashkeel = new char[] { 'ِ','ُ','ٓ','ٰ','ْ','ٌ','ٍ','ً','ّ','َ' };

            // doing the replacement
            foreach(char c in tashkeel)
                text = text.Replace(c.ToString(),"");

            return text;
        }
        public static string Reverse(this string text)
        {
            if(text == null) return null;
            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }
        public static String getUrl()
        {
            //Return variable declaration
            var appPath = string.Empty;

            //Getting the current context of HTTP request
            var context = HttpContext.Current;

            //Checking the current context content
            if(context != null)
            {
                //Formatting the fully qualified website url/name
                appPath = string.Format("{0}://{1}{2}{3}",
                                        context.Request.Url.Scheme,
                                        context.Request.Url.Host,
                                        context.Request.Url.Port == 80
                                            ? string.Empty
                                            : ":" + context.Request.Url.Port,
                                        context.Request.ApplicationPath);
            }

            if(!appPath.EndsWith("/"))
                appPath += "/";
            return appPath;
        }
        public static int CountTotalWords(this string x)
        {
            int result = 0;
            x = x.Trim();

            if(x == "")
                return 0;

            //Ensure there is only one space between each word in the passed string
            while(x.Contains("  "))
                x = x.Replace("  "," ");

            //Count the words
            foreach(string y in x.Split(' '))
                result++;

            return result;
        }
        public static String ReadMore(this string content,int length = 300,string ommission = "... ")
        {
            HtmlDocument doc = new HtmlDocument();

            string test = Formatter.FormatHtml(content);
            doc.LoadHtml(test);

            string s = "";
            StringBuilder sb = new StringBuilder();

            if(doc.DocumentNode.SelectNodes("//text()") == null)
            {
                return "";
            }
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
        public static String TruncateHtml(this string input,int length = 300,
                                           string ommission = "...")
        {
            // s.Substring(0, Math.Min(150, s.Length - 1))
            if(input == null || input.Length < length)
                return input;
            int iNextSpace = input.LastIndexOf(" ",length);
            return string.Format("{0}" + ommission,input.Substring(0,(iNextSpace > 0) ?
                                                                  iNextSpace : length).Trim());
        }
        public static string FormatNumber(this int num)
        {
            if(num >= 1000)
            {
                return (num / 1000D).ToString("0.#") + "K";
            }
            return num.ToString("#,0");
        }
        public static string MakeUrlFriednly(this string value)
        {
            value = value.ToLowerInvariant().Replace(","," ");
            value = Regex.Replace(value,@"[^0-9a-z-]",string.Empty);
            return value;
        }
        public static string FormatTime(this DateTime datetime)
        {
            return String.Format("{0:t}",datetime);
        }
        public static string FormatDate(this DateTime datetime)
        {
            return datetime.ToString("dd MMM yyyy");
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