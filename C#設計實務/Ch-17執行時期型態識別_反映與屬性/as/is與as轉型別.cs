using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learing
{

    class A { }
    class B : A { }

    class Father { }
    class  Son : Father { }
    class Example
    {
        static void Main(string[] args)
        {
            A a = new A();
            B b = new B();
            A? a2;
            B? b2;

            if (a is B)
            {
                b2 = (B)a;
                Console.WriteLine("a is B");
            }
            else
            {
                Console.WriteLine("a is not B");
            }
            if (b is A)
            {
                a2 = (A)b;
                Console.WriteLine("b is A");
            }
            else
            {
                Console.WriteLine("b is not A");
            }

            b2 = a as B;
            if (b2 != null)
            {
                Console.WriteLine("a is B");
            }
            else
            {
                Console.WriteLine("a is not B");
            }
            a2 = b as A;
            if (a2 != null)
            {
                Console.WriteLine("b is A");
            }
            else
            {
                Console.WriteLine("b is not A");
            }
        }
    }
}