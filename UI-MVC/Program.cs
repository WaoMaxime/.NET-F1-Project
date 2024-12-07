using BusinessLayer;
using DataAccessLayer;
using DataAccessLayer.EF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<IManager, Manager>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddDbContext<F1CarDbContext>(options =>
    options.UseSqlite("Data Source=f1cars.db"));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ensure database schema is initialized
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<F1CarDbContext>();
    dbContext.Database.EnsureCreated();
    dbContext.SeedDatabase();
}

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=F1Car}/{action=Index}/{id?}");
app.Run();