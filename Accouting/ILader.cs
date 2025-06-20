using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting
{
    interface ILader
    {
        public void Add(DateTime time, string name, string type, int money);
        public void Delete(string name);
        public void DeleteAll(string name);

        public List<Record> Search(string name);
        public List<Record> SearchAll(string name);

        public void Insert(DateTime time, string name, string type, int  money);

        public void Clear();

        public List<Record> Records { get; }
    }
}
