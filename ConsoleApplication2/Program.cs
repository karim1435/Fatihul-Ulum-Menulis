using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class test
    {
        public test()
        {
            names.Add("saya");
            names.Add("kamu");
            names.Add("dia");
        }
        public List<string> names = new List<string>();
        
        public List<string> Names
        {
            get { return names; }
            set { names = value; }
        }
        public string CombinedNames
        {
            get
            {
                return  string.Join(",",names); ;
            }
            set
            {
                names = value.Split(',').Select(s => s.Trim()).ToList();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            test test = new test();
            //foreach(var item in test.Names)
            //{
            //    Console.WriteLine(item);
            //}
            //test.Names.Add("siapa");

            //Console.WriteLine();

            //foreach(var item in test.names)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.WriteLine();
            test.CombinedNames = "saya, suka, kamu";
            foreach(var item in test.names)
            {
                Console.WriteLine(item);
            }
        }
    }
}
