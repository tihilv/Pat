using System.IO;
using System.Linq;
using NUnit.Framework;
using Pat.Api.Model;
using Pat.DataSource.Csv;
using Pat.Triangulation.Default;

namespace Pat.Tests
{
    [TestFixture]
    public class TriangulationTests: TestsBase
    {
        [Test]
        public void TestSimpleTriangulation()
        {
            var a = new Point3D(2, 0, 2);
            var b = new Point3D(12, 0, 3);
            var c = new Point3D(10, 10, 1);
            var d = new Point3D(2, 20, 4);
            var e = new Point3D(12, 20, 5);
            
            var points = new [] { a, b, c, d, e };
            
            SourceSurface surface = new SourceSurface(points);
            
            DefaultTriangulation triangulation = new DefaultTriangulation();

            var result = triangulation.GetTriangulatedSurface(surface, triangulation.GetDefaultOptions());
            
            Triangle3D[] triangles = new [] {new Triangle3D(a,b,c), new Triangle3D(d,e,c), new Triangle3D(a,d,c), new Triangle3D(b,e,c)};
            
            Assert.AreEqual(triangles.Length, result.Triangles.Length);

            foreach (var triangle in triangles)
                Assert.True(result.Triangles.Any(t => t.AreEqual(triangle)));
        }
        
        [Test]
        public void TestFailedTriangulation()
        {
            var a = new Point3D(2, 0, 2);
            var b = new Point3D(3, 1, 3);
            var c = new Point3D(4, 2, 1);
            var d = new Point3D(5, 3, 4);
            var e = new Point3D(6, 4, 5);
            
            var points = new [] { a, b, c, d, e };
            
            SourceSurface surface = new SourceSurface(points);
            
            DefaultTriangulation triangulation = new DefaultTriangulation();

            var result = triangulation.GetTriangulatedSurface(surface, triangulation.GetDefaultOptions());
            
            Assert.AreEqual(0, result.Triangles.Length);
        }

        [Test]
        public void TestRealTriangulation()
        {
            CsvDataSource csvDataSource = new CsvDataSource();
            CsvOptions options = (CsvOptions) csvDataSource.GetDefaultOptions();
            options.FileName = Path.Combine(TestDir, "depthvalues.csv");
            options.GridStep = "200 ft";
            options.Separator = " ";

            SourceSurface surface = csvDataSource.GetSurface(options);

            DefaultTriangulation triangulation = new DefaultTriangulation();

            var result = triangulation.GetTriangulatedSurface(surface, triangulation.GetDefaultOptions());
            
            Assert.AreEqual(15*25*2, result.Triangles.Length); // 16 x 26 points transformed to 15 x 25 rectangles, each is splitted into 2 triangles.
        }
    }
}