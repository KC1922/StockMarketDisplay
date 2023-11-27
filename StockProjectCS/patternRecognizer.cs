using System.Reflection.Metadata;

namespace StockProjectCS
{
    public abstract class patternRecognizer
    {
        public int patternSize = 0;
        public string patternName;
        public Color annotationColor;

        //constructor
        public patternRecognizer(int pSize, string pName, Color aColor)
        {
            this.patternSize = pSize;
            this.patternName = pName;
            this.annotationColor = aColor;
        }

        /// <summary>
        /// Finds which indecies the candle stick pattern is found in
        /// </summary>
        /// <param name="listSmartcs"></param>
        /// <returns></returns>
        public List<int> recognizePatterns(List<smartCandlestick> listSmartcs)
        {
            int listSize = listSmartcs.Count;
            List<int> foundIndecies = new List<int>(listSize);
            for (int index = 0; index <= listSize - patternSize; index++)
            {
                //create a sub list of candlesticks based on the pattern size provided by the other class
                List<smartCandlestick> subList = listSmartcs.GetRange(index, patternSize);
                if (recognizePattern(subList))
                {
                    //add each candlestick of the pattern to the list
                    for (int patternIndex = index; patternIndex < index + patternSize; patternIndex++)
                    {
                        foundIndecies.Add(patternIndex);
                    }
                }
            }
            return foundIndecies;
        }

        /// <summary>
        /// Abstract class that is used by other classes to form the basis of their pattern recognizing logic
        /// </summary>
        /// <param name="cs"></param>
        /// <returns></returns>
        public virtual bool recognizePattern(List<smartCandlestick> cs) { return false; }
    }

    public class BullishRecognizer : patternRecognizer
    {
        public BullishRecognizer() : base(1, "Bullish", Color.SpringGreen) { }

        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isBullish; }
    }
    public class BearishRecognizer : patternRecognizer
    {
        public BearishRecognizer() : base(1, "Bearish", Color.DarkRed) { }

        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isBearish; }
    }
    public class NeutralRecognizer : patternRecognizer
    {
        public NeutralRecognizer() : base(1, "Neutral", Color.DarkSlateGray) { }

        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isNeutral; }
    }
    public class MarubozuRecognizer : patternRecognizer
    {
        public MarubozuRecognizer() : base(1, "Marubozu", Color.MediumPurple) { }

        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isMarubozu; }
    }
    public class DojiRecognizer : patternRecognizer
    {

        public DojiRecognizer() : base(1, "Doji", Color.RoyalBlue) { }
        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isDoji; }
    }
    public class LongLeggedDojiRecognizer : patternRecognizer
    {
        public LongLeggedDojiRecognizer() : base(1, "Long-Legged Doji", Color.ForestGreen) { }
        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isLongLeggedDoji; }
    }
    public class DragonflyDojiRecognizer : patternRecognizer
    {
        public DragonflyDojiRecognizer() : base(1, "Dragonfly Doji", Color.LimeGreen) { }
        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isDragonflyDoji; }
    }
    public class GravestoneDojiRecognizer : patternRecognizer
    {
        public GravestoneDojiRecognizer() : base(1, "Gravestone Doji", Color.IndianRed) { }
        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isGravestoneDoji; }
    }
    public class FourPriceDojiRecognizer : patternRecognizer
    {
        public FourPriceDojiRecognizer() : base(1, "Four-Price Doji", Color.Gold) { }
        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isFourPriceDoji; }
    }
    public class HammerRecognizer : patternRecognizer
    {
        public HammerRecognizer() : base(1, "Hammer", Color.Orange) { }
        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isHammer; }
    }
    public class InvertedHammerRecognizer : patternRecognizer
    {
        public InvertedHammerRecognizer() : base(1, "Inverted Hammer", Color.Blue) { }
        public override bool recognizePattern(List<smartCandlestick> cs) { return cs[0].isInvertedHammer; }
    }
    public class MorningStarRecognizer : patternRecognizer
    {
        public MorningStarRecognizer() : base(3, "Morning Star", Color.DarkGreen) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var mcs = cs[1];
            var rcs = cs[2];
            return (lcs.isBearish && (mcs.isDoji || mcs.isNeutral) && rcs.isBullish && mcs.low < lcs.low && mcs.low > rcs.low);
        }
    }
    public class EveningStarRecognizer : patternRecognizer
    {
        public EveningStarRecognizer() : base(3, "Evening Star", Color.DarkRed) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var mcs = cs[1];
            var rcs = cs[2];
            return (lcs.isBullish && (mcs.isDoji || mcs.isNeutral) && rcs.isBearish && mcs.high > lcs.high && mcs.high > rcs.high);
        }
    }
    public class EngulfingRecognizer : patternRecognizer
    {
        public EngulfingRecognizer() : base(2, "Engulfing", Color.LightGoldenrodYellow) { }

        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var rcs = cs[1];
            return (lcs.high < rcs.topPrice && lcs.low > rcs.bottomPrice);
        }
    }
    public class BullishEngulfingRecognizer : patternRecognizer
    {
        public BullishEngulfingRecognizer() : base(2, "Bullish Engulfing", Color.LightGreen) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var rcs = cs[1];
            return (lcs.high < rcs.topPrice && lcs.isBearish && lcs.low > rcs.bottomPrice && rcs.isBullish);
        }
    }
    public class BearishEngulfingRecognizer : patternRecognizer
    {
        public BearishEngulfingRecognizer() : base(2, "Bearish Engulfing", Color.OrangeRed) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var rcs = cs[1];
            return (lcs.high < rcs.topPrice && lcs.isBullish && lcs.low > rcs.bottomPrice && rcs.isBearish);
        }
    }
    public class HarmaniRecognizer : patternRecognizer
    {
        public HarmaniRecognizer() : base(2, "Harmani", Color.LightYellow) { }

        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var rcs = cs[1];
            return (lcs.high > rcs.topPrice && lcs.low < rcs.bottomPrice);
        }
    }
    public class BullishHarmaniRecognizer : patternRecognizer
    {
        public BullishHarmaniRecognizer() : base(2, "Bullish Harmani", Color.SpringGreen) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var rcs = cs[1];
            return (lcs.high > rcs.topPrice && lcs.isBearish && lcs.low < rcs.bottomPrice && rcs.isBullish);
        }
    }
    public class BearishHarmaniRecognizer : patternRecognizer
    {
        public BearishHarmaniRecognizer() : base(2, "Bearish Harmani", Color.PaleVioletRed) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var rcs = cs[1];
            return (lcs.high > rcs.topPrice && lcs.isBullish && lcs.low < rcs.bottomPrice && rcs.isBearish);
        }
    }
    public class PeakRecognizer : patternRecognizer
    {
        public PeakRecognizer() : base(3, "Peak", Color.LightGreen) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var mcs = cs[1];
            var rcs = cs[2];
            return (lcs.high < mcs.high && rcs.high < mcs.high);
        }
    }
    public class ValleyRecognizer : patternRecognizer
    {
        public ValleyRecognizer() : base(3, "Valley", Color.DarkRed) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var mcs = cs[1];
            var rcs = cs[2];
            return (lcs.low > mcs.low && rcs.low > mcs.low);
        }
    }
    public class ThreeSolidersRecognizer : patternRecognizer
    {
        public ThreeSolidersRecognizer() : base(3, "Three Wise Soldiers", Color.LightGreen) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var mcs = cs[1];
            var rcs = cs[2];
            return (lcs.isBullish && mcs.isBullish && mcs.close > lcs.close && rcs.isBullish && rcs.close > mcs.close);
        }
    }
    public class ThreeCrowsRecognizer : patternRecognizer
    {
        public ThreeCrowsRecognizer() : base(3, "Three Black Crows", Color.DarkRed) { }
        public override bool recognizePattern(List<smartCandlestick> cs)
        {
            var lcs = cs[0];
            var mcs = cs[1];
            var rcs = cs[2];
            return (lcs.isBearish && mcs.isBearish && mcs.close < lcs.close && rcs.isBearish && rcs.close < mcs.close);
        }
    }
}