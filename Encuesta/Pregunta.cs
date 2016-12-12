using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase para una pregunta de la encuesta, contiene las posibles respuestas.
    /// </summary>
    public class Pregunta
    {
        private string descripcion;
        //private Dictionary<string, double> alternativas; // <id, valor>
        private List<Alternativa> alternativas;
        private Tuple<double, double, double> respuesta; // <grado, frecuencia, resultado>
        private List<string> componentes; // HB/HD/CF.

        /// <summary>
        /// Constructor, inicializa una pregunta vacia.
        /// </summary>
        public Pregunta()
        {
            //alternativas = new Dictionary<string, double>();
            alternativas = new List<Alternativa>();
            respuesta = new Tuple<double, double, double>(-1, -1, -1);
        }

        /// <summary>
        /// Devuelve el nombre de la respuesta segun su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string NombreRespuesta(string id)
        {
            string[] partes = id.Split('_');
            string nombre = "";

            foreach (string parte in partes)
            {
                nombre += parte + " ";
            }

            nombre = nombre.Remove(nombre.Length - 1);

            return nombre;
        }

        /// <summary>
        /// Devuelve una respuesta segun su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /*public KeyValuePair<string, double> Alternativa(string id)
        {
            if (alternativas.ContainsKey(id))
            {
                return new KeyValuePair<string, double>(NombreRespuesta(id), alternativas[id]);
            } else
            {
                return new KeyValuePair<string, double>();
            }
        }*/

        /// <summary>
        /// Responde la pregunta, calculando el resultado final a partir 
        /// del grado y la frecuencia.
        /// </summary>
        /// <param name="grado"></param>
        /// <param name="frecuencia"></param>
        /// <returns></returns>
        public double Responder(double grado, double frecuencia)
        {
            double resultado = -1;
            if (frecuencia >= 0)
            {
                resultado = grado * frecuencia;
                Respuesta = new Tuple<double, double, double>(grado, frecuencia, resultado);
            } else
            {
                // Si la pregunta no tiene frecuencia.
                resultado = grado;
            }

            return resultado;
        }

        /*
        public Dictionary<string, double> Alternativas
        {
            get { return alternativas; }
            set { alternativas = value; }
        }*/

        public List<Alternativa> Alternativas
        {
            get { return alternativas; }
            set { alternativas = value; }
        }

        public Tuple<double, double, double> Respuesta
        {
            get { return respuesta; }
            set { respuesta = value; }
        }

        public List<string> Componentes
        {
            get { return componentes; }
            set { componentes = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
    }
}
