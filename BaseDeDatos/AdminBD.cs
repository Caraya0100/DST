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

        public AdminBD (string servidor, string usuario, string password, string nombreBD)
        {
            bd = new BaseDeDatos();
            conn = bd.conectarBD(servidor, usuario, password, nombreBD);
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

        public void insertarEmpresa( string nombre, string razonSocial, string direccion)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO empresa (nombre,razonSocial,direccion) "
                + "VALUES('" + nombre + "','" + razonSocial + "','" + direccion + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarUsuario(string nombre, string rut, string clave, string tipoUsuario, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO usuarios (nombre,rut,clave,tipoUsuario,estado) " 
                + "VALUES('" + nombre + "','" + rut + "','" + clave + "','" + tipoUsuario + "',"
                + estado + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarSeccion( string nombre, string rutJefe )
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO secciones (nombre,rutJefe) VALUES('" + nombre + "','" + rutJefe 
                + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarTrabajador(string nombre, string rut, string fechaNacimiento, int idSeccion,
            bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO trabajadores (nombre,rut,fechaNacimiento,idSeccion,estado) "
                + "VALUES('" + nombre + "','" + rut + "'," + fechaNacimiento + "," + idSeccion.ToString()
                + "," + estado;
            cmd.ExecuteNonQuery();

            conn.Close();
        }

    }
}
