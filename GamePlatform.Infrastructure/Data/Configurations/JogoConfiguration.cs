
using GamePlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GamePlatform.Infrastructure.Data.Configurations;

public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
{
    public void Configure(EntityTypeBuilder<Jogo> builder)
    {
        builder.ToTable("Jogo");

        builder.HasKey(j => j.Id);

        builder.Property(j => j.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(j => j.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");  

        builder.HasIndex(j => j.Titulo).IsUnique();
    }
}
