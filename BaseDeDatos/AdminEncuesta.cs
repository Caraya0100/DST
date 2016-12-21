using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using DST;
using System.Diagnostics;

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
            +"WHERE t3.idComponente = t4.id) as t6 WHERE t6.idPregunta = t5.id AND t5.tipo not like 'datos';";
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
                /*Console.WriteLine("PERFIL: " + pregunta.Habilidad);
                Console.WriteLine("PERFIL: " + pregunta.TipoHabilidad);
                Console.WriteLine("PERFIL: " + pregunta.Pregunta);
                Console.WriteLine("PERFIL: " + pregunta.TipoPregunta);
                Console.WriteLine("PERFIL: " + pregunta.Id);*/

            }
            conn.Close();

            return listaPreguntas;

        }

        public Alternativa ObtenerAlternativaPregunta(int idPregunta, string alternativa)
        {
            Alternativa al = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT alternativa FROM alternativasPreguntas WHERE idPregunta=" + idPregunta + " AND alternativa='" + alternativa + "';");

            if (bd.Consulta.Read())
            {
                al = ObtenerAlternativa(bd.Consulta.GetString("alternativa"));
            }

            bd.Close();

            return al;
        }

        public List<Encuesta.DatosAlternativa> ObtenerAlternativas(int idPregunta, string tipo)
        {
            List<Encuesta.DatosAlternativa> listaAlternativas = new List<Encuesta.DatosAlternativa>();
            conn.Open();          
            cmd.CommandText = "SELECT t1.idPregunta, t2.alternativa, t2.valor, t2.tipo FROM alternativaspreguntas AS t1 INNER JOIN "
            + "alternativas as t2 WHERE t1.alternativa = t2.alternativa AND T2.tipo = '" + tipo + "' AND idPregunta =" + idPregunta + " ORDER BY t2.valor DESC;";
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
            + "alternativas as t2 WHERE t1.alternativa = t2.alternativa AND T2.tipo = 'frecuencia' AND idPregunta =" + idPregunta + " ORDER BY t2.valor DESC;";
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

            cmd.CommandText = "INSERT INTO respuestasSeccionEncuesta(idSeccion, puntaje, idPregunta, idHabilidad, tipoHabilidad)"
                +" Values ("+idSeccion+","+ptje.Replace(",",".")+","+idPregunta+",'"+idHabilidad+"','"+tipoHabilidad+"')";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public double ObtenerPuntajeHabilidadSeccion(string habilidad)
        {
            double valor=0.0;
            conn.Open();
            cmd.CommandText = "SELECT SUM(puntaje)/count(*) FROM respuestasSeccionEncuesta WHERE idHabilidad='"+habilidad+"' AND puntaje not like -1;";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                valor = consulta.GetDouble(0);
            }
            conn.Close();
            return valor;
        }


        public int InsertarPregunta(string pregunta, string tipo)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO preguntas(pregunta,tipo,estado) VALUES('" + pregunta + "', '" + tipo + "', 1);");

            bd.Close();

            return Convert.ToInt32(bd.Cmd.LastInsertedId);
        }

        public void InsertarAlternativa(string alternativa, string descripcion, double valor, string tipo)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO alternativas(alternativa,descripcion, valor, tipo) VALUES('" + alternativa + "', '" + descripcion + "', " + valor + ", '" + tipo + "');");

            bd.Close();
        }

        public void InsertarPreguntaSeccion(int idSeccion, int idPregunta, string tipoPregunta)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO preguntasSeccion(idSeccion,idPregunta) VALUES(" + idSeccion + ", " + idPregunta + ");");

            bd.Close();
        }

        public void InsertarAlternativaPregunta(int idPregunta, string alternativa)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO alternativasPreguntas(idPregunta, alternativa) VALUES(" + idPregunta + ", '" + alternativa + "');");

            bd.Close();
        }

        public void InsertarComponentePregunta(int idPregunta, string idComponente)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO componentesPreguntas(idPregunta, idComponente) VALUES(" + idPregunta + ", '" + idComponente + "');");

            bd.Close();
        }

        public Alternativa ObtenerAlternativa(string alternativa)
        {
            Alternativa al = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM alternativas WHERE alternativa='" + alternativa + "';");

            if (bd.Consulta.Read())
            {
                al = new Alternativa(
                    bd.Consulta.GetString("alternativa"),
                    bd.Consulta.GetString("descripcion"),
                    bd.Consulta.GetDouble("valor"),
                    bd.Consulta.GetString("tipo")
                );
            }

            bd.Close();

            return al;
        }

        public void InsertarRespuestaSeccion(int idSeccion, int idPregunta, string alternativa)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO respuestasSeccion(idSeccion,idPregunta,alternativa) VALUES(" + idSeccion + ", " + idPregunta + ", '" + alternativa + "');");

            bd.Close();
        }

        public Dictionary<string, Alternativa> ObtenerAlternativasPorTipo(string tipo)
        {
            Dictionary<string, Alternativa> alternativas = new Dictionary<string, Alternativa>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM alternativas WHERE tipo='" + tipo + "';");

            while (bd.Consulta.Read())
            {
                alternativas.Add(bd.Consulta.GetString("alternativa"), new Alternativa(
                    bd.Consulta.GetString("alternativa"),
                    bd.Consulta.GetString("descripcion"),
                    bd.Consulta.GetDouble("valor"),
                    bd.Consulta.GetString("tipo")
                ));
            }

            bd.Close();

            return alternativas;
        }

        public Dictionary<string, Alternativa> ObtenerAlterantivasPregunta(int idPregunta)
        {
            Dictionary<string, Alternativa> alternativas = new Dictionary<string, Alternativa>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM alternativasPreguntas WHERE idPregunta=" + idPregunta + ";");

            while (bd.Consulta.Read())
            {
                string alternativa = bd.Consulta.GetString("alternativa");
                alternativas.Add(alternativa, ObtenerAlternativa(alternativa));
            }

            bd.Close();

            return alternativas;
        }

        public string ObtenerComponentePregunta(int idPregunta, string idComponente)
        {
            string componente = "";
            AdminPerfil ap = new AdminPerfil();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT idComponente FROM componentesPreguntas WHERE idPregunta=" + idPregunta + " AND idComponente='" + idComponente + "';");

            if (bd.Consulta.Read())
            {
                componente = bd.Consulta.GetString("idComponente");
            }

            bd.Close();

            return componente;
        }

        public List<string> ObtenerComponentesPregunta(int idPregunta)
        {
            List<string> componentes = new List<string>();
            AdminPerfil ap = new AdminPerfil();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM componentesPreguntas WHERE idPregunta=" + idPregunta + ";");

            while (bd.Consulta.Read())
            {
                string idComponente = bd.Consulta.GetString("idComponente");
                Componente componente = ap.ObtenerComponente(idComponente);
                componentes.Add(componente.ID);
            }

            bd.Close();

            return componentes;
        }

        public Dictionary<string, Pregunta> ObtenerPreguntas()
        {
            Dictionary<string, Pregunta> preguntas = new Dictionary<string, Pregunta>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM preguntas;");

            while (bd.Consulta.Read())
            {
                int id = bd.Consulta.GetInt32("id");
                preguntas.Add(id.ToString(), new Pregunta(
                    id,
                    bd.Consulta.GetString("pregunta"),
                    ObtenerAlterantivasPregunta(id),
                    ObtenerComponentesPregunta(id),
                    bd.Consulta.GetString("tipo")
                ));
            }

            bd.Close();

            return preguntas;
        }

        public Pregunta ObtenerPregunta(int idPregunta)
        {
            Pregunta pregunta = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM preguntas WHERE id=" + idPregunta + ";");

            if (bd.Consulta.Read())
            {
                pregunta = new Pregunta(
                    bd.Consulta.GetInt32("id"),
                    bd.Consulta.GetString("pregunta"),
                    ObtenerAlterantivasPregunta(idPregunta),
                    ObtenerComponentesPregunta(idPregunta),
                    bd.Consulta.GetString("tipo")
                );
            }

            bd.Close();

            return pregunta;
        }

        public Alternativa ObtenerRespuestaSeccion(int idSeccion, int idPregunta, string alternativa)
        {
            Alternativa al = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT alternativa FROM respuestasSeccion WHERE idPregunta=" + idPregunta + " AND idSeccion=" + idSeccion + " AND alternativa='"+ alternativa + "';");

            if (bd.Consulta.Read())
            {
                Debug.WriteLine("_PASOOO");
                al = ObtenerAlternativa(bd.Consulta.GetString("alternativa"));
            }

            bd.Close();

            return al;
        }

        public Dictionary<string, Pregunta> ObtenerPreguntasSeccion(int idSeccion)
        {
            Dictionary<string, Pregunta> preguntas = new Dictionary<string, Pregunta>();

            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT idPregunta FROM preguntasSeccion WHERE idSeccion=" + idSeccion + ";");

            while (bd.Consulta.Read())
            {
                int idPregunta = bd.Consulta.GetInt32("idPregunta");
                preguntas.Add(idPregunta.ToString(), ObtenerPregunta(idPregunta));
            }

            bd.Close();

            return preguntas;
        }

        public Dictionary<string, Alternativa> ObtenerRespuestasSeccion(int idSeccion)
        {
            Dictionary<string, Alternativa> alternativas = new Dictionary<string, Alternativa>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT * FROM respuestasSeccion WHERE idSeccion=" + idSeccion + ";");

            while (bd.Consulta.Read())
            {
                string alternativa = bd.Consulta.GetString("alternativa");
                alternativas.Add(alternativa, ObtenerAlternativa(alternativa));
            }

            bd.Close();

            return alternativas;
        }

        public void ModificarAlternativa(string alternativaActual, string alternativa, string descripcion, double valor, string tipo)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE alternativas SET alternativa='" + alternativa + "', descripcion='" + descripcion + "', valor=" + valor + ", tipo='" + tipo + "' WHERE alternativa='" + alternativaActual + "';");

            bd.Close();
        }

        public void ActualizarPregunta(int idPregunta, string pregunta, string tipo)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE preguntas SET pregunta='" + pregunta + "', tipo='" + tipo + "' WHERE id=" + idPregunta + ";");

            bd.Close();
        }

        public void ActualizarPregunta(Pregunta pregunta)
        {
            ActualizarPregunta(pregunta.ID, pregunta.ToString(), pregunta.Tipo);

            // Se insertan los componentes (no deben existir previamente en la bd).
            if (pregunta.Tipo.ToLower() != "gqm")
            {
                foreach (string componente in pregunta.Componentes)
                {
                    InsertarComponentePregunta(pregunta.ID, componente);
                }
            }

            // Se insertan las alternativas (no deben existir previamente en la bd).
            if (pregunta.Tipo.ToLower() == "360" || pregunta.Tipo.ToLower() == "normal" || pregunta.Tipo.ToLower() == "gqm")
            {
                foreach (KeyValuePair<string, Alternativa> alternativa in pregunta.Alternativas)
                {
                    InsertarAlternativaPregunta(pregunta.ID, alternativa.Key);
                }
                if (pregunta.Tipo.ToLower() == "360")
                {
                    foreach (KeyValuePair<string, Alternativa> frecuencia in pregunta.Frecuencias)
                    {
                        InsertarAlternativaPregunta(pregunta.ID, frecuencia.Key);
                    }
                }
            }
        }

        public void ActualizarAlternativa(string alternativaActual, string alternativa, string descripcion, double valor, string tipo)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE alternativas SET alternativa='" + alternativa + "', descripcion='" + descripcion + "', valor=" + valor + ", tipo='" + tipo + "' WHERE alternativa='" + alternativaActual + "';");

            bd.Close();
        }

        public void ActualizarAlternativaPregunta(int idPregunta, string alternativaActual, string nuevaAlternativa)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE alternativasPreguntas SET alternativa='" + nuevaAlternativa + "' WHERE idPregunta=" + idPregunta + " AND alternativa='" + alternativaActual + "';");

            bd.Close();
        }

        public void ActualizarRespuestaSeccion(int idSeccion, int idPregunta, string alternativa)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("UPDATE respuestasSeccion SET alternativa='" + alternativa + "' WHERE idSeccion=" + idSeccion + " AND idPregunta=" + idPregunta + ";");

            bd.Close();
        }

        public void EliminarAlternativa(string alternativa)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("DELETE FROM alternativas WHERE alternativa='" + alternativa + "';");

            bd.Close();
        }

        public void EliminarPregunta(int idPregunta)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("DELETE FROM preguntas WHERE id=" + idPregunta + ";");

            bd.Close();
        }

        public void EliminarAlternativaPregunta(int idPregunta, string alternativa)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("DELETE FROM alternativasPreguntas WHERE idPregunta=" + idPregunta + " AND alternativa='" + alternativa + "';");

            bd.Close();
        }

        public void EliminarAlternativasPregunta(int idPregunta)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("DELETE FROM alternativasPreguntas WHERE idPregunta=" + idPregunta + ";");

            bd.Close();
        }

        public void EliminarAlternativasPregunta(Pregunta pregunta)
        {
            foreach (KeyValuePair<string, Alternativa> alternativa in pregunta.Alternativas)
            {
                EliminarAlternativaPregunta(pregunta.ID, alternativa.Key);
            }
        }

        public void EliminarComponentePregunta(int idPregunta, string idComponente)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("DELETE FROM componentesPreguntas WHERE idPregunta=" + idPregunta + " AND idComponente='" + idComponente + "';");

            bd.Close();
        }

        public void EliminarComponentesPregunta(int idPregunta)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("DELETE FROM componentesPreguntas WHERE idPregunta=" + idPregunta + ";");

            bd.Close();
        }

        public void EliminarComponentesPregunta(Pregunta pregunta)
        {
            foreach (string componente in pregunta.Componentes)
            {
                EliminarComponentePregunta(pregunta.ID, componente);
            }
        }

        public void AgregarRespuestaNormal(int idPregunta, string rutTrabajador, string rutEvaluador, string alternativa)
        {
            //string ptje = "" + puntaje;
            conn.Open();

            cmd.CommandText = "INSERT INTO respuestas(idPregunta, rutTrabajadorAsociado, rutRespuesta, alternativaRespuesta)"
                + " Values ("+idPregunta+", '"+rutTrabajador+"', '"+rutEvaluador+"', '"+alternativa+"');";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public Dictionary<string, double> ObtenerPuntajesTrabajador(string rut)
        {
            Dictionary<string, double> habilidades = new Dictionary<string, double>();
            conn.Open();

            cmd.CommandText = "SELECT t4.idComponente, AVG(t3.resultadoRespuesta) FROM componentespreguntas as t4 INNER JOIN "
                +"(SELECT * FROM preguntas as t2 INNER JOIN (SELECT idPregunta,resultadoRespuesta FROM respuestas360 "
                +"WHERE rutTrabajadorAsociado ='"+rut+"') as t1 ON t2.id = t1.idPregunta) as t3 ON t3.idpregunta=t4.idPregunta "
                +"GROUP by t4.idComponente";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                habilidades.Add(consulta.GetString(0), consulta.GetDouble(1));
            }
            conn.Close();
            return(habilidades);
        } 

    }
}
