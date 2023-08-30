using dotnet6mvcEcommerce.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet6mvcEcommerce.Controllers
{
    public class RolesController : Controller
    {
        private string AdminRole = "Admin";
        private string UserEmail = "Admin@gmail.com";

        private readonly ApplicationDbContext db;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            await roleManager.CreateAsync(new IdentityRole(AdminRole));
            IdentityUser user = new IdentityUser { UserName = UserEmail, Email = UserEmail, EmailConfirmed = true };
            await userManager.CreateAsync(user, UserEmail);
            await userManager.AddToRoleAsync(user, AdminRole);

            return Redirect("/");
        }
    }
}
