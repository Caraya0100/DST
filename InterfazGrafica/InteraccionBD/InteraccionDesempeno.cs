using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace InterfazGrafica.InteraccionBD
{
    class InteraccionDesempeno
    {

        AdminDesempeño datosDesempeno;
        private int idSeccion;
        private string idTrabajador;

        public InteraccionDesempeno()
        {
            IniciarComponentes();
        }

        private void IniciarComponentes()
        {
            datosDesempeno = new AdminDesempeño();
        }

        public List<string> TrabajadoresRanking()
        {
            
            return datosDesempeno.ObtenerRankingTrabajadoresSeccion(idSeccion);            
        }

        public double CapacidadGeneralTrabajadorRanking(int idSeccion, string idTrabajador)
        {
            return datosDesempeno.ObtenerCapacidadGeneralRanking(idTrabajador,idSeccion)*0.01;
        }

        public double CapacidadGeneralTrabajador()
        {
            return datosDesempeno.ObtenerCapacidadGeneral(idTrabajador) * 0.01;
        }

        public double CapacidadGeneralHB()
        {
            return datosDesempeno.ObtenerCapacidadHBRanking(idTrabajador);
        }

        public double CapacidadGeneralHD()
        {
            return datosDesempeno.ObtenerCapacidadHDRanking(idTrabajador);
        }

        public double CapacidadGeneralCF()
        {
            return datosDesempeno.ObtenerCapacidadCFRanking(idTrabajador);
        }

        public void ActualizacionSolicitud(string estado,int idActual, int idNueva, string rut)
        {
            datosDesempeno.CambiarEstadoSolicitud(estado,idActual,idNueva,rut);
        }

        public void ReubicarTrabajador(string rut, int idAnterior, int idActual, string fecha)
        {
            datosDesempeno.InsertarReubicacion(rut,idAnterior, idActual, DateTime.Now.ToString("yyyy-MM-dd"));
            datosDesempeno.ReubicarTrabajador(idActual, rut);
        }

        public Dictionary<string, double> CalcularPuntajesGeneralesTrabajador(string rut)
        {
            return datosDesempeno.CalcularHabilidadesGenerales(rut);
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }

        public string IdTrabajador
        {
            get { return idTrabajador; }
            set { idTrabajador = value; }
        }
    }
}
