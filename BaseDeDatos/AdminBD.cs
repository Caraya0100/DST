using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminBD
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminBD()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();
            
        }

        /*public void obtenerEmpresa()
        {
            string nombre = "";
            string razonSocial = "";
            string direccion = "";
            conn.Open();
            cmd.CommandText = "SELECT * FROM empresa;";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                nombre = consulta.GetString(0);
                razonSocial = consulta.GetString(1);
                direccion = consulta.GetString(2);
            }

            
            Console.WriteLine("Nombre: {0} Razon Social: {1} Direccion: {2}", nombre, razonSocial, direccion );
            Console.ReadKey();
            

            conn.Close();

        }*/

        /// <summary>
        /// Consulta para insertar una empresa. En nuestro caso es una sola.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="razonSocial"></param>
        /// <param name="direccion"></param>
        public void InsertarEmpresa( string nombre, string razonSocial, string direccion)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO empresa (nombre,razonSocial,direccion) "
                + "VALUES('" + nombre + "','" + razonSocial + "','" + direccion + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para insertar una nueva componente, puede ser hb,hd o cf
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        /// <param name="estado"></param>
        public void InsertarComponente(string nombre, string descripcion, string tipo, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfil (nombre,descripcion,tipo,estado) " 
                + "VALUES('" + nombre + "','" + descripcion + "','" + tipo + "'," + estado + ");" ;
            cmd.ExecuteNonQuery();

            conn.Close();
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
                + "reubicaciones,totalEmpleados,empleadosConAdvertencia) VALUES(" + idSeccion + ",'" + fecha +  "',"
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
        public void InsertarReubicacion( string rut, int idSeccionAnterior, int idSeccionNueva, string fecha)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO reubicaciones (rut,idSeccionAnterior,idSeccionNueva,fecha) VALUES('"
                + rut + "'," + idSeccionAnterior.ToString() + "," + idSeccionNueva.ToString() + ",'"
                + fecha + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para insertar preguntas asociadas a un componente
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="pregunta"></param>
        /// <param name="estado"></param>
        public void InsertarPreguntaComponente( string nombre, string pregunta, bool estado)
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
        public void InsertarAlternativaPreguntaComponente( int idPregunta, string alternativa )
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
                + idSeccion.ToString() + ",'" + mes + "','" + año + "'," + ventasAñoAnterior.ToString() +");";
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
                +  rutSolicitud + "'," + idSeccionActual.ToString() + "," + idSeccionSolicitada.ToString()
                + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        
    }
}
