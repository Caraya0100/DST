using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    class Program
    {
        static void Main(string[] args)
        {
            //new CreacionBD();

            //new PruebaInsercionReglas();

            new PruebaEvaluacionTrabajadores();

            //new PruebaAdminMatching().InsertarComponentesMatching();
            //new PruebaAdminMatching().InsertarComponentesSecciones();

            //InsertarCFTrabajadores();

            //new PruebaInferencia().PruebaPropina(3.0, 8.0, 1.0);

            //prueba.obtenerEmpresa();

            /*AdminBD prueba = new AdminBD();
            AdminUsuario consultasUsuario = new AdminUsuario();
            AdminSeccion consultasSeccion = new AdminSeccion();
            AdminTrabajador consultasTrabajador = new AdminTrabajador();

            
            prueba.InsertarEmpresa("lider milagro","8765432-1","la otra esquina");

            

            consultasUsuario.InsertarUsuario("Jose Martinez","17167145-4","1234","ADMINISTRADOR",true);
            consultasUsuario.InsertarUsuario("Jose Martinez","17163145-3","1234", "ADMINISTRADOR",false);
            consultasUsuario.InsertarUsuario("Cristan Araya","17263145-4","1234", "JEFE_SECCION",true);
            consultasUsuario.InsertarUsuario("Carlos Barraza", "15263145-4", "1234", "JEFE_SECCION", true);
            consultasUsuario.InsertarUsuario("Ivan Ibarra", "14263145-4", "1234", "JEFE_SECCION", true);
            


            consultasSeccion.InsertarSeccion("casa", "17263145-4");
            consultasSeccion.InsertarSeccion("electro", "15263145-4");
            consultasSeccion.InsertarSeccion("carniceria", "14263145-4");
            

            
            //new PruebaEvaluacionCapacidad();

            
           consultasTrabajador.InsertarTrabajador("Juan", "Perez", "Contreras", "21394189-5","1996-12-10",1, "M",true);
           consultasTrabajador.InsertarTrabajador("Juan", "Pizarro", "Contreras", "21394182-3", "1993-12-10", 1, "M", true);
           consultasTrabajador.InsertarTrabajador("Juan", "Maganha", "Contreras", "11394189-4", "1991-12-10", 1, "M", true);

           consultasTrabajador.InsertarTrabajador("Pedro", "Perez", "Contreras", "18628068-7", "1996-12-10", 2, "M", true);
           consultasTrabajador.InsertarTrabajador("Pedro", "Pizarro", "Contreras", "14628068-3", "1992-12-10", 2, "M", true);
           consultasTrabajador.InsertarTrabajador("Pedro", "Maganha", "Contreras", "13628068-4", "1990-12-10", 2, "M", true);

           consultasTrabajador.InsertarTrabajador("Pablo", "Perez", "Contreras", "12508974-5", "1980-12-10", 3, "M", true);
           consultasTrabajador.InsertarTrabajador("Pablo", "Pizarro", "Contreras", "22538974-3", "1986-12-10", 3, "M", true);
           consultasTrabajador.InsertarTrabajador("Pablo", "Maganha", "Contreras", "22208974-4", "1976-12-10", 3, "M", true);
           

            
            prueba.InsertarComponente("Responsabilidad","Esta es una descripcion de responsabilidad",
                "hb",true);
            prueba.InsertarComponente("Proactividad", "Esta es una descripcion de proactividad",
                "hb", true);
            prueba.InsertarComponente("Afabilidad", "Esta es una descripcion de afabilidad",
                "hb", true);

            prueba.InsertarComponente("Manejo de maquinas", "Esta es una descripcion de manejo de maquinas",
                "hd", true);
            prueba.InsertarComponente("Conocimiento computacional", "Esta es una descripcion de conocimiento computacional",
                "hd", true);
            prueba.InsertarComponente("Conocimientos matematicos", "Conocimientos matematicos",
                "hd", true);

            prueba.InsertarComponente("Presentacion personal", "Esta es una descripcion de Presentacion",
                "cf", true);
            prueba.InsertarComponente("Discapacidad", "Esta es una descripcion de Discapacidad",
                "cf", true);
            

            
            consultasSeccion.InsertarComponentePerfilSeccion(1, "Responsabilidad", 50, 30);
            consultasSeccion.InsertarComponentePerfilSeccion(1, "Proactividad", 50, 30);
            consultasSeccion.InsertarComponentePerfilSeccion(1, "Afabilidad", 50, 30);

            consultasSeccion.InsertarComponentePerfilSeccion(2, "Responsabilidad", 50, 30);
            consultasSeccion.InsertarComponentePerfilSeccion(2, "Proactividad", 40, 30);
            consultasSeccion.InsertarComponentePerfilSeccion(2, "Afabilidad", 50, 80);

            consultasSeccion.InsertarComponentePerfilSeccion(3, "Responsabilidad", 70, 30);
            consultasSeccion.InsertarComponentePerfilSeccion(3, "Proactividad", 20, 50);
            consultasSeccion.InsertarComponentePerfilSeccion(3, "Afabilidad", 50, 30);
            

            
            consultasTrabajador.InsertarComponentePerfilTrabajador("21394189-5", "Responsabilidad",40);
            consultasTrabajador.InsertarComponentePerfilTrabajador("21394189-5", "Proactividad", 50);
            consultasTrabajador.InsertarComponentePerfilTrabajador("21394189-5", "Afabilidad", 20);

            consultasTrabajador.InsertarComponentePerfilTrabajador("21394182-3", "Responsabilidad", 28);
            consultasTrabajador.InsertarComponentePerfilTrabajador("21394182-3", "Proactividad", 34);
            consultasTrabajador.InsertarComponentePerfilTrabajador("21394182-3", "Afabilidad", 55);*/


            //prueba.ObtenerIdSeccion( "14263145-4" );
            //prueba.ObtenerNombreSeccion( "14263145-4" );

            /*
            if( prueba.VerificarUsuario("17163145-3") == true )
            {
                Console.WriteLine("Usuario existe");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Usuario no existe");
                Console.ReadKey();
            }

            string pruebaConcatenacion = "";

            pruebaConcatenacion = "SI autoncontrol ES promedio Y responsabilidad ES alta Y ";
            //pruebaConcatenacion = pruebaConcatenacion + " a todos";

            Console.WriteLine("{0}",pruebaConcatenacion);
            Console.ReadKey();

            pruebaConcatenacion = pruebaConcatenacion.Remove( pruebaConcatenacion.Length - 3 );
            Console.WriteLine("{0}", pruebaConcatenacion);
            Console.ReadKey();
            */

            //AdminReglas pruebaAdminReglas = new AdminReglas();

            /*
            pruebaAdminReglas.GuardarRegla(1,"Y","Autocontrol","promedio","antecedente",1,"hb");
            pruebaAdminReglas.GuardarRegla(1, "Y", "Responsabilidad", "altas", "antecedente", 1, "hb");
            pruebaAdminReglas.GuardarRegla(1, "Y", "Habilidades Blandas", "altas", "antecedente", 1, "hb");

            pruebaAdminReglas.GuardarRegla(1, "Y", "Habilidades Blandas", "altas", "consecuente", 1, "general");
            */

            /*
            string reglaObtenida = pruebaAdminReglas.ObtenerRegla( 1 );
            Console.WriteLine("{0}", reglaObtenida);
            Console.ReadKey();

            prueba.ObtenerRutJefeSeccion( "electro" );
            */

            //prueba.ModificarPuntajePerfilSeccion(1,"Afabilidad",40);

            /*
            AdminTrabajador consultasTrabajador = new AdminTrabajador();*/
            /*AdminTrabajador consultasTrabajador = new AdminTrabajador();
            //AdminTrabajador consultasTrabajador = new AdminTrabajador();

            List<Trabajador> trabajadores = consultasTrabajador.ObtenerTrabajadoresEmpresa();

            foreach (Trabajador trabajador in trabajadores)
            {
                Console.WriteLine("{0} {1} {2}", trabajador.Nombre, trabajador.ApellidoPaterno, trabajador.ApellidoMaterno );
                Console.ReadKey();
            }

            //AdminSeccion consultasSeccion = new AdminSeccion();

            List<Seccion> secciones = consultasSeccion.ObtenerSecciones();

            foreach(Seccion seccion in secciones)
            {
                Console.WriteLine("{0} {1}", seccion.IdSeccion, seccion.Nombre );
                Console.ReadKey();
            }*/

            Console.ReadKey();
        }

        public static void InsertarCFTrabajadores()
        {
            AdminSeccion adminSeccion = new AdminSeccion();
            AdminTrabajador adminTrabajador = new AdminTrabajador();
            List<Seccion> secciones = adminSeccion.ObtenerSecciones();

            foreach (Seccion seccion in secciones)
            {
                Dictionary<string, Trabajador> trabajadores = adminTrabajador.ObtenerTrabajadoresSeccion(seccion.IdSeccion);

                foreach (KeyValuePair<string, Trabajador> trabajador in trabajadores)
                {
                    Random rnd = new Random();
                    double puntajeEdad = rnd.Next(18, 31);
                    double puntajePresentacion = rnd.Next(0, 100);

                    adminTrabajador.InsertarComponentePerfilTrabajador(trabajador.Value.Rut, "Edad", puntajeEdad);
                    adminTrabajador.InsertarComponentePerfilTrabajador(trabajador.Value.Rut, "Presentacion_personal", puntajePresentacion);
                    adminTrabajador.InsertarComponentePerfilTrabajador(trabajador.Value.Rut, "Discapacidad_fisica", 0.0);
                }
            }
        }
    }
}
