using stutor_core.Models.Sql;
using System.Collections.Generic;

namespace stutor_core.Services.Interfaces
{
    public interface ITimezoneService
    {
        Timezone Get(int id);

        IEnumerable<Timezone> GetAll();

        void Add(Timezone timezone);

        Timezone GetByUserId(string id);
    }
}
