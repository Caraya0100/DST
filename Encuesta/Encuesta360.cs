using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// CLase para la encuesta de la evaluación de 360.
    /// </summary>
    public class Encuesta360
    {
        private Dictionary<string, Pregunta> preguntas; // <id_pregunta, pregunta>

        /// <summary>
        /// Constructor, inicializa una encuesta vacia.
        /// </summary>
        public Encuesta360()
        {
            preguntas = new Dictionary<string, Pregunta>();
        }

        /// <summary>
        /// Agrega una pregunta a la encuesta. Si el id de la pregunta 
        /// ya existe en la encuesta, la pregunta no se agregara.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pregunta"></param>
        /// <returns></returns>
        public bool AgregarPregunta(string id, Pregunta pregunta)
        {
            if (!Preguntas.ContainsKey(id))
            {
                Preguntas.Add(id, pregunta);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Elimina una pregunta a la encuesta.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pregunta"></param>
        /// <returns></returns>
        public bool EliminarPregunta(string id, Pregunta pregunta)
        {
            if (Preguntas.ContainsKey(id))
            {
                Preguntas.Remove(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Responde una pregunta de la encuesta.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="grado"></param>
        /// <param name="frecuencia"></param>
        /// <returns>Retorna el valor resultante de la respeusta. Si la pregunta no pudo 
        /// ser respondida devuelve -1.</returns>
        public double ResponderPregunta(string id, double grado, double frecuencia)
        {
            if (Preguntas.ContainsKey(id))
            {
                return Preguntas[id].Responder(grado, frecuencia);
            }

            return -1;
        }

        /// <summary>
        /// Devuelve un diccionario con las preguntas de cada componente.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<Pregunta>> PreguntasComponentes()
        {
            Dictionary<string, List<Pregunta>> preguntascomponentes = new Dictionary<string, List<Pregunta>>();

            foreach (KeyValuePair<string, Pregunta> pregunta in Preguntas)
            {
                List<string> componentes = pregunta.Value.Componentes;
                foreach (string componente in componentes)
                {
                    if (preguntascomponentes.ContainsKey(componente))
                    {
                        preguntascomponentes[componente].Add(pregunta.Value);
                    } else
                    {
                        // Si no existe el componente, lo agregamos y agregamos la pregunta.
                        List<Pregunta> ps = new List<Pregunta>();
                        ps.Add(pregunta.Value);
                        preguntascomponentes.Add(componente, ps);
                    }
                }
            }

            return preguntascomponentes;
        }

        public Dictionary<string, Pregunta> Preguntas
        {
            get { return preguntas; }
            set { preguntas = value; }
        }
    }
}
