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
        private int id;
        private string pregunta;
        //private Dictionary<string, double> alternativas; // <id, valor>
        private Dictionary<string, Alternativa> alternativas;
        private Tuple<double, double, double> respuesta360; // <grado, frecuencia, resultado>
        private double respuestaNormal;
        private List<string> componentes; // HB/HD/CF.
        private string tipo;

        /// <summary>
        /// Constructor, inicializa una pregunta vacia.
        /// </summary>
        public Pregunta()
        {
            //alternativas = new Dictionary<string, double>();
            alternativas = new Dictionary<string, Alternativa>();
            respuesta360 = new Tuple<double, double, double>(-1, -1, -1);
            respuestaNormal = -1;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pregunta"></param>
        /// <param name="alternativas"></param>
        public Pregunta(int id, string pregunta, Dictionary<string, Alternativa> alternativas, string tipo)
        {
            this.id = id;
            this.pregunta = pregunta;
            this.alternativas = alternativas;
            this.tipo = tipo;
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
                respuesta360 = new Tuple<double, double, double>(grado, frecuencia, resultado);
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

        public Dictionary<string, Alternativa> Alternativas
        {
            get { return alternativas; }
            set { alternativas = value; }
        }

        public Tuple<double, double, double> Respuesta360
        {
            get { return respuesta360; }
            set { respuesta360 = value; }
        }

        public double RespuestaNormal
        {
            get { return respuestaNormal; }
            set { respuestaNormal = value; }
        }

        public List<string> Componentes
        {
            get { return componentes; }
            set { componentes = value; }
        }

        public string Descripcion
        {
            get { return pregunta; }
            set { pregunta = value; }
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
