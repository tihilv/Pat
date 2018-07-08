using System.ComponentModel;
using System.Windows.Controls.WpfPropertyGrid;
using Pat.Api.Editors;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.DataSource.Csv
{
    public class CsvOptions : IOptions
    {
        [PropertyEditor(typeof(FilePathPicker))]
        public string FileName { get; set; }

        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue GridStep { get; set; }
        
        public string Separator { get; set; }
        
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue HeightMultiplier { get; set; }
    }
}