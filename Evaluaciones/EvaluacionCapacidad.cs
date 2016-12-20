using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDifusa;

namespace DST
{
    /// <summary>
    /// Clase para evaluar la capacidad de un trabajador.
    /// </summary>
    public class EvaluacionCapacidad
    {
        private double compatibilidadHB;
        private double compatibilidadHD;
        private double compatibilidadCF;
        private double capacidad;

        /// <summary>
        /// Constructor.
        /// </summary>
        public EvaluacionCapacidad()
        {
            compatibilidadHB = -1;
            compatibilidadHD = -1;
            compatibilidadCF = -1;
            capacidad = -1;
        }

        /// <summary>
        /// Evalua la capacidad de un trabajador con respecto a una seccion. 
        /// Retorna la capacidad del trabajador.
        /// </summary>
        /// <param name="perfil"></param>
        /// <param name="reglas"></param>
        /// <returns></returns>
        public double Ejecutar(Componente HBS, Componente HDS, Componente CFS, Componente HBT, Componente HDT, Componente CFT)
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();
            ReglasMatching reglasM = new ReglasMatching();
            VariablesMatching variablesM = new VariablesMatching();
            //Dictionary<string, Tuple<double, double>> datos = new Dictionary<string, Tuple<double, double>>();
            Dictionary<string, double> datos = new Dictionary<string, double>();
            List<VariableLinguistica> variables = new List<VariableLinguistica>();
            AdminPerfil ap = new AdminPerfil();
            double totalImportancia = 0;

            // Obtenemos los componentes desde la bd para obtener sus importancias.
            Componente HB = ap.ObtenerComponente("HB");
            Componente HD = ap.ObtenerComponente("HD");
            Componente CF = ap.ObtenerComponente("CF");
            // calculamos el total de la importancia para la normalizacion.
            totalImportancia = HB.Importancia + HD.Importancia + CF.Importancia;


            // Evaluamos la igualdad de las HB, HD, Y CF.
            compatibilidadHB = EvaluarCompatibilidad(HBS, HBT);
            datos.Add("HB", (compatibilidadHB * HB.Importancia) / 100);
            //datos.Add("HB", compatibilidadHB * (HB.Importancia / totalImportancia));
            compatibilidadHD = EvaluarCompatibilidad(HDS, HDT);
            datos.Add("HD", (compatibilidadHD * HD.Importancia) / 100);
            //datos.Add("HD", compatibilidadHD/* * (HD.Importancia / totalImportancia);
            compatibilidadCF = EvaluarCompatibilidad(CFS, CFT);
            datos.Add("CF", (compatibilidadCF * CF.Importancia) / 100);
            //datos.Add("CF", compatibilidadCF * (CF.Importancia / totalImportancia));

            reglas = reglasM.Capacidad;
            variables.Add(variablesM.HB);
            variables.Add(variablesM.HD);
            variables.Add(variablesM.CF);
            variables.Add(variablesM.Trabajador); // Consecuente.

            capacidad = EvaluacionDifusa.Evaluacion(datos, variables, reglas);

            return capacidad;
        }

        /// <summary>
        /// Evalua la compatibilidad entre dos componentes.
        /// </summary>
        /// <param name="seccion"></param>
        /// <param name="trabajador"></param>
        /// <returns></returns>
        private double EvaluarCompatibilidad(Componente seccion, Componente trabajador)
        {
            Dictionary<string, string> reglas = new Dictionary<string, string>();
            ReglasMatching reglasM = new ReglasMatching();
            Dictionary<string, double> datos = new Dictionary<string, double>();
            List<VariableLinguistica> variables = new List<VariableLinguistica>();

            if (seccion.Nombre == "HBS" && trabajador.Nombre == "HBT")
            {
                //Console.WriteLine("Flag HB");
                VariablesMatching variablesM = new VariablesMatching();
                VariableLinguistica HBS = new VariableLinguistica(variablesM.HBPerfil);
                VariableLinguistica HBT = new VariableLinguistica(variablesM.HBPerfil);
                reglas = reglasM.HB;
                HBS.Nombre = "HBS";
                HBT.Nombre = "HBT";
                variables.Add(HBS);
                variables.Add(HBT);
                variables.Add(variablesM.HB); // Consecuente.
            }
            else if (seccion.Nombre == "HDS" && trabajador.Nombre == "HDT")
            {
                //Console.WriteLine("Flag HD");
                VariablesMatching variablesM = new VariablesMatching();
                VariableLinguistica HDS = new VariableLinguistica(variablesM.HDPerfil);
                VariableLinguistica HDT = new VariableLinguistica(variablesM.HDPerfil);
                reglas = reglasM.HD;
                HDS.Nombre = "HDS";
                HDT.Nombre = "HDT";
                variables.Add(HDS);
                variables.Add(HDT);
                variables.Add(variablesM.HD); // Consecuente.
            }
            else if (seccion.Nombre == "CFS" && trabajador.Nombre == "CFT")
            {
                //Console.WriteLine("Flag CF");
                VariablesMatching variablesM = new VariablesMatching();
                VariableLinguistica CFS = new VariableLinguistica(variablesM.CFPerfil);
                VariableLinguistica CFT = new VariableLinguistica(variablesM.CFPerfil);
                reglas = reglasM.CF;
                CFS.Nombre = "CFS";
                CFT.Nombre = "CFT";
                variables.Add(CFS);
                variables.Add(CFT);
                variables.Add(variablesM.CF); // Consecuente.
            }

            datos.Add(seccion.Nombre, seccion.Puntaje);
            datos.Add(trabajador.Nombre, trabajador.Puntaje);

            return EvaluacionDifusa.Evaluacion(datos, variables, reglas);
        }

        public double CompatibilidadHB
        {
            get { return compatibilidadHB; }
        }

        public double CompatibilidadHD
        {
            get { return compatibilidadHD; }
        }

        public double CompatibilidadCF
        {
            get { return compatibilidadCF; }
        }

        public double Capaciddad
        {
            get { return capacidad; }
        }
    }
}
