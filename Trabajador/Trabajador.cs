using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace DST
{ 
    /// <summary>
    /// Clase para el trabajador, contiene el rut, el nombre, la fecha de nacimiento, y 
    /// su perfil.
    /// </summary>
    public class Trabajador
    {
        private string rut;
        private string nombre;
        private string apellidoPaterno;
        private string apellidoMaterno;
        private string fechaNacimiento;
        private string sexo;
        private Perfil perfil;

        /// <summary>
        /// Constructor, recibe el rut, el nombre, la fecha de nacimiento y el perfil del 
        /// trabajador.
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="nombre"></param>
        /// <param name="fechaNacimiento"></param>
        /// <param name="perfil"></param>
        public Trabajador(string rut, string nombre, string apellidoPaterno, string apellidoMaterno, 
            string fechaNacimiento, string sexo, Perfil perfil)
        {
            this.rut = rut;
            this.nombre = nombre;
            this.apellidoPaterno = apellidoPaterno;
            this.apellidoMaterno = apellidoMaterno;
            this.fechaNacimiento = fechaNacimiento;
            this.sexo = sexo;
            this.perfil = new Perfil(perfil.Blandas, perfil.Duras, perfil.Fisicas);
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string ApellidoPaterno
        {
            get { return apellidoPaterno; }
            set { apellidoPaterno = value; }
        }

        public string ApellidoMaterno
        {
            get { return apellidoMaterno; }
            set { apellidoMaterno = value; }
        }

        public string Rut
        {
            get { return rut; }
            set { rut = value; }
        }

        public string FechaNacimiento
        {
            get { return fechaNacimiento; }
            set { fechaNacimiento = value; }
        }

        public string Sexo
        {
            get { return sexo; }
            set { sexo = value; }
        }

        public Perfil Perfil
        {
            get { return perfil; }
            set { perfil = value; }
        }
    }
}
