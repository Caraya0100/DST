using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace InterfazGrafica.InteraccionBD
{
    class InteraccionUsuarios
    {
        /*CONEXION A BD*/
        private AdminUsuario datosUsuario;
        private AdminSeccion datosSeccion;

        /*ESTRUCTURAS*/
        Estructuras.Usuario usuario;

        /*Variables*/
        private string idUsuario;
        private string claveUsuario;
        private List<string> listaJefeSeccion;
        private List<Estructuras.Usuario> listaUsuarios;

        public InteraccionUsuarios()
        {
            this.datosUsuario = new AdminUsuario();
            this.datosSeccion = new AdminSeccion();
            this.usuario = new Estructuras.Usuario();
            this.idUsuario = string.Empty;
            this.claveUsuario = string.Empty;
            this.listaJefeSeccion = new List<string>();
            this.listaUsuarios = new List<Estructuras.Usuario>();
        }

        /// <summary>
        /// agrega un nuevo usuario a la base de datos
        /// </summary>
        public Estructuras.Usuario NuevoUsuario
        {
            get { return this.usuario; }
            set
            {
                this.usuario = value;
                datosUsuario.InsertarUsuario
                (
                usuario.Nombre,
                usuario.Rut,
                usuario.Clave,
                usuario.TipoUsuario,
                usuario.Estado
                );
            }
        }
        /// <summary>
        /// Retorna la lista de todos los usuarios existentes.
        /// </summary>
        /// <returns></returns>
        public List<Estructuras.Usuario> UsuariosExistentes()
        {
            listaUsuarios.Clear();
             List<DST.Usuario> usuarios = datosUsuario.ObtenerUsuarios();
             foreach (DST.Usuario usuario in usuarios)
             {
                 Estructuras.Usuario infoUsuario = new Estructuras.Usuario();
                 infoUsuario.Nombre = usuario.Nombre;
                 infoUsuario.Rut = usuario.Rut;
                 infoUsuario.Clave = usuario.Clave;
                 infoUsuario.TipoUsuario = usuario.TipoUsuario;
                 listaUsuarios.Add(infoUsuario);
             }
             return listaUsuarios;
        }

        public string NombreJefeSeccion()
        {
            return datosUsuario.ObtenerNombreUsuario(idUsuario);
        }
        public string RutUsuario(string nombreUsuario)
        {
            string rut;
            List<DST.Usuario> usuarios = datosUsuario.ObtenerUsuarios();
            foreach (DST.Usuario usuario in usuarios)
            {
                if (usuario.Nombre.Equals(nombreUsuario))
                {
                    rut = usuario.Rut;
                    return rut;
                }                    
            }
            return null;
            
        }

        public string TipoUsuario()
        {
            return datosUsuario.ObtenerTipoUsuario(idUsuario);
        }

        public void ActulizacionUsuarios(Estructuras.Usuario usuario)
        {
            datosUsuario.ModificarDatosUsuario(
                usuario.Nombre,
                usuario.Rut,
                usuario.TipoUsuario,
                usuario.Clave,
                usuario.Rut);
        }

        public string SeccionAsociada(string rutJefeSeccion)
        {
            string seccion = datosSeccion.ObtenerNombreSeccion(rutJefeSeccion);
            return seccion;
        }

        public void EliminarUsuario()
        {
            datosUsuario.CambiarEstadoUsuario(idUsuario,false);
        }

        public List<string> ListaJefesSeccion()
        {
            return datosUsuario.ObtenerNombresJefesDeSeccion();
        }

        public List<string> ListaDeJefesSeccion
        {
            get { return listaJefeSeccion; }
            set { listaJefeSeccion = value; }
        }

        public bool VerificacionUsuario()
        {
            if (datosUsuario.VerificarClave(claveUsuario) && datosUsuario.VerificarUsuario(idUsuario))
                return (true);
            else
                return false;
        }

        public string IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        public string Clave
        {
            get { return claveUsuario; }
            set { claveUsuario = value; }
        }
        
    }
}
