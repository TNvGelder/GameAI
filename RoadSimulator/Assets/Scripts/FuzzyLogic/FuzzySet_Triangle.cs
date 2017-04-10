using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic
{
    public class FuzzySet_Triangle : FuzzySet
    {
        private double peakPoint;
        private double leftOffset;
        private double rightOffset;

        public FuzzySet_Triangle(double mid, double left, double right):base(mid)
        {
            peakPoint = mid;
            leftOffset = left;
            rightOffset = right;
        }


        public override double CalculateDOM(double val)
        {
            //prevents divide by zero errors
            if ((rightOffset == 0.0 && peakPoint == val) || (leftOffset == 0.0 && peakPoint == val))
            {
                return 1.0;
            }

            
            if (val <= peakPoint && val >= (peakPoint - leftOffset))
            {
                double grad = 1.0 / leftOffset;
                return grad * (val - (peakPoint - leftOffset)); 
            }else if (val > peakPoint && val < (peakPoint + rightOffset))
            {
                double grad = 1.0 / -rightOffset;
                return grad * (val - peakPoint) + 1.0; 
            }
            //out of range of FLV
            else
            {
                return 0.0;
            }
        }
    }
}
