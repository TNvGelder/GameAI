using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic
{
    public enum DefuzzifyMethod{ max_av, centroid }

    public class FuzzyModule
    {
        
        private Dictionary<string, FuzzyVariable> variables;
        private List<FuzzyRule> rules;

        public void AddRule(FuzzyTerm antecedent, FuzzyTerm consequence)
        {
            rules.Add(new FuzzyRule(antecedent, consequence));
        }

        public FuzzyVariable CreateFLV(string varName)
        {
            variables[varName] = new FuzzyVariable();
            return variables[varName];
        }

        public void Fuzzify(string nameOfFLV, double val)
        {
            if (!variables.ContainsKey(nameOfFLV))
            {
                throw new KeyNotFoundException();
            }

            variables[nameOfFLV].Fuzzify(val);
        }

        public double Defuzzify(string nameOfFLV, DefuzzifyMethod method = DefuzzifyMethod.max_av)
        {
            if (!variables.ContainsKey(nameOfFLV))
            {
                throw new KeyNotFoundException();
            }

            SetConfidencesOfConsequentsToZero();

            foreach (FuzzyRule rule in rules)
            {
                rule.Calculate();
            }

            switch (method)
            {
                case DefuzzifyMethod.centroid:
                    return variables[nameOfFLV].DefuzzifyCentroid(15);
                case DefuzzifyMethod.max_av:
                    return variables[nameOfFLV].DefuzzifyMaxAv();
            }
            return 0;
        }

        private void SetConfidencesOfConsequentsToZero()
        {
            foreach(FuzzyRule curRule in rules)
            {
                curRule.SetConfidenceOfConsequentToZero();
            }
        }

        
    }
}
