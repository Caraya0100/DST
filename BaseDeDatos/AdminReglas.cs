using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using LogicaDifusa;
using System.Diagnostics;

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
            BaseDeDatos bd = new BaseDeDatos();
            bd.Open();
            bd.ConsultaMySql("SELECT operador,nombreVariable,nombreValor,tipoVariable FROM regla WHERE idRegla=" 
                + id.ToString() + ";");

            while (bd.Consulta.Read())
            {
                if (bd.Consulta.GetString(3).Equals("antecedente"))
                {
                    //regla += consulta.GetString(1) + " ES " + consulta.GetString(2) + " " + regla 
                    //    + consulta.GetString(0) + " ";
                    sb.Append(bd.Consulta.GetString(1));
                    sb.Append(" es ");
                    sb.Append(bd.Consulta.GetString(2));
                    sb.Append(" " + bd.Consulta.GetString(0) + " ");
                }
                else if (bd.Consulta.GetString(3).Equals("consecuente"))
                {
                    regla = sb.ToString();
                    regla = regla.Remove(regla.Length - 3);


                    sb1 = new System.Text.StringBuilder(regla);

                    sb1.Append(" es ");
                    sb1.Append(bd.Consulta.GetString(1) );
                    sb1.Append( " es " );
                    sb1.Append(bd.Consulta.GetString(2) );
                }
            
            }

            bd.Close();

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
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO regla(idRegla,operador,nombreVariable,nombreValor,tipoVariable,"
                + "idSeccion,tipoComponente) VALUES(" + idRegla.ToString() + ",'" + operador + "','" + nombreVariable + "','"
                + nombreValor + "','" + tipoVariable + "'," + idSeccion.ToString() + ",'" + tipoComponente + "');");

            bd.Close();
        }

        /// <summary>
        /// Inserta una regla (completa) en la base de datos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regla"></param>
        /// <param name="idSeccion"></param>
        /// <param name="tipoComponente"></param>
        public void InsertarRegla(string id, string regla, string idSeccion, string tipoComponente)
        {
            string[] r = regla.Split(' ');
            string operador = "y";
            string nombreVariable = "";
            string nombreValor = "";

            // El tamaño minimo de una regla con mas de una variable en el antecedente.
            if (r.Length >= 12)
            {
                if (r[4] == "y" || r[4] == "o")
                {
                    operador = r[4];
                }
            }

            // Insertamos las variables linguisticas del antecedente.
            for (int i = 2; i < r.Length && r[i] != "entonces"; i++)
            {
                if ((r[i] == "es") && (i - 1 > 0) && (i + 1 < r.Length))
                {
                    nombreVariable = r[i - 1];
                    nombreValor = r[i + 1];

                    GuardarRegla(
                        Convert.ToInt32(id),
                        operador,
                        nombreVariable,
                        nombreValor,
                        "antecedente",
                        Convert.ToInt32(idSeccion),
                        tipoComponente
                    );
                }
            }
            // Insertamos el consecuente.
            string variableConsecuente = r[r.Length - 3];
            string valorConsecuente = r[r.Length - 1];

            GuardarRegla(
                Convert.ToInt32(id),
                operador,
                variableConsecuente,
                valorConsecuente,
                "consecuente",
                Convert.ToInt32(idSeccion),
                tipoComponente
            );
        }

        /// <summary>
        /// Devuelve las reglas de inferencia de una seccion desde la base de datos.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="tipoComponente"></param>
        /// <returns></returns>
        public Dictionary<string, string> ReglasSeccion(int idSeccion, string tipoComponente)
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();
            BaseDeDatos db = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT DISTINCT idRegla FROM regla WHERE idSeccion=" + idSeccion.ToString() 
                + " AND tipoComponente='" + tipoComponente + "';");

            while (bd.Consulta.Read())
            {
                //string nuevaRegla = "";

                //nuevaRegla = ObtenerRegla(consulta.GetInt16( 0 ));

                reglas.Add(bd.Consulta.GetInt16(0).ToString(), ObtenerRegla(bd.Consulta.GetInt16(0) ) );
            }

            bd.Close();

            return reglas;
        }

        /// <summary>
        /// Devuelve una regla desde la base de datos a partir de su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Regla ObtenerObjetoRegla(int id)
        {
            AdminLD adminLD = new AdminLD();
            Regla regla = new Regla();
            BaseDeDatos bd = new BaseDeDatos();
            Dictionary<string, ValorLinguistico> antecedente = new Dictionary<string, ValorLinguistico>();
            Tuple< string, ValorLinguistico > consecuente = new Tuple<string, ValorLinguistico>("", null);
            VariablesMatching matching = new VariablesMatching();
            string operador = "y";
            bd.Open();

            bd.ConsultaMySql("SELECT operador,nombreVariable,nombreValor,tipoVariable FROM regla WHERE idRegla="
                + id.ToString() + ";");

            while (bd.Consulta.Read())
            {
                string nombreValor = bd.Consulta.GetString("nombreValor");
                string nombreVariable = bd.Consulta.GetString("nombreVariable");
                operador = bd.Consulta.GetString("operador");

                if (bd.Consulta.GetString("tipoVariable").Equals("antecedente"))
                {
                    antecedente.Add(
                        nombreVariable,
                        adminLD.ObtenerValor(nombreValor, nombreVariable)
                    );
                }
                else if (bd.Consulta.GetString("tipoVariable").Equals("consecuente"))
                {
                    VariableLinguistica variable = matching.HBPerfil;

                    if (nombreVariable == "HD")
                        variable = matching.HDPerfil;
                    else if (nombreVariable == "CF")
                        variable = matching.CFPerfil;

                    Debug.WriteLine("Nombre valor consecuente: " + nombreValor);
                    consecuente = new Tuple<string, ValorLinguistico>(variable.Nombre, variable.Valores[nombreValor]);
                }
            }

            bd.Close();

            regla = new Regla(id.ToString(), antecedente, consecuente, operador);

            return regla;
        }

        /// <summary>
        /// Devuelve las reglas de inferencia de una seccion desde la base de datos.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="tipoComponente"></param>
        /// <returns></returns>
        public Dictionary<string, Regla> ObjetoReglasSeccion(int idSeccion, string tipoComponente)
        {
            Dictionary<string, Regla> reglas = new Dictionary<string, Regla>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT DISTINCT idRegla FROM regla WHERE idSeccion=" + idSeccion.ToString()
                + " AND tipoComponente='" + tipoComponente + "';");

            while (bd.Consulta.Read())
            {
                reglas.Add(bd.Consulta.GetInt16("idRegla").ToString(), ObtenerObjetoRegla(bd.Consulta.GetInt16("idRegla")));
            }

            bd.Close();

            return reglas;
        }

        /// <summary>
        /// Obtiene el ultimo id (id más grande).
        /// </summary>
        /// <returns></returns>
        public int ObtenerUltimoID()
        {
            int id = 0;

            conn.Open();

            cmd.CommandText = "SELECT idRegla FROM regla ORDER BY idRegla DESC LIMIT 0, 1;";

            consulta = cmd.ExecuteReader();

            if (consulta.Read())
            {
                id = consulta.GetInt32("idRegla");
            }

            conn.Close();

            return id;
        }

        public List<string> ObtenerIdReglaPorVariable(string nombreVariable)
        {
            List<string> ids = new List<string>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT DISTINCT idRegla FROM regla WHERE nombreVariable='" + nombreVariable + "';");

            while (bd.Consulta.Read())
            {
                ids.Add(bd.Consulta.GetInt32("idRegla").ToString());
            }

            bd.Close();

            return ids;
        }

        /// <summary>
        /// Obtiene una variable y su valor, del antecedente de una regla.
        /// </summary>
        /// <param name="idRegla"></param>
        /// <param name="nombreVariable"></param>
        /// <param name="nombreValor"></param>
        /// <returns></returns>
        public Tuple<string, ValorLinguistico> ObtenerVariableAntecedente(int idRegla, string nombreVariable)
        {
            Tuple<string, ValorLinguistico> variable;
            AdminLD ald = new AdminLD();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT idRegla, nombreVariable, nombreValor FROM regla WHERE idRegla =" + idRegla + " AND nombreVariable='" + nombreVariable + "' AND tipoVariable='antecedente';");

            if (bd.Consulta.Read())
            {
                variable = new Tuple<string, ValorLinguistico>(
                    bd.Consulta.GetString("nombreVariable"),
                    ald.ObtenerValor(
                        bd.Consulta.GetString("nombreValor"),
                        bd.Consulta.GetString("nombreVariable")
                    )
                );
            } else
            {
                variable = null;
            }

            bd.Close();

            return variable;
        }

        /// <summary>
        /// Obtiene una variable y su valor, del antecedente de una regla.
        /// </summary>
        /// <param name="idRegla"></param>
        /// <param name="nombreVariable"></param>
        /// <param name="nombreValor"></param>
        /// <returns></returns>
        public Tuple<string, ValorLinguistico> ObtenerVariableConsecuente(int idRegla, string nombreVariable)
        {
            Tuple<string, ValorLinguistico> variable;
            AdminLD ald = new AdminLD();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT idRegla, nombreVariable, nombreValor FROM regla WHERE idRegla =" + idRegla + " AND nombreVariable='" + nombreVariable + "' AND tipoVariable='consecuente';");

            if (bd.Consulta.Read())
            {
                VariablesMatching vm = new VariablesMatching();
                string nombre = bd.Consulta.GetString("nombreVariable");
                VariableLinguistica consecuente = vm.HBPerfil;

                if (nombre == "HD")
                    consecuente = vm.HDPerfil;
                else if (nombre == "CF")
                    consecuente = vm.CFPerfil;

                variable = new Tuple<string, ValorLinguistico>(
                    nombre,
                    consecuente.Valores[bd.Consulta.GetString("nombreValor")]
                );
            }
            else
            {
                variable = null;
            }

            bd.Close();

            return variable;
        }

        public void ActualizarVariable(int idRegla, string variableActual, string nuevaVariable, string nuevoValor, string nuevoOperador)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE regla SET nombreVariable='" + nuevaVariable + "', nombreValor='" + nuevoValor + "', operador='" + nuevoOperador + "' WHERE idRegla=" + idRegla + " AND nombreVariable='" + variableActual + "';");

            bd.Close();
        }

        public void ActualizarAntecedenteRegla(string variableActual, string valorActual, string nuevaVariable, string nuevoValor)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE regla SET nombreVariable='" + nuevaVariable + "', nombreValor='" + nuevoValor + "' WHERE nombreValor='" + valorActual + "' AND nombreVariable='" + variableActual + "';");

            bd.Close();
        }

        public void ActualizarNombreVariable(string actual, string nuevo)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE regla SET nombreVariable='" + nuevo + "' WHERE nombreVariable='" + actual + "';");

            bd.Close();
        }

        /// <summary>
        /// Actualiza una regla de una seccion.
        /// </summary>
        /// <param name="regla"></param>
        /// <param name="idSeccion"></param>
        public void ActualizarRegla(Regla regla, int idSeccion)
        {
            string tipoComponente = regla.Consecuente.Item1.ToLower();

            foreach (KeyValuePair<string, ValorLinguistico> variable in regla.Antecedente)
            {
                Tuple<string, ValorLinguistico> va = ObtenerVariableAntecedente(Convert.ToInt32(regla.ID), variable.Key);

                if (va != null && va.Item2.Nombre != variable.Value.Nombre)
                {
                    ActualizarVariable(
                        Convert.ToInt32(regla.ID), 
                        variable.Key,
                        variable.Key,
                        variable.Value.Nombre, 
                        regla.Operador
                    );
                } else if (va == null)
                {
                    GuardarRegla(
                        Convert.ToInt32(regla.ID), 
                        regla.Operador,
                        variable.Key,
                        variable.Value.Nombre, 
                        "antecedente", 
                        idSeccion, 
                        tipoComponente
                    );
                }
            }

            Tuple<string, ValorLinguistico> consecuente = ObtenerVariableConsecuente(Convert.ToInt32(regla.ID), regla.Consecuente.Item1);

            if (consecuente.Item2.Nombre != regla.Consecuente.Item2.Nombre)
            {
                ActualizarVariable(
                    Convert.ToInt32(regla.ID),
                    regla.Consecuente.Item1,
                    regla.Consecuente.Item1,
                    regla.Consecuente.Item2.Nombre,
                    regla.Operador
                );
            }
        }

        /// <summary>
        /// Elimina las reglas que tengan el nombre de la variable
        /// </summary>
        /// <param name="nombre"></param>
        public void EliminarReglasNombreVariable(string nombreVariable)
        {
            BaseDeDatos bd = new BaseDeDatos();
            bd.Open();

            List<string> ids = ObtenerIdReglaPorVariable(nombreVariable);

            foreach (string id in ids)
            {
                bd.Insertar("DELETE FROM regla WHERE idRegla=" + id.ToString() + ";");
            }

            bd.Close();
        }

        /// <summary>
        /// Cambia el valor de una variable linguistica de una regla.
        /// </summary>
        /// <param name="idRegla"></param>
        /// <param name="nombreVariable"></param>
        public void CambiarValor(string nuevo, string actual, string nombreVariable, int idRegla)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE regla SET nombreValor='" + nuevo + "' WHERE idRegla=" + idRegla.ToString() + " AND nombreVariable='" + nombreVariable + "' AND nombreValor='" + actual + "';");

            bd.Close();
        }

        /// <summary>
        /// Elimina una variable linguistica de una regla.
        /// </summary>
        /// <param name="idRegla"></param>
        /// <param name="nombreVariable"></param>
        public void EliminarVariable(int idRegla, string nombreVariable)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("DELETE FROM regla WHERE idRegla=" + idRegla.ToString() + " AND nombreVariable='" + nombreVariable + "';");

            bd.Close();
        }

        /// <summary>
        /// Elimina una regla.
        /// </summary>
        /// <param name="idRegla"></param>
        /// <param name="nombreVariable"></param>
        public void EliminarRegla(int idRegla)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("DELETE FROM regla WHERE idRegla=" + idRegla.ToString() + ";");

            bd.Close();
        }
    }
}
