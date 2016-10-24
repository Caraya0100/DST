using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    /// <summary>
    /// Clase para las variables linguisticas de la logica difusa.
    /// </summary>
    public class VariableLinguistica
    {
        private string nombre;
        private double min, max;
        private Dictionary<string, ValorLinguistico> valores;

        /// <summary>
        /// Constructor, recibe el nombre de la variable linguistica.
        /// Sse inicializara sin valores linguisticos.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public VariableLinguistica(string nombre, double min, double max)
        {
            Nombre = nombre;
            Max = max;
            Min = min;
            Valores = new Dictionary<string, ValorLinguistico>();
        }

        /// <summary>
        /// Constructor. Forma segura de "clonar" una variable linguistica.
        /// </summary>
        /// <param name="variable"></param>
        public VariableLinguistica(VariableLinguistica variable)
        {
            Nombre = variable.Nombre;
            Max = variable.Max;
            Min = variable.Min;
            // al inicializar los Valores se evita el transpaso de clases por parametro.
            Valores = new Dictionary<string, ValorLinguistico>();
            foreach(KeyValuePair<string, ValorLinguistico> valor in variable.Valores)
            {
                AgregarValorLinguistico(valor.Value);
            }
        }

        /// <summary>
        /// Agrega un valor linguistico a la variable linguistica.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns>Retorna true si se ha agrega el valor con exito, de lo contrario false.</returns>
        public bool AgregarValorLinguistico(string nombre, FuncionPertenencia fp)
        {
            // IMPLEMENTAR: COMPROBAR QUE EL VALOR LINGUISTICO NO SE SALGA DEL RANGO DE LA VARIABLE LINGUISTICA.
            ValorLinguistico vl = new ValorLinguistico(nombre, fp);
            valores.Add(vl.Nombre, vl);

            return true;
        }

        /// <summary>
        /// Agrega un valor linguistico a la variable linguistica.
        /// </summary>
        /// <param name="valor"></param>
        /// <returns>Retorna true si se ha agrega el valor con exito, de lo contrario false.</returns>
        public bool AgregarValorLinguistico(ValorLinguistico valor)
        {
            // IMPLEMENTAR: COMPROBAR QUE EL VALOR LINGUISTICO NO SE SALGA DEL RANGO DE LA VARIABLE LINGUISTICA.
            ValorLinguistico vl = new ValorLinguistico(valor.Nombre, valor.Fp);
            valores.Add(vl.Nombre, vl);
            return true;
        }

        /// <summary>
        /// Fuzzifica la variable, obteniendo el grado de pertenencia para cada valor linguistico.
        /// </summary>
        /// <param name="dato"></param>
        /// <returns></returns>
        public bool Fuzzificar(double dato)
        {
            if ( dato >= Min && dato <= Max )
            {
                foreach (KeyValuePair<string, ValorLinguistico> valor in Valores)
                {
                    valor.Value.CalcularGradoPertenencia(dato);
                }
                return true;
            }

            return false;
        }

        /// <summary>
        /// Elimina un valor linguistico de la variable linguistica.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns>Retorna true si se a eliminado el valor con exito, de lo contrario false.</returns>
        public bool EliminiarValorLinguistico(string nombre)
        {
            // IMPLEMENTAR: COMPROBAR QUE EL VALOR LINGUISTICO EXISTA.
            Valores.Remove(nombre);
            return true;
        }

        /// <summary>
        /// Retorna un valor linguistico segun el nombre.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public ValorLinguistico ValorLinguistico(string nombre)
        {
            if( Valores.ContainsKey(nombre) )
            {
                return Valores[nombre];
            }

            return null;
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public Dictionary<string, ValorLinguistico> Valores
        {
            get { return valores; }
            set { valores = value; }
        }

        public double Max
        {
            get { return max; }
            set { max = value; }
        }

        public double Min
        {
            get { return min; }
            set { min = value; }
        }
    }
}
