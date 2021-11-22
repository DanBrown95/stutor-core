using stutor_core.Models.Sql;
using System.Collections.Generic;

namespace stutor_core.Repositories.Interfaces
{
    public interface ITimezoneRepository
    {
        void Add(Timezone timezone);

        Timezone Get(int ID);

        IEnumerable<Timezone> GetAll();

        Timezone GetByUserId(string id);
    }
}
