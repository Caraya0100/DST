﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace DST
{
    /// <summary>
    /// Constructor, recibe el nombre, el id de la seccion, el perfil de la seccion y
    /// la lista de trabajadores pertenecientes a la seccion.
    /// </summary>
    /// <param name="nombre"></param>
    /// <param name="idSeccion"></param>
    public class Seccion
    {
        private string nombre;
        private int idSeccion;
        private Perfil perfil;
        private Dictionary<string, Trabajador> trabajadores;

        public Seccion( string nombre, int idSeccion, Perfil perfil, Dictionary<string, Trabajador> trabajadores)
        {
            this.nombre = nombre;
            this.idSeccion = idSeccion;
            this.perfil = perfil;
            this.trabajadores = trabajadores;
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }

        public Perfil Perfil
        {
            get { return perfil; }
            set { perfil = value; }
        }

        public Dictionary<string, Trabajador> Trabajadores
        {
            get { return trabajadores; }
            set { trabajadores = value; }
        }
    }
}
