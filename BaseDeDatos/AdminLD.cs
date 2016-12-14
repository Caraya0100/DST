using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            foreach (KeyValuePair<string, Componente> componente in componentes)
            {
                //Console.WriteLine("Componente: " + componente.Key);
                variables.Add(ObtenerVariable(componente.Key));
            }

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
        public void InsertarVariableLinguistica(string nombre, double minimo, double maximo)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO variablesLing(nombre,minimo,maximo) VALUES('" + nombre + "'," + minimo.ToString() + "," + maximo.ToString() + ");";

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

            cmd.CommandText = "INSERT INTO funcionTriangular(nombreVariableLing,nombreValorLing,valorIzquierda,"
                + "valorCentro,valorDerecha) VALUES('" + nombreVariableLing + "','" + nombreValorLing + "'," 
                + valorIzquierda + "," + valorCentro + "," + valorDerecha + ");";

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Obtiene una función de pertenencia a partir del nombre de la 
        /// variable linguistica y el nombre del valor linguistico.
        /// </summary>
        /// <param name="nombreVariable"></param>
        /// <param name="nombreValor"></param>
        /// <returns>Retorna null si no se encontro ninguna funcion de pertenencia</returns>
        public FuncionTriangular ObtenerFuncionTriangular(string nombreVariable, string nombreValor)
        {
            FuncionTriangular fp = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM funcionTriangular WHERE nombreVariableLing='" + nombreVariable + "' AND nombreValorLing='" + nombreValor + "';");

            if (bd.Consulta.Read())
            {
                fp = new FuncionTriangular(
                    bd.Consulta.GetDouble("valorIzquierda"),
                    bd.Consulta.GetDouble("valorCentro"),
                    bd.Consulta.GetDouble("valorDerecha")
                );
            }

            bd.Close();

            return fp;
        }

        /// <summary>
        /// Obtiene una función de pertenencia a partir del nombre de la 
        /// variable linguistica y el nombre del valor linguistico.
        /// </summary>
        /// <param name="nombreVariable"></param>
        /// <param name="nombreValor"></param>
        /// <returns>Retorna null si no se encontro ninguna funcion de pertenencia</returns>
        public FuncionTrapezoidal ObtenerFuncionTrapezoidal(string nombreVariable, string nombreValor)
        {
            FuncionTrapezoidal fp = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM funciontrapezoide WHERE nombreVariableLing='" + nombreVariable + "' AND nombreValorLing='" + nombreValor + "';");

            if (bd.Consulta.Read())
            {
                fp = new FuncionTrapezoidal(
                    bd.Consulta.GetDouble("valorInferiorIzquierdo"),
                    bd.Consulta.GetDouble("valorSuperiorIzquierdo"),
                    bd.Consulta.GetDouble("valorSuperiorDerecho"),
                    bd.Consulta.GetDouble("valorInferiorDerecho")
                );
            }

            bd.Close();

            return fp;
        }

        /// <summary>
        /// Obtiene el valor linguistico de una variable
        /// </summary>
        /// <param name="nombrevalor"></param>
        /// <param name="nombreVariable"></param>
        /// <returns></returns>
        public ValorLinguistico ObtenerValor(string nombre, string nombreVariable)
        {
            ValorLinguistico valor = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM valoresLing WHERE nombre='" + nombre + "' AND nombreVariableLing='" + nombreVariable + "';");

            if (bd.Consulta.Read())
            {
                string nombreValor = bd.Consulta.GetString("nombre");
                if (bd.Consulta.GetString("tipoFuncion") == "trapezoidal")
                {
                    FuncionTrapezoidal fp = ObtenerFuncionTrapezoidal(nombreVariable, nombreValor);
                    valor = new ValorLinguistico(nombreValor, fp);

                }
                else if (bd.Consulta.GetString("tipoFuncion") == "triangular")
                {
                    FuncionTriangular fp = ObtenerFuncionTriangular(nombreVariable, nombreValor);
                    valor = new ValorLinguistico(nombreValor, fp);
                }
            }

            bd.Close();

            return valor;
        }

        /// <summary>
        /// Obtiene los valores linguisticos a partir del nombre de la 
        /// variable linguistica.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public List<ValorLinguistico> ObtenerValores(string nombreVariable)
        {
            List<ValorLinguistico> valores = new List<ValorLinguistico>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM valoresLing WHERE nombreVariableLing='" + nombreVariable + "';");

            while (bd.Consulta.Read())
            {
                string nombreValor = bd.Consulta.GetString("nombre");
                if (bd.Consulta.GetString("tipoFuncion") == "trapezoidal")
                {
                    FuncionTrapezoidal fp = ObtenerFuncionTrapezoidal(nombreVariable, nombreValor);
                    valores.Add(new ValorLinguistico(nombreValor, fp));

                }
                else if (bd.Consulta.GetString("tipoFuncion") == "triangular")
                {
                    FuncionTriangular fp = ObtenerFuncionTriangular(nombreVariable, nombreValor);
                    valores.Add(new ValorLinguistico(nombreValor, fp));

                }
            }

            bd.Close();

            return valores;
        }

        /// <summary>
        /// Obtiene una variable linguistica a partir de su nombre.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public VariableLinguistica ObtenerVariable(string nombre)
        {
            VariableLinguistica variable = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM variablesling WHERE nombre='" + nombre + "';");

            if (bd.Consulta.Read())
            {
                // Instanceamos la variable linguistica.
                variable = new VariableLinguistica(
                    bd.Consulta.GetString("nombre"),
                    bd.Consulta.GetDouble("minimo"),
                    bd.Consulta.GetDouble("maximo")
                );
                // Agregamos los valores linguisticos.
                List<ValorLinguistico> valores = ObtenerValores(nombre);
                foreach (ValorLinguistico valor in valores)
                {
                    variable.AgregarValorLinguistico(valor);
                }
            }

            bd.Close();

            return variable;
        }

        public void ActualizarVariableLinguistica(string nombreActual, string nombre, double minimo, double maximo)
        {
            AdminReglas ar = new AdminReglas();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE variablesLing SET nombre='" + nombre + "', minimo=" + minimo + ", maximo=" + maximo + " WHERE nombre='" + nombreActual + "';");

            bd.Close();
        }

        /// <summary>
        /// Actualiza el nombre de un valor linguistico, ademas actualiza 
        /// el nombre de la funcion de pertenencia correspondiente, y las 
        /// reglas correspondientes.
        /// </summary>
        /// <param name="nuevo"></param>
        /// <param name="actual"></param>
        /// <param name="nombreVariable"></param>
        /// <param name="tipoFuncion"></param>
        public void ActualizarNombreValor(string nuevo, string actual, string nombreVariable, string tipoFuncion)
        {
            AdminReglas ar = new AdminReglas();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE valoresLing SET nombre='" + nuevo + "' WHERE nombre='" + actual + "' AND nombreVariableLing='" + nombreVariable + "' AND tipoFuncion='" + tipoFuncion + "';");

            if (tipoFuncion == "triangular")
                ActualizarNombreTriangular(nuevo, actual, nombreVariable);
            else if (tipoFuncion == "trapezoidal")
                ActualizarNombreTrapezoidal(nuevo, actual, nombreVariable);

            ar.ActualizarAntecedenteRegla(nombreVariable, actual, nombreVariable, nuevo);

            bd.Close();
        }

        public void ActualizarNombreTriangular(string nuevo, string actual, string nombreVariable)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE funcionTriangular SET nombreValorLing='" + nuevo + "' WHERE nombreValorLing='" + actual + "' AND nombreVariableLing='" + nombreVariable + "';");

            bd.Close();
        }

        public void ActualizarNombreTrapezoidal(string nuevo, string actual, string nombreVariable)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE funcionTrapezoide SET nombreValorLing='" + nuevo + "' WHERE nombreValorLing='" + actual + "' AND nombreVariableLing='" + nombreVariable + "';");

            bd.Close();
        }

        public void ActualizarValoresTriangular(string nombreVariable, string nombreValor, double inferiorIzq, double superioIzq, double superiorDerch, double inferiorDerch)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE funcionTrapezoide SET valorInferiorIzquierdo=" + inferiorIzq.ToString() + ", valorSuperiorIzquierdo=" + superioIzq.ToString() + ", valorSuperiorDerecho=" + superiorDerch.ToString() + ", valorInferiorDerecho=" + inferiorDerch.ToString() + " WHERE nombreValor='" + nombreValor + "' AND nombreVariable='" + nombreVariable + "';");

            bd.Close();
        }

        public void ActualizarValoresTriangular(string nombreVariable, string nombreValor, double izquierda, double centro, double derecha)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE funcionTriangular SET valorIzquierda=" + izquierda.ToString() + ", valorCentro=" + centro.ToString() + ", valorDerecha=" + derecha.ToString() + " WHERE nombreValor='" + nombreValor + "' AND nombreVariable='" + nombreVariable + "';");

            bd.Close();
        }
    }
}
