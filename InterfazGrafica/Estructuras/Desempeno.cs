using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Estructura para los datos de la tabla del resumen del desempeño 
    /// de las secciones.
    /// </summary>
    public struct DatosTablaDesempeno
    {
        public string Seccion { set; get; }
        public double Actuales { set; get; }
        public double Anterior { set; get; }
        public double Plan { set; get; }
        // Desempeño de la seccion respecto al año anterior.
        public double ActualAnterior { set; get; }
        // Desempeño de la seccion respecto al plan.
        public double ActualPlan { set; get; }
    }
}
