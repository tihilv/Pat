using Pat.Api.Modules;

namespace Pat.Api.Services
{
    public interface IOptionsService
    {
        IOptions GetSavedOptions(IOptions emptyOptions);
        void SaveOptions();
    }
}
