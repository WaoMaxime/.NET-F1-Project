using AspNetCoreLiveMonitoring.Extensions;
using BusinessLayer;
using DataAccessLayer;
using DataAccessLayer.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddDbContext<F1CarDbContext>(options =>
    options.UseSqlite("Data Source=f1cars.db"));
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IManager, Manager>();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
    });
builder.Services.AddControllersWithViews();
builder.Services.AddLiveMonitoring();

var app = builder.Build();

// Database initialisatie en seeding
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<F1CarDbContext>();
    dbContext.Database.EnsureCreated();
    dbContext.SeedDatabase();
}

app.UseRouting();
app.UseAndMapLiveMonitoring();
app.UseStaticFiles();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
