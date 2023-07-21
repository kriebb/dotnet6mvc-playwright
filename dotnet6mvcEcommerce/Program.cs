using dotnet6mvcEcommerce.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.把服務添加到LOC容器
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));//添加SQLContext服務

builder.Services.AddDatabaseDeveloperPageExceptionFilter();//資料庫開發異常過濾器

//identity身分驗證框架(CRUD)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()//啟用角色管理
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();//創建

// Configure the HTTP request pipeline.配置請求管道
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();//處理遷移的請求
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//順序要正確不能隨便放
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();//請求進來判斷是RazorPage還是MVC

app.UseAuthentication();//身分認證
app.UseAuthorization();//授權

//      /表示默認路由，home, home/index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
