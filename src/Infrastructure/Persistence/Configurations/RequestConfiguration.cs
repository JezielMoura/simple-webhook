using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimpleWebhook.Domain.RequestAggregate;

namespace SimpleWebhook.Infrastructure.Persistence.Configurations;

public sealed class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("requests");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WebhookId)
            .IsRequired();

        builder.Property(x => x.Method)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.Headers)
            .HasColumnType("jsonb");

        builder.Property(x => x.Body)
            .HasColumnType("varchar(5000)")
            .IsRequired();

        builder.Property(x => x.QueryParameters)
            .IsRequired()
            .HasMaxLength(2048);

        builder.Property(x => x.SourceIp)
            .IsRequired()
            .HasMaxLength(60);

        builder.Property(x => x.ReceivedAt)
            .IsRequired();

        builder.HasIndex(x => x.WebhookId);
    }
}
