﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase para evaluar el desempeño de una seccion.
    /// </summary>
    public static class EvaluacionDesempeno
    {
        /// <summary>
        /// Ejecuta la evaluacion del desempeño de la seccion.
        /// </summary>
        /// <param name="ventasActuales"></param>
        /// <param name="ventasAnterior"></param>
        /// <param name="ventasPlan"></param>
        /// <returns>Tuple (desempeño respecto al año anterior, desempeño respecto 
        /// al plan de ventas).</returns>
        public static Tuple<double, double> Ejecutar(double ventasActuales, double ventasAnterior, double ventasPlan)
        {
            double actualAnterior = (ventasActuales / ventasAnterior) * 100;
            actualAnterior = Math.Round(actualAnterior, 1);
            double actualPlan = (ventasActuales / ventasPlan) * 100;
            actualPlan = Math.Round(actualPlan, 1);

            return new Tuple<double, double>(actualAnterior, actualPlan);
        }

        /// <summary>
        /// Ejecuta la evaluacion del desempeño a traves de las respuestas de 
        /// los objetivos.
        /// </summary>
        /// <param name="respuestas"></param>
        /// <returns></returns>
        public static double EjecutarGqm(List<double> respuestas)
        {
            double desempeno = -1;
            double sum = 0;

            foreach (double respuesta in respuestas)
            {
                sum += respuesta;
            }

            desempeno = sum / respuestas.Count;

            return desempeno;
        }
    }
}
