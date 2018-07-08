using System.IO;
using NUnit.Framework;
using Pat.Api.Model;
using Pat.Api.Modules;
using Pat.BusinessLogic.Services;
using Pat.Dimension.Feet;
using Pat.Dimensions.Meters;

namespace Pat.Tests
{
    public class TestsBase
    {
        public static readonly string TestDir = Path.Combine(Path.GetDirectoryName(typeof(CsvTests).Assembly.Location), "..", "..", "Source");

        [OneTimeSetUp]
        public void Init()
        {
            DimensionedValue.DimensionService = new DimensionService(new IDimensionModule[] {new DimensionFeet(), new DimensionMeters()});
        }
    }
}