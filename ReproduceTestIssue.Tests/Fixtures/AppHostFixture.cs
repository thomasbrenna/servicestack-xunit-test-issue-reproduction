using Funq;
using Microsoft.Extensions.Configuration;
using ReproduceTestIssue.ServiceInterface;
using ReproduceTestIssue.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using Testcontainers.MsSql;
using Xunit;

namespace ReproduceTestIssues.Tests.Fixtures;

public class AppHostFixture : AppSelfHostBase, IAsyncLifetime
{
    public string BaseUri { get { return "http://localhost:5001"; } }

    public IDbConnectionFactory? DbFactory { get; set; }

    private MsSqlContainer _container;

    public AppHostFixture() : base(nameof(AppHostFixture), typeof(MyHelloService).Assembly)
    {
        _container = new MsSqlBuilder()
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        Init();
        Start(BaseUri);

        var dbFactory = Resolve<IDbConnectionFactory>();
        using var db = await dbFactory.OpenAsync();

        db.CreateTable<PersistentHello>();
        db.CreateTable<PersistentGoodbye>();
    }

    public async Task DisposeAsync()
    {
        var dbFactory = Resolve<IDbConnectionFactory>();
        using var db = await dbFactory.OpenAsync();

        db.DropTable<PersistentHello>();
        db.DropTable<PersistentGoodbye>();

        await _container.StopAsync();

        Dispose();
    }

    public override void Configure(Container container)
    {
        JsConfig.Init(
            new ServiceStack.Text.Config
            {
                DateHandler = DateHandler.ISO8601,
                AlwaysUseUtc = false,
                TextCase = TextCase.CamelCase,
                ExcludeDefaultValues = false,
                IncludeNullValues = true
            }
        );

        container.AddTransient<IConfiguration>(_ =>
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<AppHostFixture>()
                .Build();
        });

        var dialectProvider = SqlServer2019Dialect.Provider;
        dialectProvider.StringSerializer = new JsonStringSerializer();
        dialectProvider.GetStringConverter().UseUnicode = true;

        var connectionString = _container.GetConnectionString();
        var dbFactory = new OrmLiteConnectionFactory(connectionString, dialectProvider);

        container.AddSingleton<IDbConnectionFactory>(dbFactory);

        container.AddTransient<MyGoodbyeService>();
        container.AddTransient<MyHelloService>();
    }
}
