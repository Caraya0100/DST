using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encuesta
{
    public struct DatosAlternativa
    {
        public int idPregunta { get; set; }
        public string Alternativa { get; set; }
        public double Valor { get; set; }
        public string Tipo { get; set; }
    }
}
