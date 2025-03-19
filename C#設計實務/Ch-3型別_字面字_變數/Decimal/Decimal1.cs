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
            decimal amount;
            decimal rate_of_return;
            int years, i;

            amount = 1000.0m;
            rate_of_return = 0.07m;
            years = 10;

            Console.WriteLine("Original investmwnt: $" + amount);
            Console.WriteLine("Rate of return: $" + rate_of_return);
            Console.WriteLine("Over " + years + " years");

            for(i = 0; i < years; i++)
            {
                amount = amount + (amount * rate_of_return);
            }

            Console.WriteLine("Future value: $" + amount); //會列印完整長度的數字
        }
    }
}
