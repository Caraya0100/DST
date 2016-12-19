using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    /// <summary>
    /// Clase con las reglas del matching.
    /// </summary>
    public class ReglasMatching
    {
        private Dictionary<string, string> hb;
        private Dictionary<string, string> hd;
        private Dictionary<string, string> cf;
        private Dictionary<string, string> capacidad;

        public ReglasMatching()
        {
            hb = ReglasHB();
            hd = ReglasHD();
            cf = ReglasCF();
            capacidad = ReglasCapacidad();
        }

        /// <summary>
        /// Define las reglas para inferir el grado de igualdad de las HB.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReglasHB()
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            reglas.Add("1", "Si HBS es muy_bajas y HBT es muy_bajas entonces HB es iguales");
            reglas.Add("2", "Si HBS es muy_bajas y HBT es bajas entonces HB es superiores");
            reglas.Add("3", "Si HBS es muy_bajas y HBT es promedio entonces HB es superiores");
            reglas.Add("4", "Si HBS es muy_bajas y HBT es altas entonces HB es superiores");
            reglas.Add("5", "Si HBS es muy_bajas y HBT es muy_altas entonces HB es superiores");
            reglas.Add("6", "Si HBS es bajas y HBT es muy_bajas entonces HB es inferiores");
            reglas.Add("7", "Si HBS es bajas y HBT es bajas entonces HB es iguales");
            reglas.Add("8", "Si HBS es bajas y HBT es promedio entonces HB es superiores");
            reglas.Add("9", "Si HBS es bajas y HBT es altas entonces HB es superiores");
            reglas.Add("10", "Si HBS es bajas y HBT es muy_altas entonces HB es superiores");
            reglas.Add("11", "Si HBS es promedio y HBT es muy_bajas entonces HB es inferiores");
            reglas.Add("12", "Si HBS es promedio y HBT es bajas entonces HB es inferiores");
            reglas.Add("13", "Si HBS es promedio y HBT es promedio entonces HB es iguales");
            reglas.Add("14", "Si HBS es promedio y HBT es altas entonces HB es superiores");
            reglas.Add("15", "Si HBS es promedio y HBT es muy_altas entonces HB es superiores");
            reglas.Add("16", "Si HBS es altas y HBT es muy_bajas entonces HB es inferiores");
            reglas.Add("17", "Si HBS es altas y HBT es bajas entonces HB es inferiores");
            reglas.Add("18", "Si HBS es altas y HBT es promedio entonces HB es inferiores");
            reglas.Add("19", "Si HBS es altas y HBT es altas entonces HB es iguales");
            reglas.Add("20", "Si HBS es altas y HBT es muy_altas entonces HB es superiores");
            reglas.Add("21", "Si HBS es muy_altas y HBT es muy_bajas entonces HB es inferiores");
            reglas.Add("22", "Si HBS es muy_altas y HBT es bajas entonces HB es inferiores");
            reglas.Add("23", "Si HBS es muy_altas y HBT es promedio entonces HB es inferiores");
            reglas.Add("24", "Si HBS es muy_altas y HBT es altas entonces HB es inferiores");
            reglas.Add("25", "Si HBS es muy_altas y HBT es muy_altas entonces HB es iguales");

            return reglas;
        }

        /// <summary>
        /// Define las reglas para inferir el grado de igualdad de las HD.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReglasHD()
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            reglas.Add("1", "Si HDS es muy_bajas y HDT es muy_bajas entonces HD es iguales");
            reglas.Add("2", "Si HDS es muy_bajas y HDT es bajas entonces HD es superiores");
            reglas.Add("3", "Si HDS es muy_bajas y HDT es promedio entonces HD es superiores");
            reglas.Add("4", "Si HDS es muy_bajas y HDT es altas entonces HD es superiores");
            reglas.Add("5", "Si HDS es muy_bajas y HDT es muy_altas entonces HD es superiores");
            reglas.Add("6", "Si HDS es bajas y HDT es muy_bajas entonces HD es inferiores");
            reglas.Add("7", "Si HDS es bajas y HDT es bajas entonces HD es iguales");
            reglas.Add("8", "Si HDS es bajas y HDT es promedio entonces HD es superiores");
            reglas.Add("9", "Si HDS es bajas y HDT es altas entonces HD es superiores");
            reglas.Add("10", "Si HDS es bajas y HDT es muy_altas entonces HD es superiores");
            reglas.Add("11", "Si HDS es promedio y HDT es muy_bajas entonces HD es inferiores");
            reglas.Add("12", "Si HDS es promedio y HDT es bajas entonces HD es inferiores");
            reglas.Add("13", "Si HDS es promedio y HDT es promedio entonces HD es iguales");
            reglas.Add("14", "Si HDS es promedio y HDT es altas entonces HD es superiores");
            reglas.Add("15", "Si HDS es promedio y HDT es muy_altas entonces HD es superiores");
            reglas.Add("16", "Si HDS es altas y HDT es muy_bajas entonces HD es inferiores");
            reglas.Add("17", "Si HDS es altas y HDT es bajas entonces HD es inferiores");
            reglas.Add("18", "Si HDS es altas y HDT es promedio entonces HD es inferiores");
            reglas.Add("19", "Si HDS es altas y HDT es altas entonces HD es iguales");
            reglas.Add("20", "Si HDS es altas y HDT es muy_altas entonces HD es superiores");
            reglas.Add("21", "Si HDS es muy_altas y HDT es muy_bajas entonces HD es inferiores");
            reglas.Add("22", "Si HDS es muy_altas y HDT es bajas entonces HD es inferiores");
            reglas.Add("23", "Si HDS es muy_altas y HDT es promedio entonces HD es inferiores");
            reglas.Add("24", "Si HDS es muy_altas y HDT es altas entonces HD es inferiores");
            reglas.Add("25", "Si HDS es muy_altas y HDT es muy_altas entonces HD es iguales");

            return reglas;
        }

        /// <summary>
        /// Define las reglas para inferir el grado de igualdad de las CF.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReglasCF()
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            reglas.Add("1", "Si CFS es muy_bajas y CFT es muy_bajas entonces CF es iguales");
            reglas.Add("2", "Si CFS es muy_bajas y CFT es bajas entonces CF es superiores");
            reglas.Add("3", "Si CFS es muy_bajas y CFT es promedio entonces CF es superiores");
            reglas.Add("4", "Si CFS es muy_bajas y CFT es altas entonces CF es superiores");
            reglas.Add("5", "Si CFS es muy_bajas y CFT es muy_altas entonces CF es superiores");
            reglas.Add("6", "Si CFS es bajas y CFT es muy_bajas entonces CF es inferiores");
            reglas.Add("7", "Si CFS es bajas y CFT es bajas entonces CF es iguales");
            reglas.Add("8", "Si CFS es bajas y CFT es promedio entonces CF es superiores");
            reglas.Add("9", "Si CFS es bajas y CFT es altas entonces CF es superiores");
            reglas.Add("10", "Si CFS es bajas y CFT es muy_altas entonces CF es superiores");
            reglas.Add("11", "Si CFS es promedio y CFT es muy_bajas entonces CF es inferiores");
            reglas.Add("12", "Si CFS es promedio y CFT es bajas entonces CF es inferiores");
            reglas.Add("13", "Si CFS es promedio y CFT es promedio entonces CF es iguales");
            reglas.Add("14", "Si CFS es promedio y CFT es altas entonces CF es superiores");
            reglas.Add("15", "Si CFS es promedio y CFT es muy_altas entonces CF es superiores");
            reglas.Add("16", "Si CFS es altas y CFT es muy_bajas entonces CF es inferiores");
            reglas.Add("17", "Si CFS es altas y CFT es bajas entonces CF es inferiores");
            reglas.Add("18", "Si CFS es altas y CFT es promedio entonces CF es inferiores");
            reglas.Add("19", "Si CFS es altas y CFT es altas entonces CF es iguales");
            reglas.Add("20", "Si CFS es altas y CFT es muy_altas entonces CF es superiores");
            reglas.Add("21", "Si CFS es muy_altas y CFT es muy_bajas entonces CF es inferiores");
            reglas.Add("22", "Si CFS es muy_altas y CFT es bajas entonces CF es inferiores");
            reglas.Add("23", "Si CFS es muy_altas y CFT es promedio entonces CF es inferiores");
            reglas.Add("24", "Si CFS es muy_altas y CFT es altas entonces CF es inferiores");
            reglas.Add("25", "Si CFS es muy_altas y CFT es muy_altas entonces CF es iguales");

            return reglas;
        }

        /// <summary>
        /// Define las reglas para inferir la capacidad de un trabajador.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReglasCapacidad()
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            reglas.Add("1", "Si HB es inferiores y HD es inferiores y CF es inferiores entonces trabajador es no_capacitado");
            reglas.Add("2", "Si HB es inferiores y HD es inferiores y CF es iguales entonces trabajador es no_capacitado");
            reglas.Add("3", "Si HB es inferiores y HD es inferiores y CF es superiores entonces trabajador es medianamente_capacitado");
            reglas.Add("4", "Si HB es inferiores y HD es iguales y CF es inferiores entonces trabajador es no_capacitado");
            reglas.Add("5", "Si HB es inferiores y HD es iguales y CF es iguales entonces trabajador es medianamente_capacitado");
            reglas.Add("6", "Si HB es inferiores y HD es iguales y CF es superiores entonces trabajador es medianamente_capacitado");
            reglas.Add("7", "Si HB es inferiores y HD es superiores y CF es inferiores entonces trabajador es medianamente_capacitado");
            reglas.Add("8", "Si HB es inferiores y HD es superiores y CF es iguales entonces trabajador es medianamente_capacitado");
            reglas.Add("9", "Si HB es inferiores y HD es superiores y CF es superiores entonces trabajador es sobre_capacitado");
            reglas.Add("10", "Si HB es iguales y HD es inferiores y CF es inferiores entonces trabajador es no_capacitado");
            reglas.Add("11", "Si HB es iguales y HD es inferiores y CF es iguales entonces trabajador es medianamente_capacitado");
            reglas.Add("12", "Si HB es iguales y HD es inferiores y CF es superiores entonces trabajador es medianamente_capacitado");
            reglas.Add("13", "Si HB es iguales y HD es iguales y CF es inferiores entonces trabajador es medianamente_capacitado");
            reglas.Add("14", "Si HB es iguales y HD es iguales y CF es iguales entonces trabajador es capacitado");
            reglas.Add("15", "Si HB es iguales y HD es iguales y CF es superiores entonces trabajador es sobre_capacitado");
            reglas.Add("16", "Si HB es iguales y HD es superiores y CF es inferiores entonces trabajador es sobre_capacitado");
            reglas.Add("17", "Si HB es iguales y HD es superiores y CF es iguales entonces trabajador es sobre_capacitado");
            reglas.Add("18", "Si HB es iguales y HD es superiores y CF es superiores entonces trabajador es sobre_capacitado");
            reglas.Add("19", "Si HB es superiores y HD es inferiores y CF es inferiores entonces trabajador es medianamente_capacitado");
            reglas.Add("20", "Si HB es superiores y HD es inferiores y CF es iguales entonces trabajador es medianamente_capacitado");
            reglas.Add("21", "Si HB es superiores y HD es inferiores y CF es superiores entonces trabajador es medianamente_capacitado");
            reglas.Add("22", "Si HB es superiores y HD es iguales y CF es inferiores entonces trabajador es capacitado");
            reglas.Add("23", "Si HB es superiores y HD es iguales y CF es iguales entonces trabajador es sobre_capacitado");
            reglas.Add("24", "Si HB es superiores y HD es iguales y CF es superiores entonces trabajador es sobre_capacitado");
            reglas.Add("25", "Si HB es superiores y HD es superiores y CF es inferiores entonces trabajador es capacitado");
            reglas.Add("26", "Si HB es superiores y HD es superiores y CF es iguales entonces trabajador es sobre_capacitado");
            reglas.Add("27", "Si HB es superiores y HD es superiores y CF es superiores entonces trabajador es sobre_capacitado");

            return reglas;
        }

        public Dictionary<string, string> HB
        {
            get { return hb; }
        }

        public Dictionary<string, string> HD
        {
            get { return hd; }
        }

        public Dictionary<string, string> CF
        {
            get { return cf; }
        }

        public Dictionary<string, string> Capacidad
        {
            get { return capacidad; }
        }
    }
}
