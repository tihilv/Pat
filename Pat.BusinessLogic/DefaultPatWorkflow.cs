using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Pat.Api.Model;
using Pat.Api.Modules;
using Pat.Api.Services;
using Pat.BusinessLogic.Annotations;
using Pat.BusinessLogic.Services;

namespace Pat.BusinessLogic
{
    public class DefaultPatWorkflow: INotifyPropertyChanged
    {
        private ModulesLoader _modulesLoader;
        private IOptionsService _optionsService;

        private readonly ModuleSelector<IDataSourceModule> _topHorizonDataSourceSelector;
        private readonly ModuleSelector<ISourceModifierModule> _baseHorizonModifierSelector;
        private readonly ModuleSelector<ITriangulationModule> _triangulationModuleSelector;
        private readonly ModuleSelector<ITriangulationModifierModule> _fluidContactModifierSelector;
        private readonly ModuleSelector<IDimensionModule> _resultDimensionSelector;
        private readonly VolumeService _volumeService;

        private SourceSurface _sourceTopHorizon;
        
        private TriangulatedSurface _triangulatedTopHorizon;
        private TriangulatedSurface _triangulatedBaseHorizon;

        private TriangulatedSurface _cutTriangulatedTopHorizon;
        private TriangulatedSurface _cutTriangulatedBaseHorizon;

        private double _resultValue;

        public event PropertyChangedEventHandler PropertyChanged;

        public DefaultPatWorkflow(string appDirectory)
        {
            _modulesLoader = new ModulesLoader(Path.Combine(appDirectory, "Modules"));
            _optionsService = new OptionsService(Path.Combine(appDirectory, "options.bin"));

            DimensionedValue.DimensionService = new DimensionService(_modulesLoader.GetModules<IDimensionModule>());

            _topHorizonDataSourceSelector = new ModuleSelector<IDataSourceModule>(_modulesLoader.GetModules<IDataSourceModule>(), _optionsService);
            _baseHorizonModifierSelector = new ModuleSelector<ISourceModifierModule>(_modulesLoader.GetModules<ISourceModifierModule>(), _optionsService);
            _triangulationModuleSelector = new ModuleSelector<ITriangulationModule>(_modulesLoader.GetModules<ITriangulationModule>(), _optionsService);
            _fluidContactModifierSelector = new ModuleSelector<ITriangulationModifierModule>(_modulesLoader.GetModules<ITriangulationModifierModule>(), _optionsService);
            _resultDimensionSelector = new ModuleSelector<IDimensionModule>(_modulesLoader.GetModules<IDimensionModule>().Where(m => m.Type == DimensionType.Cubic), _optionsService);

            _resultDimensionSelector.PropertyChanged += ResultDimensionSelectorOnPropertyChanged;

            _volumeService = new VolumeService();
        }

        public ModuleSelector<IDataSourceModule> TopHorizonDataSourceSelector => _topHorizonDataSourceSelector;

        public ModuleSelector<ISourceModifierModule> BaseHorizonModifierSelector => _baseHorizonModifierSelector;
        
        public ModuleSelector<ITriangulationModule> TriangulationModuleSelector => _triangulationModuleSelector;

        public ModuleSelector<ITriangulationModifierModule> FluidContactModifierSelector => _fluidContactModifierSelector;

        public ModuleSelector<IDimensionModule> ResultDimensionSelector => _resultDimensionSelector;

        public DimensionedValue Result => ResultValue.AsDimensionedValue(ResultDimensionSelector.SelectedModule.Identifier);

        public bool HasResult => _cutTriangulatedTopHorizon != null;

        public TriangulatedSurface CutTriangulatedTopHorizon => _cutTriangulatedTopHorizon;

        public TriangulatedSurface CutTriangulatedBaseHorizon => _cutTriangulatedBaseHorizon;

        private double ResultValue
        {
            get => _resultValue;
            set
            {
                if (_resultValue != value)
                {
                    _resultValue = value;
                    OnPropertyChanged(nameof(Result));
                    OnPropertyChanged(nameof(HasResult));
                }
            }
        }

        public void Calculate()
        {
            _sourceTopHorizon = TopHorizonDataSourceSelector.SelectedModule.GetSurface(TopHorizonDataSourceSelector.SelectedModuleOptions);
            var baseHorizonSource = BaseHorizonModifierSelector.SelectedModule.GetModifiedSurface(_sourceTopHorizon, BaseHorizonModifierSelector.SelectedModuleOptions);

            _triangulatedTopHorizon = TriangulationModuleSelector.SelectedModule.GetTriangulatedSurface(_sourceTopHorizon, TriangulationModuleSelector.SelectedModuleOptions);
            _triangulatedBaseHorizon = TriangulationModuleSelector.SelectedModule.GetTriangulatedSurface(baseHorizonSource, TriangulationModuleSelector.SelectedModuleOptions);

            _cutTriangulatedTopHorizon = FluidContactModifierSelector.SelectedModule.GetModifiedSurface(_triangulatedTopHorizon, FluidContactModifierSelector.SelectedModuleOptions);
            _cutTriangulatedBaseHorizon = FluidContactModifierSelector.SelectedModule.GetModifiedSurface(_triangulatedBaseHorizon, FluidContactModifierSelector.SelectedModuleOptions);

            var maxZ = Math.Max(
                _cutTriangulatedBaseHorizon.Triangles.SelectMany(t => t.Points).Max(p => p.Z),
                _cutTriangulatedTopHorizon.Triangles.SelectMany(t => t.Points).Max(p => p.Z));

            var volumeTop = _volumeService.GetVolumeUnderSurface(_cutTriangulatedTopHorizon, maxZ);
            var volumeBase = _volumeService.GetVolumeUnderSurface(_cutTriangulatedBaseHorizon, maxZ);

            ResultValue = volumeTop - volumeBase;

            _optionsService.SaveOptions();
        }

        
        private void ResultDimensionSelectorOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ModuleSelector<IDimensionModule>.SelectedModule))
                OnPropertyChanged(nameof(Result));
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
