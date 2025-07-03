using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Accounting.DataBase
{
    internal class BasicList : IDataBase
    {
        private int _countid = 0;
        private DashBoard _total = new DashBoard();
        private List<Record> _records = new List<Record>();
        public void Initialize()
        {
            _total.Total = 0;
            _total.Income = 0;
            _total.Cost = 0;
        }

        private int ID { get => _countid++; }
        public void Insert(Record r)
        {
            try
            {
                if (r.Title != "" && r.Money != 0)
                {
                    _records.Add(r);
                    r.Id = ID;
                    _total.Total += r.Money;
                    if (r.Money >= 0)
                    {
                        _total.Income += r.Money;
                    }
                    else
                    {
                        _total.Cost += r.Money;
                    }
                }
                else throw new Exception("Lost records' title or money amount.");
            }
            catch(Exception e)
            {

            }
        }
        public List<Record> GetAll()
        {
            return _records;
        }
        public List<Record> SelectByTitle(Record r)
        {
            try
            {
                List<Record> result = new List<Record>();
                if (r.Title == "*") return _records;
                foreach (var temp in _records)
                {
                    if (temp.Title == r.Title)
                    {
                        result.Add(temp);
                        break;
                    }
                }
                return result;
            }
            catch
            {
                Console.WriteLine("Select failed.");
                return new List<Record>();
            }
        }
        public List<Record> SelectAllByTitle(Record r)
        {
            try
            {
                List<Record> result = new List<Record>();
                if (r.Title == "*") return _records;
                foreach (var temp in _records)
                {
                    if (temp.Title == r.Title)
                    {
                        result.Add(temp);
                    }
                }
                return result;
            }
            catch
            {
                Console.WriteLine("Select failed.");
                return new List<Record>();
            }
        }
        public List<Record> SelectByType(Record r)
        {
            List<Record> result = new List<Record>();
            foreach (var temp in _records)
            {
                if (temp.Type == r.Type)
                {
                    result.Add(temp);
                    break;
                }
            }
            return result;
        }
        public void Update(Record r)
        {
            try
            {
                for (int i = 0; i < _records.Count; i++)
                {
                    if (_records[i].Title == r.Title)
                    {
                        _total.Total -= _records[i].Money;
                        if(_records[i].Money >= 0)
                        {
                            _total.Income -= _records[i].Money;
                        }
                        else
                        {
                            _total.Cost -= _records[i].Money;
                        }
                        _records[i].Type = r.Type;
                        _records[i].Money = r.Money;
                        _total.Total -= r.Money;
                        if (r.Money >= 0)
                        {
                            _total.Income -= r.Money;
                        }
                        else
                        {
                            _total.Cost -= r.Money;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update failed.");
            }
        }
        public void UpdateAll(Record r)
        {
            try
            {
                for (int i = 0; i < _records.Count; i++)
                {
                    if (_records[i].Title == r.Title)
                    {
                        _total.Total -= _records[i].Money;
                        if (_records[i].Money >= 0)
                        {
                            _total.Income -= _records[i].Money;
                        }
                        else
                        {
                            _total.Cost -= _records[i].Money;
                        }
                        _records[i].Type = r.Type;
                        _records[i].Money = r.Money;
                        _total.Total -= r.Money;
                        if (r.Money >= 0)
                        {
                            _total.Income -= r.Money;
                        }
                        else
                        {
                            _total.Cost -= r.Money;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update failed.");
            }
        }
        public void Clear()
        {
            try
            {
                _records.Clear();
                _total.Total = 0;
                _total.Income = 0;
                _total.Cost = 0;
            }
            catch
            {
                Console.WriteLine("Clear failed.");
            }
        }
        public void Delete(Record r)
        {
            try
            {
                //_records.RemoveAll(x => x.Title == r.Title);
                for (int i = _records.Count - 1; i >= 0; i--)
                {
                    if (_records[i].Title == r.Title)
                    {
                        _records.RemoveAt(i);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Delete failed.");
            }
        }
        public void DeleteAll(Record r)
        {
            try
            {
                //_records.RemoveAll(x => x.Title == r.Title);
                for (int i = _records.Count - 1; i >= 0; i--)
                {
                    if (_records[i].Title == r.Title)
                    {
                        _records.RemoveAt(i);
                    }
                }
            }
            catch
            {
                Console.WriteLine("Delete failed.");
            }
        }
    }
}
