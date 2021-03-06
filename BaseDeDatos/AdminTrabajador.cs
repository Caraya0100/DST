﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminTrabajador
    {
        private MySqlConnection conn;
        private MySqlConnection conn2;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlDataReader consulta2;
        private MySqlCommand cmd;
        private MySqlCommand cmd2;

        public AdminTrabajador()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            conn2 = bd.conectarBD();
            cmd = conn.CreateCommand();
            cmd2 = conn2.CreateCommand();

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
                + "sexo,estado) VALUES('" + nombre + "','" + apellidoPaterno + "','" + apellidoMaterno + "','" + rut + "','"
                + fechaNacimiento + "'," + idSeccion.ToString() + ",'" + sexo + "'," + estado + ");";
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
        public void InsertarComponentePerfilTrabajador(string rut, string nombre, double puntaje)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfilTrabajadores (rut,id,puntaje) "
                + " VALUES('" + rut + "','" + nombre + "'," + puntaje.ToString() + ");";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para modificar el puntaje de una componente del perfil del trabajador.
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <param name="nombreComponente"></param>
        /// <param name="nuevoPuntaje"></param>
        public void ModificarPuntajePerfilTrabajador(string rutTrabajador, string nombreComponente, double nuevoPuntaje)
        {
            string ptje = "" + nuevoPuntaje.ToString("0.0");      

            conn.Open();

            cmd.CommandText = "INSERT INTO componentesPerfilTrabajadores (rut,id,puntaje) "
                + " VALUES('" + rutTrabajador + "','" + nombreComponente + "'," + ptje.Replace(",",".") + ") "
                +"ON DUPLICATE KEY UPDATE puntaje="+ptje.Replace(",",".")+";";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();         
            
        }

        /// <summary>
        /// Consulta para obtener el perfil del trabajador. Es necesario el rut de dicho trbajador
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <returns></returns>
        public Perfil ObtenerPerfilTrabajador(string rutTrabajador)
        {
            Perfil perfilTrabajador = new Perfil();

            //conn.Close();

            conn2.Open();
            cmd2.CommandText = "SELECT c.id, c.nombre,c.descripcion,c.tipo,t.puntaje FROM componentesPerfil AS c,"
                + "componentesPerfilTrabajadores AS t WHERE t.rut='" + rutTrabajador + "' AND c.id=t.id;";
            consulta2 = cmd2.ExecuteReader();
            while (consulta2.Read())
            {
                Componente nuevoComponente = new Componente(
                    consulta2.GetString(0), 
                    consulta2.GetString(1), 
                    consulta2.GetString(2), 
                    consulta2.GetString(3),
                    consulta2.GetDouble(4), 
                    -1
                );
                perfilTrabajador.AgregarComponente(nuevoComponente);
            }

            conn2.Close();

            return perfilTrabajador;
        }

        /// <summary>
        /// Consulta para obtener los trabajadores de una seccion. Es necesario el id de la seccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public Dictionary<string, Trabajador> ObtenerTrabajadoresSeccion(int idSeccion)
        {
            Dictionary<string, Trabajador> trabajadores = new Dictionary<string, Trabajador>();
            //AdminTrabajador adminT = new AdminTrabajador();

            conn.Open();
            cmd.CommandText = "SELECT * FROM trabajadores WHERE idSeccion=" + idSeccion.ToString() + ";";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Trabajador nuevoTrabajador = new Trabajador(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                    consulta.GetString(3), consulta.GetString(4), consulta.GetString(6),
                    ObtenerPerfilTrabajador(consulta.GetString(0)));
                trabajadores.Add(consulta.GetString(0), nuevoTrabajador);
            }

            conn.Close();


            return trabajadores;
        }

        /// <summary>
        /// Consulta que devuelve una lista de todos los trabajadores de la empresa
        /// </summary>
        /// <returns></returns>
        public List<Trabajador> ObtenerTrabajadoresEmpresa()
        {
            List<Trabajador> listaTrabajadoresEmpresa = new List<Trabajador>();

            conn.Open();
            cmd.CommandText = "SELECT * FROM trabajadores;";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                Trabajador nuevoTrabajador = new Trabajador(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                    consulta.GetString(3), consulta.GetString(4), consulta.GetString(6),
                    ObtenerPerfilTrabajador(consulta.GetString(0)));
                listaTrabajadoresEmpresa.Add(nuevoTrabajador);
            }

            conn.Close();

            return listaTrabajadoresEmpresa;
        }

        /// <summary>
        /// Funcion que contiene la consulta mara modificar los datos del trabajador
        /// </summary>
        /// <param name="nuevoRut"></param>
        /// <param name="nuevoNombre"></param>
        /// <param name="nuevoAP"></param>
        /// <param name="nuevoAM"></param>
        /// <param name="nuevaFechaNacimiento"></param>
        /// <param name="nuevaIdSeccion"></param>
        /// <param name="rutActual"></param>
        public void ModificarDatosTrabajador(string nuevoRut, string nuevoNombre, string nuevoAP, string nuevoAM,
            string nuevaFechaNacimiento, int nuevaIdSeccion, string rutActual)
        {
            conn.Open();

            cmd.CommandText = "UPDATE trabajadores SET rut='" + nuevoRut + "',nombre='" + nuevoNombre + "',apellidoPaterno='"
                + nuevoAP + "',apellidoMaterno='" + nuevoAM + "',fechaNacimiento='" + nuevaFechaNacimiento
                + "',idSeccion=" + nuevaIdSeccion + " WHERE rut='" + rutActual + "';";
            Console.WriteLine("LACONSULTA: " + cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Funcion que contiene la consulta para modificar el id de la seccion a la que pertenece un trabajador.
        /// Se usara al momento de que el administrador acepte una solicitud de cambio de seccion.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="rutTrabajador"></param>
        public void ModificarSeccionTrabajador(int idSeccion, string rutTrabajador)
        {
            conn.Open();

            cmd.CommandText = "UPDATE trabajadores SET idSeccion=" + idSeccion.ToString() + " WHERE rut='" + rutTrabajador
                + "';";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Elimina el trabajador con el rut que se pasa como parametro
        /// </summary>
        /// <param name="rutTrabajador"></param>
        public void BorrarTrabajador(string rutTrabajador)
        {
            conn.Open();

            cmd.CommandText = "DELETE FROM trabajadores WHERE rut='" + rutTrabajador + "';";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();
        }
        /************************************************************
         *                      CONSULTAS MIAS
         * **************************************************************/
        public Trabajador ObtenerInfoTrabajador(string rut)
        {
            Trabajador nuevoTrabajador = null;
            conn.Open();
            cmd.CommandText = "SELECT * FROM trabajadores WHERE rut = '" + rut + "';";
            consulta = cmd.ExecuteReader();
            while (consulta.Read())
            {
                nuevoTrabajador = new Trabajador(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                consulta.GetString(3), consulta.GetString(4), consulta.GetString(6),
                ObtenerPerfilTrabajador(consulta.GetString(0)));
            }

            conn.Close();

            return nuevoTrabajador;
        }
        /// <summary>
        /// todos los compañeros que evaluaron a un trabajador.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        public List<Trabajador> ObtenerTrabajadoresEncuestados(string rut)
        {
            List<Trabajador> encuestados = new List<Trabajador>();
            conn.Open();
            cmd.CommandText = "SELECT t1.rut, t1.nombre, t1.apellidoPaterno, t1.apellidoMaterno, t1.fechaNacimiento, t1.sexo" 
            +" FROM trabajadores AS t1 INNER JOIN listatrabajadoresencuestados AS t2 ON t2.rutTrabajadorEvaluado='"+rut+"'" 
            +" AND t1.rut = t2.rutTrabajadorEvaluador;";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString()); 
            while (consulta.Read())
            {
                Trabajador nuevoTrabajador = new Trabajador(consulta.GetString(0), consulta.GetString(1), consulta.GetString(2),
                consulta.GetString(3), consulta.GetString(4), consulta.GetString(5),
                ObtenerPerfilTrabajador(consulta.GetString(0)));
                encuestados.Add(nuevoTrabajador);
            }

            conn.Close();
            return encuestados;
        }

        public List<string> ObtenerTrabajadoresEvaluados()
        {
            List<string> encuestados = new List<string>();
            conn.Open();
            cmd.CommandText = "SELECT DISTINCT rutTrabajadorEvaluado FROM listatrabajadoresencuestados;";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {                
                encuestados.Add(consulta.GetString(0));
            }

            conn.Close();
            return encuestados;
        }

        public string ObtenerNombreTrabajador(string rut)
        {
            string nombre = string.Empty;
            conn.Open();
            cmd.CommandText = "SELECT nombre, apellidoPaterno FROM Trabajadores WHERE rut='"+rut+"';";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                nombre = consulta.GetString(0) +" "+consulta.GetString(1);
            }

            conn.Close();
            return nombre;
        }

        public int ObtenerIdSeccion(string rut)
        {
            int id = -1;
            conn.Open();
            cmd.CommandText = "SELECT id FROM secciones WHERE id = (SELECT idSeccion From trabajadores WHERE rut='"+rut+"');";
            consulta = cmd.ExecuteReader();
            Console.WriteLine(cmd.CommandText.ToString());
            while (consulta.Read())
            {
                id = consulta.GetInt32(0);
            }

            conn.Close();
            return id;
        }

        public double ObtenerCapacidad(string rut, int idSeccion)
        {
            // el valor de las fp por defecto es -1, por eso el -500.
            double capacidad = -500; 
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT capacidadTrabajador FROM capacidadTrabajador WHERE rutTrabajador='" + rut + "' AND idSeccionEvaluacion=" + idSeccion + ";");

            if (bd.Consulta.Read())
            {
                capacidad = bd.Consulta.GetDouble("capacidadTrabajador");
            }

            bd.Close();

            return capacidad;
        }

        public double ObtenerGHB(string rut, int idSeccion)
        {
            // el valor de las fp por defecto es -1, por eso el -500.
            double capacidad = 0;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT gradoIgualdadHB FROM capacidadTrabajador WHERE rutTrabajador='" + rut + "' AND idSeccionEvaluacion=" + idSeccion + ";");

            if (bd.Consulta.Read())
            {
                capacidad = bd.Consulta.GetDouble("gradoIgualdadHB");
            }

            bd.Close();

            return capacidad;
        }

        public double ObtenerGHD(string rut, int idSeccion)
        {
            // el valor de las fp por defecto es -1, por eso el -500.
            double capacidad = 0;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT gradoIgualdadHD FROM capacidadTrabajador WHERE rutTrabajador='" + rut + "' AND idSeccionEvaluacion=" + idSeccion + ";");

            if (bd.Consulta.Read())
            {
                capacidad = bd.Consulta.GetDouble("gradoIgualdadHD");
            }

            bd.Close();

            return capacidad;
        }

        public double ObtenerGCF(string rut, int idSeccion)
        {
            // el valor de las fp por defecto es -1, por eso el -500.
            double capacidad = 0;
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            bd.ConsultaMySql("SELECT gradoIgualdadCF FROM capacidadTrabajador WHERE rutTrabajador='" + rut + "' AND idSeccionEvaluacion=" + idSeccion + ";");

            if (bd.Consulta.Read())
            {
                capacidad = bd.Consulta.GetDouble("gradoIgualdadCF");
            }

            bd.Close();

            return capacidad;
        }
    }
}
