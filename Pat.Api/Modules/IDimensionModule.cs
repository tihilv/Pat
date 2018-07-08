namespace Pat.Api.Modules
{
    public interface IDimensionModule: IModule
    {
        string Identifier { get; }
        DimensionType Type { get; }
        double ToSI(double value);
        double FromSI(double value);
    }

    public enum DimensionType
    {
        Linear = 1,
        Cubic = 3
    }
}