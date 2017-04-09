using Microsoft.VisualStudio.TestTools.UnitTesting;
using Assets.Scripts.FuzzyLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.FuzzyLogic.FuzzyTerms;

namespace Assets.Scripts.FuzzyLogic.Tests
{
    [TestClass()]
    public class FuzzyModuleTests
    {
        [TestMethod()]
        public void AddRuleTest()
        {
            Assert.Fail();
        }

        public double CalculateDesirability(FuzzyModule fm, double fuel, double bankMoney)
        {
            //fuzzify inputs
            fm.Fuzzify("FuelStatus", fuel);
            fm.Fuzzify("BankStatus", bankMoney);
            return fm.Defuzzify("RobbingDesirability", DefuzzifyMethod.centroid);
        }

        [TestMethod()]
        public void CreateFLVTest()
        {
            FuzzyModule fm = new FuzzyModule();
            FuzzyVariable FuelStatus = fm.CreateFLV("FuelStatus");
            FuzzyVariable BankStatus = fm.CreateFLV("BankStatus");
            FuzzyVariable RobDesirability = fm.CreateFLV("RobbingDesirability");
            FzSet FuelLow = FuelStatus.AddLeftShoulderSet("Fuel_Low", 0, 30, 50);
            FzSet FuelMedium = FuelStatus.AddTriangularSet("Fuel_Medium", 30,55, 80);
            FzSet FuelHigh = FuelStatus.AddRightShoulderSet("Fuel_High", 55, 80, 100);
            FzSet MoneyLow = BankStatus.AddLeftShoulderSet("Money_Low", 0, 100, 500);
            FzSet MoneyMedium = BankStatus.AddTriangularSet("Money_Medium", 200, 500, 800);
            FzSet MoneyHigh = BankStatus.AddRightShoulderSet("Money_High", 500, 800, 3000);
            FzSet Undesirable = RobDesirability.AddLeftShoulderSet("Undesirable", 0, 0, 100);
            FzSet Desirable = RobDesirability.AddRightShoulderSet("Desirable", 0, 100, 100);

            fm.AddRule(new FzAND(FuelLow, MoneyLow), new FzVery(Undesirable));
            fm.AddRule(new FzAND(FuelLow, MoneyMedium), new FzVery(Undesirable));
            fm.AddRule(new FzAND(FuelLow, MoneyHigh), Undesirable);
            fm.AddRule(new FzAND(FuelMedium, MoneyLow), new FzVery(Undesirable));
            fm.AddRule(new FzAND(FuelMedium, MoneyMedium), new FzFairly(Undesirable));
            fm.AddRule(new FzAND(FuelMedium, MoneyHigh), Desirable);
            fm.AddRule(new FzAND(FuelHigh, MoneyLow), Undesirable);
            fm.AddRule(new FzAND(FuelHigh, MoneyMedium), Desirable);
            fm.AddRule(new FzAND(FuelHigh, MoneyHigh), new FzVery(Desirable));
            double result = CalculateDesirability(fm, 30, 20);
            result = CalculateDesirability(fm, 50, 1000);
            result = CalculateDesirability(fm, 50, 400);
            result = CalculateDesirability(fm, 30, 20);
            Assert.Fail();
        }

        [TestMethod()]
        public void FuzzifyTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void DefuzzifyTest()
        {
            Assert.Fail();
        }
    }
}