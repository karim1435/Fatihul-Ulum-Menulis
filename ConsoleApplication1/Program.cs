using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        public static string CloseHtml(string str)
        {
            if(str == string.Empty || str == null)
                return string.Empty;

            str = str.ToLower();

            string output = string.Empty;
            string regex;

            List<string> closeTags = new List<string>();
            List<string> openTags = new List<string>();
            List<string> missingTags = new List<string>();

            //Strip double spaces
            str = Regex.Replace(str,@" {2,}"," ");

            //Strip old BR's
            str = str.Replace("<br>",string.Empty).Replace("<br >",string.Empty);

            //All tags 
            regex = "<(.|\n)*?>";
            MatchCollection ATags = Regex.Matches(str,regex,RegexOptions.Multiline);
            str = string.Empty;
            foreach(Match tag in ATags) //we want only tags 
                str += tag.Value.ToString();

            // Open tags (attributes) 
            regex = "<[a-zA-Z]+[a-zA-Z0-9:]*(\\s+[a-zA-Z]+[a-zA-Z0-9:]*((=[^\\s\"'<>]+)|(=\"[^\"]*\")|(='[^']*')|()))*\\s*\\/?\\s*>";
            MatchCollection OTags = Regex.Matches(str,regex);//, RegexOptions.Multiline); 
            foreach(Match tag in OTags) //all tags 
                if(tag.Value.ToString().IndexOf("/>") < 0) //we dont wanna that auto closed tags 
                    openTags.Add((breakStringOn(tag.Value.ToString()," ") + ">").Replace(">>",">")); //make it simple: <a href=...> => <a> 

            // Closed tags 
            regex = "</[A-Za-z0-9]*>";
            MatchCollection CTags = Regex.Matches(str,regex);
            foreach(Match tag in CTags) //return all tags 
                closeTags.Add(tag.Value.ToString().Replace("/",string.Empty)); //<b/> => <b> {para comparacao depois} 


            bool keepLooking = true; //just unclosed tags 
            do
            {
                keepLooking = false;
                for(int ot = 0; ot < openTags.Count; ot++)
                {
                    for(int ct = 0; ct < closeTags.Count; ct++)
                    {
                        if(closeTags[ct].Equals(openTags[ot]))
                        {
                            closeTags.RemoveAt(ct);
                            openTags.RemoveAt(ot);
                            keepLooking = true;
                            break;
                        }
                    }
                }

            } while(keepLooking);

            //reverse order, we want it in an especific order 
            openTags.Reverse();

            foreach(string tag in openTags) //close tags 
                missingTags.Add(tag.Replace("<","</"));

            foreach(string s in missingTags)
                output += s;

            return output;
        }

        public static string breakStringOn(string str,string breaker)
        {
            if(str.IndexOf(breaker) > -1)
                return str.Remove(str.IndexOf(breaker)).TrimEnd();
            else
                return str;
        }
    }
}
