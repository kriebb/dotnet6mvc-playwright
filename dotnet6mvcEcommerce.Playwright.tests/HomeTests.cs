using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

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

        public async Task OnFailure()
        {

        }

        [Test]
        public async Task HomepageHasPlaywrightInTitleAndGetStartedLinkLinkingtoTheIntroPage()
        {
            
            await Page.GotoAsync(_webApplicationFactory.ServerAddress + "Home");


            // Expect a title "to contain" a substring.
            await Expect(Page).ToHaveTitleAsync(new Regex("Log in - dotnet6mvcEcommerce"));

            await Page.ScreenshotAsync(new PageScreenshotOptions() { Path= "C:\\git\\dotnet6mvc-playwright\\dotnet6mvcEcommerce.Playwright.tests\\bin\\Debug\\net6.0\\image.png" });
        }
    }
}

public class CustomWebApplicationFactory : WebApplicationFactory<dotnet6mvcEcommerce.Program>
{
    private IHost _host;

    protected override IHost CreateHost(IHostBuilder builder)
    {
        // Create the host for TestServer now before we  
        // modify the builder to use Kestrel instead.    
        var testHost = builder.Build();

        // Modify the host builder to use Kestrel instead  
        // of TestServer so we can listen on a real address.    

        builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

        // Create and start the Kestrel server before the test server,  
        // otherwise due to the way the deferred host builder works    
        // for minimal hosting, the server will not get "initialized    
        // enough" for the address it is listening on to be available.    
        // See https://github.com/dotnet/aspnetcore/issues/33846.    

        _host = builder.Build();
        _host.Start();

        // Extract the selected dynamic port out of the Kestrel server  
        // and assign it onto the client options for convenience so it    
        // "just works" as otherwise it'll be the default http://localhost    
        // URL, which won't route to the Kestrel-hosted HTTP server.     

        var server = _host.Services.GetRequiredService<IServer>();
        var addresses = server.Features.Get<IServerAddressesFeature>();

        ClientOptions.BaseAddress = addresses!.Addresses
            .Select(x => new Uri(x))
            .Last();

        // Return the host that uses TestServer, rather than the real one.  
        // Otherwise the internals will complain about the host's server    
        // not being an instance of the concrete type TestServer.    
        // See https://github.com/dotnet/aspnetcore/pull/34702.   

        testHost.Start();
        return testHost;
    }

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    private void EnsureServer()
    {
        if (_host is null)
        {
            // This forces WebApplicationFactory to bootstrap the server  
            using var _ = CreateDefaultClient();
        }
    }
}