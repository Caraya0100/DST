using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDifusa;
using SistemaInferencia;

namespace DST
{
    public static class EvaluacionDifusa
    {
        /// <summary>
        /// Realiza una evaluacion con logica difusa. Los datos entregados deben tener 
        /// una importancia.
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="variables"></param>
        /// <param name="reglas"></param>
        /// <returns></returns>
        public static double Evaluacion(Dictionary<string, Tuple<double, double>> datos, List<VariableLinguistica> variables, Dictionary<string, string> reglas)
        {
            Inferencia inferencia = new Inferencia();

            foreach (VariableLinguistica variable in variables)
            {
                inferencia.AgregarVariable(variable);
            }

            foreach (KeyValuePair<string, string> regla in reglas)
            {
                inferencia.AgregarRegla(regla.Key, regla.Value);
            }

            return inferencia.Ejecutar(datos);
        }

        /// <summary>
        /// Realiza una evaluacion con logica difusa.
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="variables"></param>
        /// <param name="reglas"></param>
        /// <returns></returns>
        public static double Evaluacion(Dictionary<string, double> datos, List<VariableLinguistica> variables, Dictionary<string, string> reglas)
        {
            Inferencia inferencia = new Inferencia();

            foreach (VariableLinguistica variable in variables)
            {
                inferencia.AgregarVariable(variable);
            }

            foreach (KeyValuePair<string, string> regla in reglas)
            {
                inferencia.AgregarRegla(regla.Key, regla.Value);
            }

            return inferencia.Ejecutar(datos);
        }
    }
}
