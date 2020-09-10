using GraphQL.Types;
using stutor_core.Models.Sql;

namespace stutor_core.GraphQL.GraphTypes
{
    public class TopicExpertType : ObjectGraphType<TopicExpert>
    {
        public TopicExpertType()
        {
            Name = "TopicExpert";

            Field(x => x.TopicId, type: typeof(IdGraphType)).Description("The ID of the Topic.");
            Field(x => x.ExpertId, type: typeof(IdGraphType)).Description("The ID of the Expert.");
        }
    }
}
