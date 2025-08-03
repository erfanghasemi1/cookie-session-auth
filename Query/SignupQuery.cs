using Cookie_Session.Models;
using Dapper;
using MySqlConnector;


namespace Cookie_Session.Query
{
    public class SignupQuery
    {
        private readonly string? _connectionString;

        public SignupQuery(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("mysqlconnection");
        }

        public async Task<Guid?> CheckUserExistsAsync(SignupModel request)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "select Id from User where Username = @u or PhoneNumber = @ph or Email = @e";

                Guid? id = await connection.QueryFirstOrDefaultAsync<Guid?>(query, new
                {
                    u = request.Username,
                    ph = request.PhoneNumber,
                    e = request.Email

                });

                return id;
            }
        }

        public async Task<bool> AddNewUserAsync(SignupModel request )
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"insert into User(Id , Username , Password , PhoneNumber , Email) 
                                 value (@id , @u , @p , @ph , @e)";
                try
                {
                    await connection.ExecuteAsync(query, new
                    {
                        id = request.UserId,
                        u = request.Username,
                        p = request.Password,
                        ph = request.PhoneNumber,
                        e = request.Email
                    });
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
