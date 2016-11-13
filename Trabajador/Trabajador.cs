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
        private string fechaNacimiento;
        private Perfil perfil;

        /// <summary>
        /// Constructor, recibe el rut, el nombre, la fecha de nacimiento y el perfil del 
        /// trabajador.
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="nombre"></param>
        /// <param name="fechaNacimiento"></param>
        /// <param name="perfil"></param>
        public Trabajador(string rut, string nombre, string fechaNacimiento, Perfil perfil)
        {
            this.nombre = nombre;
            this.rut = rut;
            this.fechaNacimiento = fechaNacimiento;
            this.perfil = new Perfil(perfil.HB, perfil.HD, perfil.CF);
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
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

        public Perfil Perfil
        {
            get { return perfil; }
            set { perfil = value; }
        }
    }
}
