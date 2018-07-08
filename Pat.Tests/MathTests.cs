using System.IO;
using NUnit.Framework;
using Pat.Api;
using Pat.Api.Model;
using Pat.BusinessLogic.Services;
using Pat.DataSource.Csv;
using Pat.SourceModifier.Mover;
using Pat.Triangulation.Default;

namespace Pat.Tests
{
    [TestFixture]
    public class MathTests: TestsBase
    {
        [Test]
        public void TestTriangleSquare()
        {
            Triangle3D triangle = new Triangle3D(new Point3D(10, 5, 17), new Point3D(20, 5, 17), new Point3D(15, 10, 17));
            
            Assert.True(Equality.AreEqual(25, triangle.GetSquare()));
        }

        [Test]
        public void TestVolumes()
        {
            VolumeService service = new VolumeService();
            
            TriangulatedSurface surface = new TriangulatedSurface(new []{new Triangle3D(new Point3D(0, 0, 10), new Point3D(10, 0, 10), new Point3D(0, 10, 10))});

            var result = service.GetVolumeUnderSurface(surface, 10);
            Assert.True(Equality.AreEqual(0, result));
            
            result = service.GetVolumeUnderSurface(surface, 20);
            Assert.True(Equality.AreEqual(500, result));
            
            surface = new TriangulatedSurface(new []{new Triangle3D(new Point3D(0, 0, 10), new Point3D(10, 0, 5), new Point3D(0, 10, 10))});

            result = service.GetVolumeUnderSurface(surface, 20);
            Assert.True(Equality.AreEqual(500+250.0/3.0, result));
        }
        
        [Test]
        public void TestRealVolumeDiff()
        {
            CsvDataSource csvDataSource = new CsvDataSource();
            CsvOptions csvOptions = (CsvOptions) csvDataSource.GetDefaultOptions();
            csvOptions.FileName = Path.Combine(TestDir, "depthvalues.csv");
            csvOptions.GridStep = "200 m";
            csvOptions.Separator = " ";

            DefaultTriangulation triangulation = new DefaultTriangulation();
            
            SourceSurface surfaceTop = csvDataSource.GetSurface(csvOptions);
            var triangulatedSurfaceTop = triangulation.GetTriangulatedSurface(surfaceTop, triangulation.GetDefaultOptions());

            SourceSurfaceMover mover = new SourceSurfaceMover();
            var moverOptions = (SourceSurfaceMoverOptions)mover.GetDefaultOptions();
            moverOptions.Z = "100 m";
            SourceSurface surfaceBottom = mover.GetModifiedSurface(surfaceTop, moverOptions);
            var triangulatedSurfaceBottom = triangulation.GetTriangulatedSurface(surfaceBottom, triangulation.GetDefaultOptions());

            VolumeService service = new VolumeService();

            var topVolume = service.GetVolumeUnderSurface(triangulatedSurfaceTop, 10000);
            var bottomVolume = service.GetVolumeUnderSurface(triangulatedSurfaceBottom, 10000);

            var diff = (int)(topVolume - bottomVolume);
            Assert.AreEqual(200 * 200 * 100 * 15 * 25, diff);
        }
    }
}