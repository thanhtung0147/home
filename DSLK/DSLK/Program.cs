using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSLK
{
    class Program
    {
        static void Main(string[] args)
        {
            Element<int> a = new Element<int>(1);
            Element<int> b = new Element<int>(2);
            Element<int> c = new Element<int>(3);
            List<int> list=new List<int>();
            list.addfirst(a);
            list.addfirst(b);
            list.addfirst(c);
            list.PrintList();

            Element<string> d = new Element<string>("tung");
            Element<string> e = new Element<string>("thanh");
            Element<string> f = new Element<string>("nguyen");
            List<string> List = new List<string>();
            List.addfirst(d);
            List.addfirst(e);
            List.addfirst(f);
            List.PrintList();
        }
    }
}
