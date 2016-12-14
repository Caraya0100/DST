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

        public double CapacidadGeneralTrabajador()
        {
            return datosDesempeno.ObtenerCapacidadGeneralRanking(idTrabajador)*0.01;
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
