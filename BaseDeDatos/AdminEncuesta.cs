using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DST;

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

        /********************************************************************
         *              CONSULTAS MIAS
         * *****************************************************************/
        public List<string> ObtenerPreguntasPorHabilidad(string pregunta)
        {
            List<string> preguntas = new List<string>();
            conn.Open();

            //cmd.CommandText = "SELECT * FROM componentesPreguntas WHERE nombre= '" + pregunta +"';";
            cmd.CommandText = "SELECT pregunta FROM Preguntas;";
            consulta = cmd.ExecuteReader();
            //Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                //preguntas.Add(consulta.GetString(2));
                preguntas.Add(consulta.GetString(0));
            }
            conn.Close();
            return preguntas;
        }

        public List<Encuesta.DatosPregunta> ObtenerTodasLasPreguntas()
        {
            List<Encuesta.DatosPregunta> listaPreguntas = new List<Encuesta.DatosPregunta>();
            conn.Open();

            //cmd.CommandText = "SELECT * FROM componentesPreguntas WHERE nombre= '" + pregunta +"';";
            cmd.CommandText = "SELECT DISTINCT t4.nombre, t4.tipo, t3.pregunta,t3.id,t3.tipo FROM componentesperfil AS t4 INNER JOIN " 
                +"(SELECT t1.id, t1.pregunta, t1.tipo, t2.idComponente FROM preguntas as t1 INNER JOIN "
            +"ComponentesPreguntas as t2 WHERE t1.id=t2.idPregunta AND t1.estado=true) as t3 WHERE t3.idComponente = t4.id;";
            consulta = cmd.ExecuteReader();
            //Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {               
                Encuesta.DatosPregunta pregunta = new Encuesta.DatosPregunta();
                pregunta.Habilidad = consulta.GetString(0);
                pregunta.TipoHabilidad= consulta.GetString(1);
                pregunta.Pregunta = consulta.GetString(2);
                pregunta.TipoPregunta = consulta.GetString(4);
                pregunta.Id = consulta.GetInt16(3);
                listaPreguntas.Add(pregunta);
                Console.WriteLine("TODAS: " + pregunta.Habilidad);
                Console.WriteLine("TODAS: " + pregunta.TipoHabilidad);
                Console.WriteLine("TODAS: " + pregunta.Pregunta);
                Console.WriteLine("TODAS: " + pregunta.TipoPregunta);
                Console.WriteLine("TODAS: " + pregunta.Id);
            }
            conn.Close();

            return listaPreguntas;
        }

        public List<Encuesta.DatosPregunta> ObtenerPreguntasDelPerfil(int idSeccion)
        {
            List<Encuesta.DatosPregunta> listaPreguntas = new List<Encuesta.DatosPregunta>();
            conn.Open();
            cmd.CommandText = "SELECT t6.nombre,t6.tipo,t5.pregunta,t6.id,t5.tipo,t6.idPregunta FROM preguntas as t5 INNER JOIN (SELECT DISTINCT t3.idPregunta,t4.tipo,t4.nombre, t4.id FROM " 
            +"componentespreguntas as t3 INNER JOIN(SELECT DISTINCT t1.id, t2.tipo,t2.nombre FROM componentesperfilsecciones as t1 "
            +"INNER JOIN componentesperfil as t2 WHERE t1.id = t2.id and t1.idSeccion = "+idSeccion+" AND t1.estado =true) as t4 "
            +"WHERE t3.idComponente = t4.id) as t6 WHERE t6.idPregunta = t5.id;";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                Encuesta.DatosPregunta pregunta = new Encuesta.DatosPregunta();
                pregunta.Habilidad = consulta.GetString(0);
                pregunta.TipoHabilidad = consulta.GetString(1);
                pregunta.Pregunta = consulta.GetString(2);
                pregunta.idHabilidad = consulta.GetString(3);
                pregunta.TipoPregunta = consulta.GetString(4);
                pregunta.Id = consulta.GetInt16(5);
                listaPreguntas.Add(pregunta);
                Console.WriteLine("PERFIL: " + pregunta.Habilidad);
                Console.WriteLine("PERFIL: " + pregunta.TipoHabilidad);
                Console.WriteLine("PERFIL: " + pregunta.Pregunta);
                Console.WriteLine("PERFIL: " + pregunta.TipoPregunta);
                Console.WriteLine("PERFIL: " + pregunta.Id);

            }
            conn.Close();

            return listaPreguntas;

        }

        public List<Encuesta.DatosAlternativa> ObtenerAlternativas(int idPregunta, string tipo)
        {
            List<Encuesta.DatosAlternativa> listaAlternativas = new List<Encuesta.DatosAlternativa>();
            conn.Open();          
            cmd.CommandText = "SELECT t1.idPregunta, t2.alternativa, t2.valor, t2.tipo FROM alternativaspreguntas AS t1 INNER JOIN "
            +"alternativas as t2 WHERE t1.alternativa = t2.alternativa AND T2.tipo = '"+tipo+"' AND idPregunta ="+idPregunta+";";
            consulta = cmd.ExecuteReader();
            Console.WriteLine("XXX: "+cmd.CommandText.ToString());
            while (consulta.Read())
            {
                Encuesta.DatosAlternativa alternativa = new Encuesta.DatosAlternativa();
                alternativa.idPregunta = consulta.GetInt16(0);
                alternativa.Alternativa = consulta.GetString(1);
                alternativa.Valor = consulta.GetDouble(2);
                alternativa.Tipo = consulta.GetString(3);
                listaAlternativas.Add(alternativa);
            }
            conn.Close();
            return listaAlternativas;
        }

        public List<Encuesta.DatosAlternativa> ObtenerFrecuencias360(int idPregunta)
        {
            List<Encuesta.DatosAlternativa> listaFrecuencia = new List<Encuesta.DatosAlternativa>(); 
            conn.Open();
            cmd.CommandText = "SELECT t1.idPregunta, t2.alternativa, t2.valor, t2.tipo FROM alternativaspreguntas AS t1 INNER JOIN "
            + "alternativas as t2 WHERE t1.alternativa = t2.alternativa AND T2.tipo = 'frecuencia' AND idPregunta =" + idPregunta + ";";
            consulta = cmd.ExecuteReader();
            //Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                Encuesta.DatosAlternativa alternativa = new Encuesta.DatosAlternativa();
                alternativa.idPregunta = consulta.GetInt16(0);
                alternativa.Alternativa = consulta.GetString(1);
                alternativa.Valor = consulta.GetDouble(2);
                alternativa.Tipo = consulta.GetString(3);
                listaFrecuencia.Add(alternativa);
            }
            conn.Close();
            return listaFrecuencia;
        }

        public double ObtenerValorAlternativa(int idPregunta, string alternativa)
        {
            double valor = 0.0;
            conn.Open();
            cmd.CommandText = "SELECT t2.valor FROM alternativaspreguntas AS t1 INNER JOIN "
            + "alternativas as t2 WHERE t1.alternativa = t2.alternativa AND T2.tipo = 'grado' AND t1.idPregunta =" + idPregunta 
            + " AND t2.alternativa ='"+alternativa+"';";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                valor = consulta.GetDouble(0);            
            }
            conn.Close();
            return valor;
        }

        public double ObtenerValorFrecuencia(int idPregunta, string alternativa)
        {
            double valor = 0.0;
            conn.Open();
            cmd.CommandText = "SELECT t2.valor FROM alternativaspreguntas AS t1 INNER JOIN "
            + "alternativas as t2 WHERE t1.alternativa = t2.alternativa AND T2.tipo = 'frecuencia' AND t1.idPregunta =" + idPregunta
            + " AND t2.alternativa ='" + alternativa + "';";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                valor = consulta.GetDouble(0);
            }
            conn.Close();
            return valor;
        }

        public void AgregarRespuesta360(int idPregunta,string rutTrabajador, string rutEncuestado, string alternativa, string frecuencia, string valor)
        {            
            conn.Open();

            cmd.CommandText = "INSERT INTO respuestas360 (idPregunta,rutTrabajadorAsociado,rutRespuesta,alternativaGrado,alternativaFrecuencia, "
            +"resultadoRespuesta) VALUES("+idPregunta+", '"+rutTrabajador+"', '"+rutEncuestado+"', '"+alternativa+"', '"+frecuencia+"', "+valor+");";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public void TrabajadorYaEvaluados(string rutTrabajador, string rutEvaluador)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO listaTrabajadoresEncuestados (rutTrabajadorEvaluado, rutTrabajadorEvaluador) "
            +"VALUES ('"+rutTrabajador+"','"+rutEvaluador+"');";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        /*******************/
        public void AgregarRespuestaSeccion(int idSeccion, double puntaje, int idPregunta, string idHabilidad,string tipoHabilidad)
        {
            string ptje = "" + puntaje;
            conn.Open();

            cmd.CommandText = "INSERT INTO respuestasSeccion(idSeccion, puntaje, idPregunta, idHabilidad, tipoHabilidad)"
                +" Values ("+idSeccion+","+ptje.Replace(",",".")+","+idPregunta+",'"+idHabilidad+"','"+tipoHabilidad+"')";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public double ObtenerPuntajeHabilidadSeccion(string habilidad)
        {
            double valor=0.0;
            conn.Open();
            cmd.CommandText = "SELECT SUM(puntaje)/count(*) FROM respuestasSeccion WHERE idHabilidad='"+habilidad+"' AND puntaje not like -1;";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                valor = consulta.GetDouble(0);
            }
            conn.Close();
            return valor;
        }

    }
}
