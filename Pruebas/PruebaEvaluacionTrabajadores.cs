using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class PruebaEvaluacionTrabajadores
    {
        public PruebaEvaluacionTrabajadores()
        {
            AdminSeccion adminSeccion = new AdminSeccion();
            Dictionary<string, Seccion> secciones;
            List<Seccion> s = adminSeccion.ObtenerSecciones();
            secciones = new Dictionary<string, Seccion>();

            foreach (Seccion seccion in s)
            {
                secciones.Add(seccion.Nombre, seccion);
            }

            EvaluarTrabajadores(secciones["Cajas"]);
        }

        public void EvaluarTrabajadores(Seccion seccion)
        {
            EvaluacionCapacidad ec = new EvaluacionCapacidad();
            AdminTrabajador at = new AdminTrabajador();
            Perfil perfilSeccion = EvaluacionPerfil.Ejecutar(seccion.Perfil, seccion.IdSeccion);

            perfilSeccion.HB.Nombre = "HBS";
            perfilSeccion.HD.Nombre = "HDS";
            perfilSeccion.CF.Nombre = "CFS";

            Console.WriteLine("Sección: " + seccion.Nombre);
            Console.WriteLine("HB: " + perfilSeccion.HB.Puntaje + " HD: " + perfilSeccion.HD.Puntaje + " CF: " + perfilSeccion.CF.Puntaje);

            foreach (KeyValuePair<string, Trabajador> trabajador in seccion.Trabajadores)
            {
                Console.WriteLine(" Trabajador: " + trabajador.Value.Nombre + " Rut: " + trabajador.Value.Rut);

                Perfil perfil = at.ObtenerPerfilTrabajador(trabajador.Key);
                Perfil perfilTrabajador = ObtenerPerfilTrabajadorSeccion(perfil, seccion.Perfil);
                perfilTrabajador = EvaluacionPerfil.Ejecutar(perfilTrabajador, seccion.IdSeccion);

                perfilTrabajador.HB.Nombre = "HBT";
                perfilTrabajador.HD.Nombre = "HDT";
                perfilTrabajador.CF.Nombre = "CFT";

                Console.WriteLine("HB: " + perfilTrabajador.HB.Puntaje + " HD: " + perfilTrabajador.HD.Puntaje + " CF: " +  perfilTrabajador.CF.Puntaje);

                ec.Ejecutar(
                    perfilSeccion.HB,
                    perfilSeccion.HD,
                    perfilSeccion.CF,
                    perfilTrabajador.HB,
                    perfilTrabajador.HD,
                    perfilTrabajador.CF
                );
                Console.WriteLine(" Igualdad HB: " + ec.CompatibilidadHB + " Igualdad Hd: " + ec.CompatibilidadHD + " Igualdad CF: " + ec.CompatibilidadCF + " Capacidad: " + ec.Capaciddad);
            }

            Console.ReadKey();
        }

        public Perfil ObtenerPerfilTrabajadorSeccion(Perfil trabajador, Perfil seccion)
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
