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
        /// <param name="seccion"></param>
        public void Capacidad(Seccion seccion)
        {
            EvaluacionCapacidad ec = new EvaluacionCapacidad();
            AdminTrabajador at = new AdminTrabajador();
            Perfil perfilSeccion = EvaluacionPerfil.Ejecutar(seccion.Perfil, seccion.IdSeccion);

            perfilSeccion.HB.Nombre = "HBS";
            perfilSeccion.HD.Nombre = "HDS";
            perfilSeccion.CF.Nombre = "CFS";

            Perfil perfil = at.ObtenerPerfilTrabajador(trabajador.Rut);
            Perfil perfilTrabajador = ObtenerPerfilTrabajadorSeccion(perfil, seccion.Perfil);
            perfilTrabajador = EvaluacionPerfil.Ejecutar(perfilTrabajador, seccion.IdSeccion);

            perfilTrabajador.HB.Nombre = "HBT";
            perfilTrabajador.HD.Nombre = "HDT";
            perfilTrabajador.CF.Nombre = "CFT";

            Console.WriteLine("HB: " + perfilTrabajador.HB.Puntaje + " HD: " + perfilTrabajador.HD.Puntaje + " CF: " + perfilTrabajador.CF.Puntaje);

            ec.Ejecutar(
                perfilSeccion.HB,
                perfilSeccion.HD,
                perfilSeccion.CF,
                perfilTrabajador.HB,
                perfilTrabajador.HD,
                perfilTrabajador.CF
            );
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
            }

            foreach (KeyValuePair<string, Componente> componente in seccion.Duras)
            {
                perfil.AgregarComponente(trabajador.Duras[componente.Key]);
            }

            foreach (KeyValuePair<string, Componente> componente in seccion.Fisicas)
            {
                perfil.AgregarComponente(trabajador.Fisicas[componente.Key]);
            }

            return perfil;
        }
    }
}
