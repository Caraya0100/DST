using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    /// <summary>
    /// Clase para lod valores linguisticos de la logica difusa.
    /// </summary>
    public class ValorLinguistico
    {
        private string nombre;
        private FuncionPertenencia fp;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="fp"></param>
        public ValorLinguistico(string nombre, FuncionPertenencia fp)
        {
            Nombre = nombre;
            Fp = fp;
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public FuncionPertenencia Fp
        {
            get { return fp; }
            set { fp = value; }
        }
    }
}
