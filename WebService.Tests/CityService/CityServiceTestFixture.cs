using WebService.Service.Interface;
using Microsoft.Extensions.DependencyInjection;

namespace WebService.Tests.CityService;

public class CityServiceTestFixture
{
    public ServiceProvider ServiceProvider { get; private set; }

    public CityServiceTestFixture()
    {
        var services = new ServiceCollection();

        services.AddSingleton<ICityService, Service.CityService>();
        this.ServiceProvider = services.BuildServiceProvider();
    }
}
