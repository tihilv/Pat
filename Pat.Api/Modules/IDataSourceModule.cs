using Pat.Api.Model;

namespace Pat.Api.Modules
{
    public interface IDataSourceModule: IModule, IHavingDefaultOptions
    {
        SourceSurface GetSurface(IOptions options);
    }
}