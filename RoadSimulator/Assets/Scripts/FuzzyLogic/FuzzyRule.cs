using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic
{
    public class FuzzyRule
    {

        private FuzzyTerm antecedent;
        private FuzzyTerm consequence;

        public FuzzyRule(FuzzyTerm antecedent, FuzzyTerm consequence)
        {
            this.antecedent = antecedent;
            this.consequence = consequence;
        }

        public void SetConfidenceOfConsequentToZero()
        {
            consequence.ClearDOM();
        }

        public void Calculate()
        {
            consequence.ORwithDOM(antecedent.DOM);
        }
    }
}
