using GraphQL.Types;
using stutor_core.Models.Sql;

namespace stutor_core.GraphQL.GraphTypes
{
    public class CategoryType : ObjectGraphType<Category>
    {
        public CategoryType()
        {
            Name = "Category";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Category.");
            Field(x => x.Name).Description("The name of the Category");
            Field(x => x.ImageUrl).Description("The url for the Category display image.");
        }
    }
}
