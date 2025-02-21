using ReproduceTestIssue.ServiceModel;
using ReproduceTestIssue.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ReproduceTestIssue.ServiceInterface;

public class MyHelloService : Service
{
    private IDbConnectionFactory _dbFactory;

    public MyHelloService(IDbConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public object Any(Hello request)
    {
        var message = $"Hello, {request.Name}!";

        var toStore = new PersistentHello()
        {
            Hello = message,
        };

        using var db = _dbFactory.Open();
        db.Insert(toStore);

        return new HelloResponse { Result = message };
    }
}