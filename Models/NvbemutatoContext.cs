using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace IN_bemutato.Models;

public partial class NvbemutatoContext : DbContext
{
    public NvbemutatoContext()
    {
    }

    public NvbemutatoContext(DbContextOptions<NvbemutatoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Meat> Meats { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;database=nvbemutato;uid=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.32-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_hungarian_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.ToTable("customers");

            entity.Property(e => e.CustomerId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.ContactNumber).HasMaxLength(15);
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.TotalOrders).HasColumnType("int(11)");
        });

        modelBuilder.Entity<Meat>(entity =>
        {
            entity.HasKey(e => e.MeatId).HasName("PRIMARY");

            entity.ToTable("meats");

            entity.Property(e => e.MeatId)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("MeatID");
            entity.Property(e => e.MeatType).HasMaxLength(20);
            entity.Property(e => e.PricePerKg).HasPrecision(5, 2);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
