﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;

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

        public void InsertarDesempeñoMensual(int idSeccion, string fecha, double ventasAñoActual, double ventasAñoAnterior,
            double ventasPlan, int reubicaciones, int totalEmpleados, int empleadosCapacitados, int empleadosNoCapacitados)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO desempeño (idSeccion,fecha,ventasAñoActual,ventasAñoAnterior,ventasPlan,"
                + "reubicaciones,totalEmpleados,empleadosConAdvertencia) VALUES(" + idSeccion + ",'" + fecha + "',"
                + ventasAñoActual.ToString() + "," + ventasAñoAnterior.ToString() + "," + ventasPlan.ToString()
                + "," + reubicaciones.ToString() + "," + totalEmpleados.ToString() + "," + empleadosCapacitados
                + ", " + empleadosNoCapacitados + ");";
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
            Console.WriteLine(cmd.CommandText.ToString());
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
        public void InsertarVentasAñoAnterior(int idSeccion, string mes, string año, float ventasAñoAnterior)
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
        public void InsertarVentasPlan(int idSeccion, string mes, string año, float ventasPlan)
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
            int idSeccionActual, int idSeccionSolicitada)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO solicitudes(fechaSolicitud,estadoSolicitud,rutSolicitud,idSeccionActual"
                + "idSeccionSolicitada) VALUES('" + fechaSolicitud + "','" + estadoSolicitud + "','"
                + rutSolicitud + "'," + idSeccionActual.ToString() + "," + idSeccionSolicitada.ToString()
                + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public double ObtenerVentasAnioAnterior(int idSeccion, int mes, int anio)
        {
            double ventas = 0;

            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT ventasAñoAnterior FROM ventasAñoAnterior WHERE idSeccion=" + idSeccion + ", mes=" + mes + ", anio=" + anio + ";");

            while (bd.Consulta.Read())
            {
                ventas = bd.Consulta.GetDouble("ventasAñoAnterior");
            }

            bd.Close();

            return ventas;
        }

        /// <summary>
        /// Obtiene una lista con los años del desempeño de una sección.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public List<int> ObtenerAnios(int idSeccion)
        {
            List<int> anios = new List<int>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT DISTINCT YEAR(fecha) AS anio FROM desempeño WHERE idSeccion=" + idSeccion + " ORDER BY anio ASC;");

            while (bd.Consulta.Read())
            {
                anios.Add(Convert.ToInt32(bd.Consulta.GetString("anio")));
            }

            bd.Close();

            return anios;
        }

        /// <summary>
        /// Obtiene una lista con los meses de un año del desempeño de una sección.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public List<int> ObtenerMesesAnio(int idSeccion, int anio)
        {
            List<int> meses = new List<int>();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT DISTINCT MONTH(fecha) AS mes FROM desempeño WHERE idSeccion=" + idSeccion + " AND YEAR(fecha)='" + anio + "' ORDER BY mes ASC;");

            while (bd.Consulta.Read())
            {
                meses.Add(Convert.ToInt32(bd.Consulta.GetString(0)));
            }

            bd.Close();

            return meses;
        }

        /// <summary>
        /// Obtiene las ventas (actuales, anteriores, plan) de una sección.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="inicioAnioFiscal"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public Dictionary<string, Tuple<double, double, double>> ObtenerVentasAnuales(int idSeccion, int inicioAnioFiscal, int anio)
        {
            // fecha, añoActual, añoAnaterior, ventasPlan
            Dictionary<string, Tuple<double, double, double>> ventas = new Dictionary<string, Tuple<double, double, double>>();
            Tuple<int, int> fechaFinal = ObtenerFechaFinal(inicioAnioFiscal, anio);
            int finalAnioFiscal = fechaFinal.Item1;
            int anioFinal = fechaFinal.Item2;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT fecha, ventasAñoActual, ventasAñoAnterior, ventasPlan FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha BETWEEN '" + anio + "-" + inicioAnioFiscal + "-01' AND '" + anioFinal + "-" + finalAnioFiscal + "-01' ORDER BY fecha ASC;");

            while (bd.Consulta.Read())
            {
                ventas.Add(
                    bd.Consulta.GetString("fecha"),
                    new Tuple<double, double, double>(
                        bd.Consulta.GetDouble(1),
                        bd.Consulta.GetDouble(2),
                        bd.Consulta.GetDouble(3)
                    )
                );
            }

            bd.Close();

            return ventas;
        }

        public Dictionary<string, int> ObtenerReubicacionesAnuales(int idSeccion, int inicioAnioFiscal, int anio)
        {
            Dictionary<string, int> reubicaciones = new Dictionary<string, int>();
            Tuple<int, int> fechaFinal = ObtenerFechaFinal(inicioAnioFiscal, anio);
            int finalAnioFiscal = fechaFinal.Item1;
            int anioFinal = fechaFinal.Item2;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT fecha, reubicaciones FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha BETWEEN '" + anio + "-" + inicioAnioFiscal + "-01' AND '" + anioFinal + "-" + finalAnioFiscal + "-01' ORDER BY fecha ASC;;");

            while (bd.Consulta.Read())
            {
                reubicaciones.Add(
                    bd.Consulta.GetString("fecha"),
                    bd.Consulta.GetInt32("reubicaciones")
                );
            }

            bd.Close();

            return reubicaciones;
        }

        /// <summary>
        /// Obtiene el total de empleados de una seccion en cada mes de un año fiscal.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="inicioAnioFiscal"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public Dictionary<string, int> ObtenerTotalEmpleadosAnuales(int idSeccion, int inicioAnioFiscal, int anio)
        {
            Dictionary<string, int> empleados = new Dictionary<string, int>();
            Tuple<int, int> fechaFinal = ObtenerFechaFinal(inicioAnioFiscal, anio);
            int finalAnioFiscal = fechaFinal.Item1;
            int anioFinal = fechaFinal.Item2;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT fecha, totalEmpleados FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha BETWEEN '" + anio + "-" + inicioAnioFiscal + "-01' AND '" + anioFinal + "-" + finalAnioFiscal + "-01' ORDER BY fecha ASC;");

            while (bd.Consulta.Read())
            {
                empleados.Add(
                    bd.Consulta.GetString("fecha"),
                    bd.Consulta.GetInt32("totalEmpleados")
                );
            }

            bd.Close();

            return empleados;
        }

        /// <summary>
        /// Obtiene los empleados capacitados de una seccion en cada mes de un año fiscal.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="inicioAnioFiscal"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public Dictionary<string, int> ObtenerEmpleadosCapacitadosAnuales(int idSeccion, int inicioAnioFiscal, int anio)
        {
            Dictionary<string, int> empleados = new Dictionary<string, int>();
            Tuple<int, int> fechaFinal = ObtenerFechaFinal(inicioAnioFiscal, anio);
            int finalAnioFiscal = fechaFinal.Item1;
            int anioFinal = fechaFinal.Item2;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT fecha, empleadosCapacitados FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha BETWEEN '" + anio + "-" + inicioAnioFiscal + "-01' AND '" + anioFinal + "-" + finalAnioFiscal + "-01' ORDER BY fecha ASC;");

            while (bd.Consulta.Read())
            {
                empleados.Add(
                    bd.Consulta.GetString("fecha"),
                    bd.Consulta.GetInt32("empleadosCapacitados")
                );
            }

            bd.Close();

            return empleados;
        }

        /// <summary>
        /// Obtiene los empleados capacitados de una seccion en cada mes de un año fiscal.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="inicioAnioFiscal"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public Dictionary<string, int> ObtenerEmpleadosNoCapacitadosAnuales(int idSeccion, int inicioAnioFiscal, int anio)
        {
            Dictionary<string, int> empleados = new Dictionary<string, int>();
            Tuple<int, int> fechaFinal = ObtenerFechaFinal(inicioAnioFiscal, anio);
            int finalAnioFiscal = fechaFinal.Item1;
            int anioFinal = fechaFinal.Item2;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT fecha, empleadosNoCapacitados FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha BETWEEN '" + anio + "-" + inicioAnioFiscal + "-01' AND '" + anioFinal + "-" + finalAnioFiscal + "-01' ORDER BY fecha ASC;");

            while (bd.Consulta.Read())
            {
                empleados.Add(
                    bd.Consulta.GetString("fecha"),
                    bd.Consulta.GetInt32("empleadosNoCapacitados")
                );
            }

            bd.Close();

            return empleados;
        }

        /// <summary>
        /// Obtiene las ventas del mes del año de una seccion.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="mes"></param>
        /// <param name="anio"></param>
        /// <returns></returns>
        public Tuple<double, double, double> ObtenerVentasMes(int idSeccion, int mes, int anio)
        {
            // añoActual, añoAnaterior, ventasPlan
            Tuple<double, double, double> ventas = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT ventasAñoActual, ventasAñoAnterior, ventasPlan FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha='" + anio + "-" + mes + "-01';");

            if (bd.Consulta.Read())
            {
                ventas = new Tuple<double, double, double>(
                    bd.Consulta.GetDouble(0),
                    bd.Consulta.GetDouble(1),
                    bd.Consulta.GetDouble(2)
                );
            }

            bd.Close();

            return ventas;
        }

        public int ObtenerReubicacionesMes(int idSeccion, int mes, int anio)
        {
            int reubicaciones = 0;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT reubicaciones FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha='" + anio + "-" + mes + "-01';");

            if (bd.Consulta.Read())
            {
                reubicaciones = bd.Consulta.GetInt32("reubicaciones");
            }

            bd.Close();

            return reubicaciones;
        }

        public int ObtenerTotalEmpleadosMes(int idSeccion, int mes, int anio)
        {
            int empleados = 0;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT totalEmpleados FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha='" + anio + "-" + mes + "-01';");

            if (bd.Consulta.Read())
            {
                empleados = bd.Consulta.GetInt32("totalEmpleados");
            }

            bd.Close();

            return empleados;
        }

        public int ObtenerEmpleadosCapacitadosMes(int idSeccion, int mes, int anio)
        {
            int empleados = 0;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT empleadosCapacitados FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha='" + anio + "-" + mes + "-01';");

            if (bd.Consulta.Read())
            {
                empleados = bd.Consulta.GetInt32("empleadosCapacitados");
            }

            bd.Close();

            return empleados;
        }

        public int ObtenerEmpleadosNoCapacitadosMes(int idSeccion, int mes, int anio)
        {
            int empleados = 0;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT empleadosNoCapacitados FROM desempeño WHERE idSeccion=" + idSeccion + " AND fecha='" + anio + "-" + mes + "-01';");

            if (bd.Consulta.Read())
            {
                empleados = bd.Consulta.GetInt32("empleadosNoCapacitados");
            }

            bd.Close();

            return empleados;
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
        public void CambiarEstadoSolicitud(string nuevoEstado, int idSeccionActual,
            int idSeccionSolicitada, string rut )
        {
            conn.Open();

            cmd.CommandText = "UPDATE solicitudes SET estadoSolicitud='" + nuevoEstado + "'"
                +" WHERE idSeccionActual=" + idSeccionActual.ToString() + " AND idSeccionSolicitada="
                + idSeccionSolicitada.ToString() + " AND rutSolicitud='"+rut+"';";
            Console.WriteLine(cmd.CommandText.ToString());
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
        /// Obtiene la fecha más actual.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public string ObtenerUltimaFecha()
        {
            string fecha = "";
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();
            bd.ConsultaMySql("SELECT max(fecha) FROM desempeño;");

            while (bd.Consulta.Read())
            {
                fecha = bd.Consulta.GetDateTime(0).ToString();
            }

            bd.Close();

            return fecha;
        }

        /// <summary>
        /// Obtiene las ventas de un mes de una seccion.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public Tuple<double, double, double> ObtenerVentas(int idSeccion, string fecha)
        {
            // año actual, año anterior, ventas plan.
            Tuple<double, double, double> ventas = null;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();
            bd.ConsultaMySql("SELECT ventasAñoActual, ventasAñoAnterior, ventasPlan FROM desempeño WHERE idSeccion=" + idSeccion + ";");

            if (bd.Consulta.Read())
            {
                ventas = new Tuple<double, double, double>(
                    bd.Consulta.GetDouble(0),
                    bd.Consulta.GetDouble(1),
                    bd.Consulta.GetDouble(2)
                );
            }

            bd.Close();

            return ventas;
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
                + "idSeccionSolicitada,capacidadSeccionActual,capacidadNuevaSeccion) VALUES('" + fechaSolicitud + "','" + estadoSolicitud + "'"
                + rutSolicitud + "," + idSeccionActual.ToString() + "," + idSeccionSolicitada.ToString()
                + "," + capacidadActual + "," + capacidadNueva + ");";
            Console.WriteLine("CONSULTA" + cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();
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
        public void InsertarEvaluacion(string rutTrabajador, int idSeccionEvaluacion, double capacidadTrabajador,
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

        public void InsertarEvaluacionTrabajador(string fechaEvaluacion, string rutTrabajador, double hb, double hd,
            double cf)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO evaluacionTrabajador(fechaEvaluacion,rutTrabajador,hb,hd,cf) VALUES ('"
                + fechaEvaluacion + "','" + rutTrabajador + "'," + hb.ToString() + "," + hd.ToString()
                + "," + cf.ToString() + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Obtiene el puntaje de la evaluacion mas reciente para las habilidades blandas de un trabajador
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <returns></returns>
        public double ObtenerPuntajeHB(string rutTrabajador)
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

        public List<string> ObtenerRankingTrabajadoresSeccion(int idSeccion)
        {
            List<string> ranking = new List<string>();

            conn.Open();
            cmd.CommandText = "SELECT rutTrabajador FROM capacidadTrabajador WHERE idSeccionEvaluacion="
                + idSeccion.ToString() + " ORDER BY capacidadTrabajador DESC;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                ranking.Add(consulta.GetString(0));
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

        /**************************************************************************
         *                              MIS CONSULTAS
         * *************************************************************************/

        public double ObtenerCapacidadGeneral(string rutTrabajador)
        {
            double puntajeCF = 0;

            conn.Open();
            cmd.CommandText = "SELECT capacidadTrabajador FROM capacidadTrabajador WHERE rutTrabajador ='" + rutTrabajador +"';";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                puntajeCF = consulta.GetDouble(0);
            }

            conn.Close();

            return puntajeCF;
        }

        public double ObtenerCapacidadHBRanking(string rutTrabajador)
        {
            double puntajeCF = 0;

            conn.Open();
            cmd.CommandText = "SELECT gradoIgualdadHB FROM capacidadTrabajador WHERE rutTrabajador ='" + rutTrabajador + "';";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                puntajeCF = consulta.GetDouble(0);
            }

            conn.Close();

            return puntajeCF;
        }

        public double ObtenerCapacidadHDRanking(string rutTrabajador)
        {
            double puntajeCF = 0;

            conn.Open();
            cmd.CommandText = "SELECT gradoIgualdadHD FROM capacidadTrabajador WHERE rutTrabajador ='" + rutTrabajador + "';";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                puntajeCF = consulta.GetDouble(0);
            }

            conn.Close();

            return puntajeCF;
        }

        public double ObtenerCapacidadCFRanking(string rutTrabajador)
        {
            double puntajeCF = 0;

            conn.Open();
            cmd.CommandText = "SELECT gradoIgualdadCF FROM capacidadTrabajador WHERE rutTrabajador ='" + rutTrabajador + "';";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                puntajeCF = consulta.GetDouble(0);
            }

            conn.Close();

            return puntajeCF;
        }


        public double ObtenerCapacidadGeneralRanking(string rutTrabajador, int idSeccion)
        {
            double puntajeCF = 0;

            conn.Open();
            cmd.CommandText = "SELECT capacidadTrabajador FROM capacidadTrabajador WHERE rutTrabajador ='" + rutTrabajador + "' AND idSeccionEvaluacion=" + idSeccion + ";";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                puntajeCF = consulta.GetDouble(0);
            }

            conn.Close();

            return puntajeCF;
        }

        public void ReubicarTrabajador(int idSeccion, string rut)
        {
            conn.Open();

            cmd.CommandText = "UPDATE Trabajadores SET idSeccion = "+idSeccion+" WHERE rut='"+rut+"';";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        /// <summary>
        /// Obtiene el mes y el año final a partir de la fecha de inicio 
        /// para el periodo anual.
        /// </summary>
        /// <param name="inicioAnioFiscal"></param>
        /// <param name="anio"></param>
        /// <returns>Retorna mes y año finales</returns>
        private Tuple<int, int> ObtenerFechaFinal(int inicioAnioFiscal, int anio)
        {
            int anioFinal = anio;
            int finalAnioFiscal = inicioAnioFiscal;

            for (int i = 1; i < 11; i++)
            {
                if (finalAnioFiscal == 12)
                {
                    finalAnioFiscal = 1;
                    anioFinal += 1;
                }
                else
                {
                    finalAnioFiscal += 1;
                }
            }

            return new Tuple<int, int>(finalAnioFiscal, anioFinal);
        }

        public void InsertarPregunta(string fecha, double desempeno, int reubicaciones, int totalEmpleados, int empleadosCapacitados, int empleadosNoCapacitados)
        {
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.Insertar("INSERT INTO desempeñoGQM(fecha, desempeno, reubicaciones, totalEmpleados, empleadosCapacitados, empleadosNoCapacitados) VALUES('" + fecha + "', " + desempeno + ", " + reubicaciones + ", " + totalEmpleados + ", " + empleadosCapacitados + ", " + empleadosNoCapacitados + ");");

            bd.Close();

        }
    }
}
