using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Accounting.DataBase
{
    internal interface IDataBase
    {
        public void Initialize();
        public void Insert(Record r); //單筆紀錄加入
        public List<Record> GetAll(); //獲取所有記錄
        public List<Record> SelectByTitle(Record r); //根據title獲取最近記錄

        public List<Record> SelectAllByTitle(Record r); //根據title獲取所有記錄
        public List<Record> SelectByType(Record r); //根據type獲取最近記錄
        public List<Record> SelectAllByType(Record r); //根據type獲取所有記錄
        public void Update(Record r); //單筆紀錄更新
        public void UpdateAll(Record r); //根據title更新所有記錄

        public void Clear(); //清空所有記錄
        public void Delete(Record r); //根據title刪除最近紀錄
        public void DeleteAll(Record r); //根據title刪除所有記錄
    }
}
