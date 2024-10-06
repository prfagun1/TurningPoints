using entities;
using Microsoft.EntityFrameworkCore;
using repository.Configuration;


namespace repository.Context
{
    public class TurningPointsContext : DbContext
    {
        public TurningPointsContext(DbContextOptions<TurningPointsContext> options) : base(options) { }

        public virtual DbSet<ParameterInternal> ParameterInternal { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Login> Login { get; set; }


        public virtual DbSet<Story> Story { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ParameterInternalConfiguration());
            modelBuilder.ApplyConfiguration(new LogConfiguration());
            modelBuilder.ApplyConfiguration(new LoginConfiguration());

            modelBuilder.ApplyConfiguration(new StoryConfiguration());
            modelBuilder.ApplyConfiguration(new ChapterConfiguration());

        }
    }

}
