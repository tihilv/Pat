using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Pat.Api.Modules;
using Pat.BusinessLogic.Annotations;

namespace Pat.BusinessLogic
{
    public interface IOptionsProvider
    {
        IOptions SelectedModuleOptions { get; }
    }
    public class ModuleSelector<T>: IOptionsProvider, INotifyPropertyChanged where T:IModule
    {
        private readonly T[] _modules;
        private T _selectedModule;
        private IOptions _selectedModuleOptions;

        public ModuleSelector(IEnumerable<T> modules)
        {
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
                    _selectedModuleOptions = (value as IHavingDefaultOptions)?.GetDefaultOptions();
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(HasOptions));
                }
            }
        }

        public IOptions SelectedModuleOptions => _selectedModuleOptions;

        public bool HasOptions => _selectedModuleOptions != null;
        
        public event PropertyChangedEventHandler PropertyChanged;

        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}