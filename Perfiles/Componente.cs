﻿using System;
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
        private string id;
        private string nombre;
        private string descripcion;
        private string tipo;
        private double puntaje;
        private double importancia;
        private bool estado;

        /// <summary>
        /// Constructor, recibe el id, nombre, la descripcion, y el tipo del componente.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        public Componente(string id, string nombre, string descripcion, string tipo)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.importancia = -1;
            this.estado = false;
        }

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
        public Componente(string id, string nombre, string descripcion, string tipo, double puntaje, double importancia)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.puntaje = puntaje;
            this.importancia = importancia;
        }
        /// <summary>
        /// Constructor que incluye el estado del componente
        /// </summary>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="descripcion"></param>
        /// <param name="tipo"></param>
        /// <param name="puntaje"></param>
        /// <param name="importancia"></param>
        public Componente(string id, string nombre, string descripcion, string tipo, double puntaje, double importancia, bool estado)
        {
            this.id = id;
            this.nombre = nombre;
            this.descripcion = descripcion;
            this.tipo = tipo;
            this.puntaje = puntaje;
            this.importancia = importancia;
            this.estado = estado;
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

        public string ID
        {
            get { return id; }
            set { id = value; }
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

        public bool Estado
        {
            get { return estado; }
            set { estado = value; }
        }
    }
}
