using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.EnumList;
using AccountAPI.Models;

namespace TestAcounting.DataStorage
{
    internal interface IDataStorage
    {
        void Add(RecordForm r);
        void Remove(RecordForm r);
        List<RecordForm> GetAllRecords();
        List<RecordForm> GetRecordsBy(RecordForm r);
        List<RecordForm> GetRecordsBy(RecordForm r1, RecordForm r2);
        RecordForm GetTotals(RecordForm r);
        void Update(RecordForm tartget, RecordForm content);
    }
}
