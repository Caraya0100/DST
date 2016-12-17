﻿using System;
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

        /********************************************************************
         *              CONSULTAS MIAS
         * *****************************************************************/
        public List<string> ObtenerPreguntasPorHabilidad(string pregunta)
        {
            List<string> preguntas = new List<string>();
            conn.Open();

            cmd.CommandText = "SELECT * FROM preguntasComponentes WHERE nombre= " + "'" + pregunta + "' AND estado=" + "TRUE" + ";";
            consulta = cmd.ExecuteReader();
            //Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                preguntas.Add(consulta.GetString(2));
            }
            conn.Close();
            return preguntas;
        }

        public void InsertarPregunta(string pregunta, string tipo)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO preguntas(pregunta,tipo) VALUES('" + pregunta + "', '" + tipo + "');");

            bd.Close();
        }

        public void InsertarAlternativa(string alternativa, string descripcion, double valor, string tipo)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO alternativas(alternativa,descripcion, valor, tipo) VALUES('" + alternativa + "', '" + descripcion + "', " + valor + ", '" + tipo + "');");

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
    }
}
