using DataAccessLayer.EF;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace UI;

public class IdentitySeeder
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly F1CarDbContext _context;

    public IdentitySeeder(UserManager<IdentityUser> userManager, F1CarDbContext context, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _context = context;
        _roleManager = roleManager;
    }

    public async Task SeedAsync()
    {
        //Admin role and User role
        var adminRole = new IdentityRole("Admin");
        await _roleManager.CreateAsync(adminRole);
        var userRole = new IdentityRole("User");
        await _roleManager.CreateAsync(userRole);
        
            
        //New admin User
        var admin = new IdentityUser
        { 
            UserName = "admin",
            Email = "Admin@gmail.com",
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(admin, "Admin123!");
        await _userManager.AddToRoleAsync(admin, "Admin");
            
        //New normal User
        var users = new List<IdentityUser>();
        for (int i = 1; i <= 5; i++)
        {
            users.Add(new IdentityUser
            {
                UserName = $"gebruiker{i}",
                Email = $"Gebruiker{i}@gmail.com",
                EmailConfirmed = true
            });
        }

        foreach (IdentityUser user in users)
        {
            var result = await _userManager.CreateAsync(user, "Gebruiker_1234");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
        //link users to existing domain data

        int count = 0;
        foreach (F1Car car in _context.F1Cars)
        {
            car.User = users[count];
            count++;
        }
        await _userManager.AddToRoleAsync(users[0], "User");
        await _context.SaveChangesAsync();
    }
}
