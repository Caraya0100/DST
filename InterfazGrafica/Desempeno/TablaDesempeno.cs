using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using DST;

namespace DST
{
    public class TablaDesempeno
    {
        private DataGrid tabla;

        /// <summary>
        /// Constructor, recibe la tabla a trabajar.
        /// </summary>
        /// <param name="tabla"></param>
        public TablaDesempeno(DataGrid tabla)
        {
            this.tabla = tabla;
        }

        /// <summary>
        /// Agrega los datos de las secciones a la tabla.
        /// </summary>
        /// <param name="secciones"></param>
        public void AgregarSecciones(List<Seccion> secciones) {
            foreach (Seccion seccion in secciones)
            {
                AgregarSeccion(seccion);
            }
        }

        /// <summary>
        /// Agrega los datos de una seccion a la tabla.
        /// </summary>
        /// <param name="secciones"></param>
        public void AgregarSeccion(Seccion seccion)
        {
            Tuple<double, double> desempenio = EvaluacionDesempeno.Ejecutar(seccion.VentasActuales, seccion.VentasAnioAnterior, seccion.VentasPlan);

            tabla.Items.Add(new DatosTablaDesempeno {
                Seccion = seccion.Nombre,
                Actuales = seccion.VentasActuales,
                Anterior = seccion.VentasAnioAnterior,
                Plan = seccion.VentasPlan,
                ActualAnterior = desempenio.Item1,
                ActualPlan = desempenio.Item2,
            });
        }
    }
}
