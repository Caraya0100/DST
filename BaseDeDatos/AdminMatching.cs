using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DST
{
    public class AdminMatching
    {
        private MySqlConnection conn;
        private BaseDeDatos bd;
        private MySqlDataReader consulta;
        private MySqlCommand cmd;

        //public AdminBD (string servidor, string usuario, string password, string nombreBD)
        public AdminMatching()
        {
            bd = new BaseDeDatos();
            //conn = bd.conectarBD(servidor, usuario, password, nombreBD);
            conn = bd.conectarBD();
            cmd = conn.CreateCommand();

        }

        /// <summary>
        /// Inserta los componentes necesarios para el matching. Estos componentes 
        /// no son vistos por los usuarios.
        /// </summary>
        public void InsertarComponentes()
        {
            AdminPerfil ap = new AdminPerfil();
            /*ap.InsertarComponente("HB", "Habilidades blandas", "Representa el conjunto de las habilidades blandas.", "general", true);
            ap.InsertarComponente("HD", "Habilidades duras", "Representa el conjunto de las habilidades duras.", "general", true);
            ap.InsertarComponente("CF", "Caracteristicas fisicas", "Representa el conjunto de las caracteristicas fisicas.", "general", true);*/
            InsertarComponenteHB();
            InsertarComponenteCF();
            InsertarComponenteHD();
        }

        public void InsertarComponenteHB()
        {
            AdminPerfil ap = new AdminPerfil();
            Componente componente = ap.ObtenerComponente("HB");

            if (componente == null)
            {
                ap.InsertarComponente("HB", "Habilidades blandas", "Representa el conjunto de las habilidades blandas.", "general", true);
            }
        }

        public void InsertarComponenteHD()
        {
            AdminPerfil ap = new AdminPerfil();
            Componente componente = ap.ObtenerComponente("HD");

            if (componente == null)
            {
                ap.InsertarComponente("HD", "Habilidades duras", "Representa el conjunto de las habilidades duras.", "general", true);
            }
        }

        public void InsertarComponenteCF()
        {
            AdminPerfil ap = new AdminPerfil();
            Componente componente = ap.ObtenerComponente("CF");

            if (componente == null)
            {
                ap.InsertarComponente("CF", "Caracteristicas fisicas", "Representa el conjunto de las caracteristicas fisicas.", "general", true);
            }
        }

        /// <summary>
        /// Inserta los componentes necesarios para el matching en el perfil 
        /// de una sección.
        /// </summary>
        /// <param name="idSeccion"></param>
        public void InsertarComponente(int idSeccion, string idComponente, double puntaje, double importancia)
        {
            if (idComponente == "HB" || idComponente == "HD" || idComponente == "CF")
            {
                AdminSeccion adminSeccion = new AdminSeccion();
                AdminPerfil ap = new AdminPerfil();
                Componente componente = ap.ObtenerComponentePerfilSeccion(idSeccion, idComponente);

                if (componente == null)
                {
                    adminSeccion.InsertarComponentePerfilSeccion(idSeccion, idComponente, puntaje, importancia);
                } else
                {
                    ap.ActualizarComponentePerfilSeccion(idSeccion, idComponente, puntaje, importancia);
                }
            }
        }

        /// <summary>
        /// Inserta la capacidad de un trabajador para una sección. Si la capacidad 
        /// ya existe la actualiza.
        /// </summary>
        /// <param name="idSeccion"></param>
        public void InsertarCapacidad(string rut, int idSeccion, double igualdadHB, double igualdadHD, double igualdadCF, double capacidad)
        {
            AdminTrabajador at = new AdminTrabajador();
            BaseDeDatos bd = new BaseDeDatos();

            bd.Open();

            double c = at.ObtenerCapacidad(rut, idSeccion);

            // Si no existe la insertamos, de otro modo la actualizamos.
            if (c == -500)
            {
                bd.Insertar("INSERT INTO capacidadTrabajador(rutTrabajador,idSeccionEvaluacion,capacidadTrabajador,gradoIgualdadHB,gradoIgualdadHD,gradoIgualdadCF) VALUES('" + rut + "'," + idSeccion + "," + capacidad.ToString("0.0").Replace(",", ".") + "," + igualdadHB.ToString("0.0").Replace(",", ".") + "," + igualdadHD.ToString("0.0").Replace(",", ".") + "," + igualdadCF.ToString("0.0").Replace(",", ".") + ");");
            } else
            {
                bd.Insertar("UPDATE capacidadTrabajador SET rutTrabajador='" + rut + "', idSeccionEvaluacion=" + idSeccion + ", capacidadTrabajador=" + capacidad.ToString("0.0").Replace(",", ".") + ", gradoIgualdadHB=" + igualdadHB.ToString("0.0").Replace(",", ".") + ", gradoIgualdadHD=" + igualdadHD.ToString("0.0").Replace(",", ".") + ", gradoIgualdadCF=" + igualdadCF.ToString("0.0").Replace(",", ".") + " WHERE rutTrabajador='" + rut + "' AND idSeccionEvaluacion=" + idSeccion + ";");
            }

            bd.Close();
        }
    }
}
