using System.Collections.Generic;
using System.Linq;
using Pat.Api;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.TriangulationModifier.Cut
{
    public class TriangulatedSurfaceCutter: ITriangulationModifierModule
    {
        public string Name => "Triangulated surface cutter";
        public string Description => "Cuts triangulated surface according given depth.";
        
        public IOptions GetDefaultOptions()
        {
            return new TriangulatedSurfaceCutterOptions() {CutDepth = "3000 m"};
        }

        public TriangulatedSurface GetModifiedSurface(TriangulatedSurface surface, IOptions options)
        {
            var cutOptions = (TriangulatedSurfaceCutterOptions) options;
            
            List<Triangle3D> triangles = new List<Triangle3D>();

            foreach (var triangle in surface.Triangles)
            {
                if (Equality.AreLessOrEqual(triangle.Points.Max(p => p.Z), cutOptions.CutDepth))
                    triangles.Add(triangle);

                else if (Equality.AreMoreOrEqual(triangle.Points.Min(p => p.Z), cutOptions.CutDepth))
                    continue;
                else
                {
                    Point3D[] pointsToInclude = triangle.Points.Where(p => Equality.Less(p.Z, cutOptions.CutDepth)).ToArray();

                    List<Point3D> intersectionList = new List<Point3D>();

                    var intersect = Intersection(triangle.A, triangle.B, cutOptions.CutDepth);
                    if (intersect != null)
                        intersectionList.Add(intersect.Value);

                    intersect = Intersection(triangle.B, triangle.C, cutOptions.CutDepth);
                    if (intersect != null)
                        intersectionList.Add(intersect.Value);

                    intersect = Intersection(triangle.A, triangle.C, cutOptions.CutDepth);
                    if (intersect != null)
                        intersectionList.Add(intersect.Value);

                    triangles.Add(new Triangle3D(pointsToInclude[0], intersectionList[0], intersectionList[1]));

                    if (pointsToInclude.Length == 2)
                    {
                        var secondPoint = pointsToInclude[1];
                        if (secondPoint.DistanceTo(intersectionList[0]) < secondPoint.DistanceTo(intersectionList[1]))
                            triangles.Add(new Triangle3D(pointsToInclude[0], secondPoint, intersectionList[0]));
                        else
                            triangles.Add(new Triangle3D(pointsToInclude[0], secondPoint, intersectionList[1]));
                    }
                }
            }

            return new TriangulatedSurface(triangles.ToArray());
        }

        private Point3D? Intersection(Point3D a, Point3D b, double z)
        {
            if ((Equality.AreLessOrEqual(a.Z, z) && Equality.AreLessOrEqual(b.Z, z)) ||
                (Equality.AreMoreOrEqual(a.Z, z) && Equality.AreMoreOrEqual(b.Z, z)))
                return null;
            
            if (Equality.AreEqual(a.X, b.X) && Equality.AreEqual(a.Y, b.Y))
                return new Point3D(a.X, a.Y, z);

            var x = (z - a.Z) * (b.X - a.X) / (b.Z - a.Z) + a.X;
            var y = (z - a.Z) * (b.Y - a.Y) / (b.Z - a.Z) + a.Y;
            
            if (Equality.AreEqual(a.X, b.X))
                return new Point3D(a.X, y, z);
            
            if (Equality.AreEqual(a.Y, b.Y))
                return new Point3D(x, a.Y, z);

            return new Point3D(x, y, z);
        }
    }
}