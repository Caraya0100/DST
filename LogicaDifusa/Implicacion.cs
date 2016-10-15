using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    /// <summary>
    /// Clase para la implicacion de la logica difusa.
    /// </summary>
    public class Implicacion
    {
        private ValorLinguistico resultado;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valor_linguistico"></param>
        /// <param name="fp"></param>
        public Implicacion(ValorLinguistico valorLinguistico)
        {
            Resultado = new ValorLinguistico(valorLinguistico.Nombre, valorLinguistico.Fp);
        }

        /// <summary>
        /// Realiza la implicacion, recibe el valor resultante de aplicar el operador
        /// a la regla evaluada.
        /// </summary>
        /// <param name="resultado_operador"></param>
        /// <returns></returns>
        public ValorLinguistico Realizar(double resultado_operador)
        {
            Resultado.CalcularGradoPertenencia(resultado_operador);

            return Resultado;
        }

        public ValorLinguistico Resultado
        {
            get { return resultado; }
            set { resultado = value; }
        }
    }
}
