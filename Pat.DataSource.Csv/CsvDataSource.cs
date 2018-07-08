using System;
using System.Collections.Generic;
using System.IO;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.DataSource.Csv
{
    public class CsvDataSource: IDataSourceModule
    {
        public string Name => "CSV Importer";
        public string Description => "Loads source surface from CSV file.";

        public IOptions GetDefaultOptions()
        {
            return new CsvOptions()
            {
                GridStep = "200 ft",
                Separator = " ",
                HeightMultiplier = "1 ft"
            };
        }

        public SourceSurface GetSurface(IOptions options)
        {
            var csvOptions = (CsvOptions) options;
            
            List<Point3D> points = new List<Point3D>();
            using (var reader = new StreamReader(csvOptions.FileName))
            {
                double y = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Split(new [] {csvOptions.Separator}, StringSplitOptions.RemoveEmptyEntries);

                    double x = 0;
                    foreach (var s in line)
                    {
                        var z = double.Parse(s) * csvOptions.HeightMultiplier;
                        points.Add(new Point3D(x, y, z));

                        x += csvOptions.GridStep;
                    }

                    y += csvOptions.GridStep;
                }
            }
            
            return new SourceSurface(points.ToArray());
        }
    }
}
