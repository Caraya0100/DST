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
    public static class Implicacion
    {
        /// <summary>
        /// Realiza la implicacion, recibe el valor resultante de aplicar el operador
        /// a la regla evaluada.
        /// </summary>
        /// <param name="resultadOperador"></param>
        /// <returns></returns>
        public static ValorLinguistico Ejecutar(double resultadOperador, ValorLinguistico valorLinguistico)
        {
            ValorLinguistico resultado = new ValorLinguistico(valorLinguistico.Nombre, valorLinguistico.Fp);
            // Obtenemos el grado de pertenencia y cortamos la funcion de pertenencia en ese grado.
            //resultado.CalcularGradoPertenencia(resultadOperador);
            resultado.Fp.CortarFuncion(resultadOperador);

            return resultado;
        }
    }
}
