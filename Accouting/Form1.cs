using System.Linq;

namespace Accounting
{
    public partial class Form1 : Form
    {
        private Lader lader = new Lader();
        public Form1()
        {
            InitializeComponent();
            ShowData();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < textBox2.Text.Length; i++)
            {
                char c = textBox2.Text[i];
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
                    MessageBox.Show("Please enter a number.");
                    textBox2.Text = "";
                    break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e) //添加
        {
            DateTime now = DateTime.Now;
            string date = now.ToString();
            if (textBox1.Text == "")
            {
                //MessageBox.Show("Please enter a title.");
                return;
            }
            if (textBox2.Text == "") return;
            lader.Add(date, textBox1.Text, int.Parse(textBox2.Text));
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
            foreach (var item in lader.All)
            {
                if(item.Money >= 0) listBox1.Items.Add(item.Time + " " + item.Title + " " + item.Money);
            }
            listBox1.Items.Add("支出:");
            foreach (var item in lader.All)
            {
                if (item.Money < 0) listBox1.Items.Add(item.Time + " " + item.Title + " " + item.Money);
            }
        }

        private void button2_Click(object sender, EventArgs e) //Search
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Enter name");
                return;
            }
            List<LaderModel>? temp = null;
            if (label2.Text == "Yes")
            {
                temp = lader.SearchAll(textBox1.Text);
            }
            else
            {
                temp = lader.Search(textBox1.Text);
            }
            if(temp is not null)
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
            lader.Delete(textBox1.Text);
            ShowData();
        }

        private void button4_Click(object sender, EventArgs e) //switch range
        {
            string temp = label2.Text;
            if(temp == "No") label2.Text = "Yes";
            else label2.Text = "No";
        }
    }
}
