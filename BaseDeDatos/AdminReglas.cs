using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    /// <summary>
    /// Clase para administrar las reglas de la base de datos.
    /// </summary>
    public class AdminReglas
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;
        public AdminReglas()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();
        }

        /// <summary>
        /// Devuelve una regla desde la base de datos a partir de su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ObtenerRegla(int id)
        {
            string regla = "";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb.Append("SI ");

            conn.Open();
            cmd.CommandText = "SELECT operador,nombreVariable,nombreValor,tipoVariable FROM regla WHERE idRegla=" 
                + id.ToString() + ";";

            consulta = cmd.ExecuteReader();

            while (consulta.Read())
            {
                if (consulta.GetString(3).Equals("antecedente"))
                {
                    //regla += consulta.GetString(1) + " ES " + consulta.GetString(2) + " " + regla 
                    //    + consulta.GetString(0) + " ";
                    sb.Append(consulta.GetString(1));
                    sb.Append(" ES ");
                    sb.Append(consulta.GetString(2));
                    sb.Append(" " + consulta.GetString(0) + " ");
                }
                else if (consulta.GetString(3).Equals("consecuente"))
                {
                    /*
                    regla = regla.Remove( regla.Length - 3 );
                    regla += " ENTONCES ";

                    regla += consulta.GetString(1);
                    regla += " ES ";
                    //regla += consulta.GetString(2);*/
                    regla = sb.ToString();
                    regla = regla.Remove(regla.Length - 3);


                    sb1 = new System.Text.StringBuilder(regla);

                    sb1.Append(" ENTONCES ");
                    sb1.Append( consulta.GetString(1) );
                    sb1.Append( " ES " );
                    sb1.Append( consulta.GetString(2) );
                }
            
            }

            conn.Close();

            return sb1.ToString();
        }

        /// <summary>
        /// Guarda una regla en la base de datos
        /// </summary>
        /// <param name="idRegla"></param>
        /// <param name="operador"></param>
        /// <param name="nombreVariable"></param>
        /// <param name="nombreValor"></param>
        /// <param name="tipoVariable"></param>
        /// <param name="idSeccion"></param>
        /// <param name="tipoComponente"></param>
        public void GuardarRegla( int idRegla, string operador, string nombreVariable, string nombreValor,
            string tipoVariable, int idSeccion, string tipoComponente )
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO regla (idRegla,operador,nombreVariable,nombreValor,tipoVariable,"
                + "idSeccion,tipoComponente) VALUES("+ idRegla.ToString() + ",'" + operador + "','" + nombreVariable + "','"
                + nombreValor + "','" + tipoVariable + "'," + idSeccion.ToString() + ",'" + tipoComponente + "');";
            
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Devuelve las reglas de inferencia de una seccion desde la base de datos.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ReglasSeccion(int idSeccion, string tipo)
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            return reglas;
        }
    }
}
