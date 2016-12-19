using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encuesta
{
    public struct DatosPregunta
    {
        public string Habilidad { get; set; }
        public string idHabilidad { get; set; }
        public string TipoHabilidad { get; set; }
        public string Pregunta { get; set; }
        public string TipoPregunta { get; set; }
        public int Id { get; set; }
        public string Respuesta360 { get; set; }
        public string Frecuencia { get; set; }
        public string RespuestaIngresoDatos { get; set; }
        public string RespuestaNormal { get; set; }

    }
}
