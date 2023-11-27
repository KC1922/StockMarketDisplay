namespace StockProjectCS
{
    partial class Form_Entry
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
            components = new System.ComponentModel.Container();
            button_loadStock = new Button();
            button_openFolder = new Button();
            dateTimePicker_Start = new DateTimePicker();
            dateTimePicker_End = new DateTimePicker();
            label_start = new Label();
            label_end = new Label();
            label_other = new Label();
            comboBox_otherOptions = new ComboBox();
            textBox_loadedFiles = new TextBox();
            openFileDialog_StockFolder = new OpenFileDialog();
            toolTip_FormEntry = new ToolTip(components);
            SuspendLayout();
            // 
            // button_loadStock
            // 
            button_loadStock.Font = new Font("Century Gothic", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            button_loadStock.Location = new Point(332, 16);
            button_loadStock.Margin = new Padding(3, 2, 3, 2);
            button_loadStock.Name = "button_loadStock";
            button_loadStock.Size = new Size(105, 67);
            button_loadStock.TabIndex = 0;
            button_loadStock.Text = "Load Stock Outlook Tool";
            button_loadStock.UseVisualStyleBackColor = true;
            button_loadStock.Click += button_loadStock_Click;
            // 
            // button_openFolder
            // 
            button_openFolder.BackColor = SystemColors.ButtonShadow;
            button_openFolder.Font = new Font("Century Gothic", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            button_openFolder.Location = new Point(18, 16);
            button_openFolder.Margin = new Padding(3, 2, 3, 2);
            button_openFolder.Name = "button_openFolder";
            button_openFolder.Size = new Size(105, 44);
            button_openFolder.TabIndex = 1;
            button_openFolder.Text = "Browse Stocks";
            button_openFolder.UseVisualStyleBackColor = false;
            button_openFolder.Click += button_openFolder_Click;
            // 
            // dateTimePicker_Start
            // 
            dateTimePicker_Start.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            dateTimePicker_Start.Format = DateTimePickerFormat.Short;
            dateTimePicker_Start.Location = new Point(192, 13);
            dateTimePicker_Start.Margin = new Padding(3, 2, 3, 2);
            dateTimePicker_Start.Name = "dateTimePicker_Start";
            dateTimePicker_Start.Size = new Size(121, 24);
            dateTimePicker_Start.TabIndex = 2;
            // 
            // dateTimePicker_End
            // 
            dateTimePicker_End.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            dateTimePicker_End.Format = DateTimePickerFormat.Short;
            dateTimePicker_End.Location = new Point(192, 38);
            dateTimePicker_End.Margin = new Padding(3, 2, 3, 2);
            dateTimePicker_End.Name = "dateTimePicker_End";
            dateTimePicker_End.Size = new Size(121, 24);
            dateTimePicker_End.TabIndex = 3;
            // 
            // label_start
            // 
            label_start.AutoSize = true;
            label_start.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            label_start.Location = new Point(139, 16);
            label_start.Name = "label_start";
            label_start.Size = new Size(44, 19);
            label_start.TabIndex = 4;
            label_start.Text = "Start:";
            // 
            // label_end
            // 
            label_end.AutoSize = true;
            label_end.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            label_end.Location = new Point(146, 41);
            label_end.Name = "label_end";
            label_end.Size = new Size(40, 19);
            label_end.TabIndex = 5;
            label_end.Text = "End:";
            // 
            // label_other
            // 
            label_other.AutoSize = true;
            label_other.Font = new Font("Century Gothic", 10.2F, FontStyle.Regular, GraphicsUnit.Point);
            label_other.Location = new Point(131, 67);
            label_other.Name = "label_other";
            label_other.Size = new Size(52, 19);
            label_other.TabIndex = 6;
            label_other.Text = "Other:";
            // 
            // comboBox_otherOptions
            // 
            comboBox_otherOptions.Font = new Font("Century Gothic", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox_otherOptions.FormattingEnabled = true;
            comboBox_otherOptions.Items.AddRange(new object[] { "None", "Past Week", "Past Month", "Past Three Months", "Year to Date", "Past Year", "Past Five Years", "All" });
            comboBox_otherOptions.Location = new Point(192, 64);
            comboBox_otherOptions.Margin = new Padding(3, 2, 3, 2);
            comboBox_otherOptions.Name = "comboBox_otherOptions";
            comboBox_otherOptions.Size = new Size(121, 23);
            comboBox_otherOptions.TabIndex = 7;
            comboBox_otherOptions.Text = "None";
            // 
            // textBox_loadedFiles
            // 
            textBox_loadedFiles.Font = new Font("Century Gothic", 7.8F, FontStyle.Regular, GraphicsUnit.Point);
            textBox_loadedFiles.Location = new Point(18, 64);
            textBox_loadedFiles.Margin = new Padding(3, 2, 3, 2);
            textBox_loadedFiles.Multiline = true;
            textBox_loadedFiles.Name = "textBox_loadedFiles";
            textBox_loadedFiles.ReadOnly = true;
            textBox_loadedFiles.ScrollBars = ScrollBars.Vertical;
            textBox_loadedFiles.Size = new Size(106, 19);
            textBox_loadedFiles.TabIndex = 8;
            // 
            // openFileDialog_StockFolder
            // 
            openFileDialog_StockFolder.FileName = "none";
            // 
            // Form_Entry
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(459, 103);
            Controls.Add(textBox_loadedFiles);
            Controls.Add(comboBox_otherOptions);
            Controls.Add(label_other);
            Controls.Add(label_end);
            Controls.Add(label_start);
            Controls.Add(dateTimePicker_End);
            Controls.Add(dateTimePicker_Start);
            Controls.Add(button_openFolder);
            Controls.Add(button_loadStock);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form_Entry";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Stock Info Entry";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_loadStock;
        private Button button_openFolder;
        private DateTimePicker dateTimePicker_Start;
        private DateTimePicker dateTimePicker_End;
        private Label label_start;
        private Label label_end;
        private Label label_other;
        private ComboBox comboBox_otherOptions;
        private TextBox textBox_loadedFiles;
        private OpenFileDialog openFileDialog_StockFolder;
        private ToolTip toolTip_FormEntry;
    }
}