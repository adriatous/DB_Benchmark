using DatabaseBenchmark.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseBenchmark.Data
{
    public interface IDbContext
    {
        public DbSet<Registro> GetRegistros();
        public DatabaseFacade GetDatabase();
        public EntityEntry GetEntry(Registro r);

        public int SaveChanges();
        public void Dispose();

    }

}
