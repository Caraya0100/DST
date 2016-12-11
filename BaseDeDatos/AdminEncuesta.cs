using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminEncuesta
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminEncuesta()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();

        }

        /// <summary>
        /// Consulta para insertar preguntas asociadas a un componente
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="pregunta"></param>
        /// <param name="estado"></param>
        public void InsertarPreguntaComponente(string nombre, string pregunta, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO preguntasComponentes (nombre,pregunta,estado) VALUES('"
                + nombre + "','" + pregunta + "'," + estado + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para insertar las alternativas asociadas a las preguntas de las componentes.
        /// </summary>
        /// <param name="idPregunta"></param>
        /// <param name="alternativa"></param>
        public void InsertarAlternativaPreguntaComponente(int idPregunta, string alternativa)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO alternativasPreguntasComponentes (idPregunta,alternativa) VALUES("
                + idPregunta.ToString() + ",'" + alternativa + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }


        /// <summary>
        /// Consulta para guardar las respuestas, se guarda el rut del trabajador que esta siendo evaluado
        /// y el rut de la persona que respondio para de esta manera evitar que una persona evalue mas de una vez
        /// a un trabajador
        /// </summary>
        /// <param name="idPregunta"></param>
        /// <param name="rutTrabajadorAsociado"></param>
        /// <param name="rutRespuesta"></param>
        /// <param name="alternativaRespuesta"></param>
        public void InsertarRespuesta(int idPregunta, string rutTrabajadorAsociado, string rutRespuesta,
            string alternativaRespuesta)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO respuestas (idPregunta,rutTrabajadorAsociado,rutRespuesta,alternativaRespuesta) "
                + "VALUES(" + idPregunta.ToString() + ",'" + rutTrabajadorAsociado + "','" + rutRespuesta + "','"
                + alternativaRespuesta + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Guarda en la base de datos el rut del trabajador que respondio una encuesta y el rut del trabajador
        /// al que evaluo en dicha encuesta.
        /// </summary>
        /// <param name="rutTrabajadorEvaluado"></param>
        /// <param name="rutTrabajadorEvaluador"></param>
        public void InsertarTrabajadorEncuestado( string rutTrabajadorEvaluado, string rutTrabajadorEvaluador)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO listaTrabajadoresEncuestados (rutTrabajadorEvaluado,rutTrabajadorEvaluador)"
                + "VALUES('" + rutTrabajadorEvaluado + "','" + rutTrabajadorEvaluador + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Guarda en la base de datos el rut de un usuario que evaluo a un trabajador, guardando el rut de este ultimo
        /// para saber si respondio
        /// </summary>
        /// <param name="rutTrabajadorEvaluado"></param>
        /// <param name="rutUsuarioEvaluador"></param>
        public void InsertarUsuarioEncuestado( string rutTrabajadorEvaluado, string rutUsuarioEvaluador)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO listaUsuariosEncuestados (rutTrabajadorEvaluado,rutUsuarioEvaluador)"
                + "VALUES('" + rutTrabajadorEvaluado + "','" + rutUsuarioEvaluador + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public Dictionary<string,string> UsuariosNoEncuestados( string rutTrabajador)
        {
            Dictionary<string,string> listaUsuariosNoEncuestados = new Dictionary<string,string>();

            conn.Open();
            cmd.CommandText = "SELECT nombre,rut FROM usuarios WHERE rut NOT IN( SELECT rutUsuarioEvaluador FROM"
                + "listaUsuariosEncuestados WHERE rutTrabajadorEvaluado='" + rutTrabajador + "');";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                listaUsuariosNoEncuestados.Add( consulta.GetString(1), consulta.GetString(0) );
            }

            conn.Close();

            return listaUsuariosNoEncuestados;
        }

        public Dictionary<string,string> TrabajadoresNoEncuestados( string rutTrabajador )
        {
            Dictionary<string, string> listaTrabajadoresNoEncuestados = new Dictionary<string, string>();

            conn.Open();
            cmd.CommandText = "SELECT rut,nombre,apellidoPaterno,apellidoMaterno FROM trabajadores WHERE rut NOT IN( SELECT" 
                + " rutUsuarioEvaluador FROM listaTrabajadoresEncuestados WHERE rutTrabajadorEvaluado='" + rutTrabajador + "');";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                listaTrabajadoresNoEncuestados.Add(consulta.GetString(0), consulta.GetString(1) + " " + consulta.GetString(2) + 
                    " " + consulta.GetString(3) );
            }

            conn.Close();

            return listaTrabajadoresNoEncuestados;
        }
    }
}
