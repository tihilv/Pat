using System;
using NUnit.Framework;
using Pat.Api;
using Pat.Api.Model;
using Pat.Api.Modules;
using Pat.Dimension.Feet;

namespace Pat.Tests
{
    [TestFixture]
    public class DimensionTests: TestsBase
    {
        [Test]
        public void TestFeetConverter()
        {
            IDimensionModule dimensionFeet = new DimensionFeet();
            
            Assert.True(Equality.AreEqual(26.24672, dimensionFeet.FromSI(8)));
            Assert.True(Equality.AreEqual(9.1439997, dimensionFeet.ToSI(30)));
        }

        [Test]
        public void TestDimensionValues()
        {
            DimensionedValue valueInFt = "200 ft";
            Assert.AreEqual("200 ft", valueInFt.ToString());
            DimensionedValue valueInM = valueInFt.ConverTo("m");
            
            Assert.AreEqual($"{200 / 3.28084} m", valueInM.ToString());
        }

        [Test]
        public void TestFailedDimension()
        {
            try
            {
                DimensionedValue valueInFt = "200 qwerty";
                Assert.True(false);
            }
            catch (ArgumentException)
            {
                Assert.True(true);
            }
            catch (Exception)
            {
                Assert.True(false);
            }
        }
        
    }
}