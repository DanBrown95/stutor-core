using GraphQL.Types;
using stutor_core.Models.Sql;

namespace stutor_core.GraphQL.GraphTypes
{
    public class TopicType : ObjectGraphType<Topic>
    {
        public TopicType()
        {
            Name = "Topic";

            Field(x => x.Id).Description("The ID of the Topic.");
            Field(x => x.CategoryId).Description("The ID of the category the topic belongs to.");
            Field(x => x.Name).Description("The name of the Topic");
        }
    }
}
