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
            MyClass myClass = new MyClass();
            PropertyInfo[] properties = t.GetProperties();
            MethodInfo[] methods = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in methods)
            {
                ParameterInfo[] p1= item.GetParameters();
                if (p1.Length == 2 && p1[0].ParameterType == typeof(int) && p1[1].ParameterType == typeof(int)) //&& p1[0].ParameterType == typeof(int)
                {
                    Console.WriteLine("found");
                    var a = item.Invoke(myClass, new object[] { 1, 2 });
                    Console.WriteLine(a);
                }   
                Console.Write(item.Name + "(");
                Console.WriteLine(")");
            }
        }
    }
}