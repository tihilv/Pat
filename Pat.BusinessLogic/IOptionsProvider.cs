using Pat.Api.Modules;

namespace Pat.BusinessLogic
{
    public interface IOptionsProvider
    {
        IOptions SelectedModuleOptions { get; }
    }
}