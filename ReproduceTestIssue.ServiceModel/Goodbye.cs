using ServiceStack;

namespace ReproduceTestIssue.ServiceModel;

[Route("/goodbye/{Name}")]
public class Goodbye : IGet, IReturn<GoodbyeResponse>
{
    public required string Name { get; set; }
}

public class GoodbyeResponse
{
    public required string Result { get; set; }
}
