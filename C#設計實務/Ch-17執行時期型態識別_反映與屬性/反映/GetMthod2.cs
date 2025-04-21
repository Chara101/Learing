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
        private string MyMethod2(int a, int b)
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
            PropertyInfo[] properties = t.GetProperties();
            MethodInfo[] methods = t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);
            foreach (var item in methods)
            {
                Console.Write(item.ReturnType + " " + item.Name + "(");
                ParameterInfo[] parameters = item.GetParameters();
                foreach (var parameter in parameters)
                {
                    Console.Write(parameter.ParameterType + " " + parameter.Name);
                }
                Console.WriteLine(")");

            }
        }
    }
}