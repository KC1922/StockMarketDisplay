using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockProjectCS
{
    public abstract class patternRecognizer
    {
        int patternSize;
        string patternName;

        public virtual bool recognize(List<aCandlestick> candlestick);
        
        patternRecognizer(int psize, string pname)
        {
            patternSize = psize;
            patternName = pname;
        }
    }
    class dojiRecognizer : patternRecognizer
    {
        dojiRecognizer() : base (2, "Doji");
        bool recognize(List<aCandlestick> cs)
        {
            return true;
        }
    }
}
