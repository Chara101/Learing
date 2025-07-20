using System.Collections.Generic;
using TestAcounting;
using TestAcounting.DataStorage;
using System.Windows.Forms.DataVisualization;

namespace testAccounting1
{
    public partial class Form1 : Form
    {
        private Controller? _controller;
        public Form1()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            _controller = new Controller();
            try
            {
                _controller.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing controller: {ex.Message}");
            }
            ChartInitialize();
            ShowData();
        }
        //private void ShowData(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
        //        var list = _controller.GetData(new RecordForm(), ETarget.title, ERange.All);
        //        foreach (var temp in list)
        //        {
        //            if (temp.Category == "���J") listBox_income.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
        //            else if (temp.Category == "��X") listBox_cost.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error retrieving data: {ex.Message}");
        //    }
        //}
        private void ChartInitialize()
        {
            chart1.Series.Clear();
            chart1.ChartAreas.Clear();
            chart1.Titles.Clear();
            chart1.ChartAreas.Add(new System.Windows.Forms.DataVisualization.Charting.ChartArea("Main"));
            chart1.Titles.Add("�]�Ȥ��");
            var series = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Income",
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
            };
            try
            {
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                var income = _controller.GetData(new RecordForm { Category = "���J" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                var cost = _controller.GetData(new RecordForm { Category = "�O��" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                var asset = _controller.GetData(new RecordForm { Category = "�겣" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                var liability = _controller.GetData(new RecordForm { Category = "�t��" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                var equity = _controller.GetData(new RecordForm { Category = "�v�q" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                series.Points.AddXY("���J", income);
                series.Points.AddXY("�O��", cost);
                series.Points.AddXY("�겣", asset);
                series.Points.AddXY("�t��", liability);
                series.Points.AddXY("�v�q", equity);
                chart1.Series.Add(series);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing chart: {ex.Message}");
            }
        }
        private void ShowChart()
        {
            try
            {
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                chart1.Series[0].Points[0].YValues[0] = _controller.GetData(new RecordForm { Category = "���J" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Series[0].Points[1].YValues[0] = _controller.GetData(new RecordForm { Category = "�O��" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Series[0].Points[2].YValues[0] = _controller.GetData(new RecordForm { Category = "�겣" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Series[0].Points[3].YValues[0] = _controller.GetData(new RecordForm { Category = "�t��" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Series[0].Points[4].YValues[0] = _controller.GetData(new RecordForm { Category = "�v�q" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing chart: {ex.Message}");
            }
        }
        private void ShowData()
        {
            listBox_income.Items.Clear();
            listBox_cost.Items.Clear();
            try
            {
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                var list = _controller.GetData(new RecordForm(), ETarget.title, ERange.All);
                foreach (var temp in list)
                {
                    if (temp.Category == "�겣" || temp.Category == "�O��") listBox_income.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
                    else if (temp.Category == "���J" || temp.Category == "�t��" || temp.Category == "�v�q") listBox_cost.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving data: {ex.Message}");
            }
        }
        private void ShowData(List<RecordForm> lr)
        {
            listBox_income.Items.Clear();
            listBox_cost.Items.Clear();
            try
            {
                foreach (var temp in lr)
                {
                    if (temp.Category == "�겣" || temp.Category == "�O��") listBox_income.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
                    else if (temp.Category == "���J" || temp.Category == "�t��" || temp.Category == "�v�q") listBox_cost.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving data: {ex.Message}");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox_money_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox_category_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox_category.SelectedItem is null) return;
            comboBox_category_name.Items.Clear();
            var category = comboBox_category.SelectedItem.ToString();
            if (category == "���J")
            {
                comboBox_category_name.Items.AddRange(new object[] { "�~��", "����", "��ꦬ�q", "��L���J" });
            }
            else if (category == "�O��")
            {
                comboBox_category_name.Items.AddRange(new object[] { "�\��", "��q", "�T��", "��L�O��" });
            }
            else if (category == "�겣")
            {
                comboBox_category_name.Items.AddRange(new object[] { "�{��", "�s��", "���겣", "��L�겣" });
            }
            else if (category == "�t��")
            {
                comboBox_category_name.Items.AddRange(new object[] { "�U��", "�H�Υd�Ű�", "��L�t��" });
            }
            else if (category == "�v�q")
            {
                comboBox_category_name.Items.AddRange(new object[] { "�ѪF�v�q", "�O�d�վl", "��L�v�q" });
            }
        }

        private void buttonInsert_Click(object sender, EventArgs e) //Add
        {
            try
            {
                var title = textBox_TItle.Text.Trim();
                var category = comboBox_category.SelectedItem?.ToString() ?? string.Empty;
                var date = dateTimePicker_Start.Value;
                var eventType = comboBox_category_name.SelectedItem?.ToString() ?? string.Empty;
                var moneyText = textBox_money.Text.Trim();
                var comment = textBox_Comment.Text.Trim();
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(moneyText) || string.IsNullOrEmpty(eventType))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }
                if (!int.TryParse(moneyText, out int money) || money < 0)
                {
                    MessageBox.Show("Please enter a valid amount of money.");
                    return;
                }
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                _controller.AddData(new RecordForm
                {
                    Title = title,
                    Category = category,
                    EventType = eventType,
                    Date = date,
                    Amount = money,
                    Comment = comment
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding data: {ex.Message}");
            }
            ShowData();
            ShowChart();
        }

        private void button_Delete_Click(object sender, EventArgs e) //delete
        {
            try
            {
                var title = textBox_TItle.Text.Trim();
                if (string.IsNullOrEmpty(title))
                {
                    MessageBox.Show("Please enter a title to delete.");
                    return;
                }
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                _controller.RemoveData(new RecordForm
                {
                    Title = title
                }, ETarget.title);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting data: {ex.Message}");
            }
            ShowData();
            ShowChart();
        }
        private void buttonDelete_TC_Click(object sender, EventArgs e)
        {
            try
            {
                var title = textBox_TItle.Text.Trim();
                var category = comboBox_category.SelectedItem?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(category))
                {
                    MessageBox.Show("Please enter a title and categoty to delete.");
                    return;
                }
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                _controller.RemoveData(new RecordForm
                {
                    Title = title,
                    Category = category
                }, ETarget.title, ETarget.category);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting data: {ex.Message}");
            }
            ShowData();
            ShowChart();
        }

        private void button_FindT_Click(object sender, EventArgs e) //find same title
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                var title = textBox_TItle.Text.Trim();
                if (string.IsNullOrEmpty(title))
                {
                    MessageBox.Show("Please enter a title to find.");
                    return;
                }
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                result = _controller.GetData(new RecordForm { Title = title }, ETarget.title, ERange.First);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding data: {ex.Message}");
            }
            ShowData(result);
        }

        private void button_FindC_Click(object sender, EventArgs e)
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                var category = comboBox_category_name.SelectedItem?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(category))
                {
                    MessageBox.Show("Please enter a title to find.");
                    return;
                }
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                result = _controller.GetData(new RecordForm { Category = category }, ETarget.category, ERange.First);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding data: {ex.Message}");
            }
            ShowData(result);
        }

        private void textBox_Comment_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonSearchD_Click(object sender, EventArgs e)
        {
            List<RecordForm> result = new List<RecordForm>();
            try
            {
                var start = dateTimePicker_Start.Value.Date;
                var end = dateTimePicker_End.Value.Date;
                if (DateTime.MinValue == start || DateTime.MinValue == end)
                {
                    MessageBox.Show("Please enter a correct date to find.");
                    return;
                }
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                result = _controller.GetData(new RecordForm { Date = start }, new RecordForm { Date = end }, ETarget.time, ERange.First);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error finding data: {ex.Message}");
            }
            ShowData(result);
        }
    }
}
