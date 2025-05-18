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
        private List<LaderModel> income = new List<LaderModel>();
        private List<LaderModel> cost = new List<LaderModel>();
        //private Dictionary<int, LaderModel> all = new Dictionary<int, LaderModel> (); //全部帳務
        //private Dictionary<int, LaderModel> income = new Dictionary<int, LaderModel>(); //帳務編號, 帳務內容
        //private Dictionary<int, LaderModel> cost = new Dictionary<int, LaderModel>(); //帳務編號, 帳務內容
        public string Name { get { return name; }}
        public int Id { get { return id; } } //帳本編號
        public int TotalMoney { get { return totalmoney; } }
        public int IncomeMoney { get { return incomemoney; } }
        public int CostMoney { get { return costmoney; } }

        public List<LaderModel> Income { get { return income; } }
        public List<LaderModel> Cost { get { return cost; } }

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
                if (money >= 0)
                {
                    incomemoney += money;
                    income.Add(temp);
                }
                else
                {
                    costmoney += money;
                    cost.Add(temp);
                }
                totalmoney += money;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        public void Delete(string title)
        {
            try
            {
                var i = income.Find(x => x.Title == title);
                var j = cost.Find(x => x.Title == title);
                if (i != null)
                {
                    income.Remove(i);
                }
                else if(j != null)
                {
                    cost.Remove(j);
                }
                else
                {
                    throw new Exception("帳務編號不存在");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}
