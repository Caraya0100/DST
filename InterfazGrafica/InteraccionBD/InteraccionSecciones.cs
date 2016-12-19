using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;
using MySql.Data.MySqlClient;

namespace InterfazGrafica.InteraccionBD
{
    class InteraccionSecciones
    {
        private AdminSeccion datosSeccion;
        private AdminUsuario datosUsuario;

        private int idSeccion;
        private string idTrabajador;
        private string nombreSeccion;
        private string rutJefeSeccion;
        private string tipoMedicion;

        public InteraccionSecciones()
        {
            IniciarComponentes();
        }

        private void IniciarComponentes()
        {
            datosSeccion = new AdminSeccion();
            datosUsuario = new AdminUsuario();
            idSeccion = -1;
            nombreSeccion = string.Empty;
            rutJefeSeccion = string.Empty;
        }

        public void NuevaSeccion()
        {
            datosSeccion.InsertarSeccion(nombreSeccion,rutJefeSeccion,tipoMedicion);
        }

        public void ActualizarSeccion(int id)
        {
            datosSeccion.ModificarDatosSeccion(id,nombreSeccion,this.rutJefeSeccion,tipoMedicion);
        }

        public List<Seccion> TodasLasSecciones()
        {
            return datosSeccion.ObtenerSecciones();
        }

        public Perfil PerfilSeccion()
        {
            return datosSeccion.ObtenerPerfilSeccion(idSeccion);
        }

        public double PuntajeGeneralCF()
        {
            return datosSeccion.ObtenerPuntajeCFSeccion(idSeccion);
        }

        public double PuntajeGeneralHB()
        {
            return datosSeccion.ObtenerPuntajeHBSeccion(idSeccion);
        }

        public double PuntajeGeneralHD()
        {
            return datosSeccion.ObtenerPuntajeHDSeccion(idSeccion);
        }

        public double ImportanciaHD()
        {
            return datosSeccion.ObtenerImportanciaHDSeccion(idSeccion);
        }

        public double ImportanciaHB()
        {
            return datosSeccion.ObtenerImportanciaHBSeccion(idSeccion);
        }

        public double ImportanciaCF()
        {
            return datosSeccion.ObtenerImportanciaCFSeccion(idSeccion);
        }
        public string NombreSeccionTrabajador(string idSeccion)
        {
            return datosSeccion.ObtenerNombreSeccion(idSeccion);
        }

        public void ActualizacionImportanciasHabilidades(string idPregunta, double importancia)
        {
            string grado = ""+importancia;
            datosSeccion.ModificarImportanciaPerfilSeccion(idSeccion,idPregunta,grado.Replace(",","."));
        }

        public void ActualizacionPuntajeHabilidad(int idSeccion, string nombreComponente, double nuevoPuntaje)
        {
            
            datosSeccion.ModificarPuntajePerfilSeccion(idSeccion,nombreComponente,nuevoPuntaje);
        }
        public string NombreSeccionPorRutJefe()
        {
            return datosUsuario.ObtenerNombreSeccionPorUsuario(rutJefeSeccion) ;
        }

        public string NombreSeccionPorRutTrabajador()
        {
            return datosSeccion.ObtenerNombreSeccionTrabajador(idTrabajador);
        }

        public int IdSeccionPorRutJefeSeccion()
        {
            return datosSeccion.ObtenerIdSeccion(rutJefeSeccion);
        }

        public int IdSeccionPorNombreSeccion()
        {
            return datosSeccion.ObtenerIdSeccionPorNombre(nombreSeccion);
        }

        public List<Componente> HabilidadesDuras()
        {
            return datosSeccion.ObtenerComponentesHD();
        }
        public List<Componente> CaracteristicasFisicas()
        {
            return datosSeccion.ObtenerComponentesCF();
        }
        public List<Componente> HabilidadesBlandas()
        {
            return datosSeccion.ObtenerComponentesHB();
        }

        public void GuardarComponentesPerfil(int idSeccion, string habilidad)
        {  
            try
            {
                datosSeccion.InsertarComponentePerfilSeccion
                (
                    idSeccion,
                    habilidad,
                    0,
                    0                        
                );
                    
            }
            catch(Exception e)
            {                    
                Console.WriteLine("ERROR AL INSERTAR: "+e);
            }
            try
            {
                datosSeccion.HabilitarComponentePerfilSeccion(idSeccion, habilidad);
            }
            catch(Exception e){
                Console.WriteLine("ERROR AL HACER UPDATE: " + e);
            }
        }

        public void EliminarComponentesPerfil(int idSeccion, string pregunta)
        {
            datosSeccion.DeshabilitarComponentePerfilSeccion(idSeccion,pregunta);
        }

        public List<string> HabilidadesBlandasPerfil()
        {
            return datosSeccion.ObtenerComponentesHBSeccion(idSeccion);
        }

        public List<string> HabilidadesDurasPerfil()
        {
            return datosSeccion.ObtenerComponentesHDSeccion(idSeccion);
        }

        public List<string> CaracteristicasFisicasPerfil()
        {
            return datosSeccion.ObtenerComponentesCFSeccion(idSeccion);
        }
        public void EliminarSeccion()
        {            
            datosSeccion.EliminarSeccion(idSeccion);
        }
        /*public string RutJefeSeccion()
        {
            return datosSeccion.ObtenerRutJefeSeccion(nombreSeccion);
        }*/

       
       
        public string NombreJefeSeccion()
        {
            string nombre = string.Empty;
            foreach (DST.Usuario unUsuario in datosUsuario.ObtenerUsuarios())
            {
                if (unUsuario.Rut.Equals(datosSeccion.ObtenerRutJefeSeccion(nombreSeccion)))
                    nombre = unUsuario.Nombre;                
            }
            return nombre;
        }

        public string NombreSeccionPorId(int id)
        {
            return datosSeccion.ObtenerNombreSeccion(id);
        }

        public string NombreSeccion
        {
            get { return nombreSeccion; }
            set { nombreSeccion = value; }
        }

        public string RutJefeSeccion
        {
            get { return rutJefeSeccion; }
            set { rutJefeSeccion = value; }
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }
        public string IdTrabajador
        {
            get { return idTrabajador; }
            set { idTrabajador = value; }
        }

        public string TipoMedicion
        {
            get { return tipoMedicion; }
            set { tipoMedicion = value; }
        }

    }
}
