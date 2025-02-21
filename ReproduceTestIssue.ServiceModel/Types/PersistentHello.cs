using ServiceStack.DataAnnotations;

namespace ReproduceTestIssue.ServiceModel.Types;
public class PersistentHello
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }
    public required string Hello { get; set; }
}
