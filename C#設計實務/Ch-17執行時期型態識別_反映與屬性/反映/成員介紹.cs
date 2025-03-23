using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Learing
{
    class Example
    {
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
        }
    }
}