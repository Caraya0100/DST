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

        public MySqlConnection conectarBD(string servidor, string usuario, string password, string nombreBD)
        {
            builder.Server = servidor; //"localhost"
            builder.UserID = usuario; //"root"
            builder.Password = password; //""
            builder.Database = nombreBD; //"bddst"
            conn = new MySqlConnection(builder.ToString());
            return conn;
        }
    }
}
