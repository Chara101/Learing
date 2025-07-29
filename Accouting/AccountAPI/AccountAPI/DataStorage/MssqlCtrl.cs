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
                    else if (string.IsNullOrEmpty(r.Title))
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
                    else if (string.IsNullOrEmpty(r.Title))
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
        public List<RecordForm> GetRecordsBy(RecordForm r1, RecordForm r2, ETarget target)
        {
            List<RecordForm> records = new List<RecordForm>();
            try
            {
                if (_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection failed.");
                RecordForm temp = new RecordForm();
                switch (target)
                {
                    case ETarget.id:
                        temp.Id = r1.Id;
                        break;
                    case ETarget.time:
                        temp.Date = r1.Date;
                        break;
                    case ETarget.title:
                        temp.Title = r1.Title;
                        break;
                    case ETarget.category:
                        temp.Category = r1.Category;
                        break;
                    default:
                        throw new ArgumentException("Invalid target for retrieval.");
                }
                string sql = "SELECT * FROM Ledger1 where";
                int count = 0;
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    if (r1.Id <= 0)
                    {
                        command.CommandText += AddLogic(_condition["id"], count > 0);
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r.Id;
                        count++;
                    }
                    else if (string.IsNullOrEmpty(r.Title))
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
                    command.Parameters.Add("@id", SqlDbType.Int);
                    command.Parameters.Add("@date", SqlDbType.DateTime);
                    command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@type", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@id2", SqlDbType.Int);
                    command.Parameters.Add("@date2", SqlDbType.DateTime);
                    command.Parameters.Add("@title2", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@category2", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@type2", SqlDbType.NVarChar, 50);
                    command.Parameters["@id"].Value = temp.Id;
                    command.Parameters["@date"].Value = temp.Date;
                    command.Parameters["@title"].Value = temp.Title;
                    command.Parameters["@category"].Value = temp.Category;
                    command.Parameters["@type"].Value = temp.EventType ?? "";
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
        int GetTotals(RecordForm r);
        void Update(RecordForm r, ETarget target);
    }
}
