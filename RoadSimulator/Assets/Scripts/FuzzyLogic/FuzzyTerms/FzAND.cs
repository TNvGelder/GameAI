using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic.FuzzyTerms
{
    public class FzAND : FuzzyTerm
    {

        public List<FuzzyTerm> Terms = new List<FuzzyTerm>();

        public FzAND(FzAND fa)
        {
            foreach(FuzzyTerm term in fa.Terms)
            {
                Terms.Add(term);
            }
            
        }

        public FzAND(FuzzyTerm op1, FuzzyTerm op2)
        {
            Terms.Add(op1.Clone());
            Terms.Add(op2.Clone());
        }

        public FzAND(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3):this(op2, op3)
        {
            Terms.Add(op1.Clone());
        }

        public FzAND(FuzzyTerm op1, FuzzyTerm op2, FuzzyTerm op3, FuzzyTerm op4):this(op2, op3, op4)
        {
            Terms.Add(op1.Clone());
        }

        public override double DOM
        {
            get
            {
                double smallest = Double.MaxValue;
                foreach (FuzzyTerm term in Terms)
                {
                    if (term.DOM < smallest)
                    {
                        smallest = term.DOM;
                    }
                }
                return smallest;
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
            return new FzAND(this);
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
