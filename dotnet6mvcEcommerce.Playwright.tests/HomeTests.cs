using System.Text.RegularExpressions;
using System.Threading.Tasks;
using dotnet6mvcEcommerce.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace dotnet6mvcEcommerce.Playwright.tests
{



    [Parallelizable(ParallelScope.Self)]
    [TestFixture()]
    public class HomeTests : PageTest
    {


        private PlaywrightCompatibleWebApplicationFactory _webApplicationFactory;

        
        public override BrowserNewContextOptions ContextOptions()
        {
            BrowserNewContextOptions? options = base.ContextOptions();
            options ??= new()
            {
                IgnoreHTTPSErrors = true,
                
                
            };

            return options;
        }
        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _webApplicationFactory = new PlaywrightCompatibleWebApplicationFactory();
        }

        [SetUp]
        public async Task Setup()
        {
            // tests setup
        }

        [OneTimeTearDown]

        public async Task OneTimeTearDown()
        {
            _webApplicationFactory.Dispose();
        }

        [Test]
        public async Task WhenProvidingBaseUrl_ShouldRedirectToLoginPage()
        {


            await Page.GotoAsync(_webApplicationFactory.ServerAddress);
            //Should be redirected.
            await Expect(Page).ToHaveURLAsync(_webApplicationFactory.ServerAddress + "Identity/Account/Login?ReturnUrl=%2F");

        }
        [Test]
        public async Task WhenProvidingWrongCredentials_ShouldRespondWithInvalidLoginAttempt()
        {
            await Page.GotoAsync(_webApplicationFactory.ServerAddress);

            await Page.GetByLabel("Email").ClickAsync();

            await Page.GetByLabel("Email").FillAsync("test@test.be");

            await Page.GetByLabel("Email").PressAsync("Tab");

            await Page.GetByLabel("Password").FillAsync("ABc.123!");

            await Page.GetByRole(AriaRole.Button, new() { Name = "Log in" }).ClickAsync();

            await Expect(Page.GetByText("Invalid login attempt.")).ToBeVisibleAsync();



        }

        [Test, Explicit]
        public async Task WhenWeRegisterANewUser_WeShouldHaveAConfirmedAccountMessage()
        {
            using (var scope = _webApplicationFactory.Services.CreateAsyncScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var adminTest = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == "admin@test.be");
                if (adminTest != null)
                    dbContext.Users.Remove(adminTest);
            }

            await Page.GotoAsync(_webApplicationFactory.ServerAddress);

            await Expect(Page.GetByText("Thank you for confirming your email.")).Not.ToBeVisibleAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = "Register as a new user" }).ClickAsync();

            await Page.GetByLabel("Email").ClickAsync();

            await Page.GetByLabel("Email").FillAsync("admin@test.be");

            await Page.GetByLabel("Email").PressAsync("Tab");

            await Page.GetByLabel("Password", new() { Exact = true }).FillAsync("Abc.123!");

            await Page.GetByLabel("Password", new() { Exact = true }).PressAsync("Tab");

            await Page.GetByLabel("Confirm password").FillAsync("Abc.123!");

            await Page.GetByRole(AriaRole.Button, new() { Name = "Register" }).ClickAsync();

            await Page.GetByRole(AriaRole.Link, new() { Name = "Click here to confirm your account" }).ClickAsync();

            await Expect(Page.GetByText("Thank you for confirming your email.")).ToBeVisibleAsync();

        }
    }
}
