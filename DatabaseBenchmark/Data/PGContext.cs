using System;
using DatabaseBenchmark.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace DatabaseBenchmark.Data
{
    public partial class PGContext : DbContext, IDbContext
    {
        public PGContext()
        {
        }

        public PGContext(DbContextOptions<PGContext> options)
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

                optionsBuilder.UseNpgsql(configuration.GetConnectionString("PGDatabase"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Registro>(entity =>
            {
                entity.ToTable("registros");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Valor)
                    .HasMaxLength(45)
                    .HasColumnName("valor")
                    .IsFixedLength(true);
            });

            //modelBuilder.Entity<Registro>(entity =>
            //{
            //    entity.HasNoKey();

            //    entity.ToTable("registros");

            //    entity.Property(e => e.Id)
            //        .ValueGeneratedOnAdd()
            //        .HasColumnName("id")
            //        .UseIdentityAlwaysColumn();

            //    entity.Property(e => e.Valor)
            //        .HasMaxLength(45)
            //        .HasColumnName("valor")
            //        .IsFixedLength(true);
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
