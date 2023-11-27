using CsvHelper.Configuration;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

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
        /// This function uses CSVHelper to parse a single line of CSV data
        /// to create a candlestick and stores it in the smartCandlesticks list
        /// </summary>
        /// <param name="csvLine">A single line from the stock csv file</param>
        public static aCandlestick createCandlestick(string csvLine)
        {
            try
            {
                //split the line using delimiters
                string[] delimiters = new string[] { "\",\"", ",", "\"", " " };
                string[] fields = csvLine.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                //ensure the correct number of fields were found
                if (fields.Length >= 9)
                {
                    //parse the date, which is split into three parts
                    string dateField = fields[3] + "/" + fields[2] + "/" + fields[4];

                    if (DateTime.TryParseExact(dateField, "d/MMM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime csvDate))
                    {
                        //parse the remaining stock data into the candlestick
                        double open = double.Parse(fields[5], CultureInfo.InvariantCulture);
                        double high = double.Parse(fields[6], CultureInfo.InvariantCulture);
                        double low = double.Parse(fields[7], CultureInfo.InvariantCulture);
                        double close = double.Parse(fields[8], CultureInfo.InvariantCulture);
                        ulong volume = ulong.Parse(fields[9], CultureInfo.InvariantCulture);

                        //use the constructor with parameters to create a new instance of aCandlestick
                        aCandlestick candlestick = new aCandlestick(csvDate, open, high, low, close, volume);

                        return candlestick;
                    }
                    else
                    {
                        Console.WriteLine("Date format is not correct");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("Incorrect number of fields in CSV line");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing CSV line: {ex.Message}");
                return null;
            }
        }
    }
}

