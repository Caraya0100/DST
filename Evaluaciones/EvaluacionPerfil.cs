using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;
using LogicaDifusa;
using SistemaInferencia;

namespace DST
{
    /// <summary>
    /// Clase para la evaluacion de un perfil.
    /// </summary>
    public static class EvaluacionPerfil
    {
        /// <summary>
        /// Evalua el perfil. Retorna el perfil evaluado.
        /// </summary>
        /// <param name="perfil"></param>
        /// <param name="reglas"></param>
        /// <returns></returns>
        public static Perfil Ejecutar(Perfil perfil, int idSeccion)
        {
            Perfil p = new Perfil(perfil);
            Dictionary<string, Tuple<double, double>> datos = new Dictionary<string, Tuple<double, double>>();
            List<VariableLinguistica> variables = new List<VariableLinguistica>();
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            // Evaluamos las HB, HD, Y CF.
            datos = Datos(p.Blandas);
            variables = AdminLD.VariablesLinsguisticas(p.Blandas);
            reglas = AdminReglas.ReglasSeccion(idSeccion, "HB");
            p.HB.Puntaje = Evaluacion(datos, variables, reglas);

            datos = Datos(p.Duras);
            variables = AdminLD.VariablesLinsguisticas(p.Duras);
            reglas = AdminReglas.ReglasSeccion(idSeccion, "HD");
            p.HD.Puntaje = Evaluacion(datos, variables, reglas);

            datos = Datos(p.Fisicas);
            variables = AdminLD.VariablesLinsguisticas(p.Fisicas);
            reglas = AdminReglas.ReglasSeccion(idSeccion, "CF");
            p.CF.Puntaje = Evaluacion(datos, variables, reglas);

            return p;
        }

        /// <summary>
        /// Realiza la inferencia para evaluar las HB/HD/CF del perifl.
        /// </summary>
        /// <param name="datos"></param>
        /// <param name="variables"></param>
        /// <param name="reglas"></param>
        /// <returns></returns>
        public static double Evaluacion(Dictionary<string, Tuple<double, double>> datos, List<VariableLinguistica> variables, Dictionary<string, string> reglas)
        {
            Inferencia inferencia = new Inferencia();

            foreach (VariableLinguistica variable in variables)
            {
                inferencia.AgregarVariable(variable);
            }

            foreach (KeyValuePair<string, string> regla in reglas)
            {
                inferencia.AgregarRegla(regla.Key, regla.Value);
            }

            return inferencia.Ejecutar(datos);
        }

        /// <summary>
        /// Devuelve los datos a partir
        /// </summary>
        /// <param name="componentes"></param>
        /// <returns></returns>
        public static Dictionary<string, Tuple<double, double>> Datos(Dictionary<string, Componente> componentes)
        {
            Dictionary<string, Tuple<double, double>> datos = new Dictionary<string, Tuple<double, double>>();

            foreach (KeyValuePair<string, Componente> componente in componentes)
            {
                datos.Add(componente.Key, new Tuple<double, double>(componente.Value.Puntaje, componente.Value.Importancia));
            }

            return datos;
        }
    }
}
