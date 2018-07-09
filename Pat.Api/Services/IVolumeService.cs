using Pat.Api.Model;

namespace Pat.Api.Services
{
    public interface IVolumeService
    {
        double GetVolumeUnderSurface(TriangulatedSurface surface, double lowerLine);
    }
}