namespace StockProjectCS
{
    partial class Form_candlestickChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            chart_candlesticks = new System.Windows.Forms.DataVisualization.Charting.Chart();
            dateTimePicker_Start = new DateTimePicker();
            dateTimePicker_End = new DateTimePicker();
            label_to = new Label();
            button_Update = new Button();
            comboBox_graphAnnotation = new ComboBox();
            toolTip_ChartForm = new ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)chart_candlesticks).BeginInit();
            SuspendLayout();
            // 
            // chart_candlesticks
            // 
            chart_candlesticks.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            chart_candlesticks.BackColor = SystemColors.Window;
            chart_candlesticks.BackSecondaryColor = Color.White;
            chartArea1.AxisY.Title = "Stock Price (USD$)";
            chartArea1.BackColor = SystemColors.ActiveBorder;
            chartArea1.BackSecondaryColor = Color.Transparent;
            chartArea1.Name = "ChartArea_candlesticks";
            chartArea2.AlignWithChartArea = "ChartArea_candlesticks";
            chartArea2.AxisY.Title = "Volume";
            chartArea2.AxisY.TitleFont = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            chartArea2.BackColor = SystemColors.ActiveBorder;
            chartArea2.Name = "ChartArea_volume";
            chart_candlesticks.ChartAreas.Add(chartArea1);
            chart_candlesticks.ChartAreas.Add(chartArea2);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            chart_candlesticks.Legends.Add(legend1);
            chart_candlesticks.Location = new Point(14, 16);
            chart_candlesticks.Margin = new Padding(3, 4, 3, 4);
            chart_candlesticks.Name = "chart_candlesticks";
            chart_candlesticks.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Light;
            series1.ChartArea = "ChartArea_candlesticks";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            series1.IsXValueIndexed = true;
            series1.LabelToolTip = "Date: #VALX\\nOpen: #VALY3\\nHigh: #VALY1\\nLow: #VALY2\\nClose: #VALY4";
            series1.Legend = "Legend1";
            series1.Name = "Series_hloc";
            series1.ToolTip = "Date: #VALX\\nOpen: #VALY3\\nHigh: #VALY1\\nLow: #VALY2\\nClose: #VALY4";
            series1.XValueMember = "date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series1.YValueMembers = "high, low, open, close";
            series1.YValuesPerPoint = 4;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.ChartArea = "ChartArea_volume";
            series2.IsXValueIndexed = true;
            series2.LabelToolTip = "Date: #VALX\\nVolume: #VALY";
            series2.Legend = "Legend1";
            series2.Name = "Series_volume";
            series2.ToolTip = "Date: #VALX\\nVolume: #VALY";
            series2.XValueMember = "date";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series2.YValueMembers = "volume";
            chart_candlesticks.Series.Add(series1);
            chart_candlesticks.Series.Add(series2);
            chart_candlesticks.Size = new Size(887, 507);
            chart_candlesticks.TabIndex = 0;
            chart_candlesticks.Text = "chart1";
            title1.Name = "Title_chart";
            title1.Text = "XXX Candle";
            chart_candlesticks.Titles.Add(title1);
            // 
            // dateTimePicker_Start
            // 
            dateTimePicker_Start.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            dateTimePicker_Start.Format = DateTimePickerFormat.Short;
            dateTimePicker_Start.Location = new Point(14, 553);
            dateTimePicker_Start.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker_Start.Name = "dateTimePicker_Start";
            dateTimePicker_Start.Size = new Size(111, 28);
            dateTimePicker_Start.TabIndex = 1;
            toolTip_ChartForm.SetToolTip(dateTimePicker_Start, "Start date");
            // 
            // dateTimePicker_End
            // 
            dateTimePicker_End.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            dateTimePicker_End.Format = DateTimePickerFormat.Short;
            dateTimePicker_End.Location = new Point(161, 553);
            dateTimePicker_End.Margin = new Padding(3, 4, 3, 4);
            dateTimePicker_End.Name = "dateTimePicker_End";
            dateTimePicker_End.Size = new Size(111, 28);
            dateTimePicker_End.TabIndex = 2;
            toolTip_ChartForm.SetToolTip(dateTimePicker_End, "End date");
            // 
            // label_to
            // 
            label_to.AutoSize = true;
            label_to.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            label_to.Location = new Point(131, 557);
            label_to.Name = "label_to";
            label_to.Size = new Size(28, 21);
            label_to.TabIndex = 3;
            label_to.Text = "to";
            // 
            // button_Update
            // 
            button_Update.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point);
            button_Update.Location = new Point(281, 552);
            button_Update.Margin = new Padding(3, 4, 3, 4);
            button_Update.Name = "button_Update";
            button_Update.Size = new Size(86, 31);
            button_Update.TabIndex = 4;
            button_Update.Text = "Update";
            toolTip_ChartForm.SetToolTip(button_Update, "Will refresh the chart to changed date range");
            button_Update.UseVisualStyleBackColor = true;
            button_Update.Click += button_Update_Click;
            // 
            // comboBox_graphAnnotation
            // 
            comboBox_graphAnnotation.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox_graphAnnotation.FormattingEnabled = true;
            comboBox_graphAnnotation.Items.AddRange(new object[] { "None", "Bullish", "Bearish", "Neutral", "Marubozu", "Doji", "Long-Legged Doji", "Dragonfly Doji", "Gravestone Doji", "Four-Price Doji", "Hammer", "Inverted Hammer" });
            comboBox_graphAnnotation.Location = new Point(393, 553);
            comboBox_graphAnnotation.Margin = new Padding(3, 4, 3, 4);
            comboBox_graphAnnotation.Name = "comboBox_graphAnnotation";
            comboBox_graphAnnotation.Size = new Size(176, 29);
            comboBox_graphAnnotation.TabIndex = 5;
            comboBox_graphAnnotation.Text = "None";
            toolTip_ChartForm.SetToolTip(comboBox_graphAnnotation, "Choose what candlestick pattern is displayed");
            comboBox_graphAnnotation.SelectedIndexChanged += comboBox_graphAnnotation_SelectedIndexChanged;
            // 
            // Form_candlestickChart
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(914, 600);
            Controls.Add(comboBox_graphAnnotation);
            Controls.Add(button_Update);
            Controls.Add(label_to);
            Controls.Add(dateTimePicker_End);
            Controls.Add(dateTimePicker_Start);
            Controls.Add(chart_candlesticks);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form_candlestickChart";
            Text = "Form_candlestickChart";
            ((System.ComponentModel.ISupportInitialize)chart_candlesticks).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart_candlesticks;
        private DateTimePicker dateTimePicker_Start;
        private DateTimePicker dateTimePicker_End;
        private Label label_to;
        private Button button_Update;
        private ComboBox comboBox_graphAnnotation;
        private ToolTip toolTip_ChartForm;
    }
}