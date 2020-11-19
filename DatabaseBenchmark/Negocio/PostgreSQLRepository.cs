using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseBenchmark.Data;
using DatabaseBenchmark.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DatabaseBenchmark.Negocio
{
    public class PostgreSQLRepository : IRegistrosRepository, IDisposable
    {
        private PGContext context;
        private readonly object contextLock = new object();

        public PostgreSQLRepository(PGContext context)
        {
            this.context = context;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return this.context.Database.BeginTransaction();
        }

        public List<Registro> GetAllRegitros()
        {
            lock (contextLock)
            {
                return context.Registros.ToList();
            }
        }

        public Registro GetRegistroById(int registroId)
        {
            lock (contextLock)
            {
                return context.Registros.Find(registroId);
            }
        }

        public void InsertRegistro(Registro registro)
        {
            lock (contextLock)
            {
                using (var _transaccion = this.context.Database.BeginTransaction())
                {
                    context.Registros.Add(registro);
                    context.SaveChanges();
                    _transaccion.Commit();
                }
            }
        }

        public void UpdateRegistro(Registro registro)
        {
            lock (contextLock)
            {
                using (var _transaccion = this.context.Database.BeginTransaction())
                {
                    context.Entry(registro).State = EntityState.Modified;
                    context.SaveChanges();
                    _transaccion.Commit();
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
