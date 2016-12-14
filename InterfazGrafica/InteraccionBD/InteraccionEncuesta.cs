using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace InterfazGrafica.InteraccionBD
{
    class InteraccionEncuesta
    {
        AdminEncuesta datosEncuesta;
        private string pregunta;

        public InteraccionEncuesta()
        {
            IniciarComponentes();
        }

        private void IniciarComponentes()
        {
            datosEncuesta = new AdminEncuesta();
        }

        public List<string> Preguntas()
        {
            return datosEncuesta.ObtenerPreguntasPorHabilidad(pregunta);
        }

        public string NombreHabiliadad
        {
            get { return pregunta; }
            set { pregunta = value; }
        }
    }
}
