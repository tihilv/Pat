using System.Linq;
using NUnit.Framework;
using Pat.Api.Model;
using Pat.TriangulationModifier.Cut;
using Pat.TriangulationModifier.Mover;

namespace Pat.Tests
{
    [TestFixture]
    public class TriangulationModifierTests: TestsBase
    {
        [Test]
        public void TestSurfaceMover()
        {
            var a = new Point3D(2, 0, 2);
            var b = new Point3D(12, 0, 3);
            var c = new Point3D(10, 10, 1);
            var d = new Point3D(2, 20, 4);
            var e = new Point3D(12, 20, 5);
            
            Triangle3D[] triangles = new [] {new Triangle3D(a,b,c), new Triangle3D(d,e,c), new Triangle3D(a,d,c), new Triangle3D(b,e,c)};
            TriangulatedSurface surface = new TriangulatedSurface(triangles);
            
            TriangulatedSurfaceMover mover = new TriangulatedSurfaceMover();
            TriangulatedSurfaceMoverOptions options = (TriangulatedSurfaceMoverOptions) mover.GetDefaultOptions();
            
            Point3D vectorToMove = new Point3D(10, 8, 3);
            options.X = $"{vectorToMove.X} m";
            options.Y = $"{vectorToMove.Y} m";
            options.Z = $"{vectorToMove.Z} m";

            var newSurface = mover.GetModifiedSurface(surface, options);
            for (var index = 0; index < triangles.Length; index++)
            {
                var triangle = triangles[index];
                Assert.True(new Triangle3D(triangle.A + vectorToMove, triangle.B + vectorToMove, triangle.C + vectorToMove).AreEqual(newSurface.Triangles[index]));
            }
        }

        [Test]
        public void TestCutter()
        {
            var a = new Point3D(1, 21, 2);
            var b = new Point3D(11, 11, 12);
            var c = new Point3D(11, 21, 2);

            var d = new Point3D(15, 17, 13);
            var e = new Point3D(3, 7, 11);
            
            var f = new Point3D(-5, 3, 3);
            var g = new Point3D(4, 4, 4);
            
            var i1 = new Point3D(6, 16, 7);
            var i2 = new Point3D(11, 16, 7);
            
            Triangle3D[] triangles = new [] {new Triangle3D(a,b,c), new Triangle3D(b, d, e), new Triangle3D(a, f, g), };
            TriangulatedSurface surface = new TriangulatedSurface(triangles);

            Triangle3D[] resultTriangles = new [] {new Triangle3D(a,i1,i2), new Triangle3D(a, c, i2), new Triangle3D(a, f, g), };
            
            TriangulatedSurfaceCutter cutter = new TriangulatedSurfaceCutter();
            TriangulatedSurfaceCutterOptions options = (TriangulatedSurfaceCutterOptions) cutter.GetDefaultOptions();
            options.CutDepth = "7 m";

            var newSurface = cutter.GetModifiedSurface(surface, options);

            Assert.AreEqual(resultTriangles.Length, newSurface.Triangles.Length);

            foreach (var resultTriangle in resultTriangles)
            {
                Assert.True(newSurface.Triangles.Any(t=>t.AreEqual(resultTriangle)));
            }
        }
    }
}