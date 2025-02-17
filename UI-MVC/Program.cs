using System.Text.Json.Serialization;
using AspNetCoreLiveMonitoring.Extensions;
using BusinessLayer;
using DataAccessLayer;
using DataAccessLayer.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddDbContext<F1CarDbContext>(options =>
    options.UseSqlite("Data Source=f1cars.db"));
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IManager, Manager>();
builder.Services.AddControllersWithViews()
    //.AddXmlSerializerFormatters();
    .AddXmlDataContractSerializerFormatters()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    });

// KdG Live Monitoring
builder.Services.AddLiveMonitoring();

//Authenticatie
builder.Services.AddAuthentication()
    .AddCookie();
//Authorizatie
builder.Services.AddAuthorization();
//ASP.NET Identity
builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<F1CarDbContext>(); // Ensure this exists

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var appDbContext = scope.ServiceProvider.GetService<F1CarDbContext>();
    const bool dropDatabase = true;
    if (dropDatabase)
        appDbContext.Database.EnsureDeleted();
    bool isDbCreated = appDbContext.Database.EnsureCreated(); // Code First-trigger
    if (isDbCreated)
    {
        // seeding
        DataSeeder dataSeeder = new DataSeeder();
        dataSeeder.Seed(appDbContext);
        UserManager<IdentityUser> userManager = scope.ServiceProvider.GetService<UserManager<IdentityUser>>();
        UI.IdentitySeeder dataSeeder2 = new UI.IdentitySeeder(userManager);
        await dataSeeder2.SeedAsync();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseRouting();
app.MapRazorPages(); //fundemental
app.UseAndMapLiveMonitoring();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
