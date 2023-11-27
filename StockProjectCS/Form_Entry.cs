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

namespace StockProjectCS
{
    public partial class Form_Entry : Form
    {
        public Form_Entry()
        {
            InitializeComponent();
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
            startDate = DateTime.Parse("1/1/1753");
            endDate = DateTime.Today;

            //None
            if (comboBox_otherOptions.SelectedIndex == 0)
            {
                //use the selected start and end dates
                startDate = dateTimePicker_Start.Value;
                endDate = dateTimePicker_End.Value;
            }
            //Past Week
            else if (comboBox_otherOptions.SelectedIndex == 1)
            {
                //show data for last week
                startDate = DateTime.Now.AddDays(-7);
                dateTimePicker_Start.Value = startDate;
                dateTimePicker_End.Value = endDate;

            }
            //Past Month
            else if (comboBox_otherOptions.SelectedIndex == 2)
            {
                startDate = DateTime.Now.AddMonths(-1);
                dateTimePicker_Start.Value = startDate;
                dateTimePicker_End.Value = endDate;
            }
            //Past Three Months
            else if (comboBox_otherOptions.SelectedIndex == 3)
            {
                startDate = DateTime.Now.AddMonths(-3);
                dateTimePicker_Start.Value = startDate;
                dateTimePicker_End.Value = endDate;
            }
            //Year to Date
            else if (comboBox_otherOptions.SelectedIndex == 4)
            {
                //get date for all of current year
                startDate = new DateTime(endDate.Year, 1, 1);
                dateTimePicker_Start.Value = startDate;
                dateTimePicker_End.Value = endDate;

            }
            //Past Year
            else if (comboBox_otherOptions.SelectedIndex == 5)
            {
                //show data from past year
                startDate = DateTime.Now.AddYears(-1);
                dateTimePicker_Start.Value = startDate;
                dateTimePicker_End.Value = endDate;
            }
            //Past Five Years
            else if (comboBox_otherOptions.SelectedIndex == 6)
            {
                //show data from past 5 years
                startDate = DateTime.Now.AddYears(-5);
                dateTimePicker_Start.Value = startDate;
                dateTimePicker_End.Value = endDate;
            }
            //All
            else if (comboBox_otherOptions.SelectedIndex == 7)
            {
                //use default values for entire date range
            }
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

            List<Form_candlestickChart> candlestickCharts = new List<Form_candlestickChart>(16);
            //List<List<aCandlestick>> listOfCandlestickLists = new List<List<aCandlestick>>(16);

            foreach (var stock in selectedFileNames)
            {
                var tempPath = Path.GetFullPath(stock);
                if (File.Exists(tempPath))
                {
                    try
                    {
                        tickerName = Path.GetFileName(stock);
                        tickerName = Path.GetFileNameWithoutExtension(tickerName);
                        List<aCandlestick> candlesticks = new List<aCandlestick>(1028);
                        csvReaderHelper.populateCandlesticks(stock, candlesticks);
                        if (candlesticks == null || candlesticks.Count <= 0)
                        {
                            //if returned list is empty, throw error. Happens when csv file is either empty, for does nt have data for the given date range
                            MessageBox.Show($"Error reading stock data for {Path.GetFileName(stock)}. Data either does not exist for given date range, or date range is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);                           //chartDataForm.Show();
                        }
                        else
                        {
                            candlestickCharts.Add(new Form_candlestickChart(candlesticks, tickerName, startDate, endDate));
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