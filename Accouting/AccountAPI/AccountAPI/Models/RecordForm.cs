using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountAPI.Models
{
    public class RecordForm
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Category_id { get; set; } = 0; //必須
        public string Category { get; set; } = ""; //必須
        public int SubCategory_id { get; set; } = 0; //必須
        public string SubCategory { get; set; } = ""; //必須
        public int Amount { get; set; } = 0;
        public int SubCount { get; set; } = 0;
        public int SubAmount { get; set; } = 0;
        public string Comment { get; set; } = "";
    }
}
