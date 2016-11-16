using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase para el componente de un perfil.
    /// </summary>
    public class Componente
    {
        private string nombre;
        private string descripcion;
        private string tipo;
        private double puntaje;
        private double importancia;

        /// <summary>
        /// Constructor, recibe el nombre, la descripcion, y el tipo del componente.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        public Componente(string nombre, string descripcion, string tipo)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.importancia = -1;
        }

        /// <summary>
        /// Constructor, recibe el nombre, la descripcion, y el tipo del componente.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        public Componente(string nombre, string descripcion, string tipo, double puntaje, double importancia)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.puntaje = puntaje;
            this.importancia = importancia;
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public double Puntaje
        {
            get { return puntaje; }
            set { puntaje = value; }
        }

        public double Importancia
        {
            get { return importancia; }
            set { importancia = value; }
        }
    }
}
