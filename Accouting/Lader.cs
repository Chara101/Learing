using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounting
{
    class Lader
    {
        private string name;
        private int id;
        private int totalmoney = 0;
        private int incomemoney = 0;
        private int costmoney = 0;
        private List<LaderModel> income;
        private List<LaderModel> cost;
        public string Name { get { return name; }}
        public int Id { get { return id; } }
        public int TotalMoney { get { return totalmoney; } }
        public int IncomeMoney { get { return incomemoney; } }
        public int CostMoney { get { return costmoney; } }

        public List<LaderModel> Income { get { return income; } }
        public List<LaderModel> Cost { get { return cost; } }

        public Lader()
        {
            name = "Default";
            id = 0;
            income = new List<LaderModel>();
            cost = new List<LaderModel>();
        }
        public Lader(string name, int id)
        {
            this.name = name;
            this.id = id;
            income = new List<LaderModel>();
            cost = new List<LaderModel>();
        }

        public void Add(string time, string title, int money)
        {
            LaderModel? temp = null;
            try
            {
                if (title is not null)
                {
                    temp = new LaderModel();
                    temp.Time = time;
                    temp.Title = title;
                    temp.Money = money;
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
    }
}
