using Microsoft.EntityFrameworkCore;

namespace Deadlines.Context
{
    public partial class DeadlineDBContext : DbContext
    {
        public DeadlineDBContext()
        {
        }

        public DeadlineDBContext(DbContextOptions<DeadlineDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Models.Deadlines> Deadlines { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Deadlines>(entity =>
            {
                entity.ToTable("deadlines");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("create_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(500)");

                entity.Property(e => e.DueDate)
                    .HasColumnName("due_date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });
        }
    }
}
