using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDifusa;

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
        private double ventasActuales;
        private double ventasAnioAnterior;
        private double ventasPlan;
        private double actualAnterior;
        private double actualPlan;
        private double desempenoGqm;
        private Dictionary<string, Pregunta> preguntas;
        private string tipo;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="idSeccion"></param>
        /// <param name="perfil"></param>
        /// <param name="trabajadores"></param>
        public Seccion(string nombre, int idSeccion, Perfil perfil, Dictionary<string, Trabajador> trabajadores, double desempenoGqm, Dictionary<string, Pregunta> preguntas, string tipo)
        {
            this.nombre = nombre;
            this.idSeccion = idSeccion;
            this.perfil = perfil;
            this.trabajadores = trabajadores;
            this.desempenoGqm = desempenoGqm;
            this.preguntas = preguntas;
            this.tipo = tipo;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="idSeccion"></param>
        /// <param name="perfil"></param>
        /// <param name="trabajadores"></param>
        public Seccion(string nombre, int idSeccion, Perfil perfil, Dictionary<string, Trabajador> trabajadores, double ventasActuales, double ventasAnioAnterior, double ventasPlan, string tipo)
        {
            this.nombre = nombre;
            this.idSeccion = idSeccion;
            this.perfil = perfil;
            this.trabajadores = trabajadores;
            this.ventasActuales = ventasActuales;
            this.ventasAnioAnterior = ventasAnioAnterior;
            this.ventasPlan = ventasPlan;
            this.tipo = tipo;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="idSeccion"></param>
        /// <param name="perfil"></param>
        /// <param name="trabajadores"></param>
        public Seccion(string nombre, int idSeccion, Perfil perfil, Dictionary<string, Trabajador> trabajadores, double ventasActuales, double ventasAnioAnterior, double ventasPlan)
        {
            this.nombre = nombre;
            this.idSeccion = idSeccion;
            this.perfil = perfil;
            this.trabajadores = trabajadores;
            this.ventasActuales = ventasActuales;
            this.ventasAnioAnterior = ventasAnioAnterior;
            this.ventasPlan = ventasPlan;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="idSeccion"></param>
        /// <param name="perfil"></param>
        /// <param name="trabajadores"></param>
        public Seccion( string nombre, int idSeccion, Perfil perfil, Dictionary<string, Trabajador> trabajadores)
        {
            this.nombre = nombre;
            this.idSeccion = idSeccion;
            this.perfil = perfil;
            this.trabajadores = trabajadores;
            ventasActuales = ObtenerVentasActuales();
            ventasAnioAnterior = ObtenerVentasAnioAnterior();
            ventasPlan = ObtenerVentasPlan();
            Tuple<double, double> desempeno = new Tuple<double, double>(52.0, 10.0);
            actualAnterior = desempeno.Item1;
            actualPlan = desempeno.Item2;
        }

        /// <summary>
        /// Devuelve las ventas actuales del mes de la seccion.
        /// </summary>
        /// <returns></returns>
        private double ObtenerVentasActuales()
        {
            return 1425500.0;
        }

        /// <summary>
        /// Devuelve las ventas del mes del año anterior de la seccion.
        /// </summary>
        /// <returns></returns>
        public double ObtenerVentasAnioAnterior()
        {
            return 2425500.0;
        }

        /// <summary>
        /// Devuelve las ventas del plan para el ultimo mes de la seccion.
        /// </summary>
        /// <returns></returns>
        public double ObtenerVentasPlan()
        {
            return 2925500.0;
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

        public double VentasActuales
        {
            get { return ventasActuales; }
        }

        public double VentasAnioAnterior
        {
            get { return ventasAnioAnterior; }
        }

        public double VentasPlan
        {
            get { return ventasPlan; }
        }

        public double ActualAnterior
        {
            get { return actualAnterior; }
            set { actualAnterior = value; }
        }

        public double ActualPlan
        {
            get { return actualPlan; }
            set { actualPlan = value; }
        }

        public double DesempenoGqm
        {
            get { return desempenoGqm; }
            set { desempenoGqm = value; }
        }

        public double Desempeno
        {
            get
            {
                if (tipo.ToLower() == "ventas") return actualAnterior;
                else return desempenoGqm;
            }
        }

        public Dictionary<string, Pregunta> Preguntas
        {
            get { return preguntas; }
            set { preguntas = value; }
        }

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }
    }
}
