using System;
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

                    List<IntersectionData> intersectionList = new List<IntersectionData>();

                    var intersect = Intersection(triangle.A, triangle.B, cutOptions.CutDepth);
                    if (intersect != null)
                        intersectionList.Add(intersect);

                    intersect = Intersection(triangle.B, triangle.C, cutOptions.CutDepth);
                    if (intersect != null)
                        intersectionList.Add(intersect);

                    intersect = Intersection(triangle.A, triangle.C, cutOptions.CutDepth);
                    if (intersect != null)
                        intersectionList.Add(intersect);

                    var firstPoint = pointsToInclude[0];
                    triangles.Add(new Triangle3D(firstPoint, intersectionList[0].Intersection, intersectionList[1].Intersection));

                    if (pointsToInclude.Length == 2)
                    {
                        var secondPoint = pointsToInclude[1];
                        if (intersectionList[0].HasPoint(secondPoint))
                            triangles.Add(new Triangle3D(firstPoint, secondPoint, intersectionList[0].Intersection));
                        else if (intersectionList[1].HasPoint(secondPoint))
                            triangles.Add(new Triangle3D(firstPoint, secondPoint, intersectionList[1].Intersection));
                        else
                        {
                            throw new Exception("AAAA");
                        }
                    }
                }
            }

            return new TriangulatedSurface(triangles.ToArray());
        }

        private IntersectionData Intersection(Point3D a, Point3D b, double z)
        {
            if ((Equality.AreLessOrEqual(a.Z, z) && Equality.AreLessOrEqual(b.Z, z)) ||
                (Equality.AreMoreOrEqual(a.Z, z) && Equality.AreMoreOrEqual(b.Z, z)))
                return null;

            if (Equality.AreEqual(a.X, b.X) && Equality.AreEqual(a.Y, b.Y))
                return new IntersectionData(new Point3D(a.X, a.Y, z), a, b);

            var x = (z - a.Z) * (b.X - a.X) / (b.Z - a.Z) + a.X;
            var y = (z - a.Z) * (b.Y - a.Y) / (b.Z - a.Z) + a.Y;

            if (Equality.AreEqual(a.X, b.X))
                return new IntersectionData(new Point3D(a.X, y, z), a, b);

            if (Equality.AreEqual(a.Y, b.Y))
                return new IntersectionData(new Point3D(x, a.Y, z), a, b);

            return new IntersectionData(new Point3D(x, y, z), a, b);
        }

        class IntersectionData
        {
            public readonly Point3D Intersection;
            private readonly Point3D _pointA;
            private readonly Point3D _pointB;

            public IntersectionData(Point3D intersection, Point3D pointA, Point3D pointB)
            {
                Intersection = intersection;
                _pointA = pointA;
                _pointB = pointB;
            }

            public bool HasPoint(Point3D point)
            {
                return _pointA.AreEqual(point) || _pointB.AreEqual(point);
            }
        }
    }
}