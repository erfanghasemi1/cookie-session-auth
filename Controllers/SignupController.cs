using Cookie_Session.Models;
using Cookie_Session.Query;
using Microsoft.AspNetCore.Mvc;

namespace Cookie_Session.Controllers
{
    public class SignupController : Controller
    {
        private readonly SignupQuery signupQuery;

        public SignupController(SignupQuery sq)
        {
            signupQuery = sq;
        }


        [HttpPost("signup")]
        public async Task Signup()
        {
            SignupModel? request = HttpContext.Items["data"] as SignupModel;

            bool result = await signupQuery.AddNewUserAsync(request);

            if (!result)
            {
                HttpContext.Response.StatusCode = 500;
                await HttpContext.Response.WriteAsJsonAsync(new
                {
                    Message = "Server error!"
                });
                return;
            }
            HttpContext.Response.StatusCode = 200;
        }
    }
}
