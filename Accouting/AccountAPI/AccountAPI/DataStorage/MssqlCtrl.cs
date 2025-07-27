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
                RecordForm temp = new RecordForm();
                if(target == ETarget.id) temp.Id = r.Id;
                else if(target == ETarget.time) temp.Date = r.Date;
                else if(target == ETarget.title) temp.Title = r.Title;
                else throw new ArgumentException("Invalid target for removal.");
                string sql = "delete from Ledger1 where record_id == @id || record_title = @title || record_date <= @date";
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int);
                    command.Parameters.Add("@date", SqlDbType.DateTime);
                    command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                    command.Parameters["@id"].Value = temp.Id;
                    command.Parameters["@date"].Value = temp.Date;
                    command.Parameters["@title"].Value = temp.Title;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Remove failed." + e.Message);
            }
        }
        public void Remove(RecordForm r, ETarget target1, ETarget target2)
        {
            try
            {
                if (_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection is not initialized.");
                if(target1 != ETarget.title || target2 != ETarget.time) throw new ArgumentException("Invalid target combination for removal.");
                string sql = "delete from Ledger1 where record_title = @title && record_date <= @date";
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@date", SqlDbType.DateTime);
                    command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                    command.Parameters["@date"].Value = r.Date;
                    command.Parameters["@title"].Value = r.Title;
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
                RecordForm temp = new RecordForm();
                switch (target)
                {
                    case ETarget.id:
                        temp.Id = r.Id;
                        break;
                    case ETarget.time:
                        temp.Date = r.Date;
                        break;
                    case ETarget.title:
                        temp.Title = r.Title;
                        break;
                    case ETarget.category:
                        temp.Category = r.Category;
                        break;
                    default:
                        throw new ArgumentException("Invalid target for retrieval.");
                }
                string sql = "SELECT * FROM Ledger1 where record_id = @id || record_date = @date || record_title = @title || record_category = @category || record_type = @type";
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int);
                    command.Parameters.Add("@date", SqlDbType.DateTime);
                    command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@type", SqlDbType.NVarChar, 50);
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
                string sql = "SELECT * FROM Ledger1 where record_id between ( @id , @id2 ) || record_date between ( @date , @date2 ) || (record_title = @title || record_title = @title2) || record_category = @category || record_category = @category2 || " +
                    "record_type = @type || record_type = @type2 ;";
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
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
