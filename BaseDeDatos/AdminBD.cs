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

        public AdminBD (string servidor, string usuario, string password, string nombreBD)
        {
            bd = new BaseDeDatos();
            conn = bd.conectarBD(servidor, usuario, password, nombreBD);
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

        public void insertarEmpresa( string nombre, string razonSocial, string direccion)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO empresa (nombre,razonSocial,direccion) "
                + "VALUES('" + nombre + "','" + razonSocial + "','" + direccion + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarUsuario(string nombre, string rut, string clave, string tipoUsuario, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO usuarios (nombre,rut,clave,tipoUsuario,estado) " 
                + "VALUES('" + nombre + "','" + rut + "','" + clave + "','" + tipoUsuario + "',"
                + estado + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarSeccion( string nombre, string rutJefe )
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO secciones (nombre,rutJefe) VALUES('" + nombre + "','" + rutJefe 
                + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarTrabajador(string nombre, string apellidoPaterno, string apellidoMaterno, string rut, 
            string fechaNacimiento, int idSeccion, string sexo, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO trabajadores (nombre,apellidoPaterno,apellidoMaterno,rut,fechaNacimiento,idSeccion,"
                + "estado) VALUES('" + nombre + "','" + apellidoPaterno + "','" + apellidoMaterno + "','" + rut + "','" 
                + fechaNacimiento + "'," + idSeccion.ToString() + "," + estado + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarComponente(string nombre, string descripcion, string tipo, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfil (nombre,descripcion,tipo,estado) " 
                + "VALUES('" + nombre + "','" + descripcion + "','" + tipo + "'," + estado + ");" ;
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarComponentePerfilSeccion(int idSeccion, string nombre, float puntaje, float importancia)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfilSecciones (idSeccion,nombre,puntaje,importancia) "
                + " VALUES(" + idSeccion.ToString() + ",'" + nombre + "'," + puntaje.ToString() + "," 
                + importancia.ToString() + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarComponentePerfilTrabajador(string rut, string nombre, float puntaje)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfilTrabajadores (rut,nombre,puntaje) "
                + " VALUES('" + rut + "','" + nombre + "'," + puntaje.ToString() + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarDesempeñoMensual(int idSeccion, string fecha, float ventasAñoActual, float ventasAñoAnterior,
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
        
        public void insertarReubicacion( string rut, int idSeccionAnterior, int idSeccionNueva, string fecha)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO reubicaciones (rut,idSeccionAnterior,idSeccionNueva,fecha) VALUES('"
                + rut + "'," + idSeccionAnterior.ToString() + "," + idSeccionNueva.ToString() + ",'"
                + fecha + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarPreguntaComponente( string nombre, string pregunta, bool estado)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO preguntasComponentes (nombre,pregunta,estado) VALUES('"
                + nombre + "','" + pregunta + "'," + estado + ");";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarAlternativaPreguntaComponente( int idPregunta, string alternativa )
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO alternativasPreguntasComponentes (idPregunta,alternativa) VALUES("
                + idPregunta.ToString() + ",'" + alternativa + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void insertarRespuesta(int idPregunta, string rutTrabajadorAsociado, string rutRespuesta, 
            string alternativaRespuesta)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO respuestas (idPregunta,rutTrabajadorAsociado,rutRespuesta,alternativaRespuesta) "
                + "VALUES(" + idPregunta.ToString() + ",'" + rutTrabajadorAsociado + "','" + rutRespuesta + "','"
                + alternativaRespuesta + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void modificarPuntajePerfilTrabajador( string rutTrabajador, float nuevoPuntaje )
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilTrabajadores SET puntaje =" + nuevoPuntaje.ToString() + "WHERE rut='"
                + rutTrabajador + "';";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void modificarPuntajePerfilSeccion( int idSeccion, float nuevoPuntaje )
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilSecciones SET puntaje =" + nuevoPuntaje.ToString() + "WHERE idSeccion="
                + idSeccion + ";";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void modificarImportanciaPerfilSeccion(int idSeccion, float nuevaImportancia )
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilSecciones SET importancia =" + nuevaImportancia.ToString() 
                + "WHERE idSeccion=" + idSeccion + ";";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

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

        public Dictionary<string,Trabajador> ObtenerTrabajadoresSeccion( int idSeccion )
        {
            Dictionary<string, Trabajador> trabajadores = new Dictionary<string, Trabajador>;

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
    }
}
