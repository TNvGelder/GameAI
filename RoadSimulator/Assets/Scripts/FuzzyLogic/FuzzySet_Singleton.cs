using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Assets.Scripts.FuzzyLogic
{
    public class FuzzySet_Singleton : FuzzySet
    {
        private double peakPoint;
        private double leftOffset;
        private double rightOffset;

        public FuzzySet_Singleton(double mid, double left, double right):base(mid)
        {
            peakPoint = mid;
            leftOffset = left;
            rightOffset = right;
        }


        public override double CalculateDOM(double val)
        {
            if ((val >= peakPoint - leftOffset) && (val <= peakPoint+ rightOffset))
            {
                return 1.0;
            }
            else//out of range of this FLV
            {
                return 0.0;
            }
        }
    }
}
