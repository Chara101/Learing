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
            decimal price;
            decimal discount;
            decimal discounted_price;


            price = 19.95m;
            discount = 0.15m;

            discounted_price = price - (price * discount);

            Console.WriteLine("Discounted price: $" + discounted_price);
        }
    }
}
