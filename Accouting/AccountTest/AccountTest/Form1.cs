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
        //            if (temp.Category == "收入") listBox_income.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
        //            else if (temp.Category == "支出") listBox_cost.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
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
            chart1.Titles.Add("財務比例");
            var series = new System.Windows.Forms.DataVisualization.Charting.Series
            {
                Name = "Income",
                ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie
            };
            try
            {
                if (_controller is null) throw new NotImplementedException("Controller is not initialized.");
                var income = _controller.GetData(new RecordForm { Category = "收入" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                var cost = _controller.GetData(new RecordForm { Category = "費用" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                var asset = _controller.GetData(new RecordForm { Category = "資產" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                var liability = _controller.GetData(new RecordForm { Category = "負債" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                var equity = _controller.GetData(new RecordForm { Category = "權益" }, ETarget.category, ERange.All).Sum(temp => temp.Amount);
                series.Points.AddXY("收入", income);
                series.Points.AddXY("費用", cost);
                series.Points.AddXY("資產", asset);
                series.Points.AddXY("負債", liability);
                series.Points.AddXY("權益", equity);
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
                chart1.Series[0].Points[0].YValues[0] = _controller.GetData(new RecordForm { Category = "收入" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Series[0].Points[1].YValues[0] = _controller.GetData(new RecordForm { Category = "費用" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Series[0].Points[2].YValues[0] = _controller.GetData(new RecordForm { Category = "資產" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Series[0].Points[3].YValues[0] = _controller.GetData(new RecordForm { Category = "負債" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
                chart1.Series[0].Points[4].YValues[0] = _controller.GetData(new RecordForm { Category = "權益" }, ETarget.category, ERange.First).Sum(temp => temp.Amount);
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
                    if (temp.Category == "資產" || temp.Category == "費用") listBox_income.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
                    else if (temp.Category == "收入" || temp.Category == "負債" || temp.Category == "權益") listBox_cost.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
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
                    if (temp.Category == "資產" || temp.Category == "費用") listBox_income.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
                    else if (temp.Category == "收入" || temp.Category == "負債" || temp.Category == "權益") listBox_cost.Items.Add($"{temp.Date.Date.ToString()} {temp.Title} {temp.Category} {temp.EventType} {temp.Amount}");
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
            if (category == "收入")
            {
                comboBox_category_name.Items.AddRange(new object[] { "薪資", "獎金", "投資收益", "其他收入" });
            }
            else if (category == "費用")
            {
                comboBox_category_name.Items.AddRange(new object[] { "餐飲", "交通", "娛樂", "其他費用" });
            }
            else if (category == "資產")
            {
                comboBox_category_name.Items.AddRange(new object[] { "現金", "存款", "投資資產", "其他資產" });
            }
            else if (category == "負債")
            {
                comboBox_category_name.Items.AddRange(new object[] { "貸款", "信用卡債務", "其他負債" });
            }
            else if (category == "權益")
            {
                comboBox_category_name.Items.AddRange(new object[] { "股東權益", "保留盈餘", "其他權益" });
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
