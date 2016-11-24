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
            //AdminBD prueba = new AdminBD("localhost", "root", "", "bddst");
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

            new PruebaEvaluacionCapacidad();            
        }
    }
}
