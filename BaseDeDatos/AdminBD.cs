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
        /// Consulta para insertar un nuevo usuario, se reciben los paramatros necesarios para la creacion.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="rut"></param>
        /// <param name="clave"></param>
        /// <param name="tipoUsuario"></param>
        /// <param name="estado"></param>
        public void InsertarUsuario(string nombre, string rut, string clave, string tipoUsuario, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO usuarios (nombre,rut,clave,tipoUsuario,estado) " 
                + "VALUES('" + nombre + "','" + rut + "','" + clave + "','" + tipoUsuario + "',"
                + estado + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para insertar una nueva seccion. Se reciben como parametros el nombre y el rut del jefe encargado
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="rutJefe"></param>
        public void InsertarSeccion( string nombre, string rutJefe )
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO secciones (nombre,rutJefe) VALUES('" + nombre + "','" + rutJefe 
                + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para insertar un nuevo trabajador, se deben pasar todos los parametros necesarios para agregar.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellidoPaterno"></param>
        /// <param name="apellidoMaterno"></param>
        /// <param name="rut"></param>
        /// <param name="fechaNacimiento"></param>
        /// <param name="idSeccion"></param>
        /// <param name="sexo"></param>
        /// <param name="estado"></param>
        public void InsertarTrabajador(string nombre, string apellidoPaterno, string apellidoMaterno, string rut, 
            string fechaNacimiento, int idSeccion, string sexo, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO trabajadores (nombre,apellidoPaterno,apellidoMaterno,rut,fechaNacimiento,idSeccion,"
                + "estado) VALUES('" + nombre + "','" + apellidoPaterno + "','" + apellidoMaterno + "','" + rut + "','" 
                + fechaNacimiento + "'," + idSeccion.ToString() + "," + estado + ");";
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
        /// Consulta para insertar un componente de un perfil de seccion. Se debe indicar el id de la seccion
        /// y el puntaje e importancia asociados a la hb,hd o cf
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nombre"></param>
        /// <param name="puntaje"></param>
        /// <param name="importancia"></param>
        public void InsertarComponentePerfilSeccion(int idSeccion, string nombre, float puntaje, float importancia)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfilSecciones (idSeccion,nombre,puntaje,importancia) "
                + " VALUES(" + idSeccion.ToString() + ",'" + nombre + "'," + puntaje.ToString() + "," 
                + importancia.ToString() + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para insertar componentes de los perfiles de trabajador, se debe indicar el rut del trabajador y
        /// el puntaje asociado a la hb,hd o cf
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="nombre"></param>
        /// <param name="puntaje"></param>
        public void InsertarComponentePerfilTrabajador(string rut, string nombre, float puntaje)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfilTrabajadores (rut,nombre,puntaje) "
                + " VALUES('" + rut + "','" + nombre + "'," + puntaje.ToString() + ");";
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
        /// Consulta para modificar el puntaje de una componente del perfil del trabajador.
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <param name="nuevoPuntaje"></param>
        public void ModificarPuntajePerfilTrabajador( string rutTrabajador, float nuevoPuntaje )
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilTrabajadores SET puntaje =" + nuevoPuntaje.ToString() + "WHERE rut='"
                + rutTrabajador + "';";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para modificar el puntaje de una componente del perfil de la secccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nuevoPuntaje"></param>
        public void ModificarPuntajePerfilSeccion( int idSeccion, float nuevoPuntaje )
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilSecciones SET puntaje =" + nuevoPuntaje.ToString() + "WHERE idSeccion="
                + idSeccion + ";";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para modificar la importancia de una componente del perfil de la seccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nuevaImportancia"></param>
        public void ModificarImportanciaPerfilSeccion(int idSeccion, float nuevaImportancia )
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilSecciones SET importancia =" + nuevaImportancia.ToString() 
                + "WHERE idSeccion=" + idSeccion + ";";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para obtener el id de la seccion. Es necesario el rut del jefe de seccion
        /// </summary>
        /// <param name="rutJefeSeccion"></param>
        /// <returns></returns>
        public int ObtenerIdSeccion( string rutJefeSeccion )
        {
            int idSeccion = 0;

            conn.Open();
            cmd.CommandText = "SELECT id FROM secciones WHERE rutJefe='" + rutJefeSeccion + "';";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                idSeccion = consulta.GetInt16(0);
            }

            Console.WriteLine("Id: {0}", idSeccion);
            Console.ReadKey();

            conn.Close();

            return idSeccion;
        }

        /// <summary>
        /// Consulta para obtener el nombre de la seccion, es necesario el rut del jefe de seccion.
        /// </summary>
        /// <param name="rutJefeSeccion"></param>
        /// <returns></returns>
        public string ObtenerNombreSeccion(string rutJefeSeccion)
        {
            string nombreSeccion = "";

            conn.Open();
            cmd.CommandText = "SELECT nombre FROM secciones WHERE rutJefe='" + rutJefeSeccion + "';";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                nombreSeccion = consulta.GetString(0);
            }

            Console.WriteLine("Nombre: {0}", nombreSeccion);
            Console.ReadKey();

            conn.Close();

            return nombreSeccion;
        }

        /// <summary>
        /// Consulta para obtener el perfil de la seccion. Es necesario el id de dicha seccion.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public Perfil ObtenerPerfilSeccion( int idSeccion )
        {
            Perfil perfilSeccion = new Perfil();

            conn.Open();
            cmd.CommandText = "SELECT c.nombre,c.descripcion,c.tipo,s.puntaje,s.importancia FROM componentesPerfil AS c," 
                + "componentesPerfilSecciones AS s WHERE s.idSeccion=" + idSeccion.ToString() + "AND c.nombre=s.nombre;";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Componente nuevoComponente = new Componente(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                    consulta.GetDouble(3), consulta.GetDouble(4) );
                perfilSeccion.AgregarComponente( nuevoComponente );
            }

            conn.Close();

            return perfilSeccion;
        }

        /// <summary>
        /// Consulta para obtener el perfil del trabajador. Es necesario el rut de dicho trbajador
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <returns></returns>
        public Perfil ObtenerPerfilTrabajador( string rutTrabajador)
        {
            Perfil perfilTrabajador = new Perfil();

            conn.Open();
            cmd.CommandText = "SELECT c.nombre,c.descripcion,c.tipo,t.puntaje FROM componentesPerfil AS c," 
                + "componentesPerfilTrabajadores AS t WHERE t.rut='" + rutTrabajador + "' AND c.nombre=t.nombre;";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Componente nuevoComponente = new Componente(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                    consulta.GetDouble(3), -1);
                perfilTrabajador.AgregarComponente(nuevoComponente);
            }

            conn.Close();

            return perfilTrabajador;
        }

        /// <summary>
        /// Consulta para obtener los trabajadores de una seccion. Es necesario el id de la seccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public Dictionary<string,Trabajador> ObtenerTrabajadoresSeccion( int idSeccion )
        {
            Dictionary<string, Trabajador> trabajadores = new Dictionary<string, Trabajador>();

            conn.Open();
            cmd.CommandText = "SELECT * FROM trabajadores WHERE idSeccion=" + idSeccion.ToString() + ";";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Trabajador nuevoTrabajador = new Trabajador(consulta.GetString(0),consulta.GetString(1), consulta.GetString(2),
                    consulta.GetString(3), consulta.GetString(4), consulta.GetString(5), 
                    ObtenerPerfilTrabajador(consulta.GetString(0)) );
                trabajadores.Add(consulta.GetString(0), nuevoTrabajador);
            }

            conn.Close();


            return trabajadores;
        }

        /// <summary>
        /// Consulta para verificar si el rut del usuario esta registrado y habilitado para el login.
        /// </summary>
        /// <param name="rutUsuario"></param>
        /// <returns></returns>
        public bool VerificarUsuario( string rutUsuario)
        {
            bool verificador = false;

            conn.Open();
            cmd.CommandText = "SELECT * FROM usuarios WHERE rut='" + rutUsuario + "';";
            consulta = cmd.ExecuteReader();

            if( consulta.Read() )
            {
                verificador = true;
            }
            else
            {
                verificador = false;
            }

            conn.Close();

            return verificador;
        }

        /// <summary>
        /// Consulta para verificar si  la clave del usuario fue ingresada correctamente
        /// </summary>
        /// <param name="clave"></param>
        /// <returns></returns>
        public bool VerificarClave( string clave)
        {
            bool verificador = false;

            conn.Open();
            cmd.CommandText = "SELECT * FROM usuarios WHERE clave='" + clave + "';";
            consulta = cmd.ExecuteReader();

            if ( consulta.Read() )
            {
                verificador = true;
            }
            else
            {
                verificador = false;
            }

            conn.Close();

            return verificador;
        }


    }
}
