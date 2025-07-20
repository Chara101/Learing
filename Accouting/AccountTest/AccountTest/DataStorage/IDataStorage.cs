using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAcounting.DataStorage
{
    internal interface IDataStorage
    {
        void Initialize();
        void Add(RecordForm r);
        void Remove(RecordForm r, ETarget target);
        void Remove(RecordForm r, ETarget target1, ETarget target2);
        List<RecordForm> GetAllRecords();
        List<RecordForm> GetRecordsBy(RecordForm r, ETarget target);
        List<RecordForm> GetRecordsBy(RecordForm r1, RecordForm r2, ETarget target);
        int GetTotals(RecordForm r);
        void Update(RecordForm r, ETarget target);
    }
}
