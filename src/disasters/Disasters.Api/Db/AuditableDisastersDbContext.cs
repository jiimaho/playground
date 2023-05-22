using Microsoft.EntityFrameworkCore;

namespace Disasters.Api.Db;

public abstract class AuditableDisastersDbContext : DbContext
{
    public DbSet<Audit> Audits { get; set; }
    
    public async Task<int> SaveChangesAsync(string username)
    {
        var auditEntries = OnBeforeSaveChanges();
        
        var result = await base.SaveChangesAsync();

        if (auditEntries != null || auditEntries.Count > 0)
        {
            await OnAfterSaveChanges(auditEntries);
        }

        return result;
    }

    private List<AuditEntry> OnBeforeSaveChanges()
    {
        var entries = ChangeTracker.Entries().Where(
            e => e.State == EntityState.Added 
                 || e.State == EntityState.Modified
                    || e.State == EntityState.Deleted);
        
        var auditEntries = new List<AuditEntry>();

        foreach (var entry in entries)
        {
            var auditEntry = new AuditEntry(entry)
            {
                TableName = entry.Metadata.GetTableName() ?? "UnknownTable",
                Action = entry.State.ToString()
            };
            auditEntries.Add(auditEntry);

            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }
                
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }
                
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;
                    case EntityState.Deleted:
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;
                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }
        
        foreach (var pendingAuditEntry in auditEntries.Where(q => q.HasTemporaryProperties == false))
        {
            Audits.Add(pendingAuditEntry.ToAudit());
        }
            
        return auditEntries.Where(q => q.HasTemporaryProperties).ToList();

        // Set common properties like ModifiedAt for example :)
        // var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
        //
        // var now = new SystemClock().UtcNow;
        //
        // foreach (var entityEntry in entries)
        // {
        //     
        // }
    }
    
    private Task OnAfterSaveChanges(List<AuditEntry> auditEntries)
    {
        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }

            Audits.Add(auditEntry.ToAudit());
        }

        return SaveChangesAsync();
    }
}