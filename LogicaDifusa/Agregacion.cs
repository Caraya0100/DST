using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    /// <summary>
    /// Clase para la agregacion.
    /// </summary>
    public static class Agregacion
    {
        /// <summary>
        /// Realiza la agregacion, recibe los consecuentes agrupados por 
        /// los diferentes valores linguisticos.
        /// </summary>
        /// <param name="consecuentes"></param>
        /// <returns></returns>
        public static List<ValorLinguistico> Ejecutar(Dictionary<string, List<ValorLinguistico>> consecuentes)
        {
            List<ValorLinguistico> resultado = new List<ValorLinguistico>();
            foreach (KeyValuePair<string, List<ValorLinguistico>> consecuente in consecuentes)
            {
                List<double> gradosPertenencias = new List<double>();
                ValorLinguistico valorLinguistico = null;

                foreach (ValorLinguistico valor in consecuente.Value)
                {
                    // Obtenemos el valor linguistico en la primera iteracion.
                    if ( valorLinguistico == null )
                        valorLinguistico = new ValorLinguistico(valor.Nombre, valor.Fp);

                    gradosPertenencias.Add(valor.GradoPertenencia);
                }
                // Obtenemos el maximo y agregamos el valor linguistico al conjunto difuso resultante.
                if (valorLinguistico != null)
                {
                    valorLinguistico.GradoPertenencia = gradosPertenencias.Max();
                    resultado.Add(valorLinguistico);
                }
            }

            return resultado;
        }
    }
}
