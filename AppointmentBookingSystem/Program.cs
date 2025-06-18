using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using AppointmentBookingSystem;
using AppointmentBookingSystem.Services;

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

// Middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Initial DB check log
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        Console.WriteLine($"? Database connected: {await db.Database.CanConnectAsync()}");
        var migrations = await db.Database.GetPendingMigrationsAsync();
        Console.WriteLine(migrations.Any()
            ? $"?? Pending migrations: {string.Join(", ", migrations)}"
            : "? No pending migrations.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"? Database connection failed: {ex.Message}");
    }
}

app.Run();
