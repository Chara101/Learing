using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountAPI.EnumList;
using AccountAPI.Models;

namespace TestAcounting.DataStorage
{
    internal class ListMode : IDataStorage
    {
        private List<RecordForm> records = new List<RecordForm>();
        private int income = 0;
        private int cost = 0;
        private int asset = 0;
        private int liability = 0;
        private int equity = 0;
        public void Initialize()
        {
            records = new List<RecordForm>();
        }
        public int GetTotals(RecordForm r)
        {
            int result = 0;
            try
            {
                if (string.IsNullOrEmpty(r.Category)) throw new ArgumentException("Category cannot be null or empty.");
                switch (r.Category)
                {
                    case "收入":
                        result = income;
                        break;
                    case "費用":
                        result = cost;
                        break;
                    case "資產":
                        result = asset;
                        break;
                    case "負債":
                        result = liability;
                        break;
                    case "權益":
                        result = equity;
                        break;
                    default:
                        throw new ArgumentException("Invalid category specified.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving totals: {ex.Message}");
            }
            return result;
        }
        private void UpdateTotals(RecordForm r, bool isPostive)
        {
            if (!isPostive)
            {
                r.Amount = -r.Amount;
            }
            switch (r.Category)
            {
                case "收入":
                    income += r.Amount;
                    break;
                case "費用":
                    cost += r.Amount;
                    break;
                case "資產":
                    asset += r.Amount;
                    break;
                case "負債":
                    liability += r.Amount;
                    break;
                case "權益":
                    equity += r.Amount;
                    break;
                default:
                    throw new ArgumentException("Invalid category specified.");
            }
        }
        public void Add(RecordForm r)
        {
            try
            {
                if(r.Title is null || r.Title == "") throw new ArgumentException("Title cannot be null or empty.");
                if(r.Category is null || r.Category == "") throw new ArgumentException("Category cannot be null or empty.");
                if (r.Date == DateTime.MinValue) r.Date = DateTime.Now;
                records.Add(r);
                UpdateTotals(r, true);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error adding record: {ex.Message}");
            }
        }
        public void Remove(RecordForm r, ETarget target)
        {
            if (target == ETarget.title)
            {
                try
                {
                    var data = records.FirstOrDefault(temp => temp.Title == r.Title);
                    var num = records.Count(temp => temp.Title == r.Title);
                    if (num == 0 || data is null) throw new ArgumentException("No record found with the specified title.");
                    records.Remove(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing record by title: {ex.Message}");
                }
            }
            else if (target == ETarget.category)
            {
                try
                {
                    var data = records.FirstOrDefault(temp => temp.Category == r.Category);
                    var num = records.Count(temp => temp.Category == r.Category);
                    if (num == 0 || data is null) throw new ArgumentException("No record found with the specified categoty.");
                    records.Remove(data);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing record by title: {ex.Message}");
                }
            }
            try
            {
                UpdateTotals(r, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category totals: {ex.Message}");
            }
        }
        public void Remove(RecordForm r, ETarget target1, ETarget target2)
        {
            if (target1 == ETarget.title && target2 == ETarget.category)
            {
                try
                {
                    var data = records.FirstOrDefault(temp => temp.Title == r.Title && temp.Category == r.Category);
                    var num = records.Count(temp => temp.Title == r.Title);
                    if (num == 0 || data is null) throw new ArgumentException("No record found with the specified title.");
                    records.Remove(data);
                    UpdateTotals(r, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing record by title: {ex.Message}");
                }
            }
        }
        public List<RecordForm> GetAllRecords()
        {
            return records;
        }
        public List<RecordForm> GetRecordsBy(RecordForm r, ETarget target)
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                if(target == ETarget.title) result = records.Where(temp => temp.Title == r.Title).ToList();
                else if(target == ETarget.category) result = records.Where(temp => temp.Category == r.Category).ToList();
                else if (target == ETarget.id) result = records.Where(temp => temp.Id == r.Id).ToList();
                else if (target == ETarget.time) result = records.Where(temp => temp.Date.Date >= r.Date.Date && temp.Date.Date <= DateTime.Now.Date).ToList();
                else if (target == ETarget.money) result = records.Where(temp => temp.Amount >= r.Amount).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error retrieving records: {ex.Message}");
            }
            return result;
        }
        public List<RecordForm> GetRecordsBy(RecordForm r1, RecordForm r2, ETarget target)
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                if (target == ETarget.title) result = records.Where(temp => temp.Title == r1.Title || temp.Title == r2.Title).ToList();
                else if (target == ETarget.category) result = records.Where(temp => temp.Category == r1.Category || temp.Category == r1.Category).ToList();
                else if (target == ETarget.id) result = records.Where(temp => temp.Id >= r1.Id && temp.Id <= r2.Id).ToList();
                else if (target == ETarget.time) result = records.Where(temp => (temp.Date.Date >= r1.Date && temp.Date.Date <= r2.Date)).ToList();
                else if (target == ETarget.money) result = records.Where(temp => temp.Amount >= r1.Amount && temp.Amount >= r2.Amount).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving records: {ex.Message}");
            }
            return result;
        }
        public void Update(RecordForm r, ETarget target)
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                if (target == ETarget.title) result = records.Where(temp => temp.Title == r.Title).ToList();
                else if (target == ETarget.category) result = records.Where(temp => temp.Category == r.Category).ToList();
                else if (target == ETarget.id) result = records.Where(temp => temp.Id == r.Id).ToList();
                else if (target == ETarget.time) result = records.Where(temp => temp.Date.Date >= r.Date.Date && temp.Date.Date <= DateTime.Now.Date).ToList();
                else if (target == ETarget.money) result = records.Where(temp => temp.Amount >= r.Amount).ToList();

                foreach (var record in result)
                {
                    if (record.Id == r.Id) // Assuming Id is unique
                    {
                        UpdateTotals(record, false);
                        record.Date = r.Date;
                        record.Title = r.Title;
                        record.Category = r.Category;
                        record.EventType = r.EventType;
                        record.Amount = r.Amount;
                        record.Comment = r.Comment;
                        UpdateTotals(r, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating record: {ex.Message}");
            }
        }
    }
}
