using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Solicitud
    {
        private string fechaSolicitud;
        private string estadoSolicitud;
        private string rutSolicitud;
        private int idSeccionActual;
        private int idSeccionSolicitada;
        private double capacidadSeccionActual;
        private double capacidadNuevaSeccion;

        public Solicitud(string fechaSolicitud, string estadoSolicitud, string rutSolicitud, int idSeccionActual,
            int idSeccionSolicitada, double capacidadSeccionActual, double capacidadNuevaSeccion)
        {
            this.fechaSolicitud = fechaSolicitud;
            this.estadoSolicitud = estadoSolicitud;
            this.rutSolicitud = rutSolicitud;
            this.idSeccionActual = idSeccionActual;
            this.idSeccionSolicitada = idSeccionSolicitada;
            this.capacidadSeccionActual = capacidadSeccionActual;
            this.capacidadNuevaSeccion = capacidadNuevaSeccion;
        }

        public string FechaSolicitud
        {
            get { return fechaSolicitud; }
            set { fechaSolicitud = value; }
        }

        public string EstadoSolicitud
        {
            get { return estadoSolicitud; }
            set { estadoSolicitud = value; }
        }

        public string RutSolicitud
        {
            get { return rutSolicitud; }
            set { rutSolicitud = value; }
        }

        public int IdSeccionActual
        {
            get { return idSeccionActual; }
            set { idSeccionActual = value; }
        }

        public int IdSeccionSolicitada
        {
            get { return idSeccionSolicitada; }
            set { idSeccionSolicitada = value; }
        }

        public double CapacidadSeccionActual
        {
            get { return capacidadSeccionActual; }
            set { capacidadSeccionActual = value; }
        }

        public double CapacidadNuevaSeccion
        {
            get { return capacidadNuevaSeccion; }
            set { capacidadNuevaSeccion = value; }
        }
    }
}
