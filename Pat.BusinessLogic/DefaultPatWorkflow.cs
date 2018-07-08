using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Pat.Api.Model;
using Pat.Api.Modules;
using Pat.BusinessLogic.Annotations;
using Pat.BusinessLogic.Services;

namespace Pat.BusinessLogic
{
    public class DefaultPatWorkflow: INotifyPropertyChanged
    {
        private ModulesLoader _modulesLoader;

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

        private double _result;

        public DefaultPatWorkflow(string modulesDirectory)
        {
            _modulesLoader = new ModulesLoader(modulesDirectory);

            DimensionedValue.DimensionService = new DimensionService(_modulesLoader.GetModules<IDimensionModule>());

            _topHorizonDataSourceSelector = new ModuleSelector<IDataSourceModule>(_modulesLoader.GetModules<IDataSourceModule>());
            _baseHorizonModifierSelector = new ModuleSelector<ISourceModifierModule>(_modulesLoader.GetModules<ISourceModifierModule>());
            _triangulationModuleSelector = new ModuleSelector<ITriangulationModule>(_modulesLoader.GetModules<ITriangulationModule>());
            _fluidContactModifierSelector = new ModuleSelector<ITriangulationModifierModule>(_modulesLoader.GetModules<ITriangulationModifierModule>());
            _resultDimensionSelector = new ModuleSelector<IDimensionModule>(_modulesLoader.GetModules<IDimensionModule>().Where(m => m.Type == DimensionType.Cubic));

            _resultDimensionSelector.PropertyChanged += ResultDimensionSelectorOnPropertyChanged;

            _volumeService = new VolumeService();
        }

        public ModuleSelector<IDataSourceModule> TopHorizonDataSourceSelector => _topHorizonDataSourceSelector;

        public ModuleSelector<ISourceModifierModule> BaseHorizonModifierSelector => _baseHorizonModifierSelector;
        
        public ModuleSelector<ITriangulationModule> TriangulationModuleSelector => _triangulationModuleSelector;

        public ModuleSelector<ITriangulationModifierModule> FluidContactModifierSelector => _fluidContactModifierSelector;

        public ModuleSelector<IDimensionModule> ResultDimensionSelector => _resultDimensionSelector;

        public DimensionedValue Result => _result.AsDimensionedValue(ResultDimensionSelector.SelectedModule.Identifier);

        public bool HasResult => _cutTriangulatedTopHorizon != null;

        public TriangulatedSurface CutTriangulatedTopHorizon => _cutTriangulatedTopHorizon;

        public TriangulatedSurface CutTriangulatedBaseHorizon => _cutTriangulatedBaseHorizon;

        public event PropertyChangedEventHandler PropertyChanged;

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

            _result = volumeTop - volumeBase;

            OnPropertyChanged(nameof(Result));
            OnPropertyChanged(nameof(HasResult));
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
