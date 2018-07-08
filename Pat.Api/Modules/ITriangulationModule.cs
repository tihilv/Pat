using Pat.Api.Model;

namespace Pat.Api.Modules
{
    public interface ITriangulationModule: IModule, IHavingDefaultOptions
    {
        TriangulatedSurface GetTriangulatedSurface(SourceSurface sourceSurface, IOptions options);
    }
}