﻿using ClientManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientManagementAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Founder> Founders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>()
            .Property(c => c.DateAdded)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Client>()
            .Property(c => c.DateUpdated)
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Founder>()
            .Property(f => f.DateAdded)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Founder>()
            .Property(f => f.DateUpdated)
            .ValueGeneratedOnAddOrUpdate()
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<Founder>()
            .HasOne(f => f.Client)
            .WithMany(c => c.Founders)
            .HasForeignKey(f => f.ClientINN);

        modelBuilder.Entity<Client>()
            .HasIndex(c => c.INN)
            .IsUnique();

        modelBuilder.Entity<Founder>()
            .HasIndex(f => f.INN)
            .IsUnique();
    }
}
