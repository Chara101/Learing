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
            Console.WriteLine("You ordered " + 2 + "items at $" + 3 + "each.");
            Console.WriteLine("10 / 3 is $" + 10.0 / 3.0);
            Console.WriteLine("February has {0} or {1} days.", 28, 29);
            Console.WriteLine("February has {0, 10} or {1, 5} days.", 28, 29);
            Console.WriteLine("10 / 3 is ${0:#.##}", 10.0 / 3.0);
            Console.WriteLine("you account is {0:##,####.###}", 123456.789);
            Console.WriteLine("you account is {0:#}", 123456.789);
            Console.WriteLine("you account is {0:##}", 123456.789); //same as above
            Console.WriteLine("you account is {0:##.#}", 123456.789); //會根據取到的位子最後沒取到的做四捨五入進位
            Console.WriteLine("you account is {0:C}", 123456.789); //貨幣格式
        }
    }
}
