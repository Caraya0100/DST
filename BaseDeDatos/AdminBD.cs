using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminBD
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminBD()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();
            
        }

        /*public void obtenerEmpresa()
        {
            string nombre = "";
            string razonSocial = "";
            string direccion = "";
            conn.Open();
            cmd.CommandText = "SELECT * FROM empresa;";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                nombre = consulta.GetString(0);
                razonSocial = consulta.GetString(1);
                direccion = consulta.GetString(2);
            }

            
            Console.WriteLine("Nombre: {0} Razon Social: {1} Direccion: {2}", nombre, razonSocial, direccion );
            Console.ReadKey();
            

            conn.Close();

        }*/

        /// <summary>
        /// Consulta para insertar una empresa. En nuestro caso es una sola.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="razonSocial"></param>
        /// <param name="direccion"></param>
        public void InsertarEmpresa( string nombre, string razonSocial, string direccion)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO empresa (nombre,razonSocial,direccion) "
                + "VALUES('" + nombre + "','" + razonSocial + "','" + direccion + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para insertar una nueva componente, puede ser hb,hd o cf
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        /// <param name="estado"></param>
        public void InsertarComponente(string nombre, string descripcion, string tipo, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfil (nombre,descripcion,tipo,estado) " 
                + "VALUES('" + nombre + "','" + descripcion + "','" + tipo + "'," + estado + ");" ;
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        
    }
}
