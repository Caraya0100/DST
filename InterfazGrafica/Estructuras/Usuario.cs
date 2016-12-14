using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazGrafica.Estructuras
{
    public struct Usuario
    {
        public string Nombre { get; set; }
        public string Rut { get; set; }
        public string Clave { get; set; }
        public string TipoUsuario { get; set; }
        public bool Estado { get; set; }
    }
}
