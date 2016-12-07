using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;
using LogicaDifusa;
using MySql.Data.MySqlClient;

namespace DST
{
    /// <summary>
    /// Clase para administrar los elementos de la logica difusa en la base de datos.
    /// </summary>
    public class AdminLD
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;
        public AdminLD()
        {
            bd = new BaseDeDatos();
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();
        }


        /// <summary>
        /// Devuele las variables linguisticas correspondientes a los componentes.
        /// </summary>
        /// <param name="componentes"></param>
        /// <returns></returns>
        public List<VariableLinguistica> VariablesLinsguisticas(Dictionary<string, Componente> componentes)
        {
            List<VariableLinguistica> variables = new List<VariableLinguistica>();



            return variables;
        }

        /// <summary>
        /// Guarda una variable linguistica en la base de datos
        /// </summary>
        /// <param name="idRegla"></param>
        /// <param name="nombre"></param>
        /// <param name="minimo"></param>
        /// <param name="maximo"></param>
        /// <param name="tipo"></param>
        public void InsertarVariableLinguistica( int idRegla, string nombre, double minimo, double maximo, 
            string tipo)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO variablesLing(idRegla,nombre,minimo,maximo,tipo) VALUES(" + idRegla.ToString()
                + ",'" + nombre + "'," + minimo.ToString() + "," + maximo.ToString() + ",'" + tipo + "');";

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Guarda un valor linguistico en la base de datos
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="nombreVariableLing"></param>
        /// <param name="tipoFuncion"></param>
        public void InsertarValorLinguistico(string nombre, string nombreVariableLing, string tipoFuncion)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO valoresLing(nombre,nombreVariableLing,tipoFuncion) VALUES('" + nombre
                + "','" + nombreVariableLing + "','" + tipoFuncion + "');";

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Guarda una funcion trapezoide en la base de datos
        /// </summary>
        /// <param name="nombreVariableLing"></param>
        /// <param name="nombreValorLing"></param>
        /// <param name="valorInferiorIzquierdo"></param>
        /// <param name="valorSuperiorIzquierdo"></param>
        /// <param name="valorSuperiorDerecho"></param>
        /// <param name="valorInferiorDerecho"></param>
        public void InsertarFuncionTrapezoide(string nombreVariableLing, string nombreValorLing, double valorInferiorIzquierdo,
            double valorSuperiorIzquierdo, double valorSuperiorDerecho, double valorInferiorDerecho)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO funcionTrapezoide(nombreVariableLing,nombreValorLing,valorInferiorIzquierdo," 
                + "valorSuperiorIzquierdo,valorSuperiorDerecho,valorInferiorDerecho) VALUES('" + nombreVariableLing
                + "','" + nombreValorLing + "'," + valorInferiorIzquierdo + "," + valorSuperiorIzquierdo + ","
                + valorSuperiorDerecho + "," + valorInferiorDerecho + ");";

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Guarda una funcion trinagular en la base de datos
        /// </summary>
        /// <param name="nombreVariableLing"></param>
        /// <param name="nombreValorLing"></param>
        /// <param name="valorIzquierda"></param>
        /// <param name="valorCentro"></param>
        /// <param name="valorDerecha"></param>
        public void InsertarFuncionTriangular(string nombreVariableLing, string nombreValorLing, double valorIzquierda,
            double valorCentro, double valorDerecha)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO funcionTrapezoide(nombreVariableLing,nombreValorLing,valorIzquierda,"
                + "valorCentro,valorDerecha) VALUES('" + nombreVariableLing + "','" + nombreValorLing + "'," 
                + valorIzquierda + "," + valorCentro + "," + valorDerecha + ");";

            cmd.ExecuteNonQuery();

            conn.Close();
        }
    }
}
