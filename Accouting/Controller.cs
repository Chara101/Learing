using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataBase;

namespace Accounting
{
    class Controller
    {
        private readonly IDataBase _db = new BasicList();
        private IViewer _viewer = new BasicView();
        private ListBox _showbox;
        public Controller()
        {
            _showbox = new ListBox();
        }
        public Controller(ListBox showbox)
        {
            _showbox = showbox;
        }

        public void Initialize()
        {
            _db.Initialize();
        }

        public void Add(string title, string type, string textmoney)
        {
            DateTime time = DateTime.Now;
            try
            {
                int money = Int32.Parse(textmoney);
                Record r = new Record(title, type, money);
                _db.Insert(r);
                try
                {
                    _showbox.Items.Add(time + " " + title + " " + money);
                }
                catch
                {
                    _showbox.Items.Clear();
                    _showbox.Items.Add("Error: cannot add item to listbox.");
                }
            }
            catch
            {
                _showbox.Items.Add("Invalid money input");
            }
        }

        public void Delete(string title)
        {
            try
            {
                Record r = new Record(title);
                _db.Delete(r);
                //_viewer.ShowAllRecord(_db.Select(new Record("*")), _showbox);
            }
            catch (Exception e)
            {
               
            }
        }

        public List<Record> Search(string title)
        {
            try
            {
                Record r = new Record(title);
                var result = _db.Select(r);
                return result;
            }
            catch
            {
                return new List<Record>();
            }
        }

        public void Update(DateTime d, string title, string type, string money)
        {
            try
            {
                Record r = new Record(title);
                r.Time = d;
                r.Type = type;
                try
                {
                    r.Money = Int32.Parse(money);
                }
                catch
                {
                    Console.WriteLine("Cannot parse money value.");
                }
                _db.Update(r);
                //_lader.Insert(d, title, type, Int32.Parse(money));
            }
            catch
            {

            }
        }
    }
}
