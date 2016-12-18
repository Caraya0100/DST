using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminMatching
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminMatching()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();

        }

        /// <summary>
        /// Inserta los componentes necesarios para el matching. Estos componentes 
        /// no son vistos por los usuarios.
        /// </summary>
        public void InsertarComponentes()
        {
            AdminPerfil ap = new AdminPerfil();
            ap.InsertarComponente("HB", "Habilidades blandas", "Representa el conjunto de las habilidades blandas.", "general", true);
            ap.InsertarComponente("HD", "Habilidades duras", "Representa el conjunto de las habilidades duras.", "general", true);
            ap.InsertarComponente("CF", "Caracteristicas fisicas", "Representa el conjunto de las caracteristicas fisicas.", "general", true);
        }

        /// <summary>
        /// Inserta los componentes necesarios para el matching en el perfil 
        /// de una sección.
        /// </summary>
        /// <param name="idSeccion"></param>
        public void InsertarComponente(int idSeccion, string idComponente, double puntaje, double importancia)
        {
            AdminSeccion adminSeccion = new AdminSeccion();

            adminSeccion.InsertarComponentePerfilSeccion(idSeccion, idComponente, puntaje, importancia);
        }
    }
}
