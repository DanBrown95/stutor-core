using GraphQL.Types;
using stutor_core.Database;
using stutor_core.GraphQL.GraphTypes;
using stutor_core.Repositories;
using stutor_core.Services;
using System.Linq;

namespace stutor_core.GraphQL.Queries
{
    public class CategoryQuery : ObjectGraphType
    {
        public CategoryQuery(ApplicationDbContext db)
        {
            var _categoryService = new CategoryService(db);
            Field<CategoryType>(
              "Category",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "Id", Description = "The ID of the Category." }),
              resolve: context =>
              {
                  var Id = context.GetArgument<int>("Id");
                  return _categoryService.Get(Id);
              });

            Field<ListGraphType<CategoryType>>(
              "Categories",
              resolve: context =>
              {
                  return _categoryService.GetAll();
              });
        }
    }
}
