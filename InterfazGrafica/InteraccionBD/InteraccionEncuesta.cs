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

        public List<Encuesta.DatosPregunta> TodasLasPreguntas()
        {
            return datosEncuesta.ObtenerTodasLasPreguntas();
        }

        public List<Encuesta.DatosPregunta> PreguntasDelPerfil(int idSeccion)
        {
            return datosEncuesta.ObtenerPreguntasDelPerfil(idSeccion);
        }

        public List<Encuesta.DatosAlternativa> Alternativas(int idPregunta, string tipo)
        {
            return datosEncuesta.ObtenerAlternativas(idPregunta,tipo);
        }

        public double ValorAlternativa(int idPregunta, string alternativa)
        {
            return datosEncuesta.ObtenerValorAlternativa(idPregunta,alternativa);
        }

        public double ValorFrecuencia(int idPregunta, string alternativa)
        {
            return datosEncuesta.ObtenerValorFrecuencia(idPregunta, alternativa);
        }

        public void ActualizarEstadoEncuestados(string rutTrabajador, string rutEvaluador)
        {
            try
            {
                datosEncuesta.TrabajadorYaEvaluados(rutTrabajador, rutEvaluador);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR ACTUALIZAR ENCUESTADOS: "+e);
            }
        }
        public void AsignarRespuesta360(int idPregunta, string rutTrabajador, string rutEncuestado, string alternativa, string frecuencia, double valor)
        {
            string valorArreglado = "" + valor;
            try
            {
                datosEncuesta.AgregarRespuesta360(idPregunta, rutTrabajador, rutEncuestado, alternativa, frecuencia, valorArreglado.Replace(",","."));
            }
            catch(Exception e)
            {
                Console.WriteLine("ERROR ASIGNAR RESPUESTA"+e);
            }
        }

        public void AgregarRespuestaSeccion(int idSeccion,double puntaje, int idPregunta, string idHabilidad, string tipoHabilidad)
        {
            datosEncuesta.AgregarRespuestaSeccion(idSeccion,puntaje,idPregunta,idHabilidad,tipoHabilidad);
        }

        public double Puntaje(string habilidad)
        {
           return datosEncuesta.ObtenerPuntajeHabilidadSeccion(habilidad);
        }

        public string NombreHabiliadad
        {
            get { return pregunta; }
            set { pregunta = value; }
        }
    }
}
