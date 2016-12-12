using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminDesempeño
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminDesempeño()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();

        }


        /// <summary>
        /// Consulta para insertar los datos asociados al desempeño de la seccion de trabajo
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="fecha"></param>
        /// <param name="ventasAñoActual"></param>
        /// <param name="ventasAñoAnterior"></param>
        /// <param name="ventasPlan"></param>
        /// <param name="reubicaciones"></param>
        /// <param name="totalEmpleados"></param>
        /// <param name="empleadosConAdvertencia"></param>
        public void InsertarDesempeñoMensual(int idSeccion, string fecha, float ventasAñoActual, float ventasAñoAnterior,
            float ventasPlan, int reubicaciones, int totalEmpleados, int empleadosConAdvertencia)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO desempeño (idSeccion,fecha,ventasAñoActual,ventasAñoAnterior,ventasPlan,"
                + "reubicaciones,totalEmpleados,empleadosConAdvertencia) VALUES(" + idSeccion + ",'" + fecha + "',"
                + ventasAñoActual.ToString() + "," + ventasAñoAnterior.ToString() + "," + ventasPlan.ToString()
                + "," + reubicaciones.ToString() + "," + totalEmpleados.ToString() + "," + empleadosConAdvertencia.ToString()
                + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }


        /// <summary>
        /// Consulta para guardar los datos asociados a una reubicacion, esto para tener un historial.
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="idSeccionAnterior"></param>
        /// <param name="idSeccionNueva"></param>
        /// <param name="fecha"></param>
        public void InsertarReubicacion(string rut, int idSeccionAnterior, int idSeccionNueva, string fecha)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO reubicaciones (rut,idSeccionAnterior,idSeccionNueva,fecha) VALUES('"
                + rut + "'," + idSeccionAnterior.ToString() + "," + idSeccionNueva.ToString() + ",'"
                + fecha + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }


        /// <summary>
        /// Funcion con la consulta para insertar las ventas asociadas al año anterior
        /// Estas deben ingresarse solo una vez, ya que al ingresar las ventas del año actual, automaticamente
        /// se ingresaran tambien como ventas del año anterior.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="mes"></param>
        /// <param name="año"></param>
        /// <param name="ventasAñoAnterior"></param>
        public void InsertarVentasAñoAnterior(int idSeccion, string mes, string año, double ventasAñoAnterior)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO ventasAñoAnterior(idSeccion,mes,año,ventasAñoAnterior) VALUES("
                + idSeccion.ToString() + ",'" + mes + "','" + año + "'," + ventasAñoAnterior.ToString() + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Funcion con la consulta para insertar las ventas del plan de la empresa
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="mes"></param>
        /// <param name="año"></param>
        /// <param name="ventasPlan"></param>
        public void InsertarVentasPlan(int idSeccion, string mes, string año, double ventasPlan)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO ventasPlan(idSeccion,mes,año,ventasAñoAnterior) VALUES("
                + idSeccion.ToString() + ",'" + mes + "','" + año + "'," + ventasPlan.ToString() + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }



        /// <summary>
        /// Funcion con la consulta para insertar en la tabla una solicitud de cambio de seccion
        /// </summary>
        /// <param name="fechaSolicitud"></param>
        /// <param name="estadoSolicitud"></param>
        /// <param name="rutSolicitud"></param>
        /// <param name="idSeccionActual"></param>
        /// <param name="idSeccionSolicitada"></param>
        public void InsertarSolicitudCambio(string fechaSolicitud, string estadoSolicitud, string rutSolicitud,
            int idSeccionActual, int idSeccionSolicitada, double capacidadActual, double capacidadNueva)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO solicitudes(fechaSolicitud,estadoSolicitud,rutSolicitud,idSeccionActual,"
                + "idSeccionSolicitada,capacidadSeccionActual,capacidadNuevaSeccion) VALUES('" + fechaSolicitud + "','" + estadoSolicitud + "','"
                + rutSolicitud + "'," + idSeccionActual.ToString() + "," + idSeccionSolicitada.ToString()
                + "," + capacidadActual.ToString() + "," + capacidadNueva.ToString() + ");";
            //Console.WriteLine("CONSULTA" + cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Se obtienen todas las solicitudes.
        /// </summary>
        /// <returns></returns>
        public List<Solicitud> ObtenerSolicitudes()
        {
            List<Solicitud> solicitudes = new List<Solicitud>();
            
            conn.Open();
            cmd.CommandText = "SELECT * FROM solicitudes;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Solicitud nuevaSolicitud = new Solicitud( consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                    consulta.GetInt16(3), consulta.GetInt16(4), consulta.GetDouble(5), consulta.GetDouble(6) );
                solicitudes.Add( nuevaSolicitud );
            }

            conn.Close();
            
            return solicitudes;
        }


        /// <summary>
        /// Cambia el estado de la solicitud de EN_ESPERA a ACEPTADA o RECHAZADA
        /// </summary>
        /// <param name="nuevoEstado"></param>
        /// <param name="fechaSolicitud"></param>
        /// <param name="idSeccionActual"></param>
        /// <param name="idSeccionSolicitada"></param>
        public void CambiarEstadoSolicitud(string nuevoEstado, string fechaSolicitud, int idSeccionActual,
            int idSeccionSolicitada )
        {
            conn.Open();

            cmd.CommandText = "UPDATE solicitudes SET estadoSolicitud='" + nuevoEstado + "' WHERE fechaSolicitud='"
                + fechaSolicitud + "' AND idSeccionActual=" + idSeccionActual.ToString() + " AND idSeccionSolicitada="
                + idSeccionSolicitada.ToString() + ";"; 
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Se obtienen las solicitudes especificadas en el parametro de entrada el cual puede ser
        /// EN_ESPERA, ACEPTADA, RECHAZADA.
        /// </summary>
        /// <param name="estadoSolicitud"></param>
        /// <returns></returns>
        public List<Solicitud> ObtenerSolicitudesEspecificas( string estadoSolicitud )
        {
            List<Solicitud> solicitudes = new List<Solicitud>();

            conn.Open();
            cmd.CommandText = "SELECT * FROM solicitudes WHERE estadoSolicitud='" + estadoSolicitud + "';";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Solicitud nuevaSolicitud = new Solicitud(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                    consulta.GetInt16(3), consulta.GetInt16(4), consulta.GetDouble(5), consulta.GetDouble(6));
                solicitudes.Add(nuevaSolicitud);
            }

            conn.Close();

            return solicitudes;
        }

        /// <summary>
        /// Guarda los resultados de la evaluacion de un trabajador
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <param name="idSeccionEvaluacion"></param>
        /// <param name="capacidadTrabajador"></param>
        /// <param name="gradoIgualdadHB"></param>
        /// <param name="gradoIgualdadHD"></param>
        /// <param name="gradoIgualdadCF"></param>
        public void InsertarEvaluacion( string rutTrabajador, int idSeccionEvaluacion, double capacidadTrabajador,
            double gradoIgualdadHB, double gradoIgualdadHD, double gradoIgualdadCF)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO capacidadTrabajador(rutTrabajador,idSeccionEvaluacion,capacidadTrabajador,"
                + "gradoIgualdadHB,gradoIgualdadHD,gradoIgualdadCF) VALUES ('" + rutTrabajador + "'," 
                + idSeccionEvaluacion.ToString() + "," + gradoIgualdadHB.ToString() + "," + gradoIgualdadHD.ToString() 
                + "," + gradoIgualdadCF.ToString() + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void InsertarEvaluacionTrabajador( string fechaEvaluacion, string rutTrabajador, double hb, double hd,
            double cf)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO evaluacionTrabajador(fechaEvaluacion,rutTrabajador,hb,hd,cf) VALUES ('" 
                + fechaEvaluacion + "','"+ rutTrabajador + "'," + hb.ToString() + "," + hd.ToString()
                + "," + cf.ToString() + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Obtiene el puntaje de la evaluacion mas reciente para las habilidades blandas de un trabajador
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <returns></returns>
        public double ObtenerPuntajeHB( string rutTrabajador)
        {
            double puntajeHB = 0;

            conn.Open();
            cmd.CommandText = "SELECT hb FROM evaluaciontrabajador WHERE rutTrabajador='" + rutTrabajador 
                + "' AND fechaEvaluacion=(SELECT MAX(fechaEvaluacion) FROM evaluaciontrabajador WHERE" 
                + " rutTrabajador='" + rutTrabajador + "');";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                puntajeHB = consulta.GetDouble(0);
            }

            conn.Close();

            return puntajeHB;
        }

        /// <summary>
        /// Obtiene el puntaje de la evaluacion mas reciente para las habilidades duras de un trabajador
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <returns></returns>
        public double ObtenerPuntajeHD(string rutTrabajador)
        {
            double puntajeHD = 0;

            conn.Open();
            cmd.CommandText = "SELECT hd FROM evaluaciontrabajador WHERE rutTrabajador='" + rutTrabajador
                + "' AND fechaEvaluacion=(SELECT MAX(fechaEvaluacion) FROM evaluaciontrabajador WHERE"
                + " rutTrabajador='" + rutTrabajador + "');";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                puntajeHD = consulta.GetDouble(0);
            }

            conn.Close();

            return puntajeHD;
        }

        /// <summary>
        /// Obtiene el puntaje de la evaluacion mas reciente para las caracteristicas fisicas de un trabajador
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <returns></returns>
        public double ObtenerPuntajeCF(string rutTrabajador)
        {
            double puntajeCF = 0;

            conn.Open();
            cmd.CommandText = "SELECT cf FROM evaluaciontrabajador WHERE rutTrabajador='" + rutTrabajador
                + "' AND fechaEvaluacion=(SELECT MAX(fechaEvaluacion) FROM evaluaciontrabajador WHERE"
                + " rutTrabajador='" + rutTrabajador + "');";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                puntajeCF = consulta.GetDouble(0);
            }

            conn.Close();

            return puntajeCF;
        }
        
        public List<string> ObtenerRankingTrabajadoresSeccion( int idSeccion )
        {
            List<string> ranking = new List<string>();

            conn.Open();
            cmd.CommandText = "SELECT rutTrabajador FROM capacidadTrabajador WHERE idSeccionEvaluacion="
                + idSeccion.ToString() + " ORDER BY capacidadTrabajador DESC;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                ranking.Add( consulta.GetString(0) );
            }

            conn.Close();

            return ranking; 
        }

        public List<int> ObtenerRankingSeccionesTrabajador(string rutTrabajador)
        {
            List<int> ranking = new List<int>();

            conn.Open();
            cmd.CommandText = "SELECT idSeccionEvaluacion FROM capacidadTrabajador WHERE rutTrabajador='"
                + rutTrabajador + "' ORDER BY capacidadTrabajador DESC;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                ranking.Add(consulta.GetInt16(0));
            }

            conn.Close();

            return ranking;
        }
    }
}
