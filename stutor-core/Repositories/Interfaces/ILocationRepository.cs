using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using System.Collections.Generic;

namespace stutor_core.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        void Add(Timezone timezone);

        Timezone Get(int ID);

        IEnumerable<Timezone> GetAll();

        Timezone GetTimezoneByUserId(string id);

        LocationData GetLocationByUserId(string id);
    }
}
