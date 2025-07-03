using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Identity.Client;

namespace Accounting.DataBase
{
    internal class SqlserverCtrl : IDataBase
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
                MessageBox.Show("Initialize failed." + e.Message);
            }
        }
        public void Insert(Record r)
        {
            try
            {
                if (_connection is null) throw new Exception("Require Initialize");
                string sql = $"INSERT INTO Money1 (record_time, title, type, money) VALUES (@time, @title, @type, @money );";
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    //command.Parameters.Add("@id", SqlDbType.Int);
                    //command.Parameters["@id"].Value = r.Id;
                    command.Parameters.Add("@time", SqlDbType.DateTime);
                    command.Parameters["@time"].Value = r.Time;
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters["@title"].Value = r.Title;
                    command.Parameters.Add("@type", SqlDbType.NVarChar);
                    command.Parameters["@type"].Value = r.Type;
                    command.Parameters.Add("@money", SqlDbType.Int);
                    command.Parameters["@money"].Value = r.Money;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Insert failed " + e.Message);
            }
        }
        public List<Record> GetAll()
        {
            List<Record> result = new List<Record>();
            string sql = "SELECT * FROM Money1";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(new Record(reader["title"].ToString() ?? "")
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Time = Convert.ToDateTime(reader["record_time"]),
                            Type = reader["type"].ToString() ?? "",
                            Money = Convert.ToInt32(reader["money"])
                        });
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("GetAll failed " + e.Message);
            }
            return result;
        }
        public List<Record> SelectByTitle(Record r)
        {
            List<Record> result = new List<Record>();
            string sql = "SELECT TOP 1 * FROM Money1 WHERE title = @title";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters["@title"].Value = r.Title;
                    command.Parameters.Add("@choice", SqlDbType.Int);
                    command.Parameters["@choice"].Value = (r.Title == "*" ? 1 : 0);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Record(reader["title"].ToString() ?? "")
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Time = Convert.ToDateTime(reader["record_time"]),
                                Type = reader["type"].ToString() ?? "",
                                Money = Convert.ToInt32(reader["money"])
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Select failed " + e.Message);
            }
            return result;
        }

        public List<Record> SelectAllByTitle(Record r)
        {
            List<Record> result = new List<Record>();
            string sql = "SELECT * FROM Money1 WHERE title = @title";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters["@title"].Value = r.Title;
                    command.Parameters.Add("@choice", SqlDbType.Int);
                    command.Parameters["@choice"].Value = (r.Title == "*" ? 1 : 0);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Record(reader["title"].ToString() ?? "")
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Time = Convert.ToDateTime(reader["record_time"]),
                                Type = reader["type"].ToString() ?? "",
                                Money = Convert.ToInt32(reader["money"])
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Select failed " + e.Message);
            }
            return result;
        }
        public void Update(Record r)
        {
            string sql = "UPDATE Money1 Set record_time = @time, type = @type, money = @money WHERE Id IN ( SELECT max(Id) FROM Money1 WHERE title = @title )";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@time", SqlDbType.DateTime);
                    command.Parameters["@time"].Value = DateTime.Now;
                    command.Parameters.Add("@type", SqlDbType.NVarChar);
                    command.Parameters["@type"].Value = r.Type;
                    command.Parameters.Add("@money", SqlDbType.Int);
                    command.Parameters["@money"].Value = r.Money;
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters["@title"].Value = r.Title;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Update failed " + e.Message);
            }
        }

        public void UpdateAll(Record r)
        {
            string sql = "UPDATE Money1 Set record_time = @time, type = @type, money = @money WHERE title = @title";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@time", SqlDbType.DateTime);
                    command.Parameters["@time"].Value = DateTime.Now;
                    command.Parameters.Add("@type", SqlDbType.NVarChar);
                    command.Parameters["@type"].Value = r.Type;
                    command.Parameters.Add("@money", SqlDbType.Int);
                    command.Parameters["@money"].Value = r.Money;
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters["@title"].Value = r.Title;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Update failed " + e.Message);
            }
        }

        public void Clear()
        {
            string sql = "TRUNCATE TABLE Money1";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Clear failed " + e.Message);
            }
        }
        public void Delete(Record r)
        {
            string sql = "DELETE FROM Money1 WHERE Id IN ( SELECT max(Id) FROM Money1 WHERE title = @title)";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters["@title"].Value = r.Title;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Delete failed " + e.Message);
            }
        }

        public void DeleteAll(Record r)
        {
            string sql = "DELETE FROM Money1 WHERE title = @title";
            try
            {
                using (SqlCommand command = new SqlCommand(sql, _connection))
                {
                    command.Parameters.Add("@title", SqlDbType.NVarChar);
                    command.Parameters["@title"].Value = r.Title;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Delete failed " + e.Message);
            }
        }
    }
}
