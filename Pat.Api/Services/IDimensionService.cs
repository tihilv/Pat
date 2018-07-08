namespace Pat.Api.Services
{
    public interface IDimensionService
    {
        double FromSI(double valueInSI, string dimensionIdentifier);
        double ToSI(double valueInDimension, string dimensionIdentifier);
    }
}