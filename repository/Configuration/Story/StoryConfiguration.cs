using entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace repository.Configuration
{

    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.RecommendedAge).IsRequired();
            builder.Property(e => e.Description).IsRequired();
        }
    }
}
