namespace Pat.Api.Model
{
    public class TriangulatedSurface
    {
        public readonly Triangle3D[] Triangles;

        public TriangulatedSurface(Triangle3D[] triangles)
        {
            Triangles = triangles;
        }
    }
}