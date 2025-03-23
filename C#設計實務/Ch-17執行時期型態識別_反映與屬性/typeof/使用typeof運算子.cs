using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learing
{
    class Example
    {
        static void Main(string[] args)
        {
            Type type = typeof(StreamReader);
            Console.WriteLine(type.FullName);

            if(type.IsClass)
            {
                Console.WriteLine("This is a class.");
            }
            if(type.IsAbstract)
            {
                Console.WriteLine("This is an abstract class.");
            }
            else
            {
                Console.WriteLine("a concrete class.");
            }
        }
    }
}