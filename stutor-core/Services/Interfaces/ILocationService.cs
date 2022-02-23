using stutor_core.Models.Sql;
using stutor_core.Models.ViewModels;
using System.Collections.Generic;

namespace stutor_core.Services.Interfaces
{
    public interface ILocationService
    {
        Timezone Get(int id);

        IEnumerable<Timezone> GetAll();

        void Add(Timezone timezone);

        Timezone GetTimezoneByUserId(string id);

        LocationData GetLocationByUserId(string id);
    }
}
