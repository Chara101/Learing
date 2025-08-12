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
            { "category_id", "category_id = @category" },
            { "subcategory_id", "subcategory_id = @sid"},
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
                    command.Parameters["@cid"].Value = r.Category_id;
                    command.Parameters["@sid"].Value = r.SubCategory_id;
                    using (var reader = command.ExecuteReader())
                    {
                        if(!reader.HasRows)
                        {
                            sql = "insert into Totals (record_date, category_id, subcategory_id, subcount, subamount) values (GetDate(), @category, @subcategory, 0, 0)";
                            using(var insertCommand = new SqlCommand(sql, _connection))
                            {
                                insertCommand.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                                insertCommand.Parameters.Add("@subcategory", SqlDbType.NVarChar, 50);
                                insertCommand.Parameters["@category"].Value = r.Category_id;
                                insertCommand.Parameters["@subcategory"].Value = r.SubCategory_id;
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
                        sql = "update Totals set subcount = subcount + 1 , subamount = the_total + @amount where category_id = @tcid and subcategory_id = @tsid";
                    }
                    else{
                        sql = "update Totals set subcount = subcount + 1 , subamount = the_total - @amount where category_id = @tcid and subcategory_id = @tsid";
                    }
                    using (SqlCommand command = new SqlCommand(sql, _connection))
                    {
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters.Add("@tcid", SqlDbType.NVarChar, 50);
                        command.Parameters.Add("@tsid", SqlDbType.NVarChar, 50);
                        command.Parameters["@amount"].Value = r.Amount;
                        command.Parameters["@tcid"].Value = r.Category_id;
                        command.Parameters["@tsid"].Value = r.SubCategory_id;
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
                string sql = "insert into Record (record_date, category_id, subcategory_id, record_amount, description) values (GetDate(), @cid, @scid, @amount, @comment);";
                using(SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@cid", SqlDbType.Int);
                    command.Parameters.Add("@scid", SqlDbType.Int);
                    command.Parameters.Add("@amount", SqlDbType.Int);
                    command.Parameters.Add("@comment", SqlDbType.NVarChar, 50);
                    command.Parameters["@cid"].Value = r.Category_id;
                    command.Parameters["@scid"].Value = r.SubCategory_id;
                    command.Parameters["@amount"].Value = r.Amount;
                    command.Parameters["@comment"].Value = r.Comment ?? "";
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
                string sql = "delete from Record where";
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
                    if (r.Date != DateTime.MinValue)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = r.Date;
                        count++;
                    }
                    if (r.Category_id != 0)
                    {
                        command.CommandText += AddLogic(_condition["category_id"], count > 0);
                        command.Parameters.Add("@category_id", SqlDbType.NVarChar, 50);
                        command.Parameters["@category_id"].Value = r.Category;
                        count++;
                        if(r.SubCategory_id != 0)
                        {
                            command.CommandText += AddLogic(_condition["subcategory_id"], count > 0);
                            command.Parameters.Add("@subcategory_id", SqlDbType.NVarChar, 50);
                            command.Parameters["@subcategory_id"].Value = r.SubCategory;
                            count++;
                        }
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
                string sql = "SELECT * FROM Record";
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
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                Category = reader["record_category"].ToString() ?? "",
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                                SubCategory = reader["record_subcategory"].ToString() ?? "",
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
                string sql = "SELECT * FROM Record as R where";
                string sql2 = "join CategoryList as C on R.categoryid = C.category_id join SubCategoryList as S on S.category_id = R.category_id join UserLIst as U on U.user_id = R.user_id;";
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
                    if (r.Date != DateTime.MinValue)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = r.Date;
                        count++;
                    }
                    if (!string.IsNullOrEmpty(r.Category))
                    {
                        command.CommandText += AddLogic(_condition["category_id"], count > 0);
                        command.Parameters.Add("@category_id", SqlDbType.NVarChar, 50);
                        command.Parameters["@category_id"].Value = r.Category;
                        count++;
                        if(r.SubCategory_id != 0)
                        {
                            command.CommandText += AddLogic(_condition["subcategory_id"], count > 0);
                            command.Parameters.Add("@subcategory_id", SqlDbType.NVarChar, 50);
                            command.Parameters["@subcategory_id"].Value = r.SubCategory;
                            count++;
                        }
                    }
                    if (r.Amount != 0)
                    {
                        command.CommandText += AddLogic(_condition["money"], count > 0);
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = r.Amount;
                        count++;
                    }
                    if (count == 0) throw new ArgumentException("Invalid argument for search.");
                    command.CommandText += sql2;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RecordForm record = new RecordForm
                            {
                                Id = Convert.ToInt32(reader["record_id"]),
                                Date = Convert.ToDateTime(reader["record_date"]),
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                Category = reader["category_name"].ToString() ?? "",
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                                SubCategory = reader["subcategory_name"].ToString() ?? "",
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
                string sql = "SELECT * FROM Record where";
                string sql2 = "join CategoryList as C on R.categoryid = C.category_id join SubCategoryList as S on S.category_id = R.category_id join UserLIst as U on U.user_id = R.user_id;";
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
                    if (DateTime.MinValue.Date <= r1.Date && r1.Date <= r2.Date)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = r1.Date;
                        command.CommandText += AddLogic(_condition["time"] + "2", count > 0);
                        command.Parameters.Add("@date2", SqlDbType.DateTime);
                        command.Parameters["@date2"].Value = r2.Date;
                        count++;
                    }
                    if (r1.Category_id != 0 && r2.Category_id != 0)
                    {
                        command.CommandText += AddLogic(_condition["category_id"], count > 0);
                        command.Parameters.Add("@category", SqlDbType.NVarChar, 50);
                        command.Parameters["@category"].Value = r1.Category;
                        command.CommandText += AddLogic(_condition["category_id"] + "2", count > 0);
                        command.Parameters.Add("@category_id2", SqlDbType.NVarChar, 50);
                        command.Parameters["@category_id2"].Value = r2.Category;
                        count++;
                        if (r1.SubCategory_id != 0 && r2.SubCategory_id != 0)
                        {
                            command.CommandText += AddLogic(_condition["subcategory_id"], count > 0);
                            command.Parameters.Add("@subcategory_id", SqlDbType.NVarChar, 50);
                            command.Parameters["@subcategory_id"].Value = r1.SubCategory;
                            command.CommandText += AddLogic(_condition["subcategory_id"] + "2", count > 0);
                            command.Parameters.Add("@subcategory_id2", SqlDbType.NVarChar, 50);
                            command.Parameters["@subcategory_id2"].Value = r2.SubCategory;
                            count++;
                        }
                    }
                    if (r1.Amount != 0 && r2.Amount >= r1.Amount)
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
                                Category_id = Convert.ToInt32(reader["category_id"]),
                                Category = reader["category_name"].ToString() ?? "",
                                SubCategory_id = Convert.ToInt32(reader["subcategory_id"]),
                                SubCategory = reader["subcategory_name"].ToString() ?? "",
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
        public RecordForm GetTotals(RecordForm r)
        {
            RecordForm totals = new RecordForm();
            string sql = "select Sum(subcount) as times, Sum(sub_amount) as amount from Totals";
            int count = 0;
            try
            {
                using(var command = new SqlCommand(sql, _connection))
                {
                    if (r.Date != DateTime.MinValue)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = r.Date;
                        count++;
                    }
                    if (!string.IsNullOrEmpty(r.Category))
                    {
                        command.CommandText += AddLogic(_condition["category_id"], count > 0);
                        command.Parameters.Add("@category_id", SqlDbType.NVarChar, 50);
                        command.Parameters["@category_id"].Value = r.Category;
                        count++;
                        if (r.SubCategory_id != 0)
                        {
                            command.CommandText += AddLogic(_condition["subcategory_id"], count > 0);
                            command.Parameters.Add("@subcategory_id", SqlDbType.NVarChar, 50);
                            command.Parameters["@subcategory_id"].Value = r.SubCategory;
                            count++;
                        }
                    }
                    if (r.Amount != 0)
                    {
                        command.CommandText += AddLogic(_condition["money"], count > 0);
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = r.Amount;
                        count++;
                    }
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totals.SubCount = Convert.ToInt32(reader["times"]);
                            totals.SubAmount = Convert.ToInt32(reader["amount"]);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("GetTotals failed." + e.Message);
            }
            return totals;
        }
        public void Update(RecordForm r, RecordForm content)
        {
            try
            {
                if(_connection == null || _connection.State == ConnectionState.Open) throw new InvalidOperationException("Connection is not initialized.");
                string sql = "update Record set (record_date = @ndate, category_id = @ncategory_id, subcategory_id = @nsubcategory_id, record_amount = @namount, description = @ndescription) where ";
                int count = 0;
                using(SqlCommand command = new SqlCommand(sql, _connection))
                {
                    if (r.Id <= 0)
                    {
                        command.CommandText += AddLogic(_condition["id"], count > 0);
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = r.Id;
                        count++;
                    }
                    if (r.Date != DateTime.MinValue)
                    {
                        command.CommandText += AddLogic(_condition["time"], count > 0);
                        command.Parameters.Add("@date", SqlDbType.DateTime);
                        command.Parameters["@date"].Value = r.Date;
                        count++;
                    }
                    if (!string.IsNullOrEmpty(r.Category))
                    {
                        command.CommandText += AddLogic(_condition["category_id"], count > 0);
                        command.Parameters.Add("@category_id", SqlDbType.NVarChar, 50);
                        command.Parameters["@category_id"].Value = r.Category;
                        count++;
                        if (r.SubCategory_id != 0)
                        {
                            command.CommandText += AddLogic(_condition["subcategory_id"], count > 0);
                            command.Parameters.Add("@subcategory_id", SqlDbType.NVarChar, 50);
                            command.Parameters["@subcategory_id"].Value = r.SubCategory;
                            count++;
                        }
                    }
                    if (r.Amount != 0)
                    {
                        command.CommandText += AddLogic(_condition["money"], count > 0);
                        command.Parameters.Add("@amount", SqlDbType.Int);
                        command.Parameters["@amount"].Value = r.Amount;
                        count++;
                    }
                    else throw new ArgumentException("Invalid argument for update.");
                    command.Parameters.Add("@ndate", SqlDbType.DateTime);
                    command.Parameters.Add("@ncategory_id", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@nsubcategory_id", SqlDbType.NVarChar, 50);
                    command.Parameters.Add("@namount", SqlDbType.Int);
                    command.Parameters.Add("@ndescript", SqlDbType.NVarChar, 50);
                    command.Parameters["@ndate"].Value = content.Date;
                    command.Parameters["@ncategory"].Value = content.Category_id;
                    command.Parameters["@nsubcategory"].Value = content.SubCategory_id;
                    command.Parameters["@namount"].Value = content.Amount;
                    command.Parameters["@ndescription"].Value = content.Comment ?? "";
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Update failed." + e.Message);
            }
    }
}
