using Cookie_Session.Models;
using Cookie_Session.Query;
using Microsoft.AspNetCore.Mvc;

namespace Cookie_Session.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginQuery _query;

        public LoginController(LoginQuery loginQuery)
        {
            _query = loginQuery;
        }

        [HttpPost("login")]
        public async Task Login()
        {
            LoginModel? request = HttpContext.Items["data"] as LoginModel;

            if (request == null)
            {
                HttpContext.Response.StatusCode = 500;
                await HttpContext.Response.WriteAsJsonAsync(new
                {
                    Message = "Server Error!"
                });
                return;
            }

            Guid? id = await _query.LoginAsync(request);

            if (id == null)
            {
                HttpContext.Response.StatusCode = 401;
                await HttpContext.Response.WriteAsJsonAsync(new
                {
                    Message = "Username or Password is incorrect!"
                });
                return;
            }

            request.Id = id.ToString();
            HttpContext.Response.StatusCode = 200;
        }
    }
}
