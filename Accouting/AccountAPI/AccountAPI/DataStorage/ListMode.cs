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
        public void Remove(RecordForm r)
        {
            List<RecordForm> data = records;
            int count = 0;
            if (!string.IsNullOrEmpty(r.Title))
            {
                try
                {
                    data.AddRange(data.Where(temp => temp.Title == r.Title));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing record by title: {ex.Message}");
                }
                count++;
            }
            else if (!string.IsNullOrEmpty(r.Category))
            {
                try
                {
                    data.AddRange(data.Where(temp => temp.Category == r.Category));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing record by title: {ex.Message}");
                }
            }
            try
            {
                var temp = data.FirstOrDefault();
                if(temp is not null) records.Remove(temp);
                UpdateTotals(r, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category totals: {ex.Message}");
            }
        }
        public List<RecordForm> GetAllRecords()
        {
            return records;
        }
        public List<RecordForm> GetRecordsBy(RecordForm r)
        {
            List<RecordForm> result = records;
            try
            {
                if (r.Id <= 0) result = result.Where(temp => temp.Id == r.Id).ToList();
                else if (!string.IsNullOrEmpty(r.Title)) result = result.Where(temp => temp.Title == r.Title).ToList();
                else if (!string.IsNullOrEmpty(r.Category)) result = result.Where(temp => temp.Category == r.Category).ToList();
                else if (DateTime.MinValue.Date <= r.Date) result = result.Where(temp => temp.Date.Date >= r.Date.Date && temp.Date.Date <= DateTime.Now.Date).ToList();
                else if (r.Amount != 0) result = records.Where(temp => temp.Amount >= r.Amount).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error retrieving records: {ex.Message}");
            }
            return result;
        }
        public List<RecordForm> GetRecordsBy(RecordForm r1, RecordForm r2)
        {
            List<RecordForm> result = records;
            try
            {
                if (!string.IsNullOrEmpty(r1.Title) && !string.IsNullOrEmpty(r2.Title)) result = result.Where(temp => temp.Title == r1.Title || temp.Title == r2.Title).ToList();
                else if (!string.IsNullOrEmpty(r1.Category) && !string.IsNullOrEmpty(r2.Category)) result = result.Where(temp => temp.Category == r1.Category || temp.Category == r1.Category).ToList();
                else if (r1.Id <= 0 && r2.Id <= 0) result = result.Where(temp => temp.Id >= r1.Id && temp.Id <= r2.Id).ToList();
                else if (DateTime.MinValue.Date <= r1.Date && r1.Date <= r2.Date) result = result.Where(temp => (temp.Date.Date >= r1.Date && temp.Date.Date <= r2.Date)).ToList();
                else if (r1.Amount != 0 && r2.Amount >= r1.Amount) result = result.Where(temp => temp.Amount >= r1.Amount && temp.Amount >= r2.Amount).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving records: {ex.Message}");
            }
            return result;
        }
        public void Update(RecordForm target, RecordForm content)
        {
            List<RecordForm> result = records;
            try
            {
                if (target.Id <= 0) result = result.Where(temp => temp.Id == target.Id).ToList();
                else if (!string.IsNullOrEmpty(target.Title)) result = result.Where(temp => temp.Title == target.Title).ToList();
                else if (!string.IsNullOrEmpty(target.Category)) result = result.Where(temp => temp.Category == target.Category).ToList();
                else if (DateTime.MinValue.Date <= target.Date) result = result.Where(temp => temp.Date.Date >= target.Date.Date && temp.Date.Date <= DateTime.Now.Date).ToList();
                else if (target.Amount != 0) result = records.Where(temp => temp.Amount >= target.Amount).ToList();
                var temp = result.FirstOrDefault();
                if(temp is null) throw new ArgumentException("Record not found for update.");
                foreach (var record in records)
                {
                    if (record.Id == temp.Id) // Assuming Id is unique
                    {
                        UpdateTotals(record, false);
                        record.Date = content.Date;
                        record.Title = content.Title;
                        record.Category = content.Category;
                        record.EventType = content.EventType;
                        record.Amount = content.Amount;
                        record.Comment = content.Comment;
                        UpdateTotals(content, true);
                        break;
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
