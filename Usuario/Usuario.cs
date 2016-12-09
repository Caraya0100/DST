using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class Usuario
    {
        private string nombre;
        private string rut;
        private string clave;
        private string tipoUsuario;

        public Usuario(string nombre, string rut, string clave, string tipoUsuario)
        {
            this.nombre = nombre;
            this.rut = rut;
            this.clave = clave;
            this.tipoUsuario = tipoUsuario;
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

        public string Clave
        {
            get { return clave; }
            set { clave = value; }
        }

        public string TipoUsuario
        {
            get { return tipoUsuario; }
            set { tipoUsuario = value; }
        }
    }
}
