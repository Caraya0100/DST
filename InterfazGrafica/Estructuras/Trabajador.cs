using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazGrafica.Estructuras
{
    public struct Trabajador
    {
        public string Nombre {get; set;}
        public string ApellidoPaterno {get;set;}
        public string ApellidoMaterno {get;set;}        
        public string Rut { get; set; }
        public string FechaNacimiento{get;set;}
        public int IdSeccion { get; set; }
        public string Sexo { get; set; }
        public bool Estado { get; set; }
        
    }
}
