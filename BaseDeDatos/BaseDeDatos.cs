using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class BaseDeDatos
    {
        private MySqlConnection conn;
        private MySqlConnectionStringBuilder builder;

        public BaseDeDatos()
        {
            builder = new MySqlConnectionStringBuilder();
        }

        //public MySqlConnection conectarBD(string servidor, string usuario, string password, string nombreBD)
        public MySqlConnection conectarBD()
        {
            //builder.Server = servidor; //"localhost"
            builder.Server = "localhost"; //"localhost"
            //builder.UserID = usuario; //"root"
            builder.UserID = "root"; //"root"
            //builder.Password = password; //""
            builder.Password = ""; //""
            //builder.Database = nombreBD; //"bddst"
            builder.Database = "bddst"; //"bddst"
            conn = new MySqlConnection(builder.ToString());
            return conn;
        }
    }
}
