using Pat.Api.Model;

namespace Pat.Api.Modules
{
    public interface ITriangulationModifierModule: IModule, IHavingDefaultOptions
    {
        TriangulatedSurface GetModifiedSurface(TriangulatedSurface surface, IOptions options);
    }
}