using System;
using DatabaseBenchmark.Data;
using DatabaseBenchmark.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DatabaseBenchmark.Data
{
    public partial class MySQLContext : DbContext, IDbContext
    {
        public MySQLContext()
        {
        }

        public MySQLContext(DbContextOptions<MySQLContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Registro> Registros { get; set; }

        public DbSet<Registro> GetRegistros()
        {
            return this.Registros;
        }

        public DatabaseFacade GetDatabase()
        {
            return this.Database;
        }

        public EntityEntry GetEntry(Registro r)
        {
            return this.Entry(r);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseMySql(configuration.GetConnectionString("MySQLDatabase"), Microsoft.EntityFrameworkCore.ServerVersion.FromString("8.0.22-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Registro>(entity =>
            {
                entity.ToTable("registros");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Valor)
                    .HasColumnType("varchar(45)")
                    .HasColumnName("valor")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
