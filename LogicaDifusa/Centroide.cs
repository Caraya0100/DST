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
    public class Centroide
    {
        private double resultado;
        /// <summary>
        /// Constructor.
        /// </summary>
        public Centroide()
        {
            resultado = 0;
        }

        /// <summary>
        /// Realiza el metodo del centroide, devolviendo el numero defuzzificado.
        /// Recibe el conjunto difuso resultante de la agregacion.
        /// </summary>
        /// <param name="agregacion"></param>
        /// <returns></returns>
        public double Realizar(List<ValorLinguistico> agregacion)
        {
            double numerador = 0;
            double denominador = 0;

            /* Las integrales de la formula del centroide se realizan para cada valor linguistico 
               que conforman el resultado de la agregación. */
            foreach (ValorLinguistico valor in agregacion)
            {
                double a = valor.Fp.LimiteInferior();
                double b = valor.Fp.LimiteSuperior();
                double epsilon = 1e-8; // tolerancia al error.
                numerador += SimpsonIntegrator.Integrate(x => valor.Fp.GradoPertenencia(x) * x, a,
                    b, epsilon);
                denominador += SimpsonIntegrator.Integrate(x => valor.Fp.GradoPertenencia(x), a,
                    b, epsilon);
            }
            // Se divide el resultado de las integrales para obtener el centroide.
            Resultado = numerador / denominador;

            return Resultado;
        }

        public double Resultado
        {
            get { return resultado; }
            set { resultado = value; }
        }
    }
}
