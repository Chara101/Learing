using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Learing
{
    class Example
    {
        class Myclass
        {
            private int x;
            private int y;

            public Myclass(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public int Sum()
            {
                return x + y;
            }

            public bool Isbetween(int value)
            {
                return (x < value && value < y);
            }

            public int X
            {
                get { return x; }
                set { x = value; }
            }
            public int Y
            {
                get { return y; }
                set { y = value; }
            }
            public void Show()
            {
                Console.WriteLine("x = " + x + ", y = " + y);
            }
        }
        static void Main(string[] args)
        {
            Type t = typeof(Myclass);
            Console.WriteLine(t.Name);
            Console.WriteLine();
            MethodInfo[] m = t.GetMethods();
            foreach (var i in m)
            {
                Console.Write(i.ReturnType.Name + " " + i.Name + "(");
                ParameterInfo[] p = i.GetParameters();
                foreach(var j in p)
                {
                    Console.Write(j.ParameterType + " " + j.Name);
                }
                Console.WriteLine(")");
            }
        }
    }
}