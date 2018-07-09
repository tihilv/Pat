using System.Collections.Generic;
using Pat.Api.Modules;

namespace Pat.Api.Services
{
    public interface IModulesService
    {
        IEnumerable<IModule> LoadModules(string path);
        IEnumerable<T> GetModules<T>() where T : IModule;
    }
}