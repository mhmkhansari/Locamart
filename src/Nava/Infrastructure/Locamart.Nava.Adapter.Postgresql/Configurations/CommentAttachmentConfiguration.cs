using Locamart.Nava.Adapter.Postgresql.Converters;
using Locamart.Nava.Domain.Entities.Comment;
using Locamart.Nava.Domain.Entities.Comment.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Locamart.Nava.Adapter.Postgresql.Configurations;

public class CommentAttachmentConfiguration : IEntityTypeConfiguration<CommentAttachmentEntity>
{
    public void Configure(EntityTypeBuilder<CommentAttachmentEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => CommentAttachmentId.Create(value).Value)
            .HasColumnType("uuid")
            .IsRequired();

        builder.Property(x => x.CommentId)
            .HasConversion(
                id => id.Value,
                value => CommentId.Create(value).Value)
            .IsRequired();

        builder.Property(x => x.Url)
            .HasConversion(
                uri => uri.ToString(),
                value => new Uri(value))
            .IsRequired()
            .HasMaxLength(2000);

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

    }
}

