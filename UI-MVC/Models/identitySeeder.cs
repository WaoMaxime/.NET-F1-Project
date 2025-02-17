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
        var user = new IdentityUser("Gebruiker");
        var createdUser = await _userManager.CreateAsync(user, "123456");
    }
}
