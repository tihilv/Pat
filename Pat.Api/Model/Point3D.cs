using System;

namespace Pat.Api.Model
{
    public struct Point3D
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool AreEqual(Point3D other)
        {
            return Equality.AreEqual(X, other.X) && Equality.AreEqual(Y, other.Y) && Equality.AreEqual(Z, other.Z);
        }

        public double DistanceTo(Point3D other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));
        }

        public Point3D Subtract(Point3D other)
        {
            return new Point3D(X - other.X, Y - other.Y, Z - other.Z);
        }
        
        public Point3D Add(Point3D other)
        {
            return new Point3D(X + other.X, Y + other.Y, Z + other.Z);
        }

        public static Point3D operator +(Point3D a, Point3D b)
        {
            return a.Add(b);
        }

        public static Point3D operator -(Point3D a, Point3D b)
        {
            return a.Subtract(b);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }
    }
}
