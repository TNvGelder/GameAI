using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic
{
    public abstract class FuzzyTerm
    {

        

        public abstract FuzzyTerm Clone();

        public abstract double DOM
        {
            get; 
        }

        public abstract void ClearDOM();

        public abstract void ORwithDOM(double val);
    }
}
