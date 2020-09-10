using Microsoft.EntityFrameworkCore;
using stutor_core.Models.Sql;

namespace stutor_core.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
              : base(options)
        { }

        public DbSet<Timezone> Timezone { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Topic> Topic { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<TopicExpert> TopicExpert { get; set; }
        public DbSet<Expert> Expert { get; set; }
        public DbSet<OrderPasskey> OrderPasskey { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TopicExpert>()
                .HasKey(e => new { e.TopicId, e.ExpertId });
        }
    }
}