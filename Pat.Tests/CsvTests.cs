using System.IO;
using System.Linq;
using NUnit.Framework;
using Pat.Api.Model;
using Pat.DataSource.Csv;

namespace Pat.Tests
{
    [TestFixture]
    public class CsvTests: TestsBase
    {
        [Test]
        public void TestCsvImport()
        {
            CsvDataSource csvDataSource = new CsvDataSource();
            CsvOptions options = (CsvOptions) csvDataSource.GetDefaultOptions();
            options.FileName = Path.Combine(TestDir, "simpleSurface.csv");
            options.GridStep = "100 m";
            options.Separator = " ";

            SourceSurface result = csvDataSource.GetSurface(options);

            AssertSimpleSurface(result);
        }

        [Test]
        public void TestCsvImportEmptyLine()
        {
            CsvDataSource csvDataSource = new CsvDataSource();
            CsvOptions options = (CsvOptions) csvDataSource.GetDefaultOptions();
            options.FileName = Path.Combine(TestDir, "simpleSurface_emptyLine.csv");
            options.GridStep = "100 m";
            options.Separator = " ";

            SourceSurface result = csvDataSource.GetSurface(options);

            AssertSimpleSurface(result);
        }

        [Test]
        public void TestCsvImportSeparator()
        {
            CsvDataSource csvDataSource = new CsvDataSource();
            CsvOptions options = (CsvOptions) csvDataSource.GetDefaultOptions();
            options.FileName = Path.Combine(TestDir, "simpleSurface_separator.csv");
            options.GridStep = "100 m";
            options.Separator = ";";

            SourceSurface result = csvDataSource.GetSurface(options);

            AssertSimpleSurface(result);
        }

        
        void AssertSimpleSurface(SourceSurface sourceSurface)
        {
            var points = new []
            {
                new Point3D(0, 0, 1), new Point3D(100, 0, 2), new Point3D(200, 0, 3), new Point3D(0, 100, 4), new Point3D(100, 100, 5), new Point3D(200, 100, 6)
            };
            Assert.AreEqual(6, sourceSurface.Points.Length);

            foreach (var point in points)
                Assert.True(sourceSurface.Points.Any(p => p.AreEqual(point)));
        }
    }
}