using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using App.Models;
using App.Areas.HoSoHS.Models;

namespace App.Data
{
    public class DataDbContext : IdentityDbContext<AppUser>
    {
        
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

             foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<HoSoHS>(entity => {
                entity.HasKey( h => new { h.Id, h.HocSinhId, h.LopHocId});
                entity.HasIndex( h => h.Slug).IsUnique();
            });

            modelBuilder.Entity<LopHoc>( entity => {
                entity.HasIndex( l => l.Slug).IsUnique();
            });
        } 

        public DbSet<LopHoc> LopHocs { get; set; }
        public DbSet<HoSoHS> HoSoHs { get; set; }

    }
}