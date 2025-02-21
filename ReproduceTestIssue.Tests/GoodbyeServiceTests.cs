using ReproduceTestIssue.ServiceModel;
using ReproduceTestIssue.ServiceModel.Types;
using ReproduceTestIssues.Tests.Fixtures;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using Xunit;

namespace ReproduceTestIssue.Tests;

public class GoodbyeServiceTests : IClassFixture<AppHostFixture>
{
    private AppHostFixture _appHostFixture;

    public GoodbyeServiceTests(AppHostFixture appHostFixture)
    {
        _appHostFixture = appHostFixture;
    }

    [Fact]
    public void Can_call_Goodbye_Service()
    {
        // given
        var client = new JsonServiceClient(_appHostFixture.BaseUri);
        var dbFactory = _appHostFixture.Resolve<IDbConnectionFactory>();

        // when
        var response = client.Get(new Goodbye { Name = "Universe" });

        // then
        Assert.Equal("Goodbye, Universe!", response.Result);

        using var db = dbFactory.Open();
        var myHello = db.Single<PersistentGoodbye>(ph => ph.Goodbye == "Goodbye, Universe!");
        Assert.NotNull(myHello);
    }
}