using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    class AdminSeccion
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminSeccion()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();

        }

        /// <summary>
        /// Consulta para insertar una nueva seccion. Se reciben como parametros el nombre y el rut del jefe encargado
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="rutJefe"></param>
        public void InsertarSeccion(string nombre, string descripcion, string rutJefe)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO secciones (nombre,descripcion,rutJefe) VALUES('" + nombre + "','" + descripcion
                + "','" + rutJefe + "');";
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
        /// Consulta para modificar el puntaje de una componente del perfil de la secccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nuevoPuntaje"></param>
        public void ModificarPuntajePerfilSeccion(int idSeccion, string nombreComponente, float nuevoPuntaje)
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilSecciones SET puntaje =" + nuevoPuntaje.ToString() + " WHERE "
                + "nombre='" + nombreComponente + "' AND idSeccion=" + idSeccion + ";";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para modificar la importancia de una componente del perfil de la seccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nuevaImportancia"></param>
        public void ModificarImportanciaPerfilSeccion(int idSeccion, string nombreComponente, float nuevaImportancia)
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilSecciones SET importancia =" + nuevaImportancia.ToString()
                + " WHERE nombre='" + nombreComponente + "' AND idSeccion =" + idSeccion + ";";
            cmd.ExecuteNonQuery();

            conn.Close();
        }


        /// <summary>
        /// Consulta para obtener el id de la seccion. Es necesario el rut del jefe de seccion
        /// </summary>
        /// <param name="rutJefeSeccion"></param>
        /// <returns></returns>
        public int ObtenerIdSeccion(string rutJefeSeccion)
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
        /// Consulta para obtener el rut del jefe de seccion a partir del nombre de la seccion
        /// </summary>
        /// <param name="nombreSeccion"></param>
        /// <returns></returns>
        public string ObtenerRutJefeSeccion(string nombreSeccion)
        {
            string rutJefeSeccion = "";

            conn.Open();
            cmd.CommandText = "SELECT rutJefe FROM secciones WHERE nombre='" + nombreSeccion + "';";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                rutJefeSeccion = consulta.GetString(0);
            }

            Console.WriteLine("RUT: {0}", rutJefeSeccion);
            Console.ReadKey();

            conn.Close();

            return rutJefeSeccion;
        }

        /// <summary>
        /// Consulta para obtener el perfil de la seccion. Es necesario el id de dicha seccion.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public Perfil ObtenerPerfilSeccion(int idSeccion)
        {
            Perfil perfilSeccion = new Perfil();

            conn.Open();
            cmd.CommandText = "SELECT c.nombre,c.descripcion,c.tipo,s.puntaje,s.importancia FROM componentesPerfil AS c,"
                + "componentesPerfilSecciones AS s WHERE s.idSeccion=" + idSeccion.ToString() + "AND c.nombre=s.nombre;";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Componente nuevoComponente = new Componente(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                    consulta.GetDouble(3), consulta.GetDouble(4));
                perfilSeccion.AgregarComponente(nuevoComponente);
            }

            conn.Close();

            return perfilSeccion;
        }

    }
}
