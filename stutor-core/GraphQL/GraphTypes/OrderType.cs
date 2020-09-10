using GraphQL.Types;
using stutor_core.Models.Sql;

namespace stutor_core.GraphQL.GraphTypes
{
    public class OrderType : ObjectGraphType<Order>
    {
        public OrderType()
        {
            Name = "Order";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Order.");
            Field(x => x.ExpertId).Description("The Id of the expert who was assigned to the order");
            Field(x => x.CallLength).Description("The length of the call in seconds.");
            Field(x => x.Charge).Description("The amount charged to the credit card for the service.");
            Field(x => x.Status).Description("The status of the order.");
            Field(x => x.Submitted).Description("The day and time the order was submitted.");
            Field(x => x.TopicId).Description("The Id of the topic requested.");
            Field(x => x.UserId).Description("The user Id.");
        }
    }
}
