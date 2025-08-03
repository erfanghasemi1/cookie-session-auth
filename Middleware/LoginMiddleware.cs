using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Cookie_Session.Models;
using Cookie_Session.Utils;
using ShopProject.Utils;

namespace Cookie_Session.Middleware
{
    public class LoginMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AES _aes;

        public LoginMiddleware(RequestDelegate requestDelegate ,AES aes)
        {
            _next = requestDelegate;
            _aes = aes;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.Equals("/login" , StringComparison.OrdinalIgnoreCase) &&
                context.Request.Method.Equals("POST" , StringComparison.OrdinalIgnoreCase))
            {
                // read Input data (JSON file )
                context.Request.EnableBuffering();
                StreamReader reader = new StreamReader(context.Request.Body , leaveOpen : true);
                var response = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                LoginModel? request = JsonSerializer.Deserialize<LoginModel?>(response , new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (request == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new
                          {
                                Message = "Request is null!"
                          });
                    return;
                }

                // validate data 
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

                request.Password = _aes.Encrypt(request.Password);
                context.Items["data"] = request;

                await _next(context);

                if (context.Response.StatusCode != 200)
                    return;

                await SetCookie.SetCookieAsync(context, request.Id);
                await context.Response.WriteAsJsonAsync(new
                {
                    Message = "User login successfully."
                });
            }

            else await _next(context);
        }
    }
}
