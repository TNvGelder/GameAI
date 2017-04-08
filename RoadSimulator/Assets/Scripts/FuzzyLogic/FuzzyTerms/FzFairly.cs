using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic.FuzzyTerms
{
    public class FzFairly : FuzzyTerm
    {

        public FuzzySet Set;

        public override double DOM
        {
            get
            {
                return Math.Sqrt(Set.DOM);
            }
        }

        private FzFairly(FzFairly inst)
        {
            Set = inst.Set;
        }

        public FzFairly(FzSet ft)
        {
            Set = ft.Set;
        }

        public override FuzzyTerm Clone()
        {
            return new FzFairly(this);
        }

        public override void ClearDOM()
        {
            Set.ClearDOM();
        }

        public override void ORwithDOM(double val)
        {
            Set.ORwithDOM(Math.Sqrt(val));
        }
    }
}
