using System.Linq;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.TriangulationModifier.Mover
{
    public class TriangulatedSurfaceMover: ITriangulationModifierModule
    {
        public string Name => "Triangulated surface mover";
        public string Description => "Module to move surface according to a given vector.";
        
        public IOptions GetDefaultOptions()
        {
            return new TriangulatedSurfaceMoverOptions() {X = "0 m", Y = "0 m", Z = "100 m"};
        }

        public TriangulatedSurface GetModifiedSurface(TriangulatedSurface surface, IOptions options)
        {
            TriangulatedSurfaceMoverOptions smOptions = (TriangulatedSurfaceMoverOptions) options;
            
            var moveVector = new Point3D(smOptions.X, smOptions.Y, smOptions.Z);
            
            return new TriangulatedSurface(
                surface.Triangles.Select(p =>
                    new Triangle3D(
                        p.A + moveVector,
                        p.B + moveVector,
                        p.C + moveVector
                    )).ToArray()
            );
        }
    }
}