using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Disasters.Api.Db;

public class AuditEntry
{
    public EntityEntry EntityEntry { get; }
    
    public string TableName { get; set; }

    public string Action { get; set; }

    public Dictionary<string, object> KeyValues { get; set; } = new();
    
    public Dictionary<string, object> OldValues { get; set; } = new();
    
    public Dictionary<string, object> NewValues { get; set; } = new();

    public List<PropertyEntry> TemporaryProperties { get; set; } = new();
    
    public bool HasTemporaryProperties => TemporaryProperties.Any();

    public AuditEntry(EntityEntry entityEntry)
    {
        EntityEntry = entityEntry;
    }

    public Audit ToAudit()
    {
        var audit = new Audit
        {
            DateTime = DateTime.Now,
            TableName = TableName,
            KeyValues = JsonSerializer.Serialize(KeyValues),
            OldValues = OldValues.Count == 0 ? null : JsonSerializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : JsonSerializer.Serialize(NewValues),
            Action = Action
        };

        return audit;
    }
}