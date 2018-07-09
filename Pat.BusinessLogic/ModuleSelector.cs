using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Pat.Api.Modules;
using Pat.Api.Services;
using Pat.BusinessLogic.Annotations;

namespace Pat.BusinessLogic
{
    public class ModuleSelector<T>: IOptionsProvider, INotifyPropertyChanged where T:IModule
    {
        private readonly IOptionsService _optionsService;
        private readonly T[] _modules;
        private T _selectedModule;
        private IOptions _selectedModuleOptions;

        public event PropertyChangedEventHandler PropertyChanged;

        public ModuleSelector(IEnumerable<T> modules, IOptionsService optionsService)
        {
            _optionsService = optionsService;
            _modules = modules.ToArray();
            SelectedModule = _modules.FirstOrDefault();
        }

        public T[] Modules => _modules;

        public T SelectedModule
        {
            get => _selectedModule;
            set
            {
                if (!Equals(_selectedModule, value))
                {
                    _selectedModule = value;
                    _selectedModuleOptions = _optionsService.GetSavedOptions((value as IHavingDefaultOptions)?.GetDefaultOptions());
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasOptions));
                }
            }
        }

        public IOptions SelectedModuleOptions => _selectedModuleOptions;

        public bool HasOptions => _selectedModuleOptions != null;
        
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}