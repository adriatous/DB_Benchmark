using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatabaseBenchmark.Manager;

namespace DatabaseBenchmark.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BenchmarkController : ControllerBase
    {

        public BenchmarkManager manager = new BenchmarkManager();

        // GET: api/<BenchmarkController>
        [HttpGet]
        public string Get()
        {
            return "Api de comparativa de BD MySQL y PostgesSQL";
        }

        [Route("MySQLInsertion")]
        [HttpGet]
        public long MySQLInsertion(int registros, int hilos)
        {
            return manager.CalculaMySQLInsertion(registros, hilos);
        }

        [Route("MySQLSelectPlusUpdate")]
        [HttpGet]
        public long MySQLSelectPlusUpdate(int registros, int hilos)
        {
            return manager.CalculaMySQLSelectPlusUpdate(registros, hilos);
        }

        [Route("MySQLSelectPlusUpdatePlusInsertion")]
        [HttpGet]
        public long MySQLSelectPlusUpdatePlusInsertion(int registros, int hilos)
        {
            return manager.CalculaMySQLSelectPlusUpdatePlusInsertion(registros, hilos);
        }

        [Route("PGInsertion")]
        [HttpGet]
        public long PGInsertion(int registros, int hilos)
        {
            return manager.CalculaPGInsertion(registros, hilos);
        }

        [Route("PGSelectPlusUpdate")]
        [HttpGet]
        public long PGSelectPlusUpdate(int registros, int hilos)
        {
            return manager.CalculaPGSelectPlusUpdate(registros, hilos);
        }

        [Route("PGSelectPlusUpdatePlusInsertion")]
        [HttpGet]
        public long PGSelectPlusUpdatePlusInsertion(int registros, int hilos)
        {
            return manager.CalculaPGSelectPlusUpdatePlusInsertion(registros, hilos);
        }
    }
}
