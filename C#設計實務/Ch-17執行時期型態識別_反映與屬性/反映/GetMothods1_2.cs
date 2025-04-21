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
    }
    public static class Program
    {

        public static void Main()
        {
            Type t = typeof(MyClass);
            PropertyInfo[] properties = t.GetProperties();
            MethodInfo[] methods = t.GetMethods();
            foreach(PropertyInfo pi in properties)
            {
                Console.WriteLine(pi.PropertyType + " " + pi.Name);
            }
            foreach(MethodInfo mi in methods)
            {
                Console.Write(mi.ReturnType + " " + mi.Name + "(");
                ParameterInfo[] parameters = mi.GetParameters();
                for(int i = 0; i < parameters.Length; i++)
                {
                    if (i > 0)
                    {
                        Console.Write(", ");
                    }
                    Console.Write(parameters[i].ParameterType + " " + parameters[i].Name);
                }
                Console.WriteLine(")");
            }
        }
    }
}