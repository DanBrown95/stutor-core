using GraphQL.Types;
using stutor_core.Models.Sql;

namespace stutor_core.GraphQL.GraphTypes
{
    public class TimezoneType : ObjectGraphType<Timezone>
    {
        public TimezoneType()
        {
            Name = "Timezone";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the timezone.");
            Field(x => x.FriendlyName).Description("Friendly display name of the timezone.");
            Field(x => x.TZName).Description("The TZ standard name for the timezone.");
        }
    }
}
