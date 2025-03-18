 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learing
{
    class Example3
    {
        static void Main(string[] args)
        {
            int ivar;
            double dvar;

            ivar = 100;
            dvar = 100.0;

            Console.WriteLine("ivar: " + ivar);
            Console.WriteLine("dvar: " + dvar);

            ivar = ivar / 3;
            dvar = dvar / 3;

            Console.WriteLine("ivar: " + ivar);
            Console.WriteLine("dvar: " + dvar);
        }
    }
}
