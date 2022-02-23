using Microsoft.Extensions.Configuration;
using stutor_core.Database;
using System.Data.SqlClient;

namespace stutor_core.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IConfiguration _configuration;

        public BaseRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public SqlConnection DBConnection()
        {
            var connString = _configuration["Database:amazonRDS"];
            return new SqlConnection(connString);
        }
    }
}
