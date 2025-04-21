using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NLog;

namespace Learing
{
    public class MyClass
    {
        private int mine = 0;
        public int MyProperty { get; set; }
        public MyClass()
        {
            mine = 0;
        }
        public MyClass(int mine)
        {
            this.mine = mine;
        }
        public string MyMethod(int a, int b)
        {
            return (a + b).ToString();
        }

        public static string MyMethod3(int a, int b)
        {
            return (a + b).ToString();
        }
    }
    public static class Program
    {

        public static void Main()
        {
            Type t = typeof(MyClass);
            ConstructorInfo[]? constructor = t.GetConstructors();
            MyClass myClass;
            int x = 10;
            foreach (var temp in constructor)
            {
                ParameterInfo[] p = temp.GetParameters();
                if(p.Length == 1 && p[0].ParameterType == typeof(String))
                {
                    myClass = new MyClass(x);
                }
            }
        }
    }
}