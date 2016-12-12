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
        /// Agrega una nueva pregunta para la encuesta
        /// </summary>
        /// <param name="pregunta"></param>
        /// <param name="tipo"></param>
        /// <param name="estado"></param>
        public void AgregarPregunta(string pregunta, string tipo, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO preguntas(pregunta,tipo,estado) VALUES('" + pregunta + "','" 
                + tipo + "'," + estado + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Se agregan alternativas que pueden ser usadas como respuestas al momento de crear preguntas nuevas.
        /// </summary>
        /// <param name="alternativa"></param>
        /// <param name="descripcion"></param>
        /// <param name="valor"></param>
        /// <param name="tipo"></param>
        public void AgregarAlternativas( string alternativa, string descripcion, double valor, string tipo)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO alternativas(alternativa,descripcion,valor,tipo) VALUES('" + alternativa + "','"
                + descripcion + "'," + valor + ",'" + valor.ToString() + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Funcion para agregar una alternativa de la tabla alternativas asociada a una pregunta de la tabla preguntas
        /// </summary>
        /// <param name="idPregunta"></param>
        /// <param name="alternativa"></param>
        public void AgregarAlternativasPreguntas( int idPregunta, string alternativa )
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO alternativasPreguntas(idPregunta,alternativa) VALUES(" + idPregunta.ToString() + ",'"
                + alternativa + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Agrega una referencia para asociar una pregunta a mas de un componente.
        /// </summary>
        /// <param name="idPregunta"></param>
        /// <param name="idComponente"></param>
        public void AgregarComponentesPreguntas( int idPregunta, int idComponente)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPreguntas(idPregunta,idComponente) VALUES(" + idPregunta.ToString() + ","
                + idComponente.ToString() + ");";
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

            cmd.CommandText = "INSERT INTO listaTrabajadoresEncuestados(rutTrabajadorEvaluado,rutTrabajadorEvaluador)"
                + " VALUES('" + rutTrabajadorEvaluado + "','" + rutTrabajadorEvaluador + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Obtiene las alternativas asociadas a una pregunta
        /// </summary>
        /// <param name="idPregunta"></param>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public List<Alternativa> ObtenerAlternativasPregunta(int idPregunta, string tipo)
        {
            List<Alternativa> alternativas = new List<Alternativa>();

            conn.Open();
            cmd.CommandText = "SELECT a.alternativa,a.descripcion,a.valor FROM alternativas AS a, alternativasPreguntas as p"
                + " WHERE a.tipo='" + tipo + "' AND p.idPregunta=" + idPregunta.ToString() + " AND a.alternativa=p.alternativa;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Alternativa nuevaAlternativa = new Alternativa(consulta.GetString(0),consulta.GetString(1),
                    consulta.GetDouble(2));
            }

            conn.Close();

            return alternativas; 
        }

        /// <summary>
        /// Obtiene todas las preguntas y las alternativas asociadas a dichas preguntas para poder
        /// evaluar las hb,hd y cf de un trabajador.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,Pregunta> ObtenerPreguntasEncuestaTrabajador()
        {
            Dictionary<string, Pregunta> preguntas = new Dictionary<string, Pregunta>();

            conn.Open();
            cmd.CommandText = "SELECT id,pregunta,tipo FROM preguntas WHERE estado=1;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Pregunta nuevaPregunta = new Pregunta();
                nuevaPregunta.Descripcion = consulta.GetString(1);

                nuevaPregunta.Alternativas = ObtenerAlternativasPregunta( consulta.GetInt16(0), consulta.GetString(2) );
            }

            conn.Close();

            return preguntas;
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
                + " VALUES('" + rutTrabajadorEvaluado + "','" + rutUsuarioEvaluador + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public Dictionary<string,string> UsuariosNoEncuestados( string rutTrabajador)
        {
            Dictionary<string,string> listaUsuariosNoEncuestados = new Dictionary<string,string>();

            conn.Open();
            cmd.CommandText = "SELECT nombre,rut FROM usuarios WHERE rut NOT IN( SELECT rutUsuarioEvaluador FROM"
                + " listaUsuariosEncuestados WHERE rutTrabajadorEvaluado='" + rutTrabajador + "');";

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
