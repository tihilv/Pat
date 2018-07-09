using System;
using System.ComponentModel;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.TriangulationModifier.Mover
{
    [Serializable]
    public class TriangulatedSurfaceMoverOptions : IOptions
    {
        [DisplayName("X Shift")]
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue X { get; set; }

        [DisplayName("Y Shift")]
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue Y { get; set; }

        [DisplayName("Z Shift")]
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue Z { get; set; }
    }
}