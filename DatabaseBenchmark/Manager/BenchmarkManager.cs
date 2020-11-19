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
        enum DB
        {
            MySQL = 1,
            PostgreSQL = 2
        }

        private IRegistrosRepository mysqlRepository = new RegistrosRepository(new MySQLContext());
        private IRegistrosRepository pgRepository = new RegistrosRepository(new PGContext());
        private DB database;

        private Random rnd = new Random();

        #region "Métodos de MySQL"
        public long CalculaMySQLInsertion(int iNumRegistries, int iNumThreads)
        {
            this.database = DB.MySQL;
            return EjecutarIteraciones(iNumRegistries, iNumThreads, new Action(Insertion));
        }

        public long CalculaMySQLSelectPlusUpdate(int iNumRegistries, int iNumThreads)
        {
            this.database = DB.MySQL;
            return EjecutarIteraciones(iNumRegistries, iNumThreads, new Action(SelectPlusUpdate));
        }

        public long CalculaMySQLSelectPlusUpdatePlusInsertion(int iNumRegistries, int iNumThreads)
        {
            return CalculaMySQLSelectPlusUpdate(iNumRegistries, iNumThreads) + CalculaMySQLInsertion(iNumRegistries, iNumThreads);
        }
        #endregion

        #region "Métodos de PostgreSQL"

        public long CalculaPGInsertion(int iNumRegistries, int iNumThreads)
        {
            this.database = DB.PostgreSQL;
            return EjecutarIteraciones(iNumRegistries, iNumThreads, new Action(Insertion));
        }

        public long CalculaPGSelectPlusUpdate(int iNumRegistries, int iNumThreads)
        {
            this.database = DB.PostgreSQL;
            return EjecutarIteraciones(iNumRegistries, iNumThreads, new Action(SelectPlusUpdate));
        }
        public long CalculaPGSelectPlusUpdatePlusInsertion(int iNumRegistries, int iNumThreads)
        {
            return CalculaPGSelectPlusUpdate(iNumRegistries, iNumThreads) + CalculaPGInsertion(iNumRegistries, iNumThreads);
        }
        #endregion

        #region "Métodos insercion, seleccion y actualizacion"

        private void Insertion()
        {
            if (this.database == DB.MySQL)
                mysqlRepository.InsertRegistro(new Registro() { Valor = RandomString(25) });
            else
                pgRepository.InsertRegistro(new Registro() { Valor = RandomString(25) });
        }

        private void SelectPlusUpdate()
        {
            if (this.database == DB.MySQL)
            {
                int count = mysqlRepository.GetAllRegitros().Count;
                Registro registro = mysqlRepository.GetRegistroById(rnd.Next(1, count));
                registro.Valor = RandomString(25);
                mysqlRepository.UpdateRegistro(registro);
            }
            else
            {
                int count = pgRepository.GetAllRegitros().Count;
                Registro registro = pgRepository.GetRegistroById(rnd.Next(1, count));
                registro.Valor = RandomString(25);
                pgRepository.UpdateRegistro(registro);
            }
               
        }

        #endregion

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

        /// <summary>
        /// Funcion que ejecuta las iteraciones dependiendo de los parametos de hilos e iteraciones. 
        /// Ejecuta un Parallel.For que ejecuta las acciones internas en paralelo en hilos distintos.
        /// Ejecuta la función delegada pasada que puede ser insertar o seleccionar y actualizar
        /// </summary>
        /// <param name="iNumRegistries"></param>
        /// <param name="iNumThreads"></param>
        /// <param name="funcionDelegada"></param>
        /// <returns>Devuelve el tiempo empleado en milisegundos</returns>
        private long EjecutarIteraciones(int iNumRegistries, int iNumThreads, Action funcionDelegada)
        {
            //inicio de iNumThreads
            var watch = Stopwatch.StartNew();
            Parallel.For(0, iNumThreads, i => {
                //cada thread hará iNumRegistries iteraciones
                for (int r = 0; r < iNumRegistries; r++)
                {
                    funcionDelegada();
                }//end for iNumRegistries
            });//end for iNumThreads

            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

     }
}
