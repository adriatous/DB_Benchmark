using DatabaseBenchmark.Modelos;
using DatabaseBenchmark.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using DatabaseBenchmark.Data;

namespace DatabaseBenchmark.Manager
{
    public class BenchmarkManager : IBenchmarckManager
    {
        private IRegistrosRepository mysqlRepository = new RegistrosRepository(new MySQLContext());
        private IRegistrosRepository pgRepository = new RegistrosRepository(new PGContext());
        private readonly object contextLock = new object();

        private Random rnd = new Random();

        #region "Cáculos"
        /// <summary>
        /// Calculo de tiempo de insercion en la base de datos indicada por parametro
        /// </summary>
        /// <param name="iNumRegistries"></param>
        /// <param name="iNumThreads"></param>
        /// <param name="database"></param>
        /// <returns>Tiempo empleado en milisegundos</returns>
        public long CalcInsertion(int iNumRegistries, int iNumThreads, DatabaseType database)
        {
            return EjecutarIteraciones(iNumRegistries, iNumThreads, database, new Action<DatabaseType, IRegistrosRepository>(Insertion));
        }

        /// <summary>
        /// Calculo de tiempo de seleccion de un elemento al azar y actualización del mismo
        /// en la base de datos indicada por parametro
        /// </summary>
        /// <param name="iNumRegistries"></param>
        /// <param name="iNumThreads"></param>
        /// <param name="database"></param>
        /// <returns>Tiempo empleado en milisegundos</returns>
        public long CalcSelectPlusUpdate(int iNumRegistries, int iNumThreads, DatabaseType database)
        {
            return EjecutarIteraciones(iNumRegistries, iNumThreads, database, new Action<DatabaseType, IRegistrosRepository>(SelectPlusUpdate));
        }

        /// <summary>
        /// Calculo de tiempo de inserción, seleccion de un elemento al azar y actualización de este ultimo
        /// en la base de datos indicada por parametro
        /// </summary>
        /// <param name="iNumRegistries"></param>
        /// <param name="iNumThreads"></param>
        /// <param name="database"></param>
        /// <returns>Tiempo empleado en milisegundos</returns>
        public long CalcSelectPlusUpdatePlusInsertion(int iNumRegistries, int iNumThreads, DatabaseType database)
        {
            return CalcSelectPlusUpdate(iNumRegistries, iNumThreads, database) + CalcInsertion(iNumRegistries, iNumThreads, database);
        }
        #endregion

        #region "Métodos inserción, selección y actualización"

        /// <summary>
        /// Ejecuta una insercion en la base de datos usando el repo pasado por parametro
        /// Uso el lock() porque no se pueden hacer llamadas concurrentes a una misma instancia dbcontext
        /// </summary>
        /// <param name="database"></param>
        /// <param name="repository"></param>
        private void Insertion(DatabaseType database, IRegistrosRepository repository)
        {
            lock (contextLock)
            {
                repository.InsertRegistro(new Registro() { Valor = RandomString(25) });
            }
        }

        /// <summary>
        /// Ejecuta una consulta de la cantidad de registros que hay en la tabla Registros
        /// De estos escoje uno al azar y ejecuta una carga completa del objeto en un nueva instancia Registro
        /// modifica el campo valor de la instancia y la guarda
        /// Uso el lock() porque no se pueden hacer llamadas concurrentes a una misma instancia dbcontext
        /// </summary>
        /// <param name="database"></param>
        /// <param name="repository"></param>
        private void SelectPlusUpdate(DatabaseType database, IRegistrosRepository repository)
        {
            lock (contextLock)
            {
                int count = repository.GetCountRegistros();
                var regRandom = repository.GetAllRegitros().Skip(rnd.Next(1, count)).Take(1).FirstOrDefault();

                Registro registro = repository.GetRegistroById(regRandom.Id);
                registro.Valor = RandomString(25);
                repository.UpdateRegistro(registro);
            }
        }

        #endregion

        #region "Hilos e iteración"
        /// <summary>
        /// Funcion que ejecuta las iteraciones dependiendo de los parametos de hilos e iteraciones. 
        /// Ejecuta un Parallel.For que ejecuta las acciones internas en paralelo en hilos distintos.
        /// Ejecuta la función delegada pasada que puede ser insertar o seleccionar y actualizar.
        /// Recibe el tipo de base de datos para pasar por parametro el repositorio con el dbcontext cotrrespondiente.
        /// <param name="iNumRegistries"></param>
        /// <param name="iNumThreads"></param>
        /// <param name="database"></param>
        /// <param name="funcionDelegada"></param>
        /// <returns></returns>
        private long EjecutarIteraciones(int iNumRegistries, int iNumThreads, DatabaseType database, Action<DatabaseType, IRegistrosRepository> funcionDelegada)
        {
            if (!(iNumRegistries > 0 && iNumThreads > 0))
                return -1;

            IRegistrosRepository bdRepository = (database == DatabaseType.MySQL) ? mysqlRepository : pgRepository;

            //inicio de iNumThreads
            var watch = Stopwatch.StartNew();
            //Parallel.For ejecuta cada iteración en paralelo y espera a que todos los hilos iniciados terminen
            Parallel.For(0, iNumThreads, i => {
                //cada thread hará iNumRegistries iteraciones
                for (int r = 0; r < iNumRegistries; r++)
                {
                    funcionDelegada(database, bdRepository);
                }//end for iNumRegistries
            });//end for iNumThreads

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        #endregion

        #region "Funciones auxiliar"

        /// <summary>
        /// Genera una cadena aleatoria de tamaño indicado
        /// </summary>
        /// <param name="size"></param>
        /// <returns>Cadena aleatoria</returns>        
        private string RandomString(int size)
        {
            var builder = new System.Text.StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = 'a';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)rnd.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return builder.ToString();
        }
        #endregion
    }
}
