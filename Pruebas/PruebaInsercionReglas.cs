using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class PruebaInsercionReglas
    {
        public PruebaInsercionReglas()
        {
            AdminReglas ar = new AdminReglas();
            AdminSeccion adminSeccion = new AdminSeccion();
            Dictionary<string, Seccion> secciones;
            List<Seccion> s = adminSeccion.ObtenerSecciones();
            secciones = new Dictionary<string, Seccion>();

            foreach (Seccion seccion in s)
            {
                secciones.Add(seccion.Nombre, seccion);
            }

            Seccion seccionActual = secciones["Cajas"];

            string[] reglas = File.ReadAllLines(@"E:\College\2016\segundo_semestre\avanzada\proyecto\implementacion\reglas\reglas_cajas.txt");

            int id = ar.ObtenerUltimoID();

            foreach (string regla in reglas)
            {
                id += 1;
                string[] r = regla.Split(' ');
                string variableConsecuente = r[r.Length - 3];

                ar.InsertarRegla(
                    id.ToString(),
                    regla,
                    seccionActual.IdSeccion.ToString(),
                    variableConsecuente.ToLower()
                );
            }
        }
    }
}
