using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class PruebaAdminMatching
    {
        AdminMatching am;

        public PruebaAdminMatching()
        {
            am = new AdminMatching();
        }

        public void InsertarComponentesMatching()
        {
            am.InsertarComponentes();
        }

        public void InsertarComponentesSecciones()
        {
            AdminSeccion adminSeccion = new AdminSeccion();
            Dictionary<string, Seccion> secciones = adminSeccion.ObtenerDiccionarioSecciones();

            // Atención al cliente
            InsertarComponentesSeccion(secciones["1"].IdSeccion, 0, 90, 0, 20, 0, 40);
            // Cajas
            InsertarComponentesSeccion(secciones["2"].IdSeccion, 0, 80, 0, 10, 0, 40);
            // Carniceria
            InsertarComponentesSeccion(secciones["3"].IdSeccion, 0, 80, 0, 50, 0, 10);
            // Panaderia
            InsertarComponentesSeccion(secciones["4"].IdSeccion, 0, 80, 0, 50, 0, 10);
            // Reponedores
            InsertarComponentesSeccion(secciones["5"].IdSeccion, 0, 90, 0, 20, 0, 10);
            // Electronica
            InsertarComponentesSeccion(secciones["6"].IdSeccion, 0, 90, 0, 50, 0, 10);
        }

        public void InsertarComponentesSeccion(int idSeccion, double puntajeHB, double importanciaHB, double puntajeHD, double importanciaHD, double puntajeCF, double importanciaCF)
        {
            am.InsertarComponente(idSeccion, "HB", puntajeHB, importanciaHB);
            am.InsertarComponente(idSeccion, "HD", puntajeHD, importanciaHD);
            am.InsertarComponente(idSeccion, "CF", puntajeCF, importanciaCF);
        }
    }
}
