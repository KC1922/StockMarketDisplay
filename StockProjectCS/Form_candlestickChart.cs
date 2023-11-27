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


namespace StockProjectCS
{
    public partial class Form_candlestickChart : Form
    {
        private List<aCandlestick> originalCandlesticks = new List<aCandlestick>(1028);
        private List<aCandlestick> filteredSticks = new List<aCandlestick>(128);
        private Dictionary<string, List<int>> candlestickPatternIndicies = new Dictionary<string, List<int>>();
        /// <summary>
        /// Creates a chart for displaying the candlestick objects
        /// </summary>
        /// <param name="candlesticks a list of aCandlestick objects"></param>
        /// <param name="tickerName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        public Form_candlestickChart(List<aCandlestick> candlesticks, string tickerName, DateTime startDate, DateTime endDate)
        {
            InitializeComponent();

            this.Text = tickerName + " Candlestick Chart";
            originalCandlesticks.AddRange(candlesticks);
            dateTimePicker_Start.Value = startDate;
            dateTimePicker_End.Value = endDate;

            /*added scrolling hard coded since it wasn't working for some reason
            chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX.ScrollBar.Enabled = true;
            chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX.ScaleView.Zoomable = true;
            chart_candlesticks.ChartAreas["ChartArea_candlesticks"].CursorX.IsUserSelectionEnabled = true;
            chart_candlesticks.ChartAreas["ChartArea_volume"].AxisX.ScrollBar.Enabled = true;
            chart_candlesticks.ChartAreas["ChartArea_volume"].AxisX.ScaleView.Zoomable = true;
            chart_candlesticks.ChartAreas["ChartArea_volume"].CursorX.IsUserSelectionEnabled = true;*/

            chart_candlesticks.Titles[0].Text = tickerName + " Candlestick & Volume";

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
            filteredSticks = originalCandlesticks.Where(c => c.date >= startDate && c.date <= endDate).ToList();
            filteredSticks.Reverse();

            filteredSticks[1].calculatePatterns();

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

                var dojiPoints = chartAnnotationFunctions.FilterDogiChartPoints(chart_candlesticks.Series["Series_hloc"]);


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
            updateChartDisplay();
        }
        /// <summary>
        /// Checks what option in the combobox the user selected and calls the appropriate function to make annotations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_graphAnnotation_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart_candlesticks.Annotations?.Clear();
            if (comboBox_graphAnnotation.SelectedItem == "Bullish")
            {
                addBullishAnnotations();
            }
            else if (comboBox_graphAnnotation.SelectedItem == "Bearish")
            {

                addBearishAnnotations();
            }
            else if (comboBox_graphAnnotation.SelectedItem == "Neutral")
            {
                addNeutralAnnotations();

            }
            else if (comboBox_graphAnnotation.SelectedItem == "Marubozu")
            {
                addMarubozuAnnotations();

            }
            else if (comboBox_graphAnnotation.SelectedItem == "Doji")
            {

                addDojiAnnotations();
            }
            else if (comboBox_graphAnnotation.SelectedItem == "Long-Legged Doji")
            {

                addLongLeggedAnnotations();
            }
            else if (comboBox_graphAnnotation.SelectedItem == "Dragonfly Doji")
            {

                addDragonflyAnnotations();
            }
            else if (comboBox_graphAnnotation.SelectedItem == "Gravestone Doji")
            {

                addGravestoneAnnotations();
            }
            else if (comboBox_graphAnnotation.SelectedItem == "Four-Price Doji")
            {

                addFourPriceAnnotations();
            }
            else if (comboBox_graphAnnotation.SelectedItem == "Hammer")
            {
                addHammerAnnotations();
            }
            else if (comboBox_graphAnnotation.SelectedItem == "Inverted Hammer")
            {
                addInvertedHammerAnnotations();
            }
            else
            {

            }
            chart_candlesticks.Invalidate();
        }
        /// <summary>
        /// adds annotations to the candlestick chart to highlight dojis
        /// </summary>
        private void addDojiAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isDoji)
                {
                    RectangleAnnotation dojiAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    dojiAnnotation.Visible = true;
                    dojiAnnotation.BackColor = Color.Transparent;
                    dojiAnnotation.LineColor = Color.RoyalBlue;
                    dojiAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition(candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((candle.low));
                    double ratio = (Math.Abs(highPosition - lowPosition) / Math.Max(highPosition, lowPosition)) * 40;
                    dojiAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    dojiAnnotation.Height = dojiAnnotation.Height / 5;
                    //if (dojiAnnotation.Height > 10) { dojiAnnotation.Height = 10; }
                    dojiAnnotation.Width = 60 / numberOfCandlesticks;
                    if (dojiAnnotation.Width == 0) { dojiAnnotation.Width = 1; }
                    //set position
                    dojiAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    dojiAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    dojiAnnotation.AnchorDataPoint = dataPoint;
                    dojiAnnotation.Y = (double)(candle.high);
                    dojiAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    dojiAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(dojiAnnotation);

                }
                index++;
            }
        }
        /// <summary>
        /// adds annotations to the candlestick chart to highlight bullish sticks
        /// </summary>
        private void addBullishAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isBullish)
                {
                    RectangleAnnotation bullAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    bullAnnotation.Visible = true;
                    bullAnnotation.BackColor = Color.Transparent;
                    bullAnnotation.LineColor = Color.LawnGreen;
                    bullAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    bullAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    bullAnnotation.Height = bullAnnotation.Height / 5;
                    bullAnnotation.Width = 60 / numberOfCandlesticks;
                    if (bullAnnotation.Width == 0) { bullAnnotation.Width = 1; }
                    //set position
                    bullAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    bullAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    bullAnnotation.AnchorDataPoint = dataPoint;
                    bullAnnotation.Y = (double)(candle.high);
                    bullAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    //dojiAnnotation.AnchorY = (double)(Decimal.Divide(candle.range, 2)); 
                    bullAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(bullAnnotation);

                }
                index++;
            }
        }
        /// <summary>
        /// adds annotations to the candlestick chart to highlight bullish sticks
        /// </summary>
        private void addBearishAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isBearish)
                {
                    RectangleAnnotation bearAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    bearAnnotation.Visible = true;
                    bearAnnotation.BackColor = Color.Transparent;
                    bearAnnotation.LineColor = Color.IndianRed;
                    bearAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    bearAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    bearAnnotation.Height = bearAnnotation.Height / 5;
                    bearAnnotation.Width = 60 / numberOfCandlesticks;
                    if (bearAnnotation.Width == 0) { bearAnnotation.Width = 1; }
                    //set position
                    bearAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    bearAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    bearAnnotation.AnchorDataPoint = dataPoint;
                    bearAnnotation.Y = (double)(candle.high);
                    bearAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    bearAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(bearAnnotation);

                }
                index++;
            }
        }
        private void addNeutralAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isNeutral)
                {
                    RectangleAnnotation neutralAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    neutralAnnotation.Visible = true;
                    neutralAnnotation.BackColor = Color.Transparent;
                    neutralAnnotation.LineColor = Color.Black;
                    neutralAnnotation.LineDashStyle = ChartDashStyle.Dash;
                    neutralAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    neutralAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    neutralAnnotation.Height = neutralAnnotation.Height / 5;
                    neutralAnnotation.Width = 60 / numberOfCandlesticks;
                    if (neutralAnnotation.Width == 0) { neutralAnnotation.Width = 1; }
                    //set position
                    neutralAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    neutralAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    neutralAnnotation.AnchorDataPoint = dataPoint;
                    neutralAnnotation.Y = (double)(candle.high);
                    neutralAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    neutralAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(neutralAnnotation);

                }
                index++;
            }
        }
        private void addMarubozuAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isMarubozu)
                {
                    RectangleAnnotation marubozuAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    marubozuAnnotation.Visible = true;
                    marubozuAnnotation.BackColor = Color.Transparent;
                    marubozuAnnotation.LineColor = Color.RebeccaPurple;
                    marubozuAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    marubozuAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    marubozuAnnotation.Height = marubozuAnnotation.Height / 5;
                    marubozuAnnotation.Width = 60 / numberOfCandlesticks;
                    if (marubozuAnnotation.Width == 0) { marubozuAnnotation.Width = 1; }
                    //set position
                    marubozuAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    marubozuAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    marubozuAnnotation.AnchorDataPoint = dataPoint;
                    marubozuAnnotation.Y = (double)(candle.high);
                    marubozuAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    marubozuAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(marubozuAnnotation);

                }
                index++;
            }
        }
        private void addLongLeggedAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isLongLeggedDoji)
                {
                    RectangleAnnotation longLeggedAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    longLeggedAnnotation.Visible = true;
                    longLeggedAnnotation.BackColor = Color.Transparent;
                    longLeggedAnnotation.LineColor = Color.FloralWhite;
                    longLeggedAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    longLeggedAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    longLeggedAnnotation.Height = longLeggedAnnotation.Height / 5;
                    longLeggedAnnotation.Width = 60 / numberOfCandlesticks;
                    if (longLeggedAnnotation.Width == 0) { longLeggedAnnotation.Width = 1; }
                    //set position
                    longLeggedAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    longLeggedAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    longLeggedAnnotation.AnchorDataPoint = dataPoint;
                    longLeggedAnnotation.Y = (double)(candle.high);
                    longLeggedAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    longLeggedAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(longLeggedAnnotation);

                }
                index++;
            }
        }
        private void addDragonflyAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isDragonflyDoji)
                {
                    RectangleAnnotation dragonflyAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    dragonflyAnnotation.Visible = true;
                    dragonflyAnnotation.BackColor = Color.Transparent;
                    dragonflyAnnotation.LineColor = Color.LimeGreen;
                    dragonflyAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    dragonflyAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    dragonflyAnnotation.Height = dragonflyAnnotation.Height / 5;
                    dragonflyAnnotation.Width = 60 / numberOfCandlesticks;
                    if (dragonflyAnnotation.Width == 0) { dragonflyAnnotation.Width = 1; }
                    //set position
                    dragonflyAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    dragonflyAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    dragonflyAnnotation.AnchorDataPoint = dataPoint;
                    dragonflyAnnotation.Y = (double)(candle.high);
                    dragonflyAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    dragonflyAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(dragonflyAnnotation);

                }
                index++;
            }
        }
        private void addGravestoneAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isGravestoneDoji)
                {
                    RectangleAnnotation gravestoneAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    gravestoneAnnotation.Visible = true;
                    gravestoneAnnotation.BackColor = Color.Transparent;
                    gravestoneAnnotation.LineColor = Color.Black;
                    gravestoneAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    gravestoneAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    gravestoneAnnotation.Height = gravestoneAnnotation.Height / 5;
                    gravestoneAnnotation.Width = 60 / numberOfCandlesticks;
                    if (gravestoneAnnotation.Width == 0) { gravestoneAnnotation.Width = 1; }
                    //set position
                    gravestoneAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    gravestoneAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    gravestoneAnnotation.AnchorDataPoint = dataPoint;
                    gravestoneAnnotation.Y = (double)(candle.high);
                    gravestoneAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    gravestoneAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(gravestoneAnnotation);

                }
                index++;
            }
        }
        private void addFourPriceAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isFourPriceDoji)
                {
                    RectangleAnnotation fourPriceAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    fourPriceAnnotation.Visible = true;
                    fourPriceAnnotation.BackColor = Color.Transparent;
                    fourPriceAnnotation.LineColor = Color.Gold;
                    fourPriceAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    fourPriceAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    fourPriceAnnotation.Height = fourPriceAnnotation.Height / 5;
                    fourPriceAnnotation.Width = 60 / numberOfCandlesticks;
                    if (fourPriceAnnotation.Width == 0) { fourPriceAnnotation.Width = 1; }
                    //set position
                    fourPriceAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    fourPriceAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    fourPriceAnnotation.AnchorDataPoint = dataPoint;
                    fourPriceAnnotation.Y = (double)(candle.high);
                    fourPriceAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    fourPriceAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(fourPriceAnnotation);

                }
                index++;
            }
        }
        private void addHammerAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isHammer)
                {
                    RectangleAnnotation hammerAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    hammerAnnotation.Visible = true;
                    hammerAnnotation.BackColor = Color.Transparent;
                    hammerAnnotation.LineColor = Color.ForestGreen;
                    hammerAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    hammerAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    hammerAnnotation.Height = hammerAnnotation.Height / 5;
                    hammerAnnotation.Width = 60 / numberOfCandlesticks;
                    if (hammerAnnotation.Width == 0) { hammerAnnotation.Width = 1; }
                    //set position
                    hammerAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    hammerAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    hammerAnnotation.AnchorDataPoint = dataPoint;
                    hammerAnnotation.Y = (double)(candle.high);
                    hammerAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    hammerAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(hammerAnnotation);

                }
                index++;
            }
        }
        private void addInvertedHammerAnnotations()
        {
            int index = 0;
            double chartAreaWidth = chart_candlesticks.Width;
            int numberOfCandlesticks = filteredSticks.Count;
            double candlestickWidth = chartAreaWidth / numberOfCandlesticks;

            foreach (var candle in filteredSticks)
            {
                DataPoint dataPoint = chart_candlesticks.Series[0].Points[index];
                if (candle.isInvertedHammer)
                {
                    RectangleAnnotation invertedHammerAnnotation = new RectangleAnnotation();
                    //set the annotation properties
                    invertedHammerAnnotation.Visible = true;
                    invertedHammerAnnotation.BackColor = Color.Transparent;
                    invertedHammerAnnotation.LineColor = Color.DarkRed;
                    invertedHammerAnnotation.LineWidth = 2;
                    double highPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)candle.high);
                    double lowPosition = chart_candlesticks.ChartAreas[0].AxisY.ValueToPixelPosition((double)(candle.low));
                    invertedHammerAnnotation.Height = Math.Max(highPosition, lowPosition) - Math.Min(highPosition, lowPosition);
                    invertedHammerAnnotation.Height = invertedHammerAnnotation.Height / 5;
                    invertedHammerAnnotation.Width = 60 / numberOfCandlesticks;
                    if (invertedHammerAnnotation.Width == 0) { invertedHammerAnnotation.Width = 1; }
                    //set position
                    invertedHammerAnnotation.AxisX = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisX;
                    invertedHammerAnnotation.AxisY = chart_candlesticks.ChartAreas["ChartArea_candlesticks"].AxisY;
                    invertedHammerAnnotation.AnchorDataPoint = dataPoint;
                    invertedHammerAnnotation.Y = (double)(candle.high);
                    invertedHammerAnnotation.AnchorAlignment = ContentAlignment.BottomCenter;
                    invertedHammerAnnotation.ClipToChartArea = "ChartArea_candlesticks";
                    chart_candlesticks.Annotations.Add(invertedHammerAnnotation);

                }
                index++;
            }
        }

    }


}
