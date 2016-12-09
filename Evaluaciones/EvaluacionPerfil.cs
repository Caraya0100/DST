﻿using System;
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
            VariablesMatching variablesM = new VariablesMatching();
            Perfil p = new Perfil(perfil);
            Dictionary<string, Tuple<double, double>> datos = new Dictionary<string, Tuple<double, double>>();
            List<VariableLinguistica> variables = new List<VariableLinguistica>();
            Dictionary<string, string> reglas = new Dictionary<string, string>();

            AdminLD adminLD = new AdminLD();
            AdminReglas adminReglas = new AdminReglas();

            // Evaluamos las HB, HD, Y CF.
            datos = Datos(p.Blandas);
            variables = adminLD.VariablesLinsguisticas(p.Blandas);
            variables.Add(variablesM.HBPerfil); // Consecuente.
            reglas = adminReglas.ReglasSeccion(idSeccion, "HB");
            p.HB.Puntaje = EvaluacionDifusa.Evaluacion(datos, variables, reglas);

            datos = Datos(p.Duras);
            variables = adminLD.VariablesLinsguisticas(p.Duras);
            variables.Add(variablesM.HDPerfil); // Consecuente.
            reglas = adminReglas.ReglasSeccion(idSeccion, "HD");
            p.HD.Puntaje = EvaluacionDifusa.Evaluacion(datos, variables, reglas);

            datos = Datos(p.Fisicas);
            variables = adminLD.VariablesLinsguisticas(p.Fisicas);
            variables.Add(variablesM.CFPerfil); // Consecuente.
            reglas = adminReglas.ReglasSeccion(idSeccion, "CF");
            p.CF.Puntaje = EvaluacionDifusa.Evaluacion(datos, variables, reglas);

            return p;
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
