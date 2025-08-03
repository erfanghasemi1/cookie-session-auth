using Cookie_Session.Models;
using Dapper;
using MySqlConnector;

namespace Cookie_Session.Query
{
    public class LoginQuery
    {
        private readonly string? _connectionString;

        public LoginQuery(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mysqlconnection");
        }

        public async Task<Guid?> LoginAsync(LoginModel request)
        {
            using (var conneciton = new MySqlConnection(_connectionString))
            {
                await conneciton.OpenAsync();

                string query = "select Id from User where Username = @u and Password = @p";

                Guid? id = await conneciton.QueryFirstOrDefaultAsync<Guid?>(query, new
                {
                    u = request.Username,
                    p = request.Password
                });

                return id;
            }
        }
    }
}
