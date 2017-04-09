using Assets.Scripts.FuzzyLogic.FuzzyTerms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic
{
    public class FuzzyVariable
    {

        private Dictionary<string, FuzzySet> memberSets;
        private double minRange;
        private double maxRange;


        public FuzzyVariable()
        {
            memberSets = new Dictionary<string, FuzzySet>();
            
        }

        /// <summary>
        /// Calculate DOM for each set in the variable from a crisp value.
        /// </summary>
        /// <param name="val"></param>
        public void Fuzzify(double val)
        {
            if (val < minRange || val > maxRange) {
                throw new ArgumentOutOfRangeException();
            }
            foreach(FuzzySet set in memberSets.Values)
            {
                set.DOM = set.CalculateDOM(val);
            }
            
        }

        /// <summary>
        /// defuzzifies the value by avaraging the maxima of the sets that have fired.
        /// </summary>
        /// <returns></returns>
        public double DefuzzifyMaxAv()
        {
            double bottom = 0.0;
            double top = 0.0;
            foreach (FuzzySet set in memberSets.Values)
            {
                bottom += set.DOM;
                top += set.GetRepresentativeValue * set.DOM;
            }

            //prevent divide by 0
            if (bottom == 0)
            {
                return 0.0;
            }
            return top / bottom;

        }

        public double DefuzzifyCentroid(int numSamples)
        {
            double stepSize = (maxRange - minRange) / numSamples;
            double totalArea = 0.0;
            double sumOfMoments = 0.0;

            for(int samp = 1; samp <= numSamples; samp++)
            {
                foreach(FuzzySet curSet in memberSets.Values)
                {
                    double contribution = Math.Min(curSet.CalculateDOM(minRange+samp * stepSize), curSet.DOM);
                    totalArea += contribution;
                    sumOfMoments += (minRange + samp * stepSize) * contribution;
                }
                
            }

            //prevent divide by 0
            if (0 == totalArea)
                return 0.0;
            return sumOfMoments / totalArea;
        }

        public FzSet AddTriangularSet(string name, double minBound, double peak, double maxBound)
        {
            memberSets[name] = new FuzzySet_Triangle(peak, peak - minBound, maxBound - peak);
            AdjustRangeToFit(minBound, maxBound);
            return new FzSet(memberSets[name]);
        }

        public FzSet AddLeftShoulderSet(string name, double minBound, double peak, double maxBound)
        {
            memberSets[name] = new FuzzySet_LeftShoulder(peak, peak - minBound, maxBound - peak);
            AdjustRangeToFit(minBound, maxBound);
            return new FzSet(memberSets[name]);
        }

        public FzSet AddRightShoulderSet(string name, double minBound, double peak, double maxBound)
        {
            memberSets[name] = new FuzzySet_RightShoulder(peak, peak - minBound, maxBound - peak);
            AdjustRangeToFit(minBound, maxBound);
            return new FzSet(memberSets[name]);
        }

        public FzSet AddSingletonSet(string name, double minBound, double peak, double maxBound)
        {
            memberSets[name] = new FuzzySet_Singleton(peak, peak - minBound, maxBound - peak);
            AdjustRangeToFit(minBound, maxBound);
            return new FzSet(memberSets[name]);
        }


        private void AdjustRangeToFit(double minBound, double maxBound)
        {
            if (minBound < minRange)
                minRange = minBound;
            if (maxBound > maxRange)
                maxRange = maxBound;
        }

        

    }
}
