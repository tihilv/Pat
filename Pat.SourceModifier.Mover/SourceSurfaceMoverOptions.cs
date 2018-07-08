using System.ComponentModel;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.SourceModifier.Mover
{
    public class SourceSurfaceMoverOptions : IOptions
    {
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue X { get; set; }

        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue Y { get; set; }

        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue Z { get; set; }
    }
}