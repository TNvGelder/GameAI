using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic.FuzzyTerms
{
    /// <summary>
    /// This class provides a proxy for a fuzzy set and can be used to create fuzzy rules.
    /// </summary>
    public class FzSet : FuzzyTerm
    {
        private FuzzySet set;

        public FzSet(FuzzySet fs)
        {
            set = fs;
        }

        public FuzzySet Set
        {
            get
            {
                return set;
            }
        }

        public override double DOM
        {
            get
            {
                return set.DOM;
            }

        }

        public override void ClearDOM()
        {
            set.ClearDOM();
        }

        public override FuzzyTerm Clone()
        {
            return new FzSet(this.set);
        }

        public override void ORwithDOM(double val)
        {
            set.ORwithDOM(val);
        }
    }
}
