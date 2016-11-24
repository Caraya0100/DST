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
            Componente HBS = new Componente("HBS", "Habilidades blandas de la seccion.", "General", 0.62, 0.2);
            Componente HDS = new Componente("HDS", "Habilidades duras de la seccion.", "General", 0.72, 0.5);
            Componente CFS = new Componente("CFS", "Caracteristicas fisicas de la seccion.", "General", 0.65, 0.1);
            Componente HBT = new Componente("HBT", "Habilidades blandas del trabajador.", "General", 0.62, 0.2);
            Componente HDT = new Componente("HDT", "Habilidades duras del trabajador.", "General", 0.72, 0.5);
            Componente CFT = new Componente("CFT", "Caracteristicas fisicas del trabajador.", "General", 0.65, 0.1);

            evaluacion.Ejecutar(HBS, HDS, CFS, HBT, HDT, CFT);

            Console.WriteLine("Compatibilidad HB: " + evaluacion.CompatibilidadHB * 100);
            Console.WriteLine("Compatibilidad HD: " + evaluacion.CompatibilidadHD * 100);
            Console.WriteLine("Compatibilidad CF: " + evaluacion.CompatibilidadCF * 100);
            Console.WriteLine("Capacidad: " + evaluacion.Capaciddad * 100);
            Console.ReadLine();
        }
    }
}
