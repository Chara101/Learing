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

            if (a is A) Console.WriteLine("a is A");
            if (b is A) Console.WriteLine("b is A because B is drived from A");
            if(a is B) Console.WriteLine("a is B because A is not drived from B");
            else Console.WriteLine("a is not B");
            if(a is object) Console.WriteLine("a is object"); //所有类都是object的子类

            Father father = new Father();
            Son son = new Son();
            if (father is Father) Console.WriteLine("father is Father");
            if (son is Father) Console.WriteLine("son is Father because Son is drived from Father");
        }
    }
}