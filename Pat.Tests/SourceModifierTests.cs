using NUnit.Framework;
using Pat.Api.Model;
using Pat.SourceModifier.Mover;

namespace Pat.Tests
{
    [TestFixture]
    public class SourceModifierTests: TestsBase
    {
        [Test]
        public void TestSurfaceMover()
        {
            var points = new Point3D[]
            {
                new Point3D(0, 0, 1), new Point3D(100, 0, 2), new Point3D(200, 0, 3), new Point3D(0, 100, 4), new Point3D(100, 100, 5), new Point3D(200, 100, 6)
            };
            
            SourceSurface surface = new SourceSurface(points);
            
            SourceSurfaceMover mover = new SourceSurfaceMover();
            SourceSurfaceMoverOptions options = (SourceSurfaceMoverOptions) mover.GetDefaultOptions();
            options.X = "10 m";
            options.Y = "8 m";
            options.Z = "3 m";

            var newSurface = mover.GetModifiedSurface(surface, options);
            for (var index = 0; index < points.Length; index++)
            {
                var point = points[index];
                Assert.True(new Point3D(point.X + 10, point.Y + 8, point.Z + 3).AreEqual(newSurface.Points[index]));
            }
        }
    }
}