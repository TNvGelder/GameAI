using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic
{
    public class FuzzySet_RightShoulder : FuzzySet
    {
        private double peakPoint;
        private double leftOffset;
        private double rightOffset;

        public FuzzySet_RightShoulder(double peak, double left, double right):base(((peak + right)+peak)/2)
        {
            peakPoint = peak;
            leftOffset = left;
            rightOffset = right;
        }


        public override double CalculateDOM(double val)
        {
            //prevents divide by zero errors
            if (leftOffset == 0 && val == peakPoint)
            {
                return 1.0;
            }

            //left of center
            if (val <= peakPoint && val > (peakPoint - leftOffset))
            {
                double grad = 1.0 / leftOffset;
                return grad * (val - (peakPoint - leftOffset));
            }
            //right of center
            else if(val > peakPoint && val <= peakPoint + rightOffset)
            {
                return 1.0;
            }
            
            //out of range of FLV
            else
            {
                return 0.0;
            }
        }
    }
}
