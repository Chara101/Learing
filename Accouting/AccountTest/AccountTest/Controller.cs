using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testAccounting1;
using TestAcounting.DataStorage;

namespace TestAcounting
{
    internal class Controller
    {
        private IDataStorage _db = new ListMode();
        public Controller()
        {
            Initialize();
        }
        public Controller(IDataStorage db)
        {
            _db = db;
            Initialize();
        }
        public void Initialize()
        {
            _db.Initialize();
        }
        public List<RecordForm> GetData(RecordForm r, ETarget target, ERange range)
        {
            List<RecordForm> result = new List<RecordForm>();
            if (range == ERange.All)
                result = _db.GetAllRecords();
            else
                result = _db.GetRecordsBy(r, target);
            return result;
        }
        public List<RecordForm> GetData(RecordForm start, RecordForm end, ETarget target, ERange range)
        {
            List<RecordForm> result = new List<RecordForm>();
            if (range == ERange.All) 
                result = _db.GetAllRecords();
            else result = _db.GetRecordsBy(start, end, target);
            return result;
        }
        public void AddData(RecordForm r)
        {
            _db.Add(r);
        }
        public void RemoveData(RecordForm r, ETarget target)
        {
            _db.Remove(r, target);
        }
        public void RemoveData(RecordForm r, ETarget target1, ETarget target2)
        {
            _db.Remove(r, target1, target2);
        }
        //public void RemoveData(RecordForm start, RecordForm end, ETarget target)
        //{
        //    _db.Remove(start, end, target);
        //}
        public void UpdateData(RecordForm r, ETarget target)
        {
            _db.Update(r, target);
        }
    }
}
