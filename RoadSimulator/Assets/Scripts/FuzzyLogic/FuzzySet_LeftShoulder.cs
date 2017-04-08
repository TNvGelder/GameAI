using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.FuzzyLogic
{
    public class FuzzySet_LeftShoulder : FuzzySet
    {
        private double peakPoint;
        private double leftOffset;
        private double rightOffset;

        public FuzzySet_LeftShoulder(double peak, double left, double right):base(((peak - left)+peak)/2)
        {
            peakPoint = peak;
            leftOffset = left;
            rightOffset = right;
        }


        public override double CalculateDOM(double val)
        {
            //prevents divide by zero errors
            if (rightOffset == 0 && val == peakPoint)
            {
                return 1.0;
            }

            //left of center
            else if (val >= peakPoint && val < (peakPoint + rightOffset))
            {
                double grad = 1.0 / -rightOffset;
                return grad * (val - peakPoint) + 1.0;
            }
            //right of center
            else if(val < peakPoint && val >= (peakPoint - leftOffset))
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
