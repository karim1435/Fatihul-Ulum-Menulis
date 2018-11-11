using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApplication1
{
    class Program
    {
        public static string FormatHtml(string url)
        {
            return HttpUtility.HtmlDecode(url);
        }
        static void Main(string[] args)
        {
            string test = "<b>Hello</b>";

            var result = FormatHtml(test);

            HtmlString C = new HtmlString(test);

            Console.WriteLine(C);
        }
    }
}
