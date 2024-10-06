
using entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace repository.Configuration
{

    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.HasKey(t => t.Id);

            builder
                .HasOne(t1 => t1.Story)
                .WithMany(t2 => t2.Chapter)
                .HasForeignKey(t1 => t1.StoryId);

        }
    }
}
