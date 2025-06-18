using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using AppointmentBookingSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AppointmentBookingSystem;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL EF Core Configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLConnection")));

// Register Services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("SecretKey");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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

var app = builder.Build();

// Map health check endpoint
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

app.UseAuthentication(); // Add authentication middleware
app.UseAuthorization();

app.MapControllers();

// Initial connection test
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        Console.WriteLine($"Database connected: {await db.Database.CanConnectAsync()}");
        Console.WriteLine($"Pending migrations: {string.Join(", ", await db.Database.GetPendingMigrationsAsync())}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
}

app.Run();