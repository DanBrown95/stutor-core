using GraphQL.Types;
using stutor_core.Database;
using stutor_core.GraphQL.GraphTypes;
using stutor_core.Services;

namespace stutor_core.GraphQL.Queries
{
    public class TopicExpertQuery : ObjectGraphType
    {
        public TopicExpertQuery(ApplicationDbContext db)
        {
            var _topicService = new TopicService(db);

            Field<ListGraphType<TopicExpertType>>(
              "TopicExperts",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "topicId", Description = "The ID of the Topic." }),
              resolve: context =>
              {
                  var topicId = context.GetArgument<int>("topicId");
                  return _topicService.GetTopicExperts(topicId);
              });
        }
    }
}
