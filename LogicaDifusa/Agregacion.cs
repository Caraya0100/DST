using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    class Agregacion
    {
        private List<ValorLinguistico> resultado;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="consecuentes"></param>
        public Agregacion()
        {
            Resultado = new List<ValorLinguistico>();
        }

        /// <summary>
        /// Realiza la agregacion, recibe los consecuentes agrupados por 
        /// los diferentes valores linguisticos.
        /// </summary>
        /// <param name="consecuentes"></param>
        /// <returns></returns>
        public List<ValorLinguistico> Realizar(Dictionary<string, List<ValorLinguistico>> consecuentes)
        {
            foreach (KeyValuePair<string, List<ValorLinguistico>> consecuente in consecuentes)
            {
                List<double> gradosPertenencias = new List<double>();
                ValorLinguistico valorLinguistico = null;

                foreach (ValorLinguistico valor in consecuente.Value)
                {
                    // obtenemos el valor linguistico en la primera iteracion.
                    if ( valorLinguistico != null )
                        valorLinguistico = new ValorLinguistico(valor.Nombre, valor.Fp);
                    gradosPertenencias.Add(valor.GradoPertenencia);
                }
                // obtenemos el maximo y agregamos el valor linguistico al conjunto difuso resultante.
                if (valorLinguistico != null)
                {
                    valorLinguistico.GradoPertenencia = gradosPertenencias.Max();
                    Resultado.Add(valorLinguistico);
                }
            }

            return Resultado;
        }

        public List<ValorLinguistico> Resultado
        {
            get { return resultado; }
            set { resultado = value; }
        }
    }
}
