using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase para realizar las diferentes evaluaciones a un trabajador.
    /// </summary>
    public class EvaluacionTrabajador
    {
        private Trabajador trabajador;
        private Perfil perfilEvaluado;
        private EvaluacionCapacidad ec;

        /// <summary>
        /// Constructor, recibe el trabajador al que se evaluara.
        /// </summary>
        /// <param name="trabajador"></param>
        public EvaluacionTrabajador(Trabajador trabajador)
        {
            this.trabajador = trabajador;
        }

        /// <summary>
        /// Evalua la capacidad del trabajador respecto a una seccion.
        /// </summary>
        /// <param name="perfilSeccion"></param>
        /// <param name="idSeccion"></param>
        public EvaluacionCapacidad EvaluarCapacidad(Perfil perfilSeccion, int idSeccion)
        {
            ec = new EvaluacionCapacidad();
            AdminTrabajador at = new AdminTrabajador();

            perfilSeccion.HB.Nombre = "HBS";
            perfilSeccion.HD.Nombre = "HDS";
            perfilSeccion.CF.Nombre = "CFS";

            Perfil perfil = at.ObtenerPerfilTrabajador(trabajador.Rut);
            Perfil perfilTrabajador = ObtenerPerfilTrabajadorSeccion(perfil, perfilSeccion);
            perfilEvaluado = EvaluacionPerfil.Ejecutar(perfilTrabajador, idSeccion);

            perfilEvaluado.HB.Nombre = "HBT";
            perfilEvaluado.HD.Nombre = "HDT";
            perfilEvaluado.CF.Nombre = "CFT";

            //Console.WriteLine("HB: " + perfilEvaluado.HB.Puntaje + " HD: " + perfilEvaluado.HD.Puntaje + " CF: " + perfilEvaluado.CF.Puntaje);

            ec.Ejecutar(
                perfilSeccion.HB,
                perfilSeccion.HD,
                perfilSeccion.CF,
                perfilEvaluado.HB,
                perfilEvaluado.HD,
                perfilEvaluado.CF
            );

            return ec;
        }

        /// <summary>
        /// Obtiene el perfil de la seccion para un trabajador.
        /// </summary>
        /// <param name="trabajador"></param>
        /// <param name="seccion"></param>
        /// <returns></returns>
        private Perfil ObtenerPerfilTrabajadorSeccion(Perfil trabajador, Perfil seccion)
        {
            Perfil perfil = new Perfil();

            foreach (KeyValuePair<string, Componente> componente in seccion.Blandas)
            {
                perfil.AgregarComponente(trabajador.Blandas[componente.Key]);
                perfil.Blandas[componente.Key].Importancia = seccion.Blandas[componente.Key].Importancia;
            }

            foreach (KeyValuePair<string, Componente> componente in seccion.Duras)
            {
                perfil.AgregarComponente(trabajador.Duras[componente.Key]);
                perfil.Duras[componente.Key].Importancia = seccion.Duras[componente.Key].Importancia;
            }

            foreach (KeyValuePair<string, Componente> componente in seccion.Fisicas)
            {
                perfil.AgregarComponente(trabajador.Fisicas[componente.Key]);
                perfil.Fisicas[componente.Key].Importancia = seccion.Fisicas[componente.Key].Importancia;
            }

            return perfil;
        }

        public Perfil PerfilEvaluado
        {
            get { return perfilEvaluado; }
        }

        public EvaluacionCapacidad EvaluacionCapacidad
        {
            get { return ec; }
        }

        public double IgualdadHB
        {
            get { return ec.CompatibilidadHB; }
        }

        public double IgualdadHD
        {
            get { return ec.CompatibilidadHD; }
        }

        public double IgualdadCF
        {
            get { return ec.CompatibilidadCF; }
        }

        public double Capacidad
        {
            get { return ec.Capaciddad; }
        }
    }
}
