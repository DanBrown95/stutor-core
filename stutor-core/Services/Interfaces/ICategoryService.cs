using stutor_core.Models.Sql;
using System.Collections.Generic;

namespace stutor_core.Services.Interfaces
{
    public interface ICategoryService
    {
        Category Get(int id);

        IEnumerable<Category> GetAll();

        void Add(Category category);

        IEnumerable<PopularCategory> GetAllPopular();
    }
}
