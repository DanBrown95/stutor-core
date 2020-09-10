using GraphQL.Types;
using stutor_core.Database;
using stutor_core.GraphQL.GraphTypes;
using stutor_core.Services;
using System.Linq;

namespace stutor_core.GraphQL.Queries
{
    public class TimezoneQuery : ObjectGraphType
    {
        public TimezoneQuery(ApplicationDbContext db)
        {
            var _timezoneService = new TimezoneService(db);

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
        }
    }
}
