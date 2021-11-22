using stutor_core.Models.Sql;
using System.Collections.Generic;

namespace stutor_core.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        void Add(Category cat);

        Category Get(int ID);

        IEnumerable<Category> GetAll();

        IEnumerable<PopularCategory> GetAllPopular();
    }
}
