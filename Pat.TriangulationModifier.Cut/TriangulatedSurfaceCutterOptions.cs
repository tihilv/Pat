using System.ComponentModel;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.TriangulationModifier.Cut
{
    public class TriangulatedSurfaceCutterOptions : IOptions
    {
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue CutDepth { get; set; }
    }
}