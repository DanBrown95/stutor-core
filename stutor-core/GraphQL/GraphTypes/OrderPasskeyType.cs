using GraphQL.Types;
using stutor_core.Models.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace stutor_core.GraphQL.GraphTypes
{
    public class OrderPasskeyType : ObjectGraphType<OrderPasskey>
    {
        public OrderPasskeyType()
        {
            Name = "OrderPasskey";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the OrderPasskey.");
            Field(x => x.OrderId).Description("The ID of the Order");
            Field(x => x.ClientPasskey).Description("The passkey for the order");
        }
    }
}
