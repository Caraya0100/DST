using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase para realizar el proceso de evaluacion de 360.
    /// </summary>
    public class Evaluacion360
    {
        Dictionary<string, Encuesta360> encuestas;
        private Dictionary<string, Dictionary<string, double>> resultadosEvaluadores; // <evaluador, <id_componente, resultado>>
        Dictionary<string, double> resultados; // <id_componente, resultado_evaluacion>
        Dictionary<string, List<double>> resultadosComponentes; // <id_componente, resultados_finales_evaluadores>

        /// <summary>
        /// Constructor, 
        /// </summary>
        /// <param name="evaluadores"></param>
        public Evaluacion360()
        {
            resultados = new Dictionary<string, double>();
            resultadosEvaluadores = new Dictionary<string, Dictionary<string, double>>();
            encuestas = new Dictionary<string, Encuesta360>();
            resultadosComponentes = new Dictionary<string, List<double>>();
        }

        /// <summary>
        /// Ejecuta la evaluacion de 360. Recibe un diccionario (evaluador, encuesta) con las preguntas respondidas por los evaluadores para cada competencia 
        /// </summary>
        /// <param name="encuestas"></param>
        /// <returns>Devuelve el resultado final de la evaluacion para cada componente 
        /// evaluado en las encuestas.</returns>
        private Dictionary<string, double> Ejecutar(Dictionary<string, Encuesta360> encuestas)
        {
            // Inicializamos denuevo las propiedades para asegurarnos de no generar conflictos.
            this.resultados = new Dictionary<string, double>();
            resultadosEvaluadores = new Dictionary<string, Dictionary<string, double>>();
            this.encuestas = new Dictionary<string, Encuesta360>();
            resultadosComponentes = new Dictionary<string, List<double>>();

            foreach (KeyValuePair<string, Encuesta360> encuesta in encuestas)
            {
                Dictionary<string, double> resultados = EvaluarComponentes(encuesta.Value.PreguntasComponentes());
                // Agregamos los resultados del evaluador para poder consultarlos posteriormente.
                resultadosEvaluadores.Add(encuesta.Key, resultados);
                /* Agregamos el resultado del actual evaluador para luego obtener el resultado final del componente */
                foreach (KeyValuePair<string, double> resultado in resultados)
                {
                    if (!resultadosComponentes.ContainsKey(resultado.Key))
                    {
                        resultadosComponentes.Add(resultado.Key, new List<double>());
                    }
                    resultadosComponentes[resultado.Key].Add(resultado.Value);
                }
            }
            // Finalmente calculamos el resultado final para cada componente.
            foreach (KeyValuePair<string, List<double>> componente in resultadosComponentes)
            {
                double resultadoFinal = 0;
                foreach (double resultado in componente.Value)
                {
                    resultadoFinal += resultado;
                }
                resultadoFinal = resultadoFinal / componente.Value.Count;
                Resultados.Add(componente.Key, resultadoFinal);
            }

            return Resultados;
        }

        /// <summary>
        /// Evalua los componentes a partir de las respuestas de un evaluador.
        /// </summary>
        /// <param name="componentes">Diccionario con: id_componente, lista preguntas 
        /// respondidas.</param>
        /// <returns></returns>
        private Dictionary<string, double> EvaluarComponentes(Dictionary<string, List<Pregunta>> componentes)
        {
            Dictionary<string, double> resultados = new Dictionary<string, double>();

            if (componentes.Count > 0)
            {
                foreach (KeyValuePair<string, List<Pregunta>> componente in componentes)
                {
                    double resultado = EvaluarComponente(componente.Value);
                    resultados.Add(componente.Key, resultado);
                }
            }

            return resultados;
        }

        /// <summary>
        /// Evalua un componente, a partir de las respuestas realizadas por los evaluadores 
        /// a las preguntas del componente.
        /// </summary>
        /// <param name="preguntas">Las preguntas respondidas para el componente de un 
        /// evaluador</param>
        /// <returns>Retorna el resultado de la evaluacion. Si no se pudo evaluar el 
        /// componente, retorna -1.</returns>
        private double EvaluarComponente(List<Pregunta> preguntas)
        {
            double resultado = -1;

            if (preguntas.Count > 0)
            {
                resultado = 0;
                foreach (Pregunta pregunta in preguntas)
                {
                    resultado += pregunta.Respuesta360.Item3; // Resultado respuesta.
                }

                resultado = resultado / preguntas.Count;
            }

            return resultado;
        }

        private Dictionary<string, Encuesta360> Encuestas
        {
            get { return encuestas; }
        }

        public Dictionary<string, double> Resultados
        {
            get { return resultados; }
            set { resultados = value; }
        }

        public Dictionary<string, Dictionary<string, double>> ResultadosEvaluadores
        {
            get { return resultadosEvaluadores; }
        }

        public Dictionary<string, List<double>> ResultadosComponentes
        {
            get { return resultadosComponentes; }
        }
    }
}
