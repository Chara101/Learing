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
            Console.WriteLine("This is a table");
            Console.WriteLine("num\tsquare\tcube");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{0}\t{1}\t{2}", i, i * i, i * i * i);
            }
            Console.WriteLine("{0, 10}{1, 10}{2, 10}", "name", "square", "cube");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("{0, 10}{1, 10}{2, 10}", i, i * i, i * i * i);
            }
            Console.WriteLine("This is the end of the table");
        }
    }
}
