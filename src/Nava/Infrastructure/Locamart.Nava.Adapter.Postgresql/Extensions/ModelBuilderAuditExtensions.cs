using Locamart.Dina;
using Locamart.Dina.ValueObjects;
using Locamart.Nava.Adapter.Postgresql.Converters;
using Microsoft.EntityFrameworkCore;

namespace Locamart.Nava.Adapter.Postgresql.Extensions;

public static class ModelBuilderAuditExtensions
{
    public static void ApplyAuditing(this ModelBuilder modelBuilder)
    {
        var auditableBase = typeof(AuditableEntity<>);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clr = entityType.ClrType;
            if (!IsSubclassOfRawGeneric(auditableBase, clr))
                continue;

            var entity = modelBuilder.Entity(clr);

            entity.Property(typeof(DateTime), nameof(AuditableEntity<object>.CreatedAt))
                .IsRequired();

            entity.Property(typeof(UserId), nameof(AuditableEntity<object>.CreatedBy))
                .HasConversion(new UserConverter())
                .IsRequired();

            entity.Property(typeof(DateTime?), nameof(AuditableEntity<object>.LastUpdatedAt));

            entity.Property(typeof(UserId), nameof(AuditableEntity<object>.UpdatedBy))
                .HasConversion(new NullableUserConverter())
                .IsRequired(false);
        }
    }

    private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
    {
        while (toCheck != null && toCheck != typeof(object))
        {
            var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
            if (generic == cur) return true;
            toCheck = toCheck.BaseType!;
        }
        return false;
    }
}
