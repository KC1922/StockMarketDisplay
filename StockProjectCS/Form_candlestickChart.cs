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
using StockProjectCS;
using System.Runtime.InteropServices;
using System.Reflection.Metadata;


namespace StockProjectCS
{
    public partial class Form_candlestickChart : Form
    {
        private List<smartCandlestick> originalCandlesticks = new List<smartCandlestick>(1028);
        private List<smartCandlestick> filteredSticks = new List<smartCandlestick>(128);
        private patternRecognizer[] listOfPatternRecognizers;
        //private Dictionary<DataPoint, List<RectangleAnnotation>> annotationDictionary;
        /// <summary>
        /// Creates a chart for displaying the candlestick objects
        /// </summary>
        /// <param name="candlesticks a list of aCandlestick objects"></param>
        /// <param name="tickerName the name of the stock"></param>
        /// <param name="startDate the start of the date range"></param>
        /// <param name="endDate the end of the date range"></param>
        public Form_candlestickChart(List<smartCandlestick> smartCandlesticks, string tickerName, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();

            this.Text = tickerName + " Candlestick Chart";
            originalCandlesticks.AddRange(smartCandlesticks);
            dateTimePicker_Start.Value = startDate;
            dateTimePicker_End.Value = endDate;
            originalCandlesticks.Reverse();
            chart_candlesticks.Titles[0].Text = tickerName + " Candlestick & Volume";

            //create the recognizerd and annotation bases
            listOfPatternRecognizers = createRecognizers();
            //populate combobox options with recognizer names
            foreach (var r in listOfPatternRecognizers)
            {
                comboBox_graphAnnotation.Items.Add(r.patternName);
            }

            updateChartDisplay();
        }
        /// <summary>
        /// Takes the current list of candlesticks in the form candlestickChart and filters out the sticks that fit in the
        /// user defined date range. Also used to update the chart when the user changes the date range.
        /// </summary>
        private void updateChartDisplay()
        {
            filteredSticks?.Clear();
            DateTime startDate = dateTimePicker_Start.Value;
            DateTime endDate = dateTimePicker_End.Value;
            //filter out the sticks in the original list so only those in the date range are in filteredSticks
            foreach (smartCandlestick candle in originalCandlesticks)
            {
                if (candle.date >= startDate && candle.date <= endDate)
                {
                    filteredSticks.Add(candle);
                }
                if (candle.date > endDate) { break; }
            }
            chart_candlesticks.DataSource = filteredSticks;
            chart_candlesticks.DataBind();

            //findCandlestickTypes(filteredSticks);
            chart_candlesticks.Annotations?.Clear();


            //check if data exists in filtered date range. This technically already happens, but the refredh button bypasses the original error check
            if (filteredSticks != null && filteredSticks.Count > 0)
            {
                //set bounds
                double minValue = filteredSticks.Min(c => c.low);
                double maxValue = filteredSticks.Max(c => c.high);
                double limitMinValue = minValue - (0.05 * minValue);
                double limitMaxValue = maxValue + (0.05 * maxValue);

                //set the limits for the candlestick, and format the value label to be more inline with american currency
                chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY.Minimum = (double)limitMinValue;
                chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY.Maximum = (double)limitMaxValue;
                chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY.LabelStyle.Format = "{0:0.00}";

                //change the color of the candlesticks and volume depending on if price goes up or down
                chart_candlesticks.Series["Series_hloc"].CustomProperties = "PriceUpColor=Green,PriceDownColor=Red";
                //To color volume based on if volume goes up or down, have to loop through the points, set first bar to green
                chart_candlesticks.Series["Series_volume"].Points[0].Color = Color.Green;
                for (int i = 1; i < chart_candlesticks.Series["Series_volume"].Points.Count; i++)
                {
                    //compare current volume to previous volume
                    if (chart_candlesticks.Series["Series_volume"].Points[i].YValues[0] > chart_candlesticks.Series["Series_volume"].Points[i - 1].YValues[0])
                    {
                        //color green if volume went up
                        chart_candlesticks.Series["Series_volume"].Points[i].Color = Color.Green;
                    }
                    else
                    {
                        chart_candlesticks.Series["Series_volume"].Points[i].Color = Color.Red;
                    }
                }

                chart_candlesticks.Refresh();

            }
            else
            {
                //if returned list is empty, throw error. Happens when csv file is either empty, for does nt have data for the given date range
                MessageBox.Show($"Error reading stock data. Data either does not exist for given date range, or date range is invalid. \n Tip:Make sure your start date is the oldest date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }

        /// <summary>
        /// Updates the chart display to the new user defined date range
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Update_Click(object sender, EventArgs e)
        {
            chart_candlesticks.Annotations?.Clear();
            richTextBox_currentAnnotations?.Clear();
            updateChartDisplay();
        }

        /// <summary>
        /// Creates a list of recognizers used to determine patterns
        /// </summary>
        /// <returns></returns>
        private patternRecognizer[] createRecognizers()
        {
            patternRecognizer[] recognizers = new patternRecognizer[] {
                new BullishRecognizer(),
                new BearishRecognizer(),
                new NeutralRecognizer(),
                new MarubozuRecognizer(),
                new DojiRecognizer(),
                new LongLeggedDojiRecognizer(),
                new DragonflyDojiRecognizer(),
                new GravestoneDojiRecognizer(),
                new FourPriceDojiRecognizer(),
                new HammerRecognizer(),
                new InvertedHammerRecognizer(),
                new MorningStarRecognizer(),
                new EveningStarRecognizer(),
                new EngulfingRecognizer(),
                new BullishEngulfingRecognizer(),
                new BearishEngulfingRecognizer(),
                new HarmaniRecognizer(),
                new BullishHarmaniRecognizer(),
                new BearishHarmaniRecognizer(),
                new PeakRecognizer(),
                new ValleyRecognizer(),
                new ThreeSolidersRecognizer(),
                new ThreeCrowsRecognizer(),
            };

            return recognizers;
        }


        /// <summary>
        /// Checks what option in the combobox the user selected and calls the appropriate function to make annotations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_graphAnnotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = filteredSticks.Count;
            int index = 0;
            List<int> indicies = new List<int>(count);
            int selectedPattern = comboBox_graphAnnotation.SelectedIndex;
            //pass the list of filtered candlesticks to the selected pattern recognizer
            patternRecognizer pattern = listOfPatternRecognizers[selectedPattern];
            Color patternColor = pattern.annotationColor;
            string patternName = pattern.patternName;
            indicies = pattern.recognizePatterns(filteredSticks);
            int lastIndex = 0;
            if (!(indicies.Count == 0))
            {
                lastIndex = indicies.Last();
                updateRichTextBoxWithPatternInfo(patternName, indicies, patternColor);
            }

            //loop through current chart candlesticks and add an annotation for each of the matching indexes
            foreach (DataPoint candle in chart_candlesticks.Series[0].Points)
            {
                //if not annotations were found, break from the loop 
                if (lastIndex == 0) { break; }
                //if (chart_candlesticks.Annotations[index].ToolTip == patternName) { chart_candlesticks.Annotations.Remove(chart_candlesticks.Annotations[index]); }
                if (indicies.Contains(index))
                {
                    double high = candle.YValues[0];
                    double low = candle.YValues[1];
                    RectangleAnnotation annotation = createAnnotation(patternColor, high, low, candle, count, patternName);
                    chart_candlesticks.Annotations.Add(annotation);
                }
                //if we've passed the last index where the pattern was found, break from the loop to save time
                if (index > lastIndex) { break; }
                index++;
            }

            chart_candlesticks.Invalidate();
        }
        /// <summary>
        /// Creates an annotation around a chart element dependent on properties passed
        /// </summary>
        /// <param name="lineColor color of the annotation"></param>
        /// <param name="high the high of the candletstick"></param>
        /// <param name="low the low of the candlestick"></param>
        /// <param name="anchorDataPoint the datapoint of the chart candlestick"></param>
        /// <param name="count how many candlesticks in the list"></param>
        /// <param name="patternName the name of the selected pattern"></param>
        /// <returns></returns>
        private RectangleAnnotation createAnnotation(Color lineColor, double high, double low, DataPoint anchorDataPoint, int count, string patternName)
        {
            int numberOfCandlesticks = count;
            double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition(high);
            double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition(low);
            double height = (Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition)) / 5;
            double width = 60.0 / numberOfCandlesticks;
            if (width < 1.0) width = 1.0;

            RectangleAnnotation annotation = new RectangleAnnotation
            {
                Visible = true,
                BackColor = Color.Transparent,
                LineColor = lineColor,
                LineWidth = 2,
                Height = height,
                Width = width,
                AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX,
                AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY,
                AnchorDataPoint = anchorDataPoint,
                Y = high,
                AnchorAlignment = ContentAlignment.BottomCenter,
                ClipToChartArea = "ChartArea_candlesticks",
                ToolTip = patternName,

            };
            /*check if the anchorDataPoint already has annotations in the dictionary
            if (annotationDictionary.ContainsKey(anchorDataPoint))
            {
                var existingAnnotations = annotationDictionary[anchorDataPoint];
                for (int i = existingAnnotations.Count - 1; i >= 0; i--)
                {
                    var existingAnnotation = existingAnnotations[i];
                    if (existingAnnotation.ToolTip == patternName)
                    {
                        //remove any matching annotation from the list
                        existingAnnotations.RemoveAt(i);
                    }
                }
            }
            else
            {
                //if the anchorDataPoint is not in the dictionary, add it with an empty list
                annotationDictionary[anchorDataPoint] = new List<RectangleAnnotation>();
            }

            //add the new annotation to the list associated with the anchorDataPoint
            annotationDictionary[anchorDataPoint].Add(annotation);*/

            return annotation;
        }
        private void updateRichTextBoxWithPatternInfo(string patternName, List<int> indices, Color color)
        {
            //format the text displayed
            string message = $"{patternName}: [{string.Join(", ", indices)}]\n";
            if (!richTextBox_currentAnnotations.Text.Contains(message))
            {
                //append the message to the RichTextBox
                richTextBox_currentAnnotations.AppendText(message);
                //set the color of the newly added text
                richTextBox_currentAnnotations.Select(richTextBox_currentAnnotations.TextLength - message.Length, message.Length);
                richTextBox_currentAnnotations.SelectionColor = color;

                richTextBox_currentAnnotations.Select(richTextBox_currentAnnotations.TextLength, 0);
                richTextBox_currentAnnotations.SelectionColor = richTextBox_currentAnnotations.ForeColor;

                richTextBox_currentAnnotations.ScrollToCaret();
            }
        }

        private void button_Clear_Click(object sender, EventArgs e)
        {
            chart_candlesticks.Annotations?.Clear();
            richTextBox_currentAnnotations?.Clear();
        }

    }
}
