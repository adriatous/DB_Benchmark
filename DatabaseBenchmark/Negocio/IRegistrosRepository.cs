using DatabaseBenchmark.Modelos;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseBenchmark.Negocio
{
    interface IRegistrosRepository
    {
        public IDbContextTransaction BeginTransaction();
        public List<Registro> GetAllRegitros();
        public Registro GetRegistroById(int registroId);
        public void InsertRegistro(Registro registro);
        public void UpdateRegistro(Registro registro);


    }
}
