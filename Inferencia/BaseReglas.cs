using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inferencia
{
    /// <summary>
    /// Clase para las reglas de la inferencia
    /// </summary>
    public class BaseReglas
    {
        private Dictionary<string, string> reglas;

        public BaseReglas()
        {
            Reglas = new Dictionary<string, string>();
        }

        /// <summary>
        /// Agrega una regla a la base de reglas.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regla"></param>
        /// <returns></returns>
        public bool AgregarRegla(string id, string regla)
        {
            Reglas.Add(id, regla);

            return true;
        }

        public Dictionary<string, string> Reglas
        {
            get { return Reglas; }
            set { Reglas = value; }
        }
    }
}
