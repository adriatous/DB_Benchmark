﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DatabaseBenchmark.Manager;
using DatabaseBenchmark.Modelos;

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
            return manager.CalcInsertion(registros, hilos, DatabaseType.MySQL);
        }

        [Route("MySQLSelectPlusUpdate")]
        [HttpGet]
        public long MySQLSelectPlusUpdate(int registros, int hilos)
        {
            return manager.CalcSelectPlusUpdate(registros, hilos, DatabaseType.MySQL);
        }

        [Route("MySQLSelectPlusUpdatePlusInsertion")]
        [HttpGet]
        public long MySQLSelectPlusUpdatePlusInsertion(int registros, int hilos)
        {
            return manager.CalcSelectPlusUpdatePlusInsertion(registros, hilos, DatabaseType.MySQL);
        }

        [Route("PGInsertion")]
        [HttpGet]
        public long PGInsertion(int registros, int hilos)
        {
            return manager.CalcInsertion(registros, hilos, DatabaseType.PostgreSQL);
        }

        [Route("PGSelectPlusUpdate")]
        [HttpGet]
        public long PGSelectPlusUpdate(int registros, int hilos)
        {
            return manager.CalcSelectPlusUpdate(registros, hilos, DatabaseType.PostgreSQL);
        }

        [Route("PGSelectPlusUpdatePlusInsertion")]
        [HttpGet]
        public long PGSelectPlusUpdatePlusInsertion(int registros, int hilos)
        {
            return manager.CalcSelectPlusUpdatePlusInsertion(registros, hilos, DatabaseType.PostgreSQL);
        }
    }
}
