using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProjectCS
{
    /// <summary>
    /// Version of aCandlestick that includes properties and patterns
    /// </summary>
    public class smartCandlestick : aCandlestick
    {
        private double _range;
        private double _bodySize;
        private double _upperShadowSize;
        private double _lowerShadowSize;
        private double _topPrice;
        private double _bottomPrice;

        public double range { get { return _range; } }
        public double bodySize { get { return _bodySize; } }
        public double upperShadowSize { get { return _upperShadowSize; } }
        public double lowerShadowSize { get { return _lowerShadowSize; } }
        public double topPrice { get { return _topPrice; } }
        public double bottomPrice { get { return _bottomPrice; } }
        public bool isBullish { get; private set; }
        public bool isBearish { get; private set; }
        public bool isNeutral { get; private set; }
        public bool isPriceUp { get; private set; }
        public bool isMarubozu { get; private set; }
        public bool isDoji { get; private set; }
        public bool isLongLeggedDoji { get; private set; }
        public bool isDragonflyDoji { get; private set; }
        public bool isGravestoneDoji { get; private set; }
        public bool isFourPriceDoji { get; private set; }
        public bool isHammer { get; private set; }
        public bool isInvertedHammer { get; private set; }

        public smartCandlestick() : base() { }

        /// <summary>
        /// constructor for smartCandlestick given a candlestick
        /// </summary>
        /// <param name="cs"></param>
        public smartCandlestick(aCandlestick cs) : base(cs.date, cs.open, cs.high, cs.low, cs.close, cs.volume)
        {
            calculateProperties();
            calculatePatterns();
        }

        /// <summary>
        /// constructor for smartCandlestick when given a CSV line
        /// </summary>
        /// <param name="csvLine"></param>
        public smartCandlestick(string csvLine) : base(csvLine)
        {
            calculateProperties();
            calculatePatterns();
        }
        //leeway variables for different kinds of patterns
        public static double leeway = 0.05;
        public static double bullishLeeway = 0.6;
        public static double bearishLeeway = 0.6;
        public static double dojiLeeway = 0.04;
        public static double hammerLeeway = 0.15;
        public static double longLegLeeway = 0.8;
        /// <summary>
        /// Calculates the physical properties of the candlestick
        /// </summary>
        public void calculateProperties()
        {
            _range = high - low;
            _bodySize = Math.Abs(open - close);
            _upperShadowSize = Math.Abs(high - Math.Max(open, close));
            _lowerShadowSize = Math.Abs(Math.Min(open, close) - low);
            _topPrice = Math.Max(open, close);
            _bottomPrice = Math.Min(open, close);
        }
        /// <summary>
        /// Calculates the type of the candlestick
        /// </summary>
        public void calculatePatterns()
        {
            isBullish = close > (open + (range * bullishLeeway));
            isBearish = open > (close + (range * bearishLeeway));
            isNeutral = !isBearish && !isBullish;
            isPriceUp = topPrice > bottomPrice && bodySize != 0;
            isMarubozu = _bodySize == _range;
            isDoji = _bodySize <= (range * dojiLeeway);
            isLongLeggedDoji = isDoji && (low <= _bottomPrice * 0.5) && (high >= _topPrice * 1.5);
            isDragonflyDoji = isDoji && high <= (_topPrice * 1.01) && low <= (_bottomPrice * 0.50);
            isGravestoneDoji = isDoji && high >= (_topPrice * 1.50) && low >= (_bottomPrice * 0.01);
            isFourPriceDoji = high == low && high == close && high == open;
            isHammer = _bodySize <= (_range * hammerLeeway) && !isDoji && high <= (_topPrice * 1.02) && _upperShadowSize != 0 && low <= (_bottomPrice * 0.50);
            if (isHammer) { isBullish = true; }
            isInvertedHammer = _bodySize >= (_range * hammerLeeway) && !isDoji && high >= (_topPrice * 1.50) && low >= (_bottomPrice * 0.02) && _lowerShadowSize != 0;
            if (isInvertedHammer) { isBearish = true; }
        }
    }
}
