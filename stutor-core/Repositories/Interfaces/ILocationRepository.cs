using stutor_core.Models.ViewModels;

namespace stutor_core.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        LocationData GetLocationByUserId(string id);
    }
}
