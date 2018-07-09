using System;
using System.ComponentModel;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.TriangulationModifier.Cut
{
    [Serializable]
    public class TriangulatedSurfaceCutterOptions : IOptions
    {
        [DisplayName("Depth of the Cut")]
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue CutDepth { get; set; }
    }
}