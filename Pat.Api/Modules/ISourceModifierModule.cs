using Pat.Api.Model;

namespace Pat.Api.Modules
{
    public interface ISourceModifierModule: IModule, IHavingDefaultOptions
    {
        SourceSurface GetModifiedSurface(SourceSurface surface, IOptions options);
    }
}