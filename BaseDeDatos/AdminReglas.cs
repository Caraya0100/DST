using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase para administrar las reglas de la base de datos.
    /// </summary>
    public static class AdminReglas
    {
        /// <summary>
        /// Devuelve una regla desde la base de datos a partir de su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ObtenerRegla(int id)
        {
            string regla = "";

            return regla;
        }

        /// <summary>
        /// Devuelve las reglas de inferencia de una seccion desde la base de datos.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ReglasSeccion(int idSeccion, string tipo)
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            return reglas;
        }
    }
}
