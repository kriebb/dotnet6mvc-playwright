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
            //創建角色
            await roleManager.CreateAsync(new IdentityRole(AdminRole));
            //創建管理員用戶
            IdentityUser user = new IdentityUser { UserName = UserEmail, Email = UserEmail };
            await userManager.CreateAsync(user,UserEmail);
            //把用戶添加到角色
            return View();
        }
    }
}
