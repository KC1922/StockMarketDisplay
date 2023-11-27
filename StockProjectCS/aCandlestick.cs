using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel;
using System.Xml.Linq;
using System.Globalization;

namespace StockProjectCS
{
    /// <summary>
    /// Class defines a basic candlestick values
    /// </summary>
    public class aCandlestick
    {
        public DateTime date { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public ulong volume { get; set; }
        /// <summary>
        /// default constructor for candlestick
        /// </summary>
        public aCandlestick()
        {
            date = DateTime.MinValue;
            open = 0.0;
            close = 0.0;
            high = 0.0;
            low = 0.0;
            volume = 0;
        }

        /// <summary>
        /// constructor with values given
        /// </summary>
        /// <param name="date"></param>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        /// <param name="volume"></param>
        public aCandlestick(DateTime date, double open, double high, double low, double close, ulong volume)
        {
            this.date = date;
            this.open = open;
            this.close = close;
            this.high = high;
            this.low = low;
            this.volume = volume;

        }

        /// <summary>
        /// constructor with string of values given
        /// </summary>
        /// <param name="csv"></param>
        /// <exception cref="ArgumentException"></exception>
        public aCandlestick(string csvLine)
        {
            aCandlestick cs = csvReaderHelper.createCandlestick(csvLine);
            date = cs.date;
            open = cs.open;
            close = cs.close;
            high = cs.high;
            low = cs.low;
            volume = cs.volume;
        }

        /// <summary>
        /// Constructor that takes in an existing candlestick
        /// </summary>
        /// <param name="cs"></param>
        public aCandlestick(aCandlestick cs)
        {
            date = cs.date;
            open = cs.open;
            close = cs.close;
            high = cs.high;
            low = cs.low;
            volume = cs.volume;
        }

    }
}
