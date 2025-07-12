using familytree_api;
using familytree_api.Database;
using familytree_api.Dtos.AppSettings;
using familytree_api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var corsOrigins = builder.Configuration["CorsOrigins"]?.Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            if (corsOrigins != null && corsOrigins.Length > 0)
            {
                policy.WithOrigins(corsOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            }

        });
});


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();

// Auto-register all services from the current assembly
builder.Services.AddServicesFromAssembly(Assembly.GetExecutingAssembly());

// Bind SMTP configuration from appsettings.json
builder.Services.Configure<SmtpConfig>(builder.Configuration.GetSection("SMTP"));

// Bind Frontend URL configuration from appsettings.json
builder.Services.Configure<FrontEndUrl>(builder.Configuration.GetSection("FrontEndUrl"));

// Bind JWT configuration from appsettings.json
builder.Services.Configure<JWTConfig>(builder.Configuration.GetSection("Jwt"));

// Configure Pomelo and MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 23))));

// Add authentication services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors();

// Add custom middleware to extract JWT token
app.UseMiddleware<JwtMiddleware>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//}
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

if (Directory.Exists(fullPath)) // Check if folder exists
{
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(fullPath),
        RequestPath = "/uploads",
        ServeUnknownFileTypes = true // Optional: allows serving any file type
    });
}

app.Run();
