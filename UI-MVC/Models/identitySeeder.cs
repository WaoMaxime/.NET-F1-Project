using BusinessLayer;
using Microsoft.AspNetCore.Identity;

namespace UI;

public class IdentitySeeder
{

    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly Manager _manager;

    public IdentitySeeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, Manager manager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _manager = manager;
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
        for (var i = 1; i <= 5; i++)
        {
            users.Add(new IdentityUser
            {
                UserName = $"gebruiker{i}",
                Email = $"Gebruiker{i}@gmail.com",
                EmailConfirmed = true
            });
        }

        foreach (var user in users)
        {
            var result = await _userManager.CreateAsync(user, "Gebruiker_1234");
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
        //link users to existing domain data

        int count = 0;
        foreach (var car in _manager.GetAllF1Cars())
        {
            car.User = users[count];
            count++;
        }
        foreach (var user in users)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

    }
}
