namespace Pat.Api.Model
{
    public class SourceSurface
    {
        public readonly Point3D[] Points;

        public SourceSurface(Point3D[] points)
        {
            Points = points;
        }
    }
}
