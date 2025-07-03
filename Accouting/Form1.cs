using System.Linq;

namespace Accounting
{
    public partial class Form1 : Form
    {
        private readonly Controller _control = new Controller();
        public Form1()
        {
            InitializeComponent();
            ShowData();
        }

        public bool IsNum(string text)
        {
            bool result = true;
            try
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char c = text[i];
                    if (i == 0 && c == '-')
                    {
                        continue;
                    }
                    else if (i == 0 && c == '+')
                    {
                        continue;
                    }
                    if (c < '0' || c > '9')
                    {
                        result = false;
                        break;
                    }
                }
                return result;
            }
            catch
            {

                return false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e) //check is number
        {
            if (!IsNum(textBox2.Text))
            {
                MessageBox.Show("Please enter a valid number.");
                textBox2.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e) //添加
        {
            if (textBox1.Text == "" || textBox1.Text == "*")
            {
                MessageBox.Show("Please enter a title.");
                return;
            }
            if (textBox2.Text == "") return;
            DateTime date = DateTime.Now;
            string name = textBox1.Text;
            string type = listBox1.Text;
            string money = textBox2.Text;
            _control.Add(name, type, money);
            listBox1.Items.Add(date + " " + textBox1.Text + " " + textBox2.Text);
            ShowData();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ShowData()
        {
            listBox1.Items.Clear();
            listBox1.Items.Add("收入:");
            var data = _control.GetData("*");
            foreach (var item in data)
            {
                if (item.Money >= 0) listBox1.Items.Add(item.Time + " " + item.Title + " " + item.Money);
            }
            listBox1.Items.Add("支出:");
            foreach (var item in data)
            {
                if (item.Money < 0) listBox1.Items.Add(item.Time + " " + item.Title + " " + item.Money);
            }
        }

        private void button2_Click(object sender, EventArgs e) //Search
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter a title.");
                return;
            }
            var temp = _control.GetData(textBox1.Text);
            if (temp is not null)
            {
                listBox1.Items.Clear();
                foreach (var item in temp)
                {
                    listBox1.Items.Add(item.Time + " " + item.Title + " " + item.Money);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                //MessageBox.Show("Please enter a title.");
                return;
            }
            _control.Delete(textBox1.Text);
            ShowData();
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e) //修改
        {
            if (textBox1.Text == "" || textBox1.Text == "*")
            {
                MessageBox.Show("Please enter a title.");
                return;
            }
            if (textBox2.Text == "") return;
            DateTime date = DateTime.Now;
            string name = textBox1.Text;
            string type = listBox1.Text;
            string money = textBox2.Text;
            _control.Update(name, type, money);
            listBox1.Items.Add(date + " " + textBox1.Text + " " + textBox2.Text);
            ShowData();
        }
    }
}
