using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UserController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET
    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(string account, string password, string submit)
    {
        /*var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name
 account)
        };
        var identity = new ClaimsIdentity(claims, "Cookies");
        var principal = new ClaimsPrincipal(identity);
        HttpContext.SignInAsync(principal);*/
        
        //var user = new IdentityUser(account);
        //_signInManager.PasswordSignInAsync(account, password, false, false);

        if (submit == "Register")
        {
            var user = new IdentityUser(account);
            
            var createdUser = await _userManager.CreateAsync(user);
        }
        
        var loginResult = await _signInManager.PasswordSignInAsync(account, password, false, false);
        
        return View();
    }
}