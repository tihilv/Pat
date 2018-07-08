using System;
using System.Collections.Generic;
using System.Linq;
using MIConvexHull;
using Pat.Api;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.Triangulation.Default
{
    public class DefaultTriangulation: ITriangulationModule
    {
        public string Name => "Default triangulation";
        public string Description => "Triangulation module that use MIConvexHull library.";
        public IOptions GetDefaultOptions()
        {
            return null;
        }

        public TriangulatedSurface GetTriangulatedSurface(SourceSurface sourceSurface, IOptions options)
        {
            var vertices = sourceSurface.Points.Select(p => new Vertex(p)).ToList();

            var triangulation = MIConvexHull.Triangulation.CreateDelaunay(vertices, Equality.Epsilon);

            List<Triangle3D> triangles = new List<Triangle3D>();
            
            foreach (var cell in triangulation.Cells)
            {
                if (cell.Vertices.Length != 3)
                    throw new Exception($"Cell vertices number is not 3. Triangulation failed.");

                triangles.Add(new Triangle3D(cell.Vertices[0].OriginalPoint, cell.Vertices[1].OriginalPoint, cell.Vertices[2].OriginalPoint));
            }

            return new TriangulatedSurface(triangles.ToArray());
        }

        class Vertex : IVertex
        {
            private readonly double[] _2dData;
            private readonly Point3D _originalPoint;

            internal Vertex(Point3D point)
            {
                _2dData = new double[] {point.X, point.Y};
                _originalPoint = point;
            }

            public double[] Position => _2dData;
            public Point3D OriginalPoint => _originalPoint;
        }
    }
}