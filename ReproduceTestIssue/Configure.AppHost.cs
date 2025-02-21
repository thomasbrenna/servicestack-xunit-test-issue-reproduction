[assembly: HostingStartup(typeof(ReproduceTestIssue.AppHost))]

namespace ReproduceTestIssue;

public class AppHost() : AppHostBase("ReproduceTestIssue"), IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            // Configure ASP.NET Core IOC Dependencies
        });

    public override void Configure()
    {
        // Configure ServiceStack, Run custom logic after ASP.NET Core Startup
        SetConfig(new HostConfig {
        });
    }
}