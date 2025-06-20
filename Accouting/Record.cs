using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting
{
    class Record
    {
        public Record(string title) { Title = title; }
        public Record(string title, int money) : this(title, "", money)
        {
        }
        public Record(string title, string type, int money)
        {
            Title = title;
            Type = type;
            Money = money;
        }


        public int Id { get; set; } = 0;
        public DateTime Time { get; set; }
        public string Title { get; set; } = "";
        public string Type { get; set; } = "";
        public int Money { get; set; }
    }
}
