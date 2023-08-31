using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;

namespace dotnet6mvcEcommerce.Playwright.tests
{



    [Parallelizable(ParallelScope.Self)]
    [TestFixture()]
    public class HomeTests : PageTest
    {
        private CustomWebApplicationFactory _webApplicationFactory;
        private HttpClient _client;

        private int _port = new Random().Next(44000, 45000);
        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            _webApplicationFactory = new CustomWebApplicationFactory()
            {
                
            };

            _webApplicationFactory.WithWebHostBuilder(configuration =>
            {
          
                configuration.ConfigureServices(configureServices =>
                {
                    //override any services, like api calls or backends
                });
            });

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
    }
}
