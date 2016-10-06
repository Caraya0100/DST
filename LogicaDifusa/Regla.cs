using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    /// <summary>
    /// Clase para las reglas de la logica difusa.
    /// </summary>
    public class Regla
    {
        private string id;
        private string operador;
        private Dictionary<string, ValorLinguistico> antecendente;
        private Tuple<string, ValorLinguistico> consecuente;

        /// <summary>
        /// Constructor, recibe el operador de la regla (Y/O).
        /// </summary>
        /// <param name="antecedente"></param>
        /// <param name="consecuente"></param>
        public Regla(string operador)
        {
            Operador = operador;
        }

        /// <summary>
        /// Agrega una variable linguistica junto con su valor linguistico correspondiente 
        /// al antecedente de la regla.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="nombre_valor"></param>
        public void AgregarAntecendente(string nombre_variable, ValorLinguistico valor)
        {
            ValorLinguistico val = new ValorLinguistico(valor.Nombre, valor.Fp);
            Antecendente.Add(nombre_variable, val);
        }

        /// <summary>
        /// Agrega o reemplaza (en caso de ya existir) el consecuente de la regla.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="nombre_valor"></param>
        public void AgregarConsecuente(string nombre_variable, ValorLinguistico valor)
        {
            ValorLinguistico val = new ValorLinguistico(valor.Nombre, valor.Fp);
            Consecuente = new Tuple<string, ValorLinguistico>(nombre_variable, val);
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Operador
        {
            get { return operador; }
            set { operador = value; }
        }

        public Dictionary<string, ValorLinguistico> Antecendente
        {
            get { return antecendente; }
            set { antecendente = value; }
        }

        public Tuple<string, ValorLinguistico> Consecuente
        {
            get { return consecuente; }
            set { consecuente = value; }
        }
    }
}
