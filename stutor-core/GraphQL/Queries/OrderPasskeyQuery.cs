using GraphQL.Types;
using stutor_core.Database;
using stutor_core.GraphQL.GraphTypes;
using stutor_core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.GraphQL.Queries
{
    public class OrderPasskeyQuery : ObjectGraphType
    {
        public OrderPasskeyQuery(ApplicationDbContext db)
        {
            var _orderService = new OrderService(db);
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
