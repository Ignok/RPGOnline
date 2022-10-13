using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RPGOnline.Infrastructure.Models
{
    public partial class RPGOnlineDbContext : DbContext
    {
        public RPGOnlineDbContext()
        {
        }

        public RPGOnlineDbContext(DbContextOptions<RPGOnlineDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Sample> Samples { get; set; } = null!;
        public virtual DbSet<Test> Tests { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sample>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.SampleName)
                    .HasMaxLength(60)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("Test");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.TestName)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("test_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
