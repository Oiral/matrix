using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Matrix.Core.Extensions;
using Microsoft.Extensions.Configuration;

namespace Matrix.Test;

public class Startup
{
    public IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureServices(ConfigureServices);
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        // Add services required for testing
        services.AddMatrixDb("TestMatrixDb"); // Use a separate test database
        services.AddMapper();
        services.AddStandardServices();
    }
}
