using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminUsuario
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminUsuario()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();

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
        /// Consulta para verificar si el rut del usuario esta registrado y habilitado para el login.
        /// </summary>
        /// <param name="rutUsuario"></param>
        /// <returns></returns>
        public bool VerificarUsuario(string rutUsuario)
        {
            bool verificador = false;

            conn.Open();
            cmd.CommandText = "SELECT * FROM usuarios WHERE rut='" + rutUsuario + "';";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
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
        public bool VerificarClave(string clave)
        {
            bool verificador = false;

            conn.Open();
            cmd.CommandText = "SELECT * FROM usuarios WHERE clave='" + clave + "';";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
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
        /// Funcion de la consulta que retorna los nombres de los jefes de seccion
        /// </summary>
        /// <returns></returns>
        public List<string> ObtenerNombresJefesDeSeccion()
        {
            List<string> listaNombreJefesDeSeccion = new List<string>();

            conn.Open();
            cmd.CommandText = "SELECT * FROM usuarios WHERE tipoUsuario='JEFE_SECCION' AND estado=1;";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                string nuevoNombreJefe = consulta.GetString(0);
                listaNombreJefesDeSeccion.Add(nuevoNombreJefe);
            }

            conn.Close();

            return listaNombreJefesDeSeccion;
        }

        /// <summary>
        /// Funcion que contiene la consulta para modificar los datos del usuario, al modificar alguno de los datos se hace
        /// una actualizacion de todos los datos, para asi no hacer una consulta para cada dato que es modificado.
        /// </summary>
        /// <param name="nuevoNombre"></param>
        /// <param name="nuevoRut"></param>
        /// <param name="nuevoTipoUsuario"></param>
        /// <param name="nuevaClave"></param>
        /// <param name="rutActual"></param>
        public void ModificarDatosUsuario(string nuevoNombre, string nuevoRut, string nuevoTipoUsuario, string nuevaClave,
            string rutActual)
        {
            conn.Open();

            cmd.CommandText = "UPDATE usuarios SET nombre='" + nuevoNombre + "',rut='" + nuevoRut + "',tipoUsuario='"
                + nuevoTipoUsuario + "',clave='" + nuevaClave + "' WHERE rut='" + rutActual + "';";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            conn.Open();
            cmd.CommandText = "SELECT * FROM usuarios WHERE estado=1;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Usuario nuevoUsuario = new Usuario(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                    consulta.GetString(3));
                usuarios.Add(nuevoUsuario);
            }

            conn.Close();

            return usuarios;
        }

        /// <summary>
        /// Consulta para borrar a un usuario permanentemente
        /// </summary>
        /// <param name="rutUsuario"></param>
        public void BorrarUsuario(string rutUsuario)
        {
            conn.Open();

            cmd.CommandText = "DELETE FROM usuarios WHERE rut='" + rutUsuario + "';";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Funcion de la consulta que permite cambiar el estado de un usuario de true a false
        /// true para cuando esta activo
        /// false para cuando esta inactivo
        /// </summary>
        /// <param name="nuevoEstado"></param>
        public void CambiarEstadoUsuario(string rutUsuario, bool nuevoEstado)
        {
            conn.Open();

            cmd.CommandText = "UPDATE usuarios SET estado=" + nuevoEstado + " WHERE rut='" + rutUsuario + "';";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /************************************************************
         *          MIS CONSULTAS 
         * *************************************************************/
        public string ObtenerNombreUsuario(string rut)
        {
            string nombre = string.Empty;

            conn.Open();
            cmd.CommandText = "SELECT nombre FROM usuarios WHERE rut ='" + rut + "' AND estado=1;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                nombre = consulta.GetString(0);
            }

            conn.Close();

            return nombre;
        }


        public string ObtenerTipoUsuario(string rut)
        {
            string tipo = string.Empty;

            conn.Open();
            cmd.CommandText = "SELECT tipoUsuario FROM usuarios WHERE rut ='" + rut + "' AND estado=1;";

            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                tipo = consulta.GetString(0);
            }

            conn.Close();

            return tipo;
        }

        public string ObtenerNombreSeccionPorUsuario(string rutTrabajador)
        {
            string nombreSeccion = "";

            conn.Open();
            cmd.CommandText = "SELECT nombre FROM secciones WHERE rutJefe = '" + rutTrabajador + "';";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                nombreSeccion = consulta.GetString(0);
            }

            /*
            Console.WriteLine("Nombre: {0}", nombreSeccion);
            Console.ReadKey();*/

            conn.Close();

            return nombreSeccion;
        }
    }
}
