using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Locamart.Liam.Adapter.Postgresql;

public class LiamDbContext(DbContextOptions<LiamDbContext> options) : IdentityDbContext(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema("liam");

        builder.ApplyConfigurationsFromAssembly(typeof(LiamDbContext).Assembly);

        builder.AddTransactionalOutboxEntities();

        builder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("OutboxMessage", schema: "liam_outbox");
        });

        builder.Entity<InboxState>(entity =>
        {
            entity.ToTable("InboxState", schema: "liam_outbox");
        });

        builder.Entity<OutboxState>(entity =>
        {
            entity.ToTable("OutboxState", schema: "liam_outbox");
        });
    }
}

