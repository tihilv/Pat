using System;
using Pat.Api.Services;

namespace Pat.Api.Model
{
    public class DimensionedValue
    {
        public static IDimensionService DimensionService;
        
        private double _siValue;
        private string _dimensionIdentifier;

        public DimensionedValue(double siValue, string dimensionIdentifier)
        {
            _siValue = siValue;
            _dimensionIdentifier = dimensionIdentifier;
        }

        public override string ToString()
        {
            double convertedValue = DimensionService.FromSI(_siValue, _dimensionIdentifier);
            return $"{convertedValue} {_dimensionIdentifier}";
        }

        public static implicit operator double(DimensionedValue value)
        {
            return value?._siValue??0;
        }

        public static implicit operator DimensionedValue(string value)
        {
            var splitted = value.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries);
            var valueInDimension = double.Parse(splitted[0]);
            var dimensionIdentifier = splitted[1];
            var siValue = DimensionService.ToSI(valueInDimension, dimensionIdentifier);
            
            return new DimensionedValue(siValue, dimensionIdentifier);
        }

        public DimensionedValue ConverTo(string dimensionIdentifier)
        {
            return new DimensionedValue(_siValue, dimensionIdentifier);
        }
    }

    public static class DimensionedValueHelper
    {
        public static DimensionedValue AsDimensionedValue(this double siValue, string dimensionIdentifier)
        {
            return new DimensionedValue(DimensionedValue.DimensionService.FromSI(siValue, dimensionIdentifier), dimensionIdentifier);
        }
    }
}