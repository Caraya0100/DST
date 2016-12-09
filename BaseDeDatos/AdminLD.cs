﻿using System;
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

            conn.Open();
            cmd.CommandText = "SELECT * FROM funciontrapezoide WHERE nombreVariableLing='" + nombreVariable + " ' AND " + "nombreValorLing='" + nombreValor + ";";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
            {
                fp = new FuncionTriangular(
                    consulta.GetDouble("valorIzquierda"),
                    consulta.GetDouble("valorCentro"),
                    consulta.GetDouble("valorDerecha")
                );
            }

            conn.Close();

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

            conn.Open();
            cmd.CommandText = "SELECT * FROM funciontrapezoide WHERE nombreVariableLing='" + nombreVariable + " ' AND " + "nombreValorLing='" + nombreValor + ";";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
            {
                fp = new FuncionTrapezoidal(
                    consulta.GetDouble("valorInferiorIzquierdo"),
                    consulta.GetDouble("valorSuperiorIzquierdo"),
                    consulta.GetDouble("valorSuperiorDerecho"),
                    consulta.GetDouble("valorInferiorDerecho")
                );
            }

            conn.Close();

            return fp;
        }

        public ValorLinguistico ObtenerValor(string nombre)
        {
            ValorLinguistico valor = null;

            conn.Open();
            cmd.CommandText = "SELECT * FROM usuarios WHERE nombre='" + nombre + "';";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
            {
                
            }

            conn.Close();

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

            conn.Open();
            cmd.CommandText = "SELECT * FROM valoresLing WHERE nombreVariableLing='" + nombreVariable + "';";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
            {
                string nombreValor = consulta.GetString("nombre");
                if (consulta.GetString("tipoFuncion") == "trapezoidal")
                {
                    FuncionTrapezoidal fp = ObtenerFuncionTrapezoidal(nombreVariable, nombreValor);
                    valores.Add(new ValorLinguistico(nombreValor, fp));

                }
                else if (consulta.GetString("tipoFuncion") == "trapezoidal")
                {
                    FuncionTriangular fp = ObtenerFuncionTriangular(nombreVariable, nombreValor);
                    valores.Add(new ValorLinguistico(nombreValor, fp));

                }
            }

            conn.Close();

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

            conn.Open();
            cmd.CommandText = "SELECT * FROM variablesling WHERE nombre='" + nombre + "';";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
            {
                // Instanceamos la variable linguistica.
                variable = new VariableLinguistica(
                    consulta.GetString("nombre"),
                    consulta.GetDouble("minimo"),
                    consulta.GetDouble("maximo")
                );
                // Agregamos los valores linguisticos.
                List<ValorLinguistico> valores = ObtenerValores(nombre);
                foreach (ValorLinguistico valor in valores)
                {
                    variable.AgregarValorLinguistico(valor);
                }
            }

            conn.Close();

            return variable;
        }
    }
}
