using Pat.Api.Modules;

namespace Pat.Dimension.Barrels
{
    public class DimensionBarrels: IDimensionModule
    {
        internal const double M3InBbl = 0.159;
        
        public string Name => "Barrels";
        public string Description => "Barrels dimension converter.";
        public string Identifier => "bbl";
        public DimensionType Type => DimensionType.Cubic;

        public double ToSI(double value)
        {
            return value * M3InBbl;
        }

        public double FromSI(double value)
        {
            return value / M3InBbl;
        }
    }
}
