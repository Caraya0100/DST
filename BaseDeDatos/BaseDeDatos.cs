using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.IO;

namespace DST
{
    public class BaseDeDatos
    {
        private MySqlConnection conn;
        private MySqlConnectionStringBuilder builder;
        private MySqlCommand cmd;
        MySqlDataReader consulta;

        public BaseDeDatos()
        {
            builder = new MySqlConnectionStringBuilder();
        }

        //public MySqlConnection conectarBD(string servidor, string usuario, string password, string nombreBD)
        public MySqlConnection conectarBD()
        {
            string[] lines = File.ReadAllLines("dataDST");
            string servidor = "localhost";
            string usuario = "";
            string password = "";
            string nombreBD = "bddst";

            if (lines.Length >= 2)
            {
                usuario = lines[0];
                password = lines[1];
            } else if (lines.Length == 1)
            {
                usuario = lines[0];
                password = "";
            }

            builder.Server = servidor; //"localhost"
            //builder.Server = "localhost"; //"localhost"
            builder.UserID = usuario; //"root"
            //builder.UserID = "root"; //"root"
            builder.Password = password; //""
            //builder.Password = ""; //""
            builder.Database = nombreBD; //"bddst"
            //builder.Database = "bddst"; //"bddst"
            conn = new MySqlConnection(builder.ToString());
            cmd = conn.CreateCommand();
            return conn;
        }

        /// <summary>
        /// Abre la conexion a la base de datos.
        /// </summary>
        public void Open()
        {
            // La conexion no deberia hacerse aqui. Por comodidad se ha puesto aqui.
            conectarBD(); 
            conn.Open();
        }

        /// <summary>
        /// Cierra la conexion a la base de datos.
        /// </summary>
        public void Close()
        {
            conn.Close();
        }

        /// <summary>
        /// Realiza una consulta a la base de datos.
        /// </summary>
        /// <param name="consulta"></param>
        /// <returns></returns>
        public MySqlDataReader ConsultaMySql(string consultaMySql)
        {
            cmd.CommandText = consultaMySql;
            consulta = cmd.ExecuteReader();

            return consulta;
        }

        /// <summary>
        /// Insetar una consulta en la base de datos.
        /// </summary>
        /// <param name="consultaMySql"></param>
        public void Insertar(string consultaMySql)
        {
            cmd.CommandText = consultaMySql;
            cmd.ExecuteNonQuery();
        }

        public MySqlConnection Conn
        {
            get { return conn; }
        }

        public MySqlCommand Cmd
        {
            get { return cmd; }
        }

        public MySqlDataReader Consulta
        {
            get { return consulta; }
        }
    }
}
