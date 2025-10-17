using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Comment;
using Locamart.Nava.Domain.Entities.Comment.ValueObjects;
using Locamart.Nava.Domain.Entities.Product;
using Locamart.Nava.Domain.Entities.Product.ValueObjects;
using Locamart.Nava.Domain.Entities.StoreCategory.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<CommentEntity>
{
    public void Configure(EntityTypeBuilder<CommentEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => CommentId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.BodyMarkdown)
            .IsRequired()
            .HasColumnName("BodyMarkdown")
            .HasMaxLength(5000);

        builder.Property(p => p.ParentId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                value => value.HasValue ? CommentId.Create(value.Value).Value : null)
            .HasColumnType("uuid")
            .IsRequired(false);

        builder.HasOne<ProductEntity>()
            .WithMany()
            .HasForeignKey(x => x.ProductId);

        builder.Property(p => p.ProductId)
            .HasConversion(
                id => id.Value,
                value => ProductId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.CreatedBy)
            .HasConversion(new UserConverter())
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(p => p.UpdatedBy)
            .HasConversion(new NullableUserConverter())
            .HasColumnType("uuid");

        builder.Property(p => p.DeletedBy)
            .HasConversion(new NullableUserConverter())
            .HasColumnType("uuid");

        builder.HasMany(c => c.Attachments)
            .WithOne()
            .HasForeignKey(a => a.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

