using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

namespace StockProjectCS
{
    public class chartAnnotationFunctions
    {
        /// <summary>
        /// Used to calculate normal Doji sticks
        /// </summary>
        /// <param name="candlestickSeries"></param>
        /// <returns></returns>
        public static List<DataPoint> FilterDogiChartPoints(Series candlestickSeries)
        {
            List<DataPoint> matchedPoints = new List<DataPoint>(128);
            foreach (DataPoint dataPoint in candlestickSeries.Points)
            {
                //values from datapoint
                double high = dataPoint.YValues[0];
                double low = dataPoint.YValues[1];
                double open = dataPoint.YValues[2];
                double close = dataPoint.YValues[3];

                //find the difference of the high and low and see if there is a 3% difference
                //formula I found online for Dojis: |O - C| <= 0.1 * (H - L)
                if ((Math.Abs(open - close)) <= (0.1 * (high - low)))
                {
                    matchedPoints.Add(dataPoint);
                    dataPoint.MarkerStyle = MarkerStyle.None;
                    dataPoint.MarkerColor = Color.Purple;
                    dataPoint.MarkerSize = 10;
                }
            }
            return matchedPoints;
        }
    }
}
