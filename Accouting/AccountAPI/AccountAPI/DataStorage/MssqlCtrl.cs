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
        private void Initialize_Totals(RecordForm r)
        {
            try
            {
                string sql = "select count(*) from Totals where record_date = GetDate() and category_id = @cid and subcategory_id = @sid";
                using(var command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@cid", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@sid", SqlDbType.NVarChar, 50);
                    command.Parameters["@cid"].Value = r.Category;
                    command.Parameters["@sid"].Value = r.EventType;
                    using (var reader = command.ExecuteReader())
                    {
                        if(!reader.HasRows)
                        {
                            sql = "insert into Totals (record_date, category_id, subcategory_id, subcount, subamount) values (GetDate(), @category, @subcategory, 0, 0)";
                            using(var insertCommand = new SqlCommand(sql, _connection))
                            {
                                insertCommand.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                                insertCommand.Parameters.Add("@subcategory", SqlDbType.NVarChar, 50);
                                insertCommand.Parameters["@category"].Value = r.Category;
                                insertCommand.Parameters["@subcategory"].Value = r.EventType;
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update_totals failed." + e.Message);
            }
        }
        private void Update_totals(RecordForm r, bool pos)
        {
            if (r.Amount > 0)
            {
                try
                {
                    string sql;
                    Initialize_Totals(r);
                    if (pos)
                    {
                        sql = "update Totals set subcount = subcount + 1 , subamount = the_total + @amount where category_id = @cid and subcategory_id = @sid";
                    }
                    else{
                        sql = "update Totals set subcount = subcount + 1 , subamount = the_total - @amount where category_id = @cid and subcategory_id = @sid";
                    }
                    using (SqlCommand command = new SqlCommand(sql, _connection))
                    {
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters.Add("@cid", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@sid", SqlDbType.NVarChar, 50);
                        command.Parameters["@amount"].Value = r.Amount;
                        command.Parameters["@@cid"].Value = r.Category;
                        command.Parameters["@id"].Value = r.EventType ?? "";
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Update_totals failed." + e.Message);
                }
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
        public void Remove(RecordForm r)
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
        public List<RecordForm> GetRecordsBy(RecordForm r)
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
        public void Update(RecordForm target, RecordForm content)
        {
            try
            {
                if(_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection is not initialized.");
                string sql = "update Ledger1 set (record_date = @ndate, record_title = @ntitle, record_category = @ncategory, record_type = @ntype, user_name = @nname, record_amount = @namount, descript = @ndescript) where ";
                int count = 0;
                using(SqlCommand command = new SqlCommand(sql, _connection))
                {
                    if (target.Id <= 0)
                    {
                        command.CommandText += AddLogic(_condition["id"], count > 0);
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = target.Id;
                        count++;
                    }
                    else if (!string.IsNullOrEmpty(target.Title))
                    {
                        command.CommandText += AddLogic(_condition["title"], count > 0);
                        command.Parameters.Add("@title", SqlDbType.NVarChar, 50);
                        command.Parameters["@title"].Value = target.Title;
                        count++;
                    }
                    else if (target.Date != DateTime.MinValue)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = target.Date;
                        count++;
                    }
                    else if (!string.IsNullOrEmpty(target.Category))
                    {
                        command.CommandText += AddLogic(_condition["category"], count > 0);
                        command.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                        command.Parameters["@category"].Value = target.Category;
                        count++;
                    }
                    else if (target.Amount != 0)
                    {
                        command.CommandText += AddLogic(_condition["money"], count > 0);
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = target.Amount;
                        count++;
                    }
                    else throw new ArgumentException("Invalid argument for update.");
                    command.Parameters.Add("@ndate", SqlDbType.DateTime);
                    command.Parameters.Add("@ntitle", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@ncategory", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@ntype", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@nname", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@namount", SqlDbType.Int);
                    command.Parameters.Add("@ndescript", SqlDbType.NVarChar, 50);
                    command.Parameters["@date"].Value = content.Date;
                    command.Parameters["@title"].Value = content.Title;
                    command.Parameters["@category"].Value = content.Category;
                    command.Parameters["@type"].Value = content.EventType ?? "";
                    command.Parameters["@name"].Value = content.Title;
                    command.Parameters["@amount"].Value = content.Amount;
                    command.Parameters["@descript"].Value = content.Comment ?? "";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update failed." + e.Message);
            }
    }
}
