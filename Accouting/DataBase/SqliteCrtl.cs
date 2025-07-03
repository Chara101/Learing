using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace Accounting.DataBase
{
    internal class SqliteCrtl : IDataBase
    {
        private int _countid = 0;
        SqliteConnection? connection;

        private int ID { get => _countid++; }
        ~SqliteCrtl()
        {
            if(connection is not null)
            {
                connection.Close();
                connection.Dispose();
            }
        }
        public void Initialize()
        {
            try
            {
                string sql = "CREATE TABLE IF NOT EXISTS Money (ID INTEGER PRIMARY KEY AUTOINCREMENT, time TEXT, Title TEXT, Type TEXT, amount INTEGER);";
                connection = new SqliteConnection("Data Source=\"C:\\Lab\\test.db\";");
                connection.Open();
            }
            catch(Exception e)
            {
                MessageBox.Show("Initialize failed." + e.Message);
            }
        }
        public void Insert(Record r)
        {
            try
            {
                if (connection is null) throw new Exception("Require Initialize");
                string sql = $"INSERT INTO Money (ID,time, Title, Type, amount) VALUES (@id, @time, @title, @type, @money);";
                var command = new SqliteCommand(sql, connection);
                command.Parameters.AddWithValue("@id", ID);
                command.Parameters.AddWithValue("@time", DateTime.Now);
                command.Parameters.AddWithValue("@title", r.Title);
                command.Parameters.AddWithValue("@type", r.Type);
                command.Parameters.AddWithValue("@money", r.Money);
                SqliteDataReader data = command.ExecuteReader();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public List<Record> GetAll()
        {
            List<Record> result = new List<Record>();
            try
            {
                if (connection is null) throw new Exception("Require Initialize");
                string sql = "SELECT * FROM Money";
                using (var command = new SqliteCommand(sql, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Record(reader["title"].ToString() ?? ""
                            )
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                //Time = DateTime.Parse(reader["time"].ToString() ?? ""),
                                Type = reader["type"].ToString() ?? "",
                                Money = Convert.ToInt32(reader["amount"])
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }
        public List<Record> SelectByTitle(Record r)
        {
            List<Record> result = new List<Record>();
            try
            {
                if(connection is null) throw new Exception("Require Initialize");
                string sql = "SELECT TOP 1 * FROM Money WHERE title = @title";
                if(r.Title == "*")
                {
                    sql = "Select * From Money;";
                }
                using (var command = new SqliteCommand(sql, connection))
                {
                    if(r.Title != "*") command.Parameters.AddWithValue("@title", r.Title);
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Record(reader["title"].ToString() ?? ""
                            )
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                //Time = DateTime.Parse(reader["time"].ToString() ?? ""),
                                Type = reader["type"].ToString() ?? "",
                                Money = Convert.ToInt32(reader["amount"])
                            });
                        }
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }
        public List<Record> SelectAllByTitle(Record r)
        {
            List<Record> result = new List<Record>();
            try
            {
                if (connection is null) throw new Exception("Require Initialize");
                string sql = "SELECT * FROM Money WHERE title = @title";
                if (r.Title == "*")
                {
                    sql = "Select * From Money;";
                }
                using (var command = new SqliteCommand(sql, connection))
                {
                    if (r.Title != "*") command.Parameters.AddWithValue("@title", r.Title);
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Record(reader["title"].ToString() ?? ""
                            )
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                //Time = DateTime.Parse(reader["time"].ToString() ?? ""),
                                Type = reader["type"].ToString() ?? "",
                                Money = Convert.ToInt32(reader["amount"])
                            });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return result;
        }
        public void Update(Record r)
        {
            string sql = "UPDATE Money SET type = @type, amount = @money WHERE title = @title;";
            try
            {
                using(SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@type", r.Type);
                    command.Parameters.AddWithValue("@money", r.Money);
                    command.Parameters.AddWithValue("@title", r.Title);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void UpdateAll(Record r)
        {
            string sql = "UPDATE Money SET type = @type, amount = @money WHERE title = @title;";
            try
            {
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@type", r.Type);
                    command.Parameters.AddWithValue("@money", r.Money);
                    command.Parameters.AddWithValue("@title", r.Title);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void Clear()
        {
            string sql = "TRUNCATE TABLE Money;";
            try
            {
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void Delete(Record r)
        {
            //string sql = "DELETE FROM Money WHERE type = @type OR amount = @money OR title = @title;";
            string sql = "DELETE FROM Money WHERE title = @title;";
            try
            {
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    //command.Parameters.AddWithValue("@type", r.Type);
                    //command.Parameters.AddWithValue("@money", r.Money);
                    command.Parameters.AddWithValue("@title", r.Title);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void DeleteAll(Record r)
        {
            //string sql = "DELETE FROM Money WHERE type = @type OR amount = @money OR title = @title;";
            string sql = "DELETE FROM Money WHERE title = @title;";
            try
            {
                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    //command.Parameters.AddWithValue("@type", r.Type);
                    //command.Parameters.AddWithValue("@money", r.Money);
                    command.Parameters.AddWithValue("@title", r.Title);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
