using ServiceStack.DataAnnotations;

namespace ReproduceTestIssue.ServiceModel.Types;
public class PersistentGoodbye
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }
    public required string Goodbye { get; set; }
}
