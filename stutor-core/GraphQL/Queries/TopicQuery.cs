using GraphQL.Types;
using stutor_core.Database;
using System.Linq;
using stutor_core.GraphQL.GraphTypes;
using stutor_core.Services;

namespace stutor_core.GraphyQL.Queries
{
    public class TopicQuery : ObjectGraphType
    {
        public TopicQuery(ApplicationDbContext db)
        {
            var _topicService = new TopicService(db);

            Field<TopicType>(
              "Topic",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id", Description = "The ID of the Topic." }),
              resolve: context =>
              {
                  var id = context.GetArgument<int>("id");
                  return _topicService.Get(id);
              });

            Field<ListGraphType<CategoryType>>(
              "Topics",
              resolve: context =>
              {
                  return _topicService.GetAll();
              });

            Field<TopicType>(
              "TopicsByCategory",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id", Description = "The Category ID to retrieve the topics for." }),
              resolve: context =>
              {
                  var id = context.GetArgument<int>("id");
                  return _topicService.GetTopicsByCategory(id);
              });
        }
    }
}
