using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Accounting
{
    class Lader : ILader
    {
        private string name;
        private int id;
        private int totalmoney = 0;
        private int incomemoney = 0;
        private int costmoney = 0;
        private int countid = 1; //帳務編號
        private List<Record> all = new List<Record>(); //全部帳務
        //private Dictionary<int, LaderModel> income = new Dictionary<int, LaderModel>(); //帳務編號, 帳務內容
        //private Dictionary<int, LaderModel> cost = new Dictionary<int, LaderModel>(); //帳務編號, 帳務內容
        public string Name { get { return name; }}
        public int Id { get { return id; } } //帳本編號
        public int TotalMoney { get { return totalmoney; } }
        public int IncomeMoney { get { return incomemoney; } }
        public int CostMoney { get { return costmoney; } }

        public List<Record> Records { get { return all; } }

        public Lader()
        {
            name = "Default";
            id = 0;
        }
        public Lader(string name, int id)
        {
            this.name = name;
            this.id = id;
        }

        public void Add(DateTime time, string title, string type, int money)
        {
            if (title == "" || type == "") return;
            if (title is null || type is null) return;
            Record temp = new Record(title);
            temp.Time = time;
            temp.Id = countid++;
            temp.Type = type;
            temp.Money = money;
            all.Add(temp);
        }
        public void Delete(string title)
        {
            try
            {
                for(int i =  all.Count - 1; i >= 0; i--)
                {
                    if (all[i].Title == title)
                    {
                        all.RemoveAt(i);
                    }
                    else if(i == 0)
                    {
                        throw new Exception("帳務編號不存在");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public void DeleteAll(string title)
        {
            try
            {
                int check = 0;
                foreach(var i in all)
                {
                    if (i.Title == title)
                    {
                        all.Remove(i);
                        check++;
                    }
                }
                if(check == 0)
                {
                    throw new Exception("帳務編號不存在");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public List<Record> Search(string title)
        {
            List<Record> temp = new List<Record>();
            try
            {
                for (int i = all.Count - 1; i >= 0; i--)
                {
                    if (all[i].Title == title)
                    {
                        temp.Add(all[i]);
                        break;
                    }
                }
                if (temp.Count == 0) throw new Exception("帳務編號不存在");
                else return temp;
            }
            catch (Exception e)
            {
                List<Record> error = new List<Record>();
                //error.Add(new Record(0));
                error[0].Time = DateTime.Now;
                error[0].Title = "Error";
                error[0].Money = 0;
                return error;
            }
        }

        public List<Record> SearchAll(string title)
        {
            List<Record> temp = new List<Record>();
            try
            {
                for (int i = all.Count - 1; i >= 0; i--)
                {
                    if (all[i].Title == title)
                    {
                        temp.Add(all[i]);
                    }
                }
                if(temp.Count == 0) throw new Exception("帳務編號不存在");
                else return temp;
            }
            catch (Exception e)
            {
                List<Record> error = new List<Record>();
                //error.Add(new Record(0));
                error[0].Time = DateTime.Now;
                error[0].Title = "Error";
                error[0].Money = 0;
                return error;
            }
        }

        public void Insert(DateTime time, string title, string type, int money)
        {
            try
            {
                if (title == "" || type == "") return;
                if (title is null || type is null) return;
                Record temp = new Record(title);
                temp.Time = time;
                temp.Title = title;
                temp.Type = type;
                temp.Money = money;
                for (int i = 0; i < all.Count; i++)
                {
                    if (DateTime.Compare(all[i].Time, temp.Time) > 0)
                    {
                        all.Insert(i, temp);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void Clear()
        {
            try
            {
                foreach (var i in all)
                {
                    i.Title = "NULL";
                    i.Type = "NULL";
                    i.Money = 0;
                }
            }
            catch
            {
                Console.WriteLine("Error: Can not clear all.");
            }
        }
    }
}
