using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Integracion;

namespace LogicaDifusa
{
    /// <summary>
    /// Clase para el metodo del centroide.
    /// </summary>
    public static class Centroide
    {
        /// <summary>
        /// Realiza el metodo del centroide, devolviendo el numero defuzzificado.
        /// Recibe el conjunto difuso resultante de la agregacion.
        /// </summary>
        /// <param name="agregacion"></param>
        /// <returns></returns>
        public static double Ejecutar(List<ValorLinguistico> agregacion)
        {
            double resultado = 0, numerador = 0, denominador = 0;

            /* Las integrales de la formula del centroide se realizan para cada valor linguistico
               que conforman el resultado de la agregación. */
            foreach (ValorLinguistico valor in agregacion)
            {
                Console.WriteLine("Valor linguistico: " + valor.Nombre + " Valor corte: " + valor.Fp.ValorCorte);
                double a = valor.Fp.LimiteInferior();
                double b = valor.Fp.LimiteSuperior();
                double epsilon = 1e-8; // tolerancia al error.

                numerador += SimpsonIntegrator.Integrate(x => valor.Fp.GradoPertenencia(x) * x, a,
                    b, epsilon);

                denominador += SimpsonIntegrator.Integrate(x => valor.Fp.GradoPertenencia(x), a,
                    b, epsilon);
                Console.WriteLine("Numerador: " + numerador + " Denominador: " + denominador);
            }
            // Se divide el resultado de las integrales para obtener el centroide.
            resultado = numerador / denominador;
            Console.WriteLine("Defuzzificacion: " + resultado);
            return resultado;
        }
    }
}
