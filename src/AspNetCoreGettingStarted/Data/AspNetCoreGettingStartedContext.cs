﻿using AspNetCoreGettingStarted.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreGettingStarted.Data
{
    public interface IAspNetCoreGettingStartedContext
    {
        DbSet<Category> Categories { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Tenant> Tenants { get; set; }
        DbSet<User> Users { get; set; }
    }

    
    public class AspNetCoreGettingStartedContext: DbContext, IAspNetCoreGettingStartedContext
    {
        public AspNetCoreGettingStartedContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder) {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Tenant>(b => {
                b.Property(t => t.TenantId)
                .HasDefaultValueSql("newsequentialid()");
            });
        }
    }
}
