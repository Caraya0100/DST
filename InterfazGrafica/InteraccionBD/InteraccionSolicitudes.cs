using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace InterfazGrafica.InteraccionBD
{
    class InteraccionSolicitudes
    {
        AdminDesempeño datosSolicitud;

        public InteraccionSolicitudes()
        {
            IniciarComponentes();
        }

        public void IniciarComponentes()
        {
            datosSolicitud = new AdminDesempeño();
        }

        public void GeneraSolicitud(string rutJefe, int seccionActual, int seccionNueva, double capActual, double capNueva)
        {            
            datosSolicitud.InsertarSolicitudCambio
                (
                DateTime.Today.ToString("yyyy-MM-dd"),
                "EN_ESPERA",
                rutJefe,
                seccionActual,//seccionActual
                seccionNueva,//seccionNueva
                capActual,
                capNueva
                );
        }

        public List<Solicitud> ListaDeSolicitudes()
        {
            return datosSolicitud.ObtenerSolicitudesEspecificas("EN_ESPERA");
        }
    }
}
