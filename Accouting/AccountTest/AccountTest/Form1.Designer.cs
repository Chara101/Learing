
namespace testAccounting1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            textBox_TItle = new TextBox();
            comboBox_category = new ComboBox();
            comboBox_category_name = new ComboBox();
            textBox_money = new TextBox();
            listBox_income = new ListBox();
            listBox_cost = new ListBox();
            chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            dateTimePicker_Start = new DateTimePicker();
            dateTimePicker_End = new DateTimePicker();
            buttonInsert = new Button();
            button_Delete = new Button();
            button_FindT = new Button();
            button_FindC = new Button();
            buttonDelete_TC = new Button();
            textBox_Comment = new TextBox();
            buttonSearchD = new Button();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            SuspendLayout();
            // 
            // textBox_TItle
            // 
            textBox_TItle.Location = new Point(12, 82);
            textBox_TItle.Name = "textBox_TItle";
            textBox_TItle.Size = new Size(125, 27);
            textBox_TItle.TabIndex = 0;
            // 
            // comboBox_category
            // 
            comboBox_category.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_category.FormattingEnabled = true;
            comboBox_category.Items.AddRange(new object[] { "收入", "費用", "資產", "負債", "權益" });
            comboBox_category.Location = new Point(143, 82);
            comboBox_category.Name = "comboBox_category";
            comboBox_category.Size = new Size(151, 27);
            comboBox_category.TabIndex = 1;
            comboBox_category.SelectedIndexChanged += comboBox_category_SelectedIndexChanged;
            // 
            // comboBox_category_name
            // 
            comboBox_category_name.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_category_name.FormattingEnabled = true;
            comboBox_category_name.Location = new Point(300, 82);
            comboBox_category_name.Name = "comboBox_category_name";
            comboBox_category_name.Size = new Size(151, 27);
            comboBox_category_name.TabIndex = 2;
            // 
            // textBox_money
            // 
            textBox_money.Location = new Point(457, 82);
            textBox_money.Name = "textBox_money";
            textBox_money.Size = new Size(125, 27);
            textBox_money.TabIndex = 3;
            textBox_money.TextChanged += textBox_money_TextChanged;
            // 
            // listBox_income
            // 
            listBox_income.FormattingEnabled = true;
            listBox_income.ItemHeight = 19;
            listBox_income.Location = new Point(36, 145);
            listBox_income.Name = "listBox_income";
            listBox_income.Size = new Size(399, 118);
            listBox_income.TabIndex = 4;
            // 
            // listBox_cost
            // 
            listBox_cost.FormattingEnabled = true;
            listBox_cost.ItemHeight = 19;
            listBox_cost.Location = new Point(36, 269);
            listBox_cost.Name = "listBox_cost";
            listBox_cost.Size = new Size(399, 118);
            listBox_cost.TabIndex = 5;
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            chart1.Legends.Add(legend2);
            chart1.Location = new Point(457, 221);
            chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chart1.Series.Add(series2);
            chart1.Size = new Size(188, 166);
            chart1.TabIndex = 6;
            chart1.Text = "chart1";
            // 
            // dateTimePicker_Start
            // 
            dateTimePicker_Start.Location = new Point(12, 21);
            dateTimePicker_Start.Name = "dateTimePicker_Start";
            dateTimePicker_Start.Size = new Size(250, 27);
            dateTimePicker_Start.TabIndex = 7;
            // 
            // dateTimePicker_End
            // 
            dateTimePicker_End.Location = new Point(280, 21);
            dateTimePicker_End.Name = "dateTimePicker_End";
            dateTimePicker_End.Size = new Size(250, 27);
            dateTimePicker_End.TabIndex = 8;
            // 
            // buttonInsert
            // 
            buttonInsert.Location = new Point(666, 80);
            buttonInsert.Name = "buttonInsert";
            buttonInsert.Size = new Size(94, 29);
            buttonInsert.TabIndex = 9;
            buttonInsert.Text = "新增";
            buttonInsert.UseVisualStyleBackColor = true;
            buttonInsert.Click += buttonInsert_Click;
            // 
            // button_Delete
            // 
            button_Delete.Location = new Point(666, 125);
            button_Delete.Name = "button_Delete";
            button_Delete.Size = new Size(94, 29);
            button_Delete.TabIndex = 10;
            button_Delete.Text = "刪除";
            button_Delete.UseVisualStyleBackColor = true;
            button_Delete.Click += button_Delete_Click;
            // 
            // button_FindT
            // 
            button_FindT.Location = new Point(666, 214);
            button_FindT.Name = "button_FindT";
            button_FindT.Size = new Size(94, 29);
            button_FindT.TabIndex = 11;
            button_FindT.Text = "尋找同名";
            button_FindT.UseVisualStyleBackColor = true;
            button_FindT.Click += button_FindT_Click;
            // 
            // button_FindC
            // 
            button_FindC.Location = new Point(666, 259);
            button_FindC.Name = "button_FindC";
            button_FindC.Size = new Size(94, 29);
            button_FindC.TabIndex = 12;
            button_FindC.Text = "尋找同科目";
            button_FindC.UseVisualStyleBackColor = true;
            button_FindC.Click += button_FindC_Click;
            // 
            // buttonDelete_TC
            // 
            buttonDelete_TC.Location = new Point(666, 169);
            buttonDelete_TC.Name = "buttonDelete_TC";
            buttonDelete_TC.Size = new Size(122, 29);
            buttonDelete_TC.TabIndex = 13;
            buttonDelete_TC.Text = "根據名與科刪除";
            buttonDelete_TC.UseVisualStyleBackColor = true;
            buttonDelete_TC.Click += buttonDelete_TC_Click;
            // 
            // textBox_Comment
            // 
            textBox_Comment.Location = new Point(457, 145);
            textBox_Comment.Name = "textBox_Comment";
            textBox_Comment.Size = new Size(125, 27);
            textBox_Comment.TabIndex = 14;
            textBox_Comment.TextChanged += textBox_Comment_TextChanged;
            // 
            // buttonSearchD
            // 
            buttonSearchD.Location = new Point(666, 306);
            buttonSearchD.Name = "buttonSearchD";
            buttonSearchD.Size = new Size(94, 29);
            buttonSearchD.TabIndex = 15;
            buttonSearchD.Text = "日期尋找";
            buttonSearchD.UseVisualStyleBackColor = true;
            buttonSearchD.Click += buttonSearchD_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonSearchD);
            Controls.Add(textBox_Comment);
            Controls.Add(buttonDelete_TC);
            Controls.Add(button_FindC);
            Controls.Add(button_FindT);
            Controls.Add(button_Delete);
            Controls.Add(buttonInsert);
            Controls.Add(dateTimePicker_End);
            Controls.Add(dateTimePicker_Start);
            Controls.Add(chart1);
            Controls.Add(listBox_cost);
            Controls.Add(listBox_income);
            Controls.Add(textBox_money);
            Controls.Add(comboBox_category_name);
            Controls.Add(comboBox_category);
            Controls.Add(textBox_TItle);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox_TItle;
        private ComboBox comboBox_category;
        private ComboBox comboBox_category_name;
        private TextBox textBox_money;
        private ListBox listBox_income;
        private ListBox listBox_cost;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private DateTimePicker dateTimePicker_Start;
        private DateTimePicker dateTimePicker_End;
        private Button buttonInsert;
        private Button button_Delete;
        private Button button_FindT;
        private Button button_FindC;
        private Button buttonDelete_TC;
        private TextBox textBox_Comment;
        private Button buttonSearchD;
    }
}
