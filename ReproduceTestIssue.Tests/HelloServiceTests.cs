using ReproduceTestIssue.ServiceModel;
using ReproduceTestIssue.ServiceModel.Types;
using ReproduceTestIssues.Tests.Fixtures;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using Xunit;

namespace ReproduceTestIssue.Tests;

public class HelloServiceTests : IClassFixture<AppHostFixture>
{
    private AppHostFixture _appHostFixture;

    public HelloServiceTests(AppHostFixture appHostFixture)
    {
        _appHostFixture = appHostFixture;
    }

    [Fact]
    public void Can_call_Hello_Service()
    {
        // given
        var client = new JsonServiceClient(_appHostFixture.BaseUri);
        var dbFactory = _appHostFixture.Resolve<IDbConnectionFactory>();

        // when
        var response = client.Get(new Hello { Name = "World" });

        // then
        Assert.Equal("Hello, World!", response.Result);

        using var db = dbFactory.Open();
        var myHello = db.Single<PersistentHello>(ph => ph.Hello == "Hello, World!");
        Assert.NotNull(myHello);
    }
}