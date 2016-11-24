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
            AdminBD prueba = new AdminBD("localhost", "root", "", "bddst");
            //prueba.obtenerEmpresa();
            //prueba.insertarEmpresa("lider milagro","8765432-1","la otra esquina");

            /*
            prueba.insertarUsuario("Jose Martinez","17167145-4","1234","ADMINISTRADOR",true);
            prueba.insertarUsuario("Jose Martinez","17163145-3","1234", "ADMINISTRADOR",false);
            prueba.insertarUsuario("Cristan Araya","17263145-4","1234", "JEFE_SECCION",true);
            prueba.insertarUsuario("Carlos Barraza", "15263145-4", "1234", "JEFE_SECCION", true);
            prueba.insertarUsuario("Ivan Ibarra", "14263145-4", "1234", "JEFE_SECCION", true);
            */


            /*prueba.insertarSeccion("casa", "17263145-4");
            prueba.insertarSeccion("electro", "15263145-4");
            prueba.insertarSeccion("carniceria", "14263145-4");*/

            //new PruebaEvaluacionCapacidad();

            /*
           prueba.insertarTrabajador("Juan", "Perez", "Contreras", "21394189-5","1996-12-10",1, "M",true);
           prueba.insertarTrabajador("Juan", "Pizarro", "Contreras", "21394182-3", "1993-12-10", 1, "M", true);
           prueba.insertarTrabajador("Juan", "Maganha", "Contreras", "11394189-4", "1991-12-10", 1, "M", true);

           prueba.insertarTrabajador("Pedro", "Perez", "Contreras", "18628068-7", "1996-12-10", 2, "M", true);
           prueba.insertarTrabajador("Pedro", "Pizarro", "Contreras", "14628068-3", "1992-12-10", 2, "M", true);
           prueba.insertarTrabajador("Pedro", "Maganha", "Contreras", "13628068-4", "1990-12-10", 2, "M", true);

           prueba.insertarTrabajador("Pablo", "Perez", "Contreras", "12508974-5", "1980-12-10", 3, "M", true);
           prueba.insertarTrabajador("Pablo", "Pizarro", "Contreras", "22538974-3", "1986-12-10", 3, "M", true);
           prueba.insertarTrabajador("Pablo", "Maganha", "Contreras", "22208974-4", "1976-12-10", 3, "M", true);
           */

            /*
            prueba.insertarComponente("Responsabilidad","Esta es una descripcion de responsabilidad",
                "hb",true);
            prueba.insertarComponente("Proactividad", "Esta es una descripcion de proactividad",
                "hb", true);
            prueba.insertarComponente("Afabilidad", "Esta es una descripcion de afabilidad",
                "hb", true);

            prueba.insertarComponente("Manejo de maquinas", "Esta es una descripcion de manejo de maquinas",
                "hd", true);
            prueba.insertarComponente("Conocimiento computacional", "Esta es una descripcion de conocimiento computacional",
                "hd", true);
            prueba.insertarComponente("Conocimientos matematicos", "Conocimientos matematicos",
                "hd", true);

            prueba.insertarComponente("Presentacion personal", "Esta es una descripcion de Presentacion",
                "cf", true);
            prueba.insertarComponente("Discapacidad", "Esta es una descripcion de Discapacidad",
                "cf", true);
            */

            /*
            prueba.insertarComponentePerfilSeccion(1, "Responsabilidad", 50, 30);
            prueba.insertarComponentePerfilSeccion(1, "Proactividad", 50, 30);
            prueba.insertarComponentePerfilSeccion(1, "Afabilidad", 50, 30);

            prueba.insertarComponentePerfilSeccion(2, "Responsabilidad", 50, 30);
            prueba.insertarComponentePerfilSeccion(2, "Proactividad", 40, 30);
            prueba.insertarComponentePerfilSeccion(2, "Afabilidad", 50, 80);

            prueba.insertarComponentePerfilSeccion(3, "Responsabilidad", 70, 30);
            prueba.insertarComponentePerfilSeccion(3, "Proactividad", 20, 50);
            prueba.insertarComponentePerfilSeccion(3, "Afabilidad", 50, 30);
            */

            /*
            prueba.insertarComponentePerfilTrabajador("21394189-5", "Responsabilidad",40);
            prueba.insertarComponentePerfilTrabajador("21394189-5", "Proactividad", 50);
            prueba.insertarComponentePerfilTrabajador("21394189-5", "Afabilidad", 20);

            prueba.insertarComponentePerfilTrabajador("21394182-3", "Responsabilidad", 28);
            prueba.insertarComponentePerfilTrabajador("21394182-3", "Proactividad", 34);
            prueba.insertarComponentePerfilTrabajador("21394182-3", "Afabilidad", 55);
            */

            prueba.ObtenerIdSeccion( "14263145-4" );
            prueba.ObtenerNombreSeccion( "14263145-4" );        }
    }
}
