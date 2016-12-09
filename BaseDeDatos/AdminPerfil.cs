using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminPerfil
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        public AdminPerfil()
        {
            bd = new BaseDeDatos();
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();
        }

        /// <summary>
        /// Consulta para insertar una nueva componente, puede ser hb,hd o cf
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        /// <param name="estado"></param>
        public void InsertarComponente(string id, string nombre, string descripcion, string tipo, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfil (id, nombre,descripcion,tipo,estado) "
                + "VALUES('" + id + "', '" + nombre + "','" + descripcion + "','" + tipo + "'," + estado + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Obtiene un componente a partir de su id.
        /// </summary>
        /// <param name="id">Id del componente</param>
        /// <returns>Devuelve el componente. Si no existe un componente con ese id 
        /// se retorna null.</returns>
        public Componente ObtenerComponente(string id)
        {
            Componente componente = null;

            conn.Open();
            cmd.CommandText = "SELECT * FROM componentesPerfil WHERE id='" + id + "';";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
            {
                componente = new Componente(consulta.GetString("id"), consulta.GetString("nombre"), consulta.GetString("descripcion"), consulta.GetString("tipo"));
            }

            conn.Close();

            return componente;
        }

        /// <summary>
        /// Obtiene un componente a partir de su id.
        /// </summary>
        /// <param name="id">Id del componente</param>
        /// <returns>Devuelve el componente. Si no existe un componente con ese id 
        /// se retorna null.</returns>
        public Componente ObtenerComponentePorNombre(string nombre)
        {
            Componente componente = null;

            conn.Open();
            cmd.CommandText = "SELECT * FROM componentesPerfil WHERE nombre='" + nombre + "';";
            consulta = cmd.ExecuteReader();

            if (consulta.Read())
            {
                componente = new Componente(consulta.GetString("id"), consulta.GetString("nombre"), consulta.GetString("descripcion"), consulta.GetString("tipo"));
            }

            conn.Close();

            return componente;
        }
        /// <summary>
        /// Obtiene los componentes segun su tipo.
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns>Devuelve un diccionario vacia si no existieran componentes 
        /// de ese tipo.</returns>
        public Dictionary<string, Componente> ObtenerComponentesPorTipo(string tipo)
        {
            Dictionary<string, Componente> componentes = new Dictionary<string, Componente>();

            conn.Open();
            cmd.CommandText = "SELECT * FROM componentesPerfil WHERE tipo='" + tipo + "';";
            consulta = cmd.ExecuteReader();

            while (consulta.Read())
            {
                string id = consulta.GetString("id");
                componentes.Add(id, new Componente(id, consulta.GetString("nombre"), consulta.GetString("descripcion"), consulta.GetString("tipo")));
            }

            conn.Close();

            return componentes;
        }
    }
}
