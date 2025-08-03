using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Cookie_Session.Models;
using Cookie_Session.Query;
using ShopProject.Utils;

namespace Cookie_Session.Middleware
{
    public class SignupMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AES _aes;
        private readonly SignupQuery signupQuery;

        public SignupMiddleware(RequestDelegate next , AES aes , SignupQuery sq)
        {
            _next = next;
            _aes = aes;
            signupQuery = sq;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/signup" , StringComparison.OrdinalIgnoreCase) &&
                context.Request.Method.Equals("POST" , StringComparison.OrdinalIgnoreCase) )
            {
                // reade request body ( JSON file )
                context.Request.EnableBuffering();
                StreamReader reader = new StreamReader(context.Request.Body , leaveOpen : true);
                var response = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                var request = JsonSerializer.Deserialize<SignupModel>(response, new JsonSerializerOptions { 
                    PropertyNameCaseInsensitive = true
                });

                if (request == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "Issue with your request!"
                    });
                    return;
                }

                // validate inputs
                ValidationContext validationContext = new ValidationContext(request);
                List<ValidationResult> results = new List<ValidationResult>();

                bool IsValid = Validator.TryValidateObject(
                    request,
                    validationContext,
                    results,
                    true
                );

                if (!IsValid)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Errors = results.Select(e => e.ErrorMessage)
                    });
                    return;
                }

                // check if user exists 
                Guid? id = await signupQuery.CheckUserExistsAsync(request);

                if (id != null)
                {
                    context.Response.StatusCode = 409;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "This Username or Email or Phone number exists!"
                    });
                    return;
                }

                request.UserId = Guid.NewGuid().ToString();

                request.Password = _aes.Encrypt(request.Password);

                context.Items["data"] = request;

                await _next(context);

                int statusCode = context.Response.StatusCode;

                if (statusCode == 500)
                    return;

                // setting cookie
                await Utils.SetCookie.SetCookieAsync(context, request.UserId);
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "User added successfully."
                });
            }

            else await _next(context);
        }
    }
}
