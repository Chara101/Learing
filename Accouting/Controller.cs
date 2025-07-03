using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataBase;
using Accounting.View;

namespace Accounting
{
    class Controller
    {
        private readonly IDataBase _db = new SqlserverCtrl();
        private IViewer _viewer = new BasicView();
        private ListBox _showbox;
        public Controller()
        {
            _showbox = new ListBox();
            Initialize();
        }
        public Controller(ListBox showbox)
        {
            _showbox = showbox;
            Initialize();
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
                r.Time = time;
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
                _viewer.ShowAllRecord(_db.GetAll(), _showbox);
            }
            catch (Exception e)
            {
               
            }
        }

        public List<Record> GetData(string title)
        {
            try
            {
                if (title == "*")
                {
                    return _db.GetAll();
                }
                Record r = new Record(title);
                var result = _db.SelectByTitle(r);
                return result;
            }
            catch
            {
                return new List<Record>();
            }
        }

        public void Update(string title, string type, string money)
        {
            try
            {
                Record r = new Record(title);
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
            }
            catch
            {
                MessageBox.Show("Error updating record. Please check your input.");
            }
        }
    }
}
