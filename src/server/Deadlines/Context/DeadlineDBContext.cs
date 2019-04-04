using System;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("appDBString") ?? throw new InvalidOperationException("Couldn't find the appDBString connection string"));
            }
        }

        public virtual DbSet<Models.Deadlines> Deadlines { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Models.Deadlines>(entity =>
            {
                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.DueDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
