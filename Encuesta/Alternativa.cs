using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase para una respuesta de un pregunta. Contiene 
    /// </summary>
    public class Alternativa
    {
        private string nombre;
        private string descripcion;
        private double valor;
        private string tipo; // "conducta" o "frecuencia". "normal" (no 360).

        /// <summary>
        /// Constructor, recib el nombre, descripcion, y el valor de la respuesta.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="valor"></param>
        /// <param name="tipo"></param>
        public Alternativa(string nombre, string descripcion, double valor, string tipo)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.valor = valor;
            this.tipo = tipo;
        }

        /// <summary>
        /// Constructor, recib el nombre, descripcion, y el valor de la respuesta.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="valor"></param>
        public Alternativa(string nombre, string descripcion, double valor)
        {
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.valor = valor;
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

        public double Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
