using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace InterfazGrafica.InteraccionBD
{
    class InteraccionTrabajadores
    {
        private AdminTrabajador datosTrabajador;
        private AdminDesempeño datosPuntajes;
        /*variables*/
        private int idSeccion;
        private string idTrabajador;

        public InteraccionTrabajadores()
        {
            InciarComponentes();
        }

        private void InciarComponentes()
        {
            datosTrabajador = new AdminTrabajador();
            datosPuntajes = new AdminDesempeño();
        }

        public void NuevoTrabajador(Estructuras.Trabajador nuevoTrabajador)
        {
            datosTrabajador.InsertarTrabajador
                (
                    nuevoTrabajador.Nombre,
                    nuevoTrabajador.ApellidoPaterno,
                    nuevoTrabajador.ApellidoMaterno,
                    nuevoTrabajador.Rut,
                    nuevoTrabajador.FechaNacimiento,
                    nuevoTrabajador.IdSeccion,
                    nuevoTrabajador.Sexo,
                    nuevoTrabajador.Estado);
        }

        public void ModificaTrabajador(Estructuras.Trabajador trabajadorModificado, string rut)
        {
            datosTrabajador.ModificarDatosTrabajador
                (
                    trabajadorModificado.Rut,
                    trabajadorModificado.Nombre,
                    trabajadorModificado.ApellidoPaterno,
                    trabajadorModificado.ApellidoMaterno,
                    trabajadorModificado.FechaNacimiento,
                    trabajadorModificado.IdSeccion,
                    rut
                );
        }
        public double PuntajeGeneralCF()
        {
            return datosPuntajes.ObtenerPuntajeCF(idTrabajador);
        }
        public double PuntajeGeneralHB()
        {
            return datosPuntajes.ObtenerPuntajeHB(idTrabajador);
        }
        public double PuntajeGeneralHD()
        {
            return datosPuntajes.ObtenerPuntajeHD(idTrabajador);
        }
        public Dictionary<string, Trabajador> TrabajadoresSeccion()
        {            
            Dictionary<string, Trabajador> trabajadores;
            trabajadores = datosTrabajador.ObtenerTrabajadoresSeccion(idSeccion);
            return trabajadores;
        }

        public List<Trabajador> TrabajadoresEmpresa()
        {
            return datosTrabajador.ObtenerTrabajadoresEmpresa();
        }

        public Trabajador InfoTrabajador()
        {
            return datosTrabajador.ObtenerInfoTrabajador(idTrabajador);
        }

        public Perfil PerfilTrabajador()
        {
            return datosTrabajador.ObtenerPerfilTrabajador(idTrabajador);
        }

        public List<Trabajador> TrabajadoresEncuestados()
        {
            return datosTrabajador.ObtenerTrabajadoresEncuestados(idTrabajador);
        }

         public List<string> TrabajadoresEvaluados()
        {
            return datosTrabajador.ObtenerTrabajadoresEvaluados();
        }

        public void EliminarTrabajador()
        {
            datosTrabajador.BorrarTrabajador(idTrabajador);
        }

        public string NombreTrabajadorPorRut(string rut)
        {
            return datosTrabajador.ObtenerNombreTrabajador(rut);
        }

        public int IdSeccionPorRutTrabajador(string rut)
        {
            return datosTrabajador.ObtenerIdSeccion(rut);
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }

        public string IdTrabajador
        {
            get { return idTrabajador; }
            set { idTrabajador= value; }
        }
    }
}
