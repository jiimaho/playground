namespace Disasters.Api.Db;

public class Audit
{
    public Guid Id { get; set; }
    public string TableName { get; set; }
    public string Action { get; set; }
    public DateTime DateTime { get; set; }
    public string KeyValues { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
}