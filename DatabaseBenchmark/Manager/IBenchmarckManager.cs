using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseBenchmark.Manager
{
    interface IBenchmarckManager
    {
        public long CalculaMySQLInsertion(int iNumRegistries, int iNumThreads);
        public long CalculaMySQLSelectPlusUpdate(int iNumRegistries, int iNumThreads);
        public long CalculaMySQLSelectPlusUpdatePlusInsertion(int iNumRegistries, int iNumThreads);
        public long CalculaPGInsertion(int iNumRegistries, int iNumThreads);
        public long CalculaPGSelectPlusUpdate(int iNumRegistries, int iNumThreads);
        public long CalculaPGSelectPlusUpdatePlusInsertion(int iNumRegistries, int iNumThreads);

    }
}
