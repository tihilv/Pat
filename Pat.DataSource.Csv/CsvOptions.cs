using System;
using System.ComponentModel;
using System.Windows.Controls.WpfPropertyGrid;
using Pat.Api.Model;
using Pat.Api.Modules;
using Pat.Api.Ui;

namespace Pat.DataSource.Csv
{
    [Serializable]
    public class CsvOptions : IOptions
    {
        [DisplayName("File Name")]
        [PropertyEditor(typeof(FilePathPicker))]
        public string FileName { get; set; }

        [DisplayName("Grip Step")]
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue GridStep { get; set; }
        
        [DisplayName("CSV Separator")]
        public string Separator { get; set; }
        
        [DisplayName("Height Unit")]
        [TypeConverter(typeof(DimensionedValueConverter))]
        public DimensionedValue HeightMultiplier { get; set; }
    }
}