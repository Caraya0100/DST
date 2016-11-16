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
            hd = new Dictionary<string, string>();
            cf = new Dictionary<string, string>();
            capacidad = new Dictionary<string, string>();
        }

        /// <summary>
        /// Define las reglas para inferir la compatibilidad de las HB.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReglasHB()
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            reglas.Add("1", "Si HBS es muy_bajas y HBT es muy_bajas entonces HB es compatibles");
            reglas.Add("2", "Si HBS es muy_bajas y HBT es bajas entonces HB es exceden");
            reglas.Add("3", "Si HBS es muy_bajas y HBT es promedio entonces HB es exceden");
            reglas.Add("4", "");
            reglas.Add("5", "");
            reglas.Add("6", "");
            reglas.Add("7", "");
            reglas.Add("8", "");
            reglas.Add("9", "");
            reglas.Add("10", "");
            reglas.Add("11", "");
            reglas.Add("12", "");
            reglas.Add("13", "");
            reglas.Add("14", "");
            reglas.Add("15", "");
            reglas.Add("16", "");
            reglas.Add("17", "");
            reglas.Add("18", "");
            reglas.Add("19", "");
            reglas.Add("20", "");
            reglas.Add("21", "");
            reglas.Add("22", "");
            reglas.Add("23", "");
            reglas.Add("24", "");
            reglas.Add("25", "");

            return reglas;
        }

        /// <summary>
        /// Define las reglas para inferir la compatibilidad de las HD.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReglasHD()
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            reglas.Add("1", "Si HBS es muy_bajas y HBT es muy_bajas entonces HB es compatibles");
            reglas.Add("2", "Si HBS es muy_bajas y HBT es bajas entonces HB es exceden");
            reglas.Add("3", "Si HBS es muy_bajas y HBT es promedio entonces HB es exceden");
            reglas.Add("4", "");
            reglas.Add("5", "");
            reglas.Add("6", "");
            reglas.Add("7", "");
            reglas.Add("8", "");
            reglas.Add("9", "");
            reglas.Add("10", "");
            reglas.Add("11", "");
            reglas.Add("12", "");
            reglas.Add("13", "");
            reglas.Add("14", "");
            reglas.Add("15", "");
            reglas.Add("16", "");
            reglas.Add("17", "");
            reglas.Add("18", "");
            reglas.Add("19", "");
            reglas.Add("20", "");
            reglas.Add("21", "");
            reglas.Add("22", "");
            reglas.Add("23", "");
            reglas.Add("24", "");
            reglas.Add("25", "");

            return reglas;
        }

        /// <summary>
        /// Define las reglas para inferir la compatibilidad de las CF.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReglasCF()
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            reglas.Add("1", "Si HBS es muy_bajas y HBT es muy_bajas entonces HB es compatibles");
            reglas.Add("2", "Si HBS es muy_bajas y HBT es bajas entonces HB es exceden");
            reglas.Add("3", "Si HBS es muy_bajas y HBT es promedio entonces HB es exceden");
            reglas.Add("4", "");
            reglas.Add("5", "");
            reglas.Add("6", "");
            reglas.Add("7", "");
            reglas.Add("8", "");
            reglas.Add("9", "");
            reglas.Add("10", "");
            reglas.Add("11", "");
            reglas.Add("12", "");
            reglas.Add("13", "");
            reglas.Add("14", "");
            reglas.Add("15", "");
            reglas.Add("16", "");
            reglas.Add("17", "");
            reglas.Add("18", "");
            reglas.Add("19", "");
            reglas.Add("20", "");
            reglas.Add("21", "");
            reglas.Add("22", "");
            reglas.Add("23", "");
            reglas.Add("24", "");
            reglas.Add("25", "");

            return reglas;
        }

        /// <summary>
        /// Define las reglas para inferir la capacidad de un trabajador.
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string> ReglasCapacidad()
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            reglas.Add("1", "Si HBS es muy_bajas y HBT es muy_bajas entonces HB es compatibles");
            reglas.Add("2", "Si HBS es muy_bajas y HBT es bajas entonces HB es exceden");
            reglas.Add("3", "Si HBS es muy_bajas y HBT es promedio entonces HB es exceden");
            reglas.Add("4", "");
            reglas.Add("5", "");
            reglas.Add("6", "");
            reglas.Add("7", "");
            reglas.Add("8", "");
            reglas.Add("9", "");
            reglas.Add("10", "");
            reglas.Add("11", "");
            reglas.Add("12", "");
            reglas.Add("13", "");
            reglas.Add("14", "");
            reglas.Add("15", "");
            reglas.Add("16", "");
            reglas.Add("17", "");
            reglas.Add("18", "");
            reglas.Add("19", "");
            reglas.Add("20", "");
            reglas.Add("21", "");
            reglas.Add("22", "");
            reglas.Add("23", "");
            reglas.Add("24", "");
            reglas.Add("25", "");
            reglas.Add("26", "");
            reglas.Add("27", "");
            reglas.Add("28", "");
            reglas.Add("29", "");
            reglas.Add("30", "");
            reglas.Add("31", "");
            reglas.Add("32", "");
            reglas.Add("33", "");
            reglas.Add("34", "");
            reglas.Add("35", "");
            reglas.Add("36", "");
            reglas.Add("37", "");
            reglas.Add("38", "");
            reglas.Add("39", "");
            reglas.Add("40", "");
            reglas.Add("41", "");
            reglas.Add("42", "");
            reglas.Add("43", "");
            reglas.Add("44", "");
            reglas.Add("45", "");
            reglas.Add("46", "");
            reglas.Add("47", "");
            reglas.Add("48", "");
            reglas.Add("49", "");
            reglas.Add("50", "");
            reglas.Add("51", "");
            reglas.Add("52", "");
            reglas.Add("53", "");
            reglas.Add("54", "");
            reglas.Add("55", "");
            reglas.Add("56", "");
            reglas.Add("57", "");
            reglas.Add("58", "");
            reglas.Add("59", "");
            reglas.Add("60", "");
            reglas.Add("61", "");
            reglas.Add("62", "");
            reglas.Add("63", "");
            reglas.Add("64", "");

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
