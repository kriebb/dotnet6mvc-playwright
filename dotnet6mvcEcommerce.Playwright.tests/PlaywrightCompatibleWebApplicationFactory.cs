using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

public class PlaywrightCompatibleWebApplicationFactory : WebApplicationFactory<dotnet6mvcEcommerce.Program>
{
    private IHost? _hostThatRunsKestrelImpl;
    private IHost? _hostThatRunsTestServer;

    /// <summary>
    /// Hack to ensure we can use the deferred way of capturing the program.cs Webhostbuilder without refactoring program.cs
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    protected override IHost CreateHost(IHostBuilder builder)
    {
        try
        {

            // Create the host for TestServer now before we  
            // modify the builder to use Kestrel instead.    
            _hostThatRunsTestServer = builder.Build();

            // Modify the host builder to use Kestrel instead  
            // of TestServer so we can listen on a real address.    

            builder.ConfigureWebHost(webHostBuilder => webHostBuilder.UseKestrel());

            // Create and start the Kestrel server before the test server,  
            // otherwise due to the way the deferred host builder works    
            // for minimal hosting, the server will not get "initialized    
            // enough" for the address it is listening on to be available.    
            // See https://github.com/dotnet/aspnetcore/issues/33846.    

            _hostThatRunsKestrelImpl = builder.Build();
            _hostThatRunsKestrelImpl.Start();

            // Extract the selected dynamic port out of the Kestrel server  
            // and assign it onto the client options for convenience so it    
            // "just works" as otherwise it'll be the default http://localhost    
            // URL, which won't route to the Kestrel-hosted HTTP server.     

            var server = _hostThatRunsKestrelImpl.Services.GetRequiredService<IServer>();
            var addresses = server.Features.Get<IServerAddressesFeature>();

            ClientOptions.BaseAddress = addresses!.Addresses
                .Select(x => new Uri(x))
                .Last();

            // Return the host that uses TestServer, rather than the real one.  
            // Otherwise the internals will complain about the host's server    
            // not being an instance of the concrete type TestServer.    
            // See https://github.com/dotnet/aspnetcore/pull/34702.   

            _hostThatRunsTestServer.Start();
            return _hostThatRunsTestServer;

        }
        catch (Exception e)
        {
            _hostThatRunsKestrelImpl?.Dispose();
            _hostThatRunsTestServer?.Dispose();
            throw;
        }
    }

    public string ServerAddress
    {
        get
        {
            EnsureServer();
            return ClientOptions.BaseAddress.ToString();
        }
    }

    public override IServiceProvider Services
    {
        get
        {
            EnsureServer();
            return _hostThatRunsKestrelImpl?.Services;
        }
    }


    private void EnsureServer()
    {
        if (_hostThatRunsKestrelImpl is null)
        {
            // This forces WebApplicationFactory to bootstrap the server  
            using var _ = CreateDefaultClient();
        }
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        try
        {
            _hostThatRunsTestServer?.Dispose();

            if (_hostThatRunsTestServer != null)
            {
                await _hostThatRunsTestServer.StopAsync().ConfigureAwait(false);
                _hostThatRunsTestServer?.Dispose();
            }
        }
        catch (Exception e)
        {
        }

        try
        {
            _hostThatRunsKestrelImpl?.Dispose();

            if (_hostThatRunsKestrelImpl != null)
            {
                await _hostThatRunsKestrelImpl.StopAsync().ConfigureAwait(false);
                _hostThatRunsKestrelImpl?.Dispose();
            }
        }
        catch (Exception e)
        {
        }


    }
}