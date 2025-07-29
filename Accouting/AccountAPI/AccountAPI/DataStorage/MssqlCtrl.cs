using System.Data.Common;
using System.Data;
using AccountAPI.EnumList;
using AccountAPI.Models;
using Microsoft.Data.SqlClient;
using TestAcounting.DataStorage;

namespace AccountAPI.DataStorage
{
    public class MssqlCtrl : IDataStorage
    {
        private SqlConnection? _connection;
        private Dictionary<string, string> _condition = new Dictionary<string, string>()
        {
            { "id", "record_id = @id" },
            { "time", "record_date = @date" },
            { "title", "record_title = @title" },
            { "category", "record_category = @category" },
            { "money", "record_amount = @amount" }
        };
        private Dictionary<string, int> _record_totals = new Dictionary<string, int>()
        {
            { "收入", 0 },
            { "費用", 0 },
            { "資產", 0 },
            { "負債", 0 },
            { "權益", 0 }
        };
        private void Update_totals(RecordForm r, bool pos)
        {
            if (r.Amount > 0)
            {
                if(pos) _record_totals["money"] += r.Amount;
                else _record_totals["money"] -= r.Amount;
            }
            else return;
        }
        private string AddLogic(string org, bool j)
        {
            return j ? " and " + org : org;
        }
        public void Initialize()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "MSI";
                builder.InitialCatalog = "Cash";
                builder.UserID = "Apple";
                builder.Password = "ApplePen";

                string connectionString = builder.ConnectionString;
                _connection = new SqlConnection(connectionString);
                _connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Initialize failed." + e.Message);
            }
        }
        public void Add(RecordForm r)
        {
            try
            {
                if (_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection is not initialized.");
                string sql = "INSERT INTO Ledger1 (record_date, record_title, record_category, record_type, user_name, record_amount, descript) VALUES (@date, @title, @category, @type, @name, @amount, @descript)";
                using(SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@date", SqlDbType.DateTime);
                    command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@type", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@name", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@amount", SqlDbType.Int);
                    command.Parameters.Add("@descript", SqlDbType.NVarChar, 50);
                    command.Parameters["@date"].Value = r.Date;
                    command.Parameters["@title"].Value = r.Title;
                    command.Parameters["@category"].Value = r.Category;
                    command.Parameters["@type"].Value = r.EventType ?? "";
                    command.Parameters["@name"].Value = r.Title;
                    command.Parameters["@amount"].Value = r.Amount;
                    command.Parameters["@descript"].Value = r.Comment ?? "";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Add failed." + e.Message);
            }
        }
        public void Remove(RecordForm r, ETarget target)
        {
            try
            {
                if(_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection is not initialized.");
                string sql = "delete from Ledger1 where";
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    if (r.Id > 0)
                    {
                        command.CommandText += AddLogic(_condition["id"], count > 0);
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r.Id;
                        count++;
                    }
                    else if (!string.IsNullOrEmpty(r.Title))
                    {
                        command.CommandText += AddLogic(_condition["title"], count > 0);
                        command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                        command.Parameters["@title"].Value = r.Title;
                        count++;
                    }
                    else if (r.Date != DateTime.MinValue)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = r.Date;
                        count++;
                    }
                    else if (!string.IsNullOrEmpty(r.Category))
                    {
                        command.CommandText += AddLogic(_condition["category"], count > 0);
                        command.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                        command.Parameters["@category"].Value = r.Category;
                        count++;
                    }
                    if(count == 0) throw new ArgumentException("Invalid argument for removal.");
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Remove failed." + e.Message);
            }
        }
        public List<RecordForm> GetAllRecords()
        {
            List<RecordForm> records = new List<RecordForm>();
            try
            {
                if (_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection failed.");
                string sql = "SELECT * FROM Ledger1";
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Title = reader["record_title"].ToString() ?? "",
                                Category = reader["record_category"].ToString() ?? "",
                                EventType = reader["record_type"].ToString() ?? "",
                                Amount = Convert.ToInt32(reader["record_amount"]),
                                Comment = reader["descript"].ToString() ?? ""
                            };
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllRecords failed." + e.Message);
            }
            return records;
        }
        public List<RecordForm> GetRecordsBy(RecordForm r, ETarget target)
        {
            List<RecordForm> records = new List<RecordForm>();
            try
            {
                if (_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection failed.");
                string sql = "SELECT * FROM Ledger1 where";
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    if (r.Id <= 0)
                    {
                        command.CommandText += AddLogic(_condition["id"], count > 0);
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r.Id;
                        count++ ;
                    }
                    else if (!string.IsNullOrEmpty(r.Title))
                    {
                        command.CommandText += AddLogic(_condition["title"], count > 0);
                        command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                        command.Parameters["@title"].Value = r.Title;
                        count++;
                    }
                    else if (r.Date != DateTime.MinValue)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = r.Date;
                        count++;
                    }
                    else if (!string.IsNullOrEmpty(r.Category))
                    {
                        command.CommandText += AddLogic(_condition["category"], count > 0);
                        command.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                        command.Parameters["@category"].Value = r.Category;
                        count++;
                    }
                    else if (r.Amount != 0)
                    {
                        command.CommandText += AddLogic(_condition["money"], count > 0);
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = r.Amount;
                        count++;
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Title = reader["record_title"].ToString() ?? "",
                                Category = reader["record_category"].ToString() ?? "",
                                EventType = reader["record_type"].ToString() ?? "",
                                Amount = Convert.ToInt32(reader["record_amount"]),
                                Comment = reader["descript"].ToString() ?? ""
                            };
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllRecords failed." + e.Message);
            }
            return records;
        }
        public List<RecordForm> GetRecordsBy(RecordForm r1, RecordForm r2)
        {
            List<RecordForm> records = new List<RecordForm>();
            try
            {
                if (_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection failed.");
                string sql = "SELECT * FROM Ledger1 where";
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    if (r1.Id <= 0 && r2.Id <= 0)
                    {
                        command.CommandText += AddLogic(_condition["id"], count > 0);
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r1.Id;
                        command.CommandText += AddLogic(_condition["id"] + "2", count > 0);
                        command.Parameters.Add("@id2", SqlDbType.Int);
                        command.Parameters["@id2"].Value = r2.Id;
                        count++;
                    }
                    else if (!string.IsNullOrEmpty(r1.Title) && !string.IsNullOrEmpty(r2.Title))
                    {
                        command.CommandText += AddLogic(_condition["title"], count > 0);
                        command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                        command.Parameters["@title"].Value = r1.Title;
                        command.CommandText += AddLogic(_condition["title"] + "2", count > 0);
                        command.Parameters.Add("@title2", SqlDbType.NVarChar, 50);
                        command.Parameters["@title2"].Value = r2.Title;
                        count++;
                    }
                    else if (DateTime.MinValue.Date <= r1.Date && r1.Date <= r2.Date)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = r1.Date;
                        command.CommandText += AddLogic(_condition["time"] + "2", count > 0);
                        command.Parameters.Add("@date2", SqlDbType.DateTime);
                        command.Parameters["@date2"].Value = r2.Date;
                        count++;
                    }
                    else if (!string.IsNullOrEmpty(r1.Category) && !string.IsNullOrEmpty(r2.Category))
                    {
                        command.CommandText += AddLogic(_condition["category"], count > 0);
                        command.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                        command.Parameters["@category"].Value = r1.Category;
                        command.CommandText += AddLogic(_condition["category"] + "2", count > 0);
                        command.Parameters.Add("@category2", SqlDbType.NVarChar, 50);
                        command.Parameters["@category2"].Value = r2.Category;
                        count++;
                    }
                    else if (r1.Amount != 0 && r2.Amount >= r1.Amount)
                    {
                        command.CommandText += AddLogic(_condition["money"], count > 0);
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = r1.Amount;
                        command.CommandText += AddLogic(_condition["money"] + "2", count > 0);
                        command.Parameters.Add("@amount2", SqlDbType.Int);
                        command.Parameters["@amount2"].Value = r2.Amount;
                        count++;
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Title = reader["record_title"].ToString() ?? "",
                                Category = reader["record_category"].ToString() ?? "",
                                EventType = reader["record_type"].ToString() ?? "",
                                Amount = Convert.ToInt32(reader["record_amount"]),
                                Comment = reader["descript"].ToString() ?? ""
                            };
                            records.Add(record);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetAllRecords failed." + e.Message);
            }
            return records;
        }
        public int GetTotals(RecordForm r)
        {
            int result = 0;
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine("GetTotals failed." + e.Message);
            }
            return result;
        }
        public void Update(RecordForm r)
        {

        }
    }
}
