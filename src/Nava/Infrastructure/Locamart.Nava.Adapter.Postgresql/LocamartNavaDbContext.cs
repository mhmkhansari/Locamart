using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Adapter.Postgresql.Configurations;
using Locamart.Nava.Adapter.Postgresql.Extensions;
using Locamart.Nava.Domain.Entities.Cart;
using Locamart.Nava.Domain.Entities.Comment;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.Store;
using Locamart.Nava.Domain.Entities.StoreCategory;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;



namespace Locamart.Nava.Adapter.Postgresql;

public class LocamartNavaDbContext : DbContext
{
    private readonly ICurrentUser _currentUser;

    public DbSet<ProductEntity> Products => Set<ProductEntity>();
    public DbSet<StoreEntity> Stores => Set<StoreEntity>();
    public DbSet<StoreCategoryEntity> StoreCategories => Set<StoreCategoryEntity>();
    public DbSet<CommentEntity> Comments => Set<CommentEntity>();
    public DbSet<CommentAttachmentEntity> CommentAttachments => Set<CommentAttachmentEntity>();
    public DbSet<CartEntity> Carts => Set<CartEntity>();

    public LocamartNavaDbContext(DbContextOptions<LocamartNavaDbContext> options, ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("nava");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocamartNavaDbContext).Assembly);

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfiguration(new ProductConfiguration());

        modelBuilder.ApplyConfiguration(new StoreConfiguration());

        modelBuilder.ApplyConfiguration(new StoreCategoryConfiguration());

        modelBuilder.ApplyConfiguration(new CommentConfiguration());

        modelBuilder.ApplyConfiguration(new CommentAttachmentConfiguration());

        modelBuilder.ApplyConfiguration(new CartConfiguration());

        modelBuilder.ApplyAuditing();

        modelBuilder.AddTransactionalOutboxEntities();

        /*modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("OutboxMessage", schema: "nava_outbox");
        });

        modelBuilder.Entity<InboxState>(entity =>
        {
            entity.ToTable("InboxState", schema: "nava_outbox");
        });

        modelBuilder.Entity<OutboxState>(entity =>
        {
            entity.ToTable("OutboxState", schema: "nava_outbox");
        });*/



    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var e in ChangeTracker.Entries().Where(e => e.Entity is IAuditable))
        {
            var a = (IAuditable)e.Entity;
            if (e.State == EntityState.Added) a.SetCreated(DateTime.UtcNow, _currentUser.UserId);
            if (e.State == EntityState.Modified) a.SetUpdated(DateTime.UtcNow, _currentUser.UserId);
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
