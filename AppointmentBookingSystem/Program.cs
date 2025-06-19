using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using AppointmentBookingSystem.Services;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using AppointmentBookingSystem;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, new string[] {}
        }
    });
});

// PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// JWT Setup
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ?? Login Endpoint
app.MapPost("/login", async ([FromBody] LoginRequest request, AppDbContext db) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
    if (user == null || user.PasswordHash != request.Password)
        return Results.Unauthorized();

    var claims = new[]
    {
        new System.Security.Claims.Claim("username", user.Username),
        new System.Security.Claims.Claim("role", user.Role ?? "User")
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
        claims: claims,
        expires: DateTime.UtcNow.AddHours(2),
        signingCredentials: creds);

    var tokenString = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler().WriteToken(token);
    return Results.Ok(new { token = tokenString });
});

// ?? Health Check Endpoint
app.MapGet("/db-health", async (IServiceProvider services) =>
{
    try
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        return Results.Ok(new
        {
            Status = "Healthy",
            Database = await db.Database.CanConnectAsync() ? "Connected" : "Disconnected",
            PendingMigrations = await db.Database.GetPendingMigrationsAsync(),
            AppointmentCount = await db.Appointments.CountAsync()
        });
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.ToString());
    }
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Example protected routes
app.MapGet("/user-area", [Authorize] () => "Welcome, authenticated user!");
app.MapGet("/admin-area", [Authorize(Roles = "Admin")] () => "Welcome, Admin user!");

app.Run();

record LoginRequest(string Username, string Password);
