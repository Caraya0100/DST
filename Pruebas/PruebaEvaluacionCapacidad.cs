using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace Pruebas
{
    public class PruebaEvaluacionCapacidad
    {
        public PruebaEvaluacionCapacidad()
        {
            EvaluacionCapacidad evaluacion = new EvaluacionCapacidad();
            Componente HBS = new Componente("HBS", "Habilidades blandas de la seccion.", "General", 0.52, 0.8);
            Componente HDS = new Componente("HDS", "Habilidades duras de la seccion.", "General", 0.12, 0.5);
            Componente CFS = new Componente("CFS", "Caracteristicas fisicas de la seccion.", "General", 0.21, 0.1);
            Componente HBT = new Componente("HBT", "Habilidades blandas del trabajador.", "General", 0.52, 0.8);
            Componente HDT = new Componente("HDT", "Habilidades duras del trabajador.", "General", 0.12, 0.5);
            Componente CFT = new Componente("CFT", "Caracteristicas fisicas del trabajador.", "General", 0.21, 0.1);

            evaluacion.Ejecutar(HBS, HDS, CFS, HBT, HDT, CFT);

            Console.WriteLine("Compatibilidad HB: " + evaluacion.CompatibilidadHB);
            Console.WriteLine("Compatibilidad HD: " + evaluacion.CompatibilidadHD);
            Console.WriteLine("Compatibilidad CF: " + evaluacion.CompatibilidadCF);
            Console.WriteLine("Capacidad: " + evaluacion.Capaciddad);
            Console.ReadLine();
        }
    }
}
