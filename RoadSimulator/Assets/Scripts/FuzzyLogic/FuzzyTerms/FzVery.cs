using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic.FuzzyTerms
{
    public class FzVery : FuzzyTerm
    {

        public FuzzySet Set;

        public override double DOM
        {
            get
            {
                return Set.DOM * Set.DOM;
            }
        }

        private FzVery(FzVery inst)
        {
            Set = inst.Set;
        }

        public FzVery(FzSet ft)
        {
            Set = ft.Set;
        }

        public override FuzzyTerm Clone()
        {
            return new FzVery(this);
        }

        public override void ClearDOM()
        {
            Set.ClearDOM();
        }

        public override void ORwithDOM(double val)
        {
            Set.ORwithDOM(val * val);
        }
    }
}
