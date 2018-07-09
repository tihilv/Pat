using System;
using System.Linq;
using Pat.Api;
using Pat.Api.Model;
using Pat.Api.Services;

namespace Pat.BusinessLogic.Services
{
    public class VolumeService : IVolumeService
    {
        public double GetVolumeUnderSurface(TriangulatedSurface surface, double lowerLine)
        {
            double result = 0;

            foreach (var triangle in surface.Triangles)
            {
                result += GetVolumeUnderTriangle(triangle, lowerLine);
            }

            return result;
        }

        private double GetVolumeUnderTriangle(Triangle3D triangle, double lowerLine)
        {
            double lowerZ = triangle.Points.Max(p => p.Z);

            var volumeA = triangle.GetTriangleNoZ().GetSquare() * (lowerLine - lowerZ);

            Point3D a;
            Point3D b;
            Point3D c;

            if (Equality.AreEqual(triangle.A.Z, lowerZ))
            {
                a = triangle.A;
                b = triangle.B;
                c = triangle.C;
            } else if (Equality.AreEqual(triangle.B.Z, lowerZ))
            {
                a = triangle.B;
                b = triangle.A;
                c = triangle.C;
            }
            else
            {
                a = triangle.C;
                b = triangle.B;
                c = triangle.A;
            }
            
            Point3D d = new Point3D(b.X, b.Y, lowerZ);
            Point3D e = new Point3D(c.X, c.Y, lowerZ);

            var volumeB = GetTetrahedronVolume(a, b, c, e);
            var volumeC = GetTetrahedronVolume(a, b, e, d);
            
            return volumeA + volumeB + volumeC;
        }

        private double GetTetrahedronVolume(Point3D a, Point3D b, Point3D c, Point3D d)
        {
            var vectorA = a.Subtract(b);
            var vectorB = a.Subtract(c);
            var vectorC = a.Subtract(d);

            return Math.Abs((vectorA.X * vectorB.Y * vectorC.Z + vectorA.Y * vectorB.Z * vectorC.X + vectorA.Z * vectorB.X * vectorC.Y - vectorA.Z * vectorB.Y * vectorC.X - vectorA.X * vectorB.Z * vectorC.Y - vectorA.Y * vectorB.X * vectorC.Z) / 6);
        }
    }
}