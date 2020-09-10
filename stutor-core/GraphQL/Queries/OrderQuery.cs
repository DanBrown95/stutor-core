using GraphQL.Types;
using stutor_core.Database;
using stutor_core.GraphQL.GraphTypes;
using stutor_core.Services;

namespace stutor_core.GraphQL.Queries
{
    public class OrderQuery : ObjectGraphType
    {
        public OrderQuery(ApplicationDbContext db)
        {
            var _orderService = new OrderService(db);

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
                new QueryArgument<IdGraphType> { Name = "userId", Description = "The ID of the user to retrieve orders for." }),
              resolve: context =>
              {
                  var id = context.GetArgument<string>("userId");
                  return _orderService.GetAllByUserId(id);
              });
        }
    }
}