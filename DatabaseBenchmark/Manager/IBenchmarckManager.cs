using DatabaseBenchmark.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseBenchmark.Manager
{
    interface IBenchmarckManager
    {
        public long CalcInsertion(int iNumRegistries, int iNumThreads, DatabaseType database);
        public long CalcSelectPlusUpdate(int iNumRegistries, int iNumThreads, DatabaseType database);
        public long CalcSelectPlusUpdatePlusInsertion(int iNumRegistries, int iNumThreads, DatabaseType database);
    }
}
