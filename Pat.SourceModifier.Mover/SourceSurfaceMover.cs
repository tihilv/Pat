using System.Linq;
using Pat.Api.Model;
using Pat.Api.Modules;

namespace Pat.SourceModifier.Mover
{
    public class SourceSurfaceMover: ISourceModifierModule
    {
        public string Name => "Surface mover";
        public string Description => "Module to move surface according to a given vector.";
        
        public IOptions GetDefaultOptions()
        {
            return new SourceSurfaceMoverOptions() {X = "0 m", Y = "0 m", Z = "100 m"};
        }

        public SourceSurface GetModifiedSurface(SourceSurface surface, IOptions options)
        {
            SourceSurfaceMoverOptions smOptions = (SourceSurfaceMoverOptions) options;
            
            var moveVector = new Point3D(smOptions.X, smOptions.Y, smOptions.Z);

            return new SourceSurface(surface.Points.Select(p => p + moveVector).ToArray());
        }
    }
}