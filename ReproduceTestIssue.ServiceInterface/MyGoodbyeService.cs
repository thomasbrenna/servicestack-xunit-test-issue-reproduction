using ReproduceTestIssue.ServiceModel;
using ReproduceTestIssue.ServiceModel.Types;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace ReproduceTestIssue.ServiceInterface;

public class MyGoodbyeService : Service
{
    private IDbConnectionFactory _dbFactory;

    public MyGoodbyeService(IDbConnectionFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public object Any(Goodbye request)
    {
        var message = $"Goodbye, {request.Name}!";

        var toStore = new PersistentGoodbye()
        {
            Goodbye = message,
        };

        using var db = _dbFactory.Open();
        db.Insert(toStore);

        return new GoodbyeResponse { Result = message };

    }
}