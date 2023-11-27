using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace StockProjectCS
{
    public class aCandlestick
    {
        private double _range;
        private double _bodySize;
        private double _upperShadowSize;
        private double _lowerShadowSize;
        private double _topPrice;
        private double _bottomPrice;
        private bool _isBullish;
        private bool _isBearish;
        private bool _isNeutral;
        private bool _isMarubozu;
        private bool _isDoji;
        private bool _isLongLeggedDoji;
        private bool _isDragonflyDoji;
        private bool _isGravestoneDoji;
        private bool _isFourPriceDoji;
        private bool _isHammer;
        private bool _isInvertedHammer;

        public DateTime date { get; set; }
        public double open { get; set; }
        public double close { get; set; }
        public double high { get; set; }
        public double low { get; set; }
        public ulong volume { get; set; }

        public double range { get { return _range; } }
        public double bodySize { get { return _bodySize; } }
        public double upperShadowSize { get { return _upperShadowSize; } }
        public double lowerShadowSize { get { return _lowerShadowSize; } }
        public double topPrice { get { return _topPrice; } }
        public double bottomPrice { get { return _bottomPrice; } }
        public bool isBullish { get { return _isBullish; } }
        public bool isBearish { get { return _isBearish; } }
        public bool isNeutral { get { return _isNeutral; } }
        public bool isMarubozu { get { return _isMarubozu; } }
        public bool isDoji { get { return _isDoji; } }
        public bool isLongLeggedDoji { get { return _isLongLeggedDoji; } }
        public bool isDragonflyDoji { get { return _isDragonflyDoji; } }
        public bool isGravestoneDoji { get { return _isGravestoneDoji; } }
        public bool isFourPriceDoji { get { return _isFourPriceDoji; } }
        public bool isHammer { get { return _isHammer; } }
        public bool isInvertedHammer { get { return _isInvertedHammer; } }

        //leeway variables for different kinds of patterns
        public static double leeway = 0.05;
        public static double doji_leeway = 0.03;
        public static double hammer_leeway = 0.15;
        public static double longLeg_leeway = 0.8;

        public void calculateProperties()
        {
            _range = high - low;
            _bodySize = Math.Abs(open - close);
            _upperShadowSize = Math.Abs(high - Math.Max(open, close));
            _lowerShadowSize = Math.Abs(Math.Min(open, close) - low);
            _topPrice = Math.Max(open, close);
            _bottomPrice = Math.Min(open, close);
        }

        public void calculatePatterns()
        {
            _isBullish = (open * 1.2) < close;
            _isBearish = (close * 1.2) < open;
            _isNeutral = !_isBearish && !_isBullish;
            _isMarubozu = (_bodySize == _range);
            _isDoji = _bodySize / _topPrice <= doji_leeway;
            _isLongLeggedDoji = _isDoji && low <= _bottomPrice * 0.5 && high >= _topPrice * 1.5;
            _isDragonflyDoji = _isDoji && high <= (_topPrice * 1.01) && low <= (_bottomPrice * 0.50);
            _isGravestoneDoji = _isDoji && high >= (_topPrice * 1.50) && low >= (_bottomPrice * 0.01);
            _isFourPriceDoji = high == low && high == close && high == open;
            _isHammer = _bodySize <= (_range * hammer_leeway) && !_isDoji && high <= (_topPrice * 1.02) && _upperShadowSize != 0 && low <= (_bottomPrice * 0.50);
            if(_isHammer) { _isBullish = true; }
            _isInvertedHammer = _bodySize >= (_range * hammer_leeway) && !_isDoji &&  high >= (_topPrice * 1.50) && low >= (_bottomPrice * 0.02) && _lowerShadowSize != 0;
            if (_isInvertedHammer) { _isBearish = true; }
        }
    }
}
