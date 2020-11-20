using System;
using System.Collections.Generic;
using System.Linq;
using DatabaseBenchmark.Data;
using DatabaseBenchmark.Modelos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DatabaseBenchmark.Negocio
{
    public class RegistrosRepository : IRegistrosRepository, IDisposable
    {
        private IDbContext context;
        private readonly object contextLock = new object();

        public RegistrosRepository(IDbContext context)
        {
            this.context = context;
        }

        public IDbContextTransaction BeginTransaction()
        {
            return this.context.GetDatabase().BeginTransaction();
        }

        public List<Registro> GetAllRegitros()
        {
            //lock (contextLock)
            //{
                return context.GetRegistros().ToList();
            //}
        }

        public int GetCountRegistros()
        {
            //lock (contextLock)
            //{
                return context.GetRegistros().Count();
            //}
        }

        public Registro GetRegistroById(int registroId)
        {
            //lock (contextLock)
            //{
                return context.GetRegistros().Find(registroId);
            //}
        }

        public void InsertRegistro(Registro registro)
        {
            //lock (contextLock)
            //{
                using (var _transaccion = this.context.GetDatabase().BeginTransaction())
                {
                    context.GetRegistros().Add(registro);
                    context.SaveChanges();
                    _transaccion.Commit();
                }
            //}
        }

        public void UpdateRegistro(Registro registro)
        {
            //lock (contextLock)
            //{
                using (var _transaccion = this.context.GetDatabase().BeginTransaction())
                {
                    context.GetEntry(registro).State = EntityState.Modified;
                    context.SaveChanges();
                    _transaccion.Commit();
                }
            //}
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
