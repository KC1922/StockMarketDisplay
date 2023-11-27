using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using System.Diagnostics;
using System.Windows.Forms.Design;
using StockProjectCS;
using System.Threading.Tasks;
using System.Runtime.Intrinsics.X86;
using CsvHelper.Configuration;
using CsvHelper;

namespace StockProjectCS
{
    public partial class Form_Entry : Form
    {
        public Form_Entry()
        {
            InitializeComponent();
            dateTimePicker_Start.Value = DateTime.Parse("1/1/2021");
        }

        /// <summary>
        /// Handles the open file dialog function. Stores an array of file paths to be read
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_openFolder_Click(object sender, EventArgs e)
        {
            //Expects stock folder to be riht outside project solution
            string stockFolderPath = Path.GetFullPath("../../../../../Stock Data");
            List<string> selectedFileNames;

            openFileDialog_StockFolder.InitialDirectory = stockFolderPath;
            openFileDialog_StockFolder.Multiselect = true;

            //either get one stock, or mutliple
            if (openFileDialog_StockFolder.ShowDialog() == DialogResult.OK)
            {
                selectedFileNames = openFileDialog_StockFolder.FileNames.Select(Path.GetFileName).ToList();

                //convert the list of file names to a single string for display
                string displayText = string.Join(", ", selectedFileNames);

                textBox_loadedFiles.Text = displayText;
            }
        }
        /// <summary>
        /// Calculates the date range the user requested on the entry form
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public void findDateRange(out DateTime startDate, out DateTime endDate)
        {
            //set default endDate
            endDate = DateTime.Today;
            switch (comboBox_otherOptions.SelectedIndex)
            {
                case 0: //none
                    startDate = dateTimePicker_Start.Value;
                    endDate = dateTimePicker_End.Value;
                    break;
                case 1: //past Week
                    startDate = endDate.AddDays(-7);
                    break;
                case 2: //past Month
                    startDate = endDate.AddMonths(-1);
                    break;
                case 3: //past Three Months
                    startDate = endDate.AddMonths(-3);
                    break;
                case 4: //year to Date
                    startDate = new DateTime(endDate.Year, 1, 1);
                    break;
                case 5: //past Year
                    startDate = endDate.AddYears(-1);
                    break;
                case 6: //past Five Years
                    startDate = endDate.AddYears(-5);
                    break;
                case 7: //all
                    startDate = DateTime.Parse("1/1/1753");
                    break;
                default: //default case to handle unexpected index
                    startDate = DateTime.Parse("1/1/1753");
                    break;
            }

            //update date pickers only once, here
            dateTimePicker_Start.Value = startDate;
            dateTimePicker_End.Value = endDate;
        }

        /// <summary>
        /// Handles loading the candlesticks and then the forms to display the data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_loadStock_Click(object sender, EventArgs e)
        {
            string tickerName;
            string stockFolderPath = "../../../../../Stock Data";
            stockFolderPath = Path.GetFullPath(stockFolderPath);
            List<string> filesToCheck = new List<string>(16);
            var selectedFileNames = openFileDialog_StockFolder.FileNames.ToArray();

            //check to see if openFileDialog returned the default file of "none"
            if (selectedFileNames.Contains("none"))
            {
                MessageBox.Show("Please select a stock.");
                return;
            }

            DateTime startDate, endDate;
            findDateRange(out startDate, out endDate);
            //create a list of forms
            List<Form_candlestickChart> candlestickCharts = new List<Form_candlestickChart>(16);
            //loop through each file name and create a form for each
            foreach (var stock in selectedFileNames)
            {
                var tempPath = Path.GetFullPath(stock);
                if (File.Exists(tempPath))
                {
                    try
                    {
                        tickerName = Path.GetFileName(stock);
                        tickerName = Path.GetFileNameWithoutExtension(tickerName);
                        //make a list of smart candlesticks and populate
                        List<smartCandlestick> smartCandlesticks = new List<smartCandlestick>(1028);
                        //use CSVhelper to parse in lines
                        using (var reader = new StreamReader(tempPath))
                        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                        {
                            while (csv.Read())
                            {
                                var record = csv.GetRecord<csvReader>();
                                string csvLine = csv.Context.Parser.RawRecord;
                                smartCandlestick smartCandle = new smartCandlestick(csvLine);
                                if (smartCandle != null)
                                {
                                    smartCandlesticks.Add(smartCandle);
                                }
                            }
                        }

                        if (smartCandlesticks == null || smartCandlesticks.Count <= 0)
                        {
                            //if returned list is empty, throw error. Happens when csv file is either empty, for does nt have data for the given date range
                            MessageBox.Show($"Error reading stock data for {Path.GetFileName(stock)}. Data either does not exist for given date range, or date range is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                           //chartDataForm.Show();
                        }
                        else
                        {
                            candlestickCharts.Add(new Form_candlestickChart(smartCandlesticks, tickerName, startDate, endDate));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reading stock data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"File {stock} not found");
                }
            }

            foreach (var form in candlestickCharts)
            {
                form.Show();
            }

        }
    }
}