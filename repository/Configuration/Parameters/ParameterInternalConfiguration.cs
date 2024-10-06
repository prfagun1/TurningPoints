using entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace repository.Configuration
{

    public class ParameterInternalConfiguration : IEntityTypeConfiguration<ParameterInternal>
    {
        public void Configure(EntityTypeBuilder<ParameterInternal> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Name).IsRequired();
            builder.Property(e => e.Value).IsRequired();
        }
    }
}
