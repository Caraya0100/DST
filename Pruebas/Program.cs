using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace Pruebas
{
    class Program
    {
        static void Main(string[] args)
        {
            //new CreacionBD();

            //new PruebaInferencia().PruebaPropina(3.0, 8.0, 1.0);
            AdminBD prueba = new AdminBD();
            //prueba.obtenerEmpresa();

            /*
            prueba.InsertarEmpresa("lider milagro","8765432-1","la otra esquina");

            
            prueba.InsertarUsuario("Jose Martinez","17167145-4","1234","ADMINISTRADOR",true);
            prueba.InsertarUsuario("Jose Martinez","17163145-3","1234", "ADMINISTRADOR",false);
            prueba.InsertarUsuario("Cristan Araya","17263145-4","1234", "JEFE_SECCION",true);
            prueba.InsertarUsuario("Carlos Barraza", "15263145-4", "1234", "JEFE_SECCION", true);
            prueba.InsertarUsuario("Ivan Ibarra", "14263145-4", "1234", "JEFE_SECCION", true);
            


            prueba.InsertarSeccion("casa", "descripcion seccion casa", "17263145-4");
            prueba.InsertarSeccion("electro", "descripcion seccion electro", "15263145-4");
            prueba.InsertarSeccion("carniceria", "descripcion seccion carniceria", "14263145-4");
            

            
            //new PruebaEvaluacionCapacidad();

            
           prueba.InsertarTrabajador("Juan", "Perez", "Contreras", "21394189-5","1996-12-10",1, "M",true);
           prueba.InsertarTrabajador("Juan", "Pizarro", "Contreras", "21394182-3", "1993-12-10", 1, "M", true);
           prueba.InsertarTrabajador("Juan", "Maganha", "Contreras", "11394189-4", "1991-12-10", 1, "M", true);

           prueba.InsertarTrabajador("Pedro", "Perez", "Contreras", "18628068-7", "1996-12-10", 2, "M", true);
           prueba.InsertarTrabajador("Pedro", "Pizarro", "Contreras", "14628068-3", "1992-12-10", 2, "M", true);
           prueba.InsertarTrabajador("Pedro", "Maganha", "Contreras", "13628068-4", "1990-12-10", 2, "M", true);

           prueba.InsertarTrabajador("Pablo", "Perez", "Contreras", "12508974-5", "1980-12-10", 3, "M", true);
           prueba.InsertarTrabajador("Pablo", "Pizarro", "Contreras", "22538974-3", "1986-12-10", 3, "M", true);
           prueba.InsertarTrabajador("Pablo", "Maganha", "Contreras", "22208974-4", "1976-12-10", 3, "M", true);
           

            
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
            

            
            prueba.InsertarComponentePerfilSeccion(1, "Responsabilidad", 50, 30);
            prueba.InsertarComponentePerfilSeccion(1, "Proactividad", 50, 30);
            prueba.InsertarComponentePerfilSeccion(1, "Afabilidad", 50, 30);

            prueba.InsertarComponentePerfilSeccion(2, "Responsabilidad", 50, 30);
            prueba.InsertarComponentePerfilSeccion(2, "Proactividad", 40, 30);
            prueba.InsertarComponentePerfilSeccion(2, "Afabilidad", 50, 80);

            prueba.InsertarComponentePerfilSeccion(3, "Responsabilidad", 70, 30);
            prueba.InsertarComponentePerfilSeccion(3, "Proactividad", 20, 50);
            prueba.InsertarComponentePerfilSeccion(3, "Afabilidad", 50, 30);
            

            
            prueba.InsertarComponentePerfilTrabajador("21394189-5", "Responsabilidad",40);
            prueba.InsertarComponentePerfilTrabajador("21394189-5", "Proactividad", 50);
            prueba.InsertarComponentePerfilTrabajador("21394189-5", "Afabilidad", 20);

            prueba.InsertarComponentePerfilTrabajador("21394182-3", "Responsabilidad", 28);
            prueba.InsertarComponentePerfilTrabajador("21394182-3", "Proactividad", 34);
            prueba.InsertarComponentePerfilTrabajador("21394182-3", "Afabilidad", 55);
            */

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

            prueba.ModificarPuntajePerfilSeccion(1,"Afabilidad",40);
        }
    }
}
