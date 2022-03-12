using stutor_core.Models.ViewModels;

namespace stutor_core.Services.Interfaces
{
    public interface ILocationService
    {
        LocationData GetLocationByUserId(string id);
    }
}
