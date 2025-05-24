using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Accounting
{
    class Lader
    {
        private string name;
        private int id;
        private int totalmoney = 0;
        private int incomemoney = 0;
        private int costmoney = 0;
        private int countid = 1; //帳務編號
        private List<LaderModel> all = new List<LaderModel>(); //全部帳務
        //private Dictionary<int, LaderModel> income = new Dictionary<int, LaderModel>(); //帳務編號, 帳務內容
        //private Dictionary<int, LaderModel> cost = new Dictionary<int, LaderModel>(); //帳務編號, 帳務內容
        public string Name { get { return name; }}
        public int Id { get { return id; } } //帳本編號
        public int TotalMoney { get { return totalmoney; } }
        public int IncomeMoney { get { return incomemoney; } }
        public int CostMoney { get { return costmoney; } }

        public List<LaderModel> All { get { return all; } }

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

        public void Add(string time, string title, int money)
        {
            LaderModel? temp = null;
            try
            {
                if (title is not null)
                {
                    temp = new LaderModel(countid);
                    temp.Time = time;
                    temp.Title = title;
                    temp.Money = money;
                    countid++;
                }
                else
                {
                    throw new Exception("Required title.");
                }
                all.Add(temp);
                totalmoney += money;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
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

        public List<LaderModel> Search(string title)
        {
            List<LaderModel> temp = new List<LaderModel>();
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
                List<LaderModel> error = new List<LaderModel>();
                error.Add(new LaderModel(0));
                error[0].Time = "Error";
                error[0].Title = "Error";
                error[0].Money = 0;
                return error;
            }
        }

        public List<LaderModel> SearchAll(string title)
        {
            List<LaderModel> temp = new List<LaderModel>();
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
                List<LaderModel> error = new List<LaderModel>();
                error.Add(new LaderModel(0));
                error[0].Time = "Error";
                error[0].Title = "Error";
                error[0].Money = 0;
                return error;
            }
        }
    }
}
