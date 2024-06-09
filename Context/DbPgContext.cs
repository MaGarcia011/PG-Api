using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PG_Api.Models.Entity;

namespace PG_Api.Context;

public partial class DbPgContext : DbContext
{
    public DbPgContext()
    {
    }

    public DbPgContext(DbContextOptions<DbPgContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.IdFood);

            entity.ToTable("food");

            entity.Property(e => e.IdFood).HasColumnName("id_food");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.ToTable("users");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pwd)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
