using Cookie_Session.Middleware;
using Cookie_Session.Query;
using ShopProject.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Register distributed memory cache (required for session)
builder.Services.AddDistributedMemoryCache();

// Register session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Register cookie
builder.Services.AddAuthentication("MyCookieAuth")
    .AddCookie("MycookieAuth" , options =>
    {
        options.Cookie.Name = "AuthCookie";
        options.LoginPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.Cookie.MaxAge = TimeSpan.FromHours(30);
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization();

builder.Services.AddSingleton<AES>();

builder.Services.AddSingleton<SignupQuery>();

builder.Services.AddSingleton<LoginQuery>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseSession();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<SignupMiddleware>();
app.UseMiddleware<LoginMiddleware>();

app.Run();
