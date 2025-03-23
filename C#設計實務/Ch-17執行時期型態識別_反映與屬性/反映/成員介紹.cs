using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learing
{
    class Example
    {
        class A { }
        /*
         System.Reflection.MenberInfo是抽象類別
        定義了以下成員:
        Type DeclaringType 類別或介面，宣告的成員
        MenberTypes MemberType 成員
        string Name 型別名稱
        Type ReflectedType 被反映物件的型別

        MemberTypes是列舉型別，定義了以下成員:
        Constructor 建構函式
        Method 方法
        Property 屬性
        Event 事件
        Field 欄位
        */
        static void Main(string[] args)
        {
            Type type = typeof(StreamReader);
            //以下為取得StreamReader類別的所有成員列表，傳回MemberInfo陣列
            //type.GetConstructors()
            //type.GetMethods()
            //type.GetProperties();
            //type.GetEvents();
            //type.GetFields();

            //type.Assembly 取得包含目前類別的組件
            //type.Attributes 取得目前類別的屬性
            //type.BaseType 取得目前類別的基底類別
            //type.FullName 取得目前類別的完整名稱
            //type.FullName 取得目前類別的名稱
            //type.IsAbstract 取得值，指出目前的類別是否為抽象的
            //type.IsClass 取得值，指出目前的類別是否為類別
            //type.IsEnum 取得值，指出目前的類別是否為列舉
            //type.IsInterface 取得值，指出目前的類別是否為介面
            string? temp = typeof(A).Namespace;
            if(temp is not null)
            {
                Console.WriteLine("Namespace: " + temp);
            }   
        }
    }
}