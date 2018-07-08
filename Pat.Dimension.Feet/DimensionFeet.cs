using System;
using Pat.Api.Modules;

namespace Pat.Dimension.Feet
{
    public class DimensionFeet: IDimensionModule
    {
        internal const double FeetInM = 3.28084;
        
        public string Name => "Feet";
        public string Description => "Feet dimension converter.";
        public string Identifier => "ft";
        public DimensionType Type => DimensionType.Linear;

        public double ToSI(double value)
        {
            return value / FeetInM;
        }

        public double FromSI(double value)
        {
            return value * FeetInM;
        }
    }

    public class DimensionCubicFeet: IDimensionModule
    {
        private readonly double FeetInM = Math.Pow(DimensionFeet.FeetInM, 3);

        public string Name => "Cubic Feet";
        public string Description => "Feet dimension converter.";
        public string Identifier => "ft3";
        public DimensionType Type => DimensionType.Cubic;

        public double ToSI(double value)
        {
            return value / FeetInM;
        }

        public double FromSI(double value)
        {
            return value * FeetInM;
        }
    }
}