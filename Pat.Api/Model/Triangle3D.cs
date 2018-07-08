using System;
using System.Linq;

namespace Pat.Api.Model
{
    public struct Triangle3D
    {
        private readonly Point3D[] _points;
        
        public Triangle3D(Point3D a, Point3D b, Point3D c)
        {
            _points = new Point3D[] {a, b, c};
        }

        public Point3D A => _points[0];
        public Point3D B => _points[1];
        public Point3D C => _points[2];

        public Point3D[] Points => _points;

        public bool AreEqual(Triangle3D other)
        {
            foreach (var otherPoint in other._points)
                if (!_points.Any(p => p.AreEqual(otherPoint)))
                    return false;
            
            return true;
        }

        public Triangle3D GetTriangleNoZ()
        {
            return new Triangle3D(new Point3D(A.X, A.Y, 0), new Point3D(B.X, B.Y, 0), new Point3D(C.X, C.Y, 0));
        }

        public double GetPerimeter()
        {
            double ab = A.DistanceTo(B);
            double bc = B.DistanceTo(C);
            double ac = A.DistanceTo(C);

            return ab + bc + ac;
        }
        
        public double GetSquare()
        {
            double ab = A.DistanceTo(B);
            double bc = B.DistanceTo(C);
            double ac = A.DistanceTo(C);

            var p = (ab + bc + ac) / 2;
            return Math.Sqrt(p * (p - ab) * (p - bc) * (p - ac));
        }
    }
}