using Pat.Api.Modules;

namespace Pat.Dimensions.Meters
{
    public class DimensionMeters : IDimensionModule
    {
        public string Name => "Meters";
        public string Description => "Meters dimension converter.";
        public string Identifier => "m";
        public DimensionType Type => DimensionType.Linear;

        public double ToSI(double value)
        {
            return value;
        }

        public double FromSI(double value)
        {
            return value;
        }
    }

    public class DimensionCubicMeters : IDimensionModule
    {
        public string Name => "Cubic Meters";
        public string Description => "Cubic Meters dimension converter.";
        public string Identifier => "m3";
        public DimensionType Type => DimensionType.Cubic;

        public double ToSI(double value)
        {
            return value;
        }

        public double FromSI(double value)
        {
            return value;
        }
    }

}