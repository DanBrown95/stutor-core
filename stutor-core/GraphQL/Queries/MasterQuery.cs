using GraphQL.Types;
using stutor_core.Database;
using stutor_core.GraphQL.GraphTypes;
using stutor_core.Services;

namespace stutor_core.GraphQL.Queries
{
    public class MasterQuery : ObjectGraphType
    {
        public MasterQuery(ApplicationDbContext db)
        {
            var _timezoneService = new TimezoneService(db);
            var _categoryService = new CategoryService(db);
            var _topicService = new TopicService(db);
            var _orderService = new OrderService(db);

            //Timezone Queries
            Field<TimezoneType>(
              "Timezone",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id", Description = "The ID of the Timezone." }),
              resolve: context =>
              {
                  var id = context.GetArgument<int>("id");
                  return _timezoneService.Get(id);
              });

            Field<ListGraphType<TimezoneType>>(
              "Timezones",
              resolve: context =>
              {
                  return _timezoneService.GetAll();
              });

            // Category Queries
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

            // Topic Queries
            Field<TopicType>(
              "Topic",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id", Description = "The ID of the Topic." }),
              resolve: context =>
              {
                  var id = context.GetArgument<int>("id");
                  return _topicService.Get(id);
              });

            Field<ListGraphType<TopicType>>(
              "Topics",
              resolve: context =>
              {
                  var res = _topicService.GetAll();
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


            // Order Queries
            Field<OrderType>(
              "Order",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "id", Description = "The ID of the order." }),
              resolve: context =>
              {
                  var id = context.GetArgument<int>("id");
                  return _orderService.Get(id);
              });

            Field<ListGraphType<CategoryType>>(
              "Orders",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "userId", Description = "The user to retrieve orders for." }),
              resolve: context =>
              {
                  var id = context.GetArgument<string>("userId");
                  return _orderService.GetAllByUserId(id);
              });

            // TopicExpert Queries
            Field<ListGraphType<TopicExpertType>>(
              "TopicExperts",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "topicId", Description = "The ID of the Topic." }),
              resolve: context =>
              {
                  var topicId = context.GetArgument<int>("topicId");
                  return _topicService.GetTopicExperts(topicId);
              });

            // OrderPasskey Queries
            Field<OrderPasskeyType>(
              "OrderPasskey",
              arguments: new QueryArguments(
                new QueryArgument<IdGraphType> { Name = "OrderId", Description = "The ID of the Order the passkey is linked to." }),
              resolve: context =>
              {
                  var OrderId = context.GetArgument<int>("OrderId");
                  return _orderService.GetOrderPasskey(OrderId);
              });

        }
    }
}
