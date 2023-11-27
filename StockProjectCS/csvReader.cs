using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProjectCS
{
    /// <summary>
    /// The expected format of headers in a csv file 
    /// CSV Helper uses this to fetch/assign values
    /// </summary>
    public class csvReader
    {
        public string Ticker { get; set; }
        public string Period { get; set; }
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public ulong Volume { get; set; }
    }
    public class csvReaderHelper
    {
        /// <summary>
        /// This function uses CSVHelper to assign the values found in each line of the csv file
        /// to a new candlestick that stores the date, open, high, low, close, and volume
        /// </summary>
        /// <param name="filePath the directory path to the stock csv file"></param>
        /// <param name="candlesticks a list of type aCandlestick to be populated"></param>
        public static void populateCandlesticks(string filePath, List<aCandlestick> candlesticks)
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    //make list of records using csv helper
                    var records = csv.GetRecords<csvReader>().ToList();

                    foreach (var record in records)
                    {
                        DateTime defaultDateValue = DateTime.MinValue;
                        //grabbing values from CSV line to create candlestick
                        var candlestick = new aCandlestick
                        {
                            //set values in the candlestick object, also set default values if it reads nothing
                            open = record.Open != default(double) ? record.Open : 0,
                            close = record.Close != default(double) ? record.Close : 0,
                            high = record.High != default(double) ? record.High : 0,
                            low = record.Low != default(double) ? record.Low : 0,
                            volume = record.Volume, //default value of ulong is already 0
                            date = record.Date != default(DateTime) ? record.Date : defaultDateValue
                        };
                        candlestick.calculateProperties();
                        candlestick.calculatePatterns();
                        candlesticks.Add(candlestick);
               
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading and filtering CSV: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
