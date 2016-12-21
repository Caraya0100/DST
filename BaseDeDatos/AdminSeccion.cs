using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminSeccion
    {
        private MySqlConnection conn;
        private MySqlConnection conn2;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlDataReader consulta2;
        private MySqlCommand cmd;
        private MySqlCommand cmd2;
        private MySqlConnection conn3;
        private MySqlDataReader consulta3;
        private MySqlCommand cmd3;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminSeccion()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();
            conn2 = bd.conectarBD();
            cmd2 = conn2.CreateCommand();
            conn3 = bd.conectarBD();
            cmd3 = conn3.CreateCommand();
        }

        /// <summary>
        /// Consulta para insertar una nueva seccion. Se reciben como parametros el nombre y el rut del jefe encargado
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="rutJefe"></param>
       /* public void InsertarSeccion(string nombre, string descripcion, string rutJefe)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO secciones (nombre,descripcion,rutJefe) VALUES('" + nombre + "','" + descripcion
                + "','" + rutJefe + "');";
            cmd.ExecuteNonQuery();

            conn.Close();
        }*/

        /// <summary>
        /// Consulta para insertar un componente de un perfil de seccion. Se debe indicar el id de la seccion
        /// y el puntaje e importancia asociados a la hb,hd o cf
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nombre"></param>
        /// <param name="puntaje"></param>
        /// <param name="importancia"></param>
        public void InsertarComponentePerfilSeccion(int idSeccion, string nombre, double puntaje, double importancia)
        {
            conn.Open();

            try
            {
                

                cmd.CommandText = "INSERT INTO componentesPerfilSecciones (idSeccion,id,puntaje,importancia,estado) "
                    + " VALUES(" + idSeccion.ToString() + ",'" + nombre + "'," + puntaje.ToString("0.0").Replace(",", ".") + ","
                    + importancia.ToString("0.0").Replace(",", ".") + ", true);";
                Console.WriteLine(cmd.CommandText.ToString());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR EN BD:" + e);
            }
            conn.Close();
        }

        /// <summary>
        /// Consulta para modificar el puntaje de una componente del perfil de la secccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nuevoPuntaje"></param>
        public void ModificarPuntajePerfilSeccion(int idSeccion, string nombreComponente, double nuevoPuntaje)
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilSecciones SET puntaje =" + nuevoPuntaje.ToString("0.0").Replace(",",".") + " WHERE "
                + "id='" + nombreComponente + "' AND idSeccion=" + idSeccion + ";";
            Console.WriteLine(cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Consulta para modificar la importancia de una componente del perfil de la seccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nuevaImportancia"></param>
        public void ModificarImportanciaPerfilSeccion(int idSeccion, string nombreComponente, string nuevaImportancia)//cambio float por double
        {
            conn.Open();

            cmd.CommandText = "UPDATE componentesPerfilSecciones SET importancia =" + nuevaImportancia.ToString()
                + " WHERE id='" + nombreComponente + "' AND idSeccion =" + idSeccion + ";";
            //Console.WriteLine("LA CONSULTAX: "+cmd.CommandText.ToString());
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
            //Console.ReadKey();

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

            /*Console.WriteLine("Nombre: {0}", nombreSeccion);
            Console.ReadKey();*/

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

            /*Console.WriteLine("RUT: {0}", rutJefeSeccion);
            Console.ReadKey();*/

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

            conn2.Open();
            cmd2.CommandText = "SELECT c.id, c.nombre,c.descripcion,c.tipo,s.puntaje,s.importancia, s.estado FROM componentesPerfil AS c,"
                + "componentesPerfilSecciones AS s WHERE s.idSeccion=" + idSeccion.ToString() + " AND c.id=s.id AND s.estado=true;";
            consulta2 = cmd2.ExecuteReader();
            while (consulta2.Read())
            {
                Componente nuevoComponente = new Componente(consulta2.GetString(0), consulta2.GetString(1), consulta2.GetString(2), 
                    consulta2.GetString(3),consulta2.GetDouble(4), consulta2.GetDouble(5),consulta2.GetBoolean(6));
                Console.WriteLine("ENCONSULTA: "+consulta2.GetBoolean(6));
                perfilSeccion.AgregarComponente(nuevoComponente);
            }

            conn2.Close();
            
            return perfilSeccion;
        }

        /// <summary>
        /// Funcion que retorna una lista de las secciones con todos sus campos.
        /// Esta debe ser usada en la ventana del administrador.
        /// </summary>
        /// <returns></returns>
        public List<Seccion> ObtenerSecciones()
        {
            List<Seccion> secciones = new List<Seccion>();
            AdminDesempeño ad = new AdminDesempeño();
            AdminEncuesta ae = new AdminEncuesta();
            AdminTrabajador obtenerTrabajadores = new AdminTrabajador();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();
            bd.ConsultaMySql("SELECT * FROM secciones;");

            while (bd.Consulta.Read())
            {
                int idSeccion = bd.Consulta.GetInt16(0);
                string fecha = "";
                string tipo = bd.Consulta.GetString("tipoSeccion");
                Tuple<double, double, double> ventas = null;

                if (tipo.ToLower() == "ventas")
                {
                    fecha = ad.ObtenerUltimaFecha(idSeccion);
                    ventas = ad.ObtenerVentas(idSeccion, fecha);
                }

                if (ventas == null) ventas = new Tuple<double, double, double>(0,0,0);

                Seccion nuevaSeccion = null;
                if (tipo.ToLower() == "ventas")
                {
                    nuevaSeccion = new Seccion(
                        bd.Consulta.GetString(1),
                        bd.Consulta.GetInt16(0),
                        ObtenerPerfilSeccion(bd.Consulta.GetInt16(0)),
                        obtenerTrabajadores.ObtenerTrabajadoresSeccion(bd.Consulta.GetInt16(0)),
                        ventas.Item1,
                        ventas.Item2,
                        ventas.Item3,
                        bd.Consulta.GetString("tipoSeccion")
                    );
                } else if (tipo.ToLower() == "gqm")
                {
                    fecha = ad.ObtenerUltimaFechaGqm(idSeccion);
                    int mes = 0;
                    int anio = 0;

                    if (fecha != "")
                    {
                        fecha = new AdminFecha().FechaConFormato(fecha);
                        mes = Convert.ToInt32(fecha.Split('-')[1]);
                        anio = Convert.ToInt32(fecha.Split('-')[0]);
                    }
                    
                    nuevaSeccion = new Seccion(
                        bd.Consulta.GetString(1),
                        bd.Consulta.GetInt16(0),
                        ObtenerPerfilSeccion(bd.Consulta.GetInt16(0)),
                        obtenerTrabajadores.ObtenerTrabajadoresSeccion(bd.Consulta.GetInt16(0)),
                        ad.ObtenerDesempenoGqm(bd.Consulta.GetInt16(0), mes, anio),
                        ae.ObtenerPreguntasSeccion(bd.Consulta.GetInt32("id")),
                        tipo
                    );
                }

                secciones.Add( nuevaSeccion );
            }

            bd.Close();

            return secciones;
        }

        /// <summary>
        /// Funcion que retorna una lista de las secciones con todos sus campos.
        /// Esta debe ser usada en la ventana del administrador.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Seccion> ObtenerDiccionarioSecciones()
        {
            Dictionary<string, Seccion> secciones = new Dictionary<string, Seccion>();
            AdminDesempeño ad = new AdminDesempeño();
            AdminEncuesta ae = new AdminEncuesta();
            AdminTrabajador obtenerTrabajadores = new AdminTrabajador();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();
            bd.ConsultaMySql("SELECT * FROM secciones;");

            while (bd.Consulta.Read())
            {
                int idSeccion = bd.Consulta.GetInt16(0);
                string fecha = ad.ObtenerUltimaFecha();
                string tipo = bd.Consulta.GetString("tipoSeccion");
                Tuple<double, double, double> ventas = null;

                if (tipo.ToLower() == "ventas")
                    ventas = ad.ObtenerVentas(idSeccion, fecha);

                if (ventas == null) ventas = new Tuple<double, double, double>(-1, -1, -1);

                Seccion nuevaSeccion = new Seccion(
                    bd.Consulta.GetString(1),
                    bd.Consulta.GetInt16(0),
                    ObtenerPerfilSeccion(bd.Consulta.GetInt16(0)),
                    obtenerTrabajadores.ObtenerTrabajadoresSeccion(bd.Consulta.GetInt16(0)),
                    ventas.Item1,
                    ventas.Item2,
                    ventas.Item3,
                    bd.Consulta.GetString("tipoSeccion")
                );

                if (tipo.ToLower() == "gqm")
                {
                    nuevaSeccion.Preguntas = ae.ObtenerPreguntasSeccion(bd.Consulta.GetInt32("id"));
                }

                secciones.Add(nuevaSeccion.IdSeccion.ToString(), nuevaSeccion);
            }


            bd.Close();

            return secciones;
        }

        /// <summary>
        /// Consulta para insertar una nueva seccion. Se reciben como parametros el nombre y el rut del jefe encargado
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="rutJefe"></param>
        public void InsertarSeccion(string nombre, string rutJefe, string tipoSeccion)
        {
            conn.Open();

            cmd.CommandText = "INSERT INTO secciones (nombre,rutJefe,tipoSeccion) VALUES('" + nombre + "','" + rutJefe + "','"+tipoSeccion+"');";
            Console.WriteLine("INSERTAR SECCION: "+cmd.CommandText.ToString());
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Obtiene el nombre de la seccion con el id de la seeccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public string ObtenerNombreSeccion(int idSeccion)
        {
            string nombreSeccion = "";

            conn.Open();
            cmd.CommandText = "SELECT nombre FROM secciones WHERE id='" + idSeccion + "';";
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

        /// <summary>
        /// Obtiene el nombre de la seccion con el rut del trabajador
        /// </summary>
        /// <param name="rutTrabajador"></param>
        /// <returns></returns>
        public string ObtenerNombreSeccionTrabajador(string rutTrabajador)
        {
            string nombreSeccion = "";

            conn.Open();
            cmd.CommandText = "SELECT nombre FROM secciones WHERE id=(SELECT idSeccion FROM trabajadores WHERE rut='"
                + rutTrabajador + "');";
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

        /// <summary>
        /// Modifica el nombre y el rut del jefe de seccion
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="nuevoNombre"></param>
        /// <param name="nuevoRutJefe"></param>
        public void ModificarDatosSeccion(int idSeccion, string nuevoNombre, string nuevoRutJefe, string tipo)
        {
            conn.Open();

            cmd.CommandText = "UPDATE secciones SET nombre='" + nuevoNombre + "', rutJefe='"
                + nuevoRutJefe + "', tipoSeccion='"+tipo+"' WHERE id=" + idSeccion.ToString() + ";";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Elimina una seccion y todo lo que este asociado a dicha seccion
        /// </summary>
        /// <param name="idSeccion"></param>
        public void EliminarSeccion(int idSeccion)
        {
            conn.Open();

            cmd.CommandText = "DELETE FROM secciones WHERE id=" + idSeccion.ToString() + ";";
            cmd.ExecuteNonQuery();

            conn.Close();
        }

        /// <summary>
        /// Obtiene el puntaje de la evaluacion mas reciente de las hb
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public double ObtenerPuntajeHBSeccion(int idSeccion)
        {
            double puntajeHB = 0;

            conn3.Open();
            /*cmd3.CommandText = "SELECT hb FROM evaluacionSeccion WHERE fechaEvaluacion = (SELECT MAX(fechaEvaluacion)"
                + "FROM evaluaciontrabajador WHERE idSeccion=" + idSeccion.ToString() + ");";*/
            cmd3.CommandText = "SELECT hb FROM evaluacionSeccion WHERE idSeccion=" + idSeccion.ToString() + " ORDER BY fechaEvaluacion DESC LIMIT 1;";

            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                puntajeHB = consulta3.GetDouble(0);
            }

            conn3.Close();

            return puntajeHB;
        }


        /// <summary>
        /// Obtiene el puntaje de la evaluacion mas reciente de las hd
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public double ObtenerPuntajeHDSeccion(int idSeccion)
        {
            double puntajeHD = 0;

            conn3.Open();
            /*cmd3.CommandText = "SELECT hd FROM evaluacionSeccion WHERE fechaEvaluacion = (SELECT MAX(fechaEvaluacion)"
                + "FROM evaluaciontrabajador WHERE idSeccion=" + idSeccion.ToString() + ");";*/
            cmd3.CommandText = "SELECT hd FROM evaluacionSeccion WHERE idSeccion=" + idSeccion.ToString() + " ORDER BY fechaEvaluacion DESC LIMIT 1;";

            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                puntajeHD = consulta3.GetDouble(0);
            }

            conn3.Close();

            return puntajeHD;
        }

        /// <summary>
        /// Obtiene el puntaje de la evaluacion mas reciente de las cf
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <returns></returns>
        public double ObtenerPuntajeCFSeccion(int idSeccion)
        {
            double puntajeCF = 0;

            conn3.Open();
            /*cmd3.CommandText = "SELECT cf FROM evaluacionSeccion WHERE fechaEvaluacion = (SELECT MAX(fechaEvaluacion)"
                + "FROM evaluaciontrabajador WHERE idSeccion=" + idSeccion.ToString() + ");";*/
            cmd3.CommandText = "SELECT cf FROM evaluacionSeccion WHERE idSeccion=" + idSeccion.ToString() + " ORDER BY fechaEvaluacion DESC LIMIT 1;";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                puntajeCF = consulta3.GetDouble(0);
            }

            conn3.Close();

            return puntajeCF;
        }

        /*************************************************************
         *                  MIS CONSULTAS
         * *******************************************************/
        public List<Componente> ObtenerComponentesHD()
        {
            List<Componente> listaComponentes = new List<Componente>();

            conn3.Open();
            cmd3.CommandText = "SELECT id,nombre FROM componentesPerfil WHERE tipo = 'hd';";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                Componente preguntas = new Componente
                    (
                        consulta3.GetString(0),
                        consulta3.GetString(1),
                        "",
                        ""
                    );
                listaComponentes.Add(preguntas);
            }
            conn3.Close();

            return listaComponentes;
        }

        public List<Componente> ObtenerComponentesHB()
        {
            List<Componente> listaComponentes = new List<Componente>();
            
            conn3.Open();
            cmd3.CommandText = "SELECT id,nombre FROM componentesPerfil WHERE tipo = 'hb';";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {                
                Componente preguntas = new Componente
                    (
                        consulta3.GetString(0),
                        consulta3.GetString(1),
                        "",
                        ""
                    );
                listaComponentes.Add(preguntas);
            }
            conn3.Close();

            return listaComponentes;
        }

        public List<Componente> ObtenerComponentesCF()
        {
            List<Componente> listaComponentes = new List<Componente>();

            conn3.Open();
            cmd3.CommandText = "SELECT id,nombre FROM componentesPerfil WHERE tipo = 'cf';";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                Componente preguntas = new Componente
                    (
                        consulta3.GetString(0),
                        consulta3.GetString(1),
                        "",
                        ""
                    );
                listaComponentes.Add(preguntas);
            }
            conn3.Close();

            return listaComponentes;
        }

        public void EliminarComponentePerfilSeccion(string nombre, int idSeccion)
        {
            try
            {
                conn.Open();
                //cmd.CommandText = "DELETE FROM componentesPerfilSecciones WHERE id='" + nombre + "' AND idSeccion=" + idSeccion + ";";
                cmd.CommandText ="UPDATE componentesperfilsecciones SET estado=false, importancia=0.0 WHERE id='"+nombre+"' AND idSeccion = "+idSeccion+";";
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR:" + e);
            }
        }

        public List<string> ObtenerComponentesHDSeccion(int idSeccion)//arreglado
        {
            List<string> listaComponentes = new List<string>();

            conn3.Open();
            cmd3.CommandText = "SELECT DISTINCT t2.nombre FROM componentesperfilsecciones AS t1 INNER JOIN componentesperfil AS t2 "
                                + "ON t2.tipo = 'hd' AND t1.id= t2.id AND t1.estado =true AND t1.idSeccion= " + idSeccion + ";";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                listaComponentes.Add(consulta3.GetString(0));
            }
            conn3.Close();
            return listaComponentes;
        }


        public List<string> ObtenerComponentesHBSeccion(int idSeccion)//arreglado
        {
            List<string> listaComponentes = new List<string>();

            conn3.Open();
            cmd3.CommandText = "SELECT DISTINCT t2.nombre FROM componentesperfilsecciones AS t1 INNER JOIN componentesperfil AS t2 "
                                + "ON t2.tipo = 'hb' AND t1.id= t2.id AND t1.estado =true AND t1.idSeccion= " + idSeccion + ";";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                listaComponentes.Add(consulta3.GetString(0));
            }
            conn3.Close();
            return listaComponentes;
        }

        public List<string> ObtenerComponentesCFSeccion(int idSeccion)//arreglado
        {
            List<string> listaComponentes = new List<string>();

            conn3.Open();
            cmd3.CommandText = "SELECT DISTINCT t2.nombre FROM componentesperfilsecciones AS t1 INNER JOIN componentesperfil AS t2 "
                                + "ON t2.tipo = 'cf' AND t1.id= t2.id AND t1.estado =true AND t1.idSeccion= " + idSeccion + ";";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                listaComponentes.Add(consulta3.GetString(0));
            }
            conn3.Close();
            return listaComponentes;
        }

        public double ObtenerImportanciaHDSeccion(int idSeccion)
        {
            double puntajeHD = 0;

            conn3.Open();
            cmd3.CommandText = "SELECT importancia FROM componentesperfilsecciones WHERE id='hd' AND idSeccion =" + idSeccion + ";";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                puntajeHD = consulta3.GetDouble(0);
            }

            conn3.Close();

            return puntajeHD;
        }

        public double ObtenerImportanciaHBSeccion(int idSeccion)
        {
            double puntajeHD = 0;

            conn3.Open();
            cmd3.CommandText = "SELECT importancia FROM componentesperfilsecciones WHERE id='hb' AND idSeccion =" + idSeccion + ";";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                puntajeHD = consulta3.GetDouble(0);
            }

            conn3.Close();

            return puntajeHD;
        }

        public double ObtenerImportanciaCFSeccion(int idSeccion)
        {
            double puntajeHD = 0;

            conn3.Open();
            cmd3.CommandText = "SELECT importancia FROM componentesperfilsecciones WHERE id='cf' AND idSeccion =" + idSeccion + ";";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                puntajeHD = consulta3.GetDouble(0);
            }

            conn3.Close();

            return puntajeHD;
        }

        public void HabilitarComponentePerfilSeccion(int idSeccion,string idPregunta)
        {
            conn.Open();
            try
            {
                cmd.CommandText = "UPDATE componentesPerfilSecciones SET estado=true WHERE idseccion="+idSeccion+" AND id='"+idPregunta+"';";                    
                Console.WriteLine(cmd.CommandText.ToString());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR EN BD:" + e);
            }
            conn.Close();
        }

        public void DeshabilitarComponentePerfilSeccion(int idSeccion, string idPregunta)
        {
            conn.Open();
            try
            {
                cmd.CommandText = "UPDATE componentesPerfilSecciones SET estado=false WHERE idseccion=" + idSeccion + " AND id='" + idPregunta + "';";
                Console.WriteLine(cmd.CommandText.ToString());
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR EN BD:" + e);
            }
            conn.Close();
        }


        public int ObtenerIdSeccionPorNombre(string nombreSeccion)
        {
            int id =-1;
            conn3.Open();
            cmd3.CommandText = "SELECT id FROM secciones WHERE nombre='"+nombreSeccion+"';";
            Console.WriteLine(cmd3.CommandText.ToString());
            consulta3 = cmd3.ExecuteReader();
            while (consulta3.Read())
            {
                id = consulta3.GetInt32(0);
            }

            conn3.Close();
            return id;
        }
    }
}
