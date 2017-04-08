namespace Assets.Scripts.FuzzyLogic
{
    public abstract class FuzzySet
    {
        protected double degreeOfMembership;
        protected double representativeValue;

        public double DOM
        {
            get
            {
                return degreeOfMembership;
            }
            set
            {
                degreeOfMembership = value;
            }
        }

        public double GetRepresentativeValue
        {
            get
            {
                return representativeValue;
            }
        }

        /// <summary>
        /// Calculates the degree of membership
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public abstract double CalculateDOM(double val);

        public FuzzySet(double repVal)
        {
            degreeOfMembership = 0.0;
            representativeValue = repVal;
        }

        public void ClearDOM()
        {
            degreeOfMembership = 0.0;
        }

        /// <summary>
        /// Sets the DOM to the maximum of the current DOM and given value.
        /// </summary>
        /// <param name="val"></param>
        public void ORwithDOM(double val)
        {
            if(val> degreeOfMembership)
            {
                degreeOfMembership = val;
            }
        }


        


    }
}