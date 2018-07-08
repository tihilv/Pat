using System;
using System.Collections.Generic;
using Pat.Api.Modules;
using Pat.Api.Services;

namespace Pat.BusinessLogic.Services
{
    public class DimensionService: IDimensionService
    {
        private readonly Dictionary<string, IDimensionModule> _dimensionModules;

        public DimensionService(IEnumerable<IDimensionModule> dimensionModules)
        {
            _dimensionModules = new Dictionary<string, IDimensionModule>();

            foreach (var dimensionModule in dimensionModules)
                _dimensionModules.Add(dimensionModule.Identifier, dimensionModule);
        }

        public double FromSI(double valueInSI, string dimensionIdentifier)
        {
            IDimensionModule dimensionModule;
            if (!_dimensionModules.TryGetValue(dimensionIdentifier, out dimensionModule))
                throw new ArgumentException($"The dimension with identifier {dimensionIdentifier} is not found.");
            
            return dimensionModule.FromSI(valueInSI);
        }

        public double ToSI(double valueInDimension, string dimensionIdentifier)
        {
            IDimensionModule dimensionModule;
            if (!_dimensionModules.TryGetValue(dimensionIdentifier, out dimensionModule))
                throw new ArgumentException($"The dimension with identifier {dimensionIdentifier} is not found.");

            return dimensionModule.ToSI(valueInDimension);
        }
    }
}