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
        private Dictionary<string, Pregunta> preguntas;

        public Encuesta360()
        {
            preguntas = new Dictionary<string, Pregunta>();
        }
    }
}
