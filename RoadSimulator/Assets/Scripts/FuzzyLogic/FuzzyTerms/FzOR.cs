using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic.FuzzyTerms
{
    public class FzOR : FuzzyTerm
    {

        public List<FuzzyTerm> Terms = new List<FuzzyTerm>();

        public FzOR(FzOR fa)
        {
            foreach (FuzzyTerm term in fa.Terms)
            {
                Terms.Add(term);
            }

        }

        public FzOR(FuzzyTerm op1, FuzzyTerm op2)
        {
            Terms.Add(op1.Clone());
            Terms.Add(op2.Clone());
        }

        public FzOR(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3):this(op2, op3)
        {
            Terms.Add(op1.Clone());
        }

        public FzOR(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4):this(op2, op3, op4)
        {
            Terms.Add(op1.Clone());
        }

        public override double DOM
        {
            get
            {
                double largest = Double.MinValue;
                foreach (FuzzyTerm term in Terms)
                {
                    if (term.DOM > largest)
                    {
                        largest = term.DOM;
                    }
                }
                return largest;
            }
        }

        public override void ClearDOM()
        {
            foreach (FuzzyTerm term in Terms)
            {
                term.ClearDOM();
            }
        }

        public override FuzzyTerm Clone()
        {
            return new FzOR(this);
        }

        public override void ORwithDOM(double val)
        {
            foreach (FuzzyTerm term in Terms)
            {
                term.ORwithDOM(val);
            }
        }
    }
}
