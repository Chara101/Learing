using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting
{
    class LaderModel
    {
        public LaderModel(int id)
        {
            Id = id;
        }
        public int Id { get; }
        public string Time { get; set; } = "NULL";
        public string Title { get; set; } = "NULL";
        public int Money { get; set; }
    }
}
