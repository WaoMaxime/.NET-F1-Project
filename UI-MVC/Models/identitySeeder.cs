using Microsoft.AspNetCore.Identity;

namespace UI;

public class IdentitySeeder
{

    private readonly UserManager<IdentityUser> _userManager;

    public IdentitySeeder(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        //Admin role and User role
        /*var adminRole = new IdentityRole("Admin");
        await _roleManager.CreateAsync(adminRole);*/
        
            
        /*//New admin User
        var admin = new IdentityUser
        { 
            UserName = "admin",
            Email = "Admin@gmail.com",
            EmailConfirmed = true
        };

        await _userManager.CreateAsync(admin, "Admin123!");
        await _userManager.AddToRoleAsync(admin, "Admin");*/
            
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

        int count = 0;
        foreach (IdentityUser user in users)
        {
            count++;
            await _userManager.CreateAsync(user, $"User{count}xx!");
        }
    }
}
