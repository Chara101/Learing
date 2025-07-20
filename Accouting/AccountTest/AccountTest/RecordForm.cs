using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAcounting
{
    internal class RecordForm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; } = ""; //必須
        public string Category { get; set; } = ""; //必須
        public string EventType { get; set; } = ""; //必須
        public int Amount { get; set; } = 0;
        public string Comment { get; set; } = "";
    }
}
