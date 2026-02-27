using Locamart.Dina;
using Locamart.Dina.Abstracts;
using Locamart.Nava.Adapter.Postgresql.Configurations;
using Locamart.Nava.Domain.Entities.Cart;
using Locamart.Nava.Domain.Entities.Comment;
using Locamart.Nava.Domain.Entities.Inventory;
using Locamart.Nava.Domain.Entities.Location;
using Locamart.Nava.Domain.Entities.Order;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.ProductCategory;
using Locamart.Nava.Domain.Entities.Store;
using Locamart.Nava.Domain.Entities.StoreCategory;
using Locamart.Nava.Domain.Entities.UserAddress;
using MassTransit;
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
    public DbSet<InventoryEntity> Inventories => Set<InventoryEntity>();
    public DbSet<ProductCategoryEntity> ProductCategories => Set<ProductCategoryEntity>();
    public DbSet<UserAddressEntity> UserAddresses => Set<UserAddressEntity>();
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<ProvinceEntity> Provinces => Set<ProvinceEntity>();
    public DbSet<CityEntity> Cities => Set<CityEntity>();
    public DbSet<LocaleEntity> Locales => Set<LocaleEntity>();

    public LocamartNavaDbContext(DbContextOptions<LocamartNavaDbContext> options, ICurrentUser currentUser)
        : base(options)
    {
        _currentUser = currentUser;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("nava");

        //modelBuilder.ApplyConfigurationsFromAssembly(typeof(LocamartNavaDbContext).Assembly);

        modelBuilder.Ignore<DomainEvent>();

        modelBuilder.ApplyConfiguration(new ProductConfiguration());

        modelBuilder.ApplyConfiguration(new StoreConfiguration());

        modelBuilder.ApplyConfiguration(new StoreCategoryConfiguration());

        modelBuilder.ApplyConfiguration(new CommentConfiguration());

        modelBuilder.ApplyConfiguration(new CommentAttachmentConfiguration());

        modelBuilder.ApplyConfiguration(new CartConfiguration());

        modelBuilder.ApplyConfiguration(new InventoryConfiguration());

        modelBuilder.ApplyConfiguration(new UserAddressEntityConfiguration());

        modelBuilder.ApplyConfiguration(new ProductCategoryConfiguration());

        modelBuilder.ApplyConfiguration(new OrderConfiguration());

        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());

        modelBuilder.ApplyConfiguration(new OrderPaymentConfiguration());

        modelBuilder.ApplyConfiguration(new ProvinceConfiguration());

        modelBuilder.ApplyConfiguration(new CityConfiguration());

        modelBuilder.ApplyConfiguration(new LocaleConfiguration());

        //modelBuilder.ApplyAuditing();

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
