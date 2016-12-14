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
        private Dictionary<string, ValorLinguistico> antecedente;
        private Tuple<string, ValorLinguistico> consecuente;
        private string texto;

        /// <summary>
        /// Constructor, instancia una regla vacia.
        /// </summary>
        public Regla()
        {
            id = "";
            antecedente = new Dictionary<string, ValorLinguistico>();
            texto = "";
            operador = "";
        }

        /// <summary>
        /// Constructor, inicia una regla con solo su id.
        /// </summary>
        public Regla(string id)
        {
            this.id = id;
            antecedente = new Dictionary<string, ValorLinguistico>();
            texto = "";
            operador = "";
        }

        /// <summary>
        /// Constructor, recibe el antecedente, el consecuente y el operador de la regla.
        /// </summary>
        /// <param name="antecedente"></param>
        /// <param name="consecuente"></param>
        /// <param name="operador"></param>
        public Regla(string id, Dictionary<string, ValorLinguistico> antecedente, Tuple<string, ValorLinguistico> consecuente, string operador)
        {
            this.id = id;
            Antecedente = new Dictionary<string, ValorLinguistico>();

            int contador = 0;
            this.texto = "Si";
            foreach (KeyValuePair<string, ValorLinguistico> actual in antecedente)
            {
                if (contador > 0)
                    this.texto += " " + operador;
                this.texto += " " + actual.Key + " es " + actual.Value.Nombre;
                AgregarAntecendente(actual.Key, actual.Value);
                contador += 1;
            }

            this.texto += " entonces ";
            if (consecuente != null)
            {
                this.texto += " " + consecuente.Item1 + " es " + consecuente.Item2.Nombre;
                AgregarConsecuente(consecuente.Item1, consecuente.Item2);
            }

            Operador = operador;
        }

        /// <summary>
        /// Constructor, recibe el antecedente, el consecuente y el operador de la regla.
        /// </summary>
        /// <param name="antecedente"></param>
        /// <param name="consecuente"></param>
        /// <param name="operador"></param>
        public Regla(Dictionary<string, ValorLinguistico> antecedente, Tuple<string, ValorLinguistico> consecuente, string operador)
        {
            Antecedente = new Dictionary<string, ValorLinguistico>();

            foreach (KeyValuePair<string, ValorLinguistico> actual in antecedente)
            {
                AgregarAntecendente(actual.Key, actual.Value);
            }

            if( consecuente != null )
            {
                AgregarConsecuente(consecuente.Item1, consecuente.Item2);
            }
            Operador = operador;
        }

        /// <summary>
        /// Agrega una variable linguistica junto con su valor linguistico correspondiente 
        /// al antecedente de la regla.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="nombre_valor"></param>
        public void AgregarAntecendente(string nombreVariable, ValorLinguistico valor)
        {
            //ValorLinguistico val = new ValorLinguistico(valor.Nombre, valor.Fp);
            Antecedente.Add(nombreVariable, valor);
        }

        /// <summary>
        /// Agrega o reemplaza (en caso de ya existir) el consecuente de la regla.
        /// </summary>
        /// <param name="variable"></param>
        /// <param name="nombre_valor"></param>
        public void AgregarConsecuente(string nombreVariable, ValorLinguistico valor)
        {
            ValorLinguistico val = new ValorLinguistico(valor.Nombre, valor.Fp);
            Consecuente = new Tuple<string, ValorLinguistico>(nombreVariable, val);
        }

        /// <summary>
        /// Evalua la regla regla.
        /// </summary>
        /// <returns></returns>
        public double EvaluarRegla()
        {
            List<double> valoresLinguisticos = new List<double>();
            double resultado = 0;

            foreach (KeyValuePair<string, ValorLinguistico> actual in Antecedente)
            {
                valoresLinguisticos.Add(actual.Value.GradoPertenencia);
            }

            if ( operador == "y" )
            {
                resultado = valoresLinguisticos.Min();
            } else if (operador == "o")
            {
                resultado = valoresLinguisticos.Max();
            }

            return resultado;
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Operador
        {
            get { return operador; }
            set { operador = value; }
        }

        public Dictionary<string, ValorLinguistico> Antecedente
        {
            get { return antecedente; }
            set { antecedente = value; }
        }

        public Tuple<string, ValorLinguistico> Consecuente
        {
            get { return consecuente; }
            set { consecuente = value; }
        }

        public string Texto
        {
            get { return texto; }
        }

        public override string ToString()
        {
            return this.texto;
        }
    }
}
