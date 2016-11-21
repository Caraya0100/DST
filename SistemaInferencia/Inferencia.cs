using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDifusa;

namespace SistemaInferencia
{
    /// <summary>
    /// Clase para la inferencia.
    /// </summary>
    public class Inferencia
    {
        private Dictionary<string, string> baseReglas;
        private Dictionary<string, VariableLinguistica> variablesLinguisticas;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Inferencia()
        {
            BaseReglas = new Dictionary<string, string>();
            VariablesLinguisticas = new Dictionary<string, VariableLinguistica>();
        }

        /// <summary>
        /// Ejecuta la inferencia difusa, retornando un valor defuzzificado.
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public double Ejecutar(Dictionary<string, double> datos)
        {
            double defuzzificacion = 0;

            Fuzzificacion(datos);

            Dictionary<string, List<ValorLinguistico>> consecuentes = EvaluacionReglas();
            List<ValorLinguistico> conjuntoDifuso = Agregacion.Ejecutar(consecuentes);

            defuzzificacion = Centroide.Ejecutar(conjuntoDifuso);

            return defuzzificacion;
        }

        /// <summary>
        /// Ejecuta la inferencia difusa, con valores de importancia para los datos a fuzzificar, retornando un valor defuzzificado.
        /// </summary>
        /// <param name="datos"></param>
        /// <returns></returns>
        public double Ejecutar(Dictionary<string, Tuple<double, double>> datos)
        {
            double defuzzificacion = 0;

            Fuzzificacion(datos);

            Dictionary<string, List<ValorLinguistico>> consecuentes = EvaluacionReglas();
            
            List<ValorLinguistico> conjuntoDifuso = Agregacion.Ejecutar(consecuentes);

            defuzzificacion = Centroide.Ejecutar(conjuntoDifuso);

            return defuzzificacion;
        }

        /// <summary>
        /// Fuzzifica las variables.
        /// </summary>
        /// <param name="datos"></param>
        public void Fuzzificacion(Dictionary<string, double> datos)
        {
            foreach (KeyValuePair<string, double> dato in datos)
            {
                Console.WriteLine("Dato " + dato.Key + ": " + dato.Value);
                if (VariablesLinguisticas.ContainsKey(dato.Key))
                {
                    VariablesLinguisticas[dato.Key].Fuzzificar(dato.Value);
                }
            }
        }

        /// <summary>
        /// Fuzzifica las variables aplicando la importancia definida para cada una de ellas.
        /// </summary>
        /// <param name="datos"></param>
        public void Fuzzificacion(Dictionary<string, Tuple<double, double>> datos)
        {
            double totalImportancia = 0;
            // Obtenemos el total de las importancias.
            foreach (KeyValuePair<string, Tuple<double, double>> dato in datos)
            {
                totalImportancia += dato.Value.Item2;
            }
            // Fuzzificamos y normalizamos el grado de pertenencia resultante.
            foreach (KeyValuePair<string, Tuple<double, double>> dato in datos)
            {
                if (VariablesLinguisticas.ContainsKey(dato.Key))
                {
                    double importancia = dato.Value.Item2;
                    VariablesLinguisticas[dato.Key].Fuzzificar(dato.Value.Item1);

                    // Normalizamos.
                    foreach (KeyValuePair<string, ValorLinguistico > val in VariablesLinguisticas[dato.Key].Valores)
                    {
                        double normalizacion = val.Value.GradoPertenencia * (importancia / totalImportancia);

                        val.Value.GradoPertenencia = normalizacion;
                    }
                }
            }
        }

        /// <summary>
        /// Evalua las reglas de la base de reglas. Retorna 
        /// los consecuentes agrupados por los diferentes valores linguisticos.
        /// </summary>
        public Dictionary<string, List<ValorLinguistico>> EvaluacionReglas()
        {
            Dictionary<string, List<ValorLinguistico>> consecuentes = new Dictionary<string, List<ValorLinguistico>>();
            foreach (KeyValuePair<string, string> r in BaseReglas)
            {
                Regla regla = ObtenerRegla(r.Value);
                // Si se obtuve la regla a partir del string, se evalua.
                if (regla != null)
                {
                    //Console.WriteLine("Consecuente regla: " + regla.Consecuente.Item1);
                    string valorLinguistico = regla.Consecuente.Item2.Nombre;
                    // Se agrega el valor linguistico del consecuente si no ha sido agregado al diccionario.
                    if (!consecuentes.ContainsKey(valorLinguistico))
                    {
                        consecuentes.Add(valorLinguistico, new List<ValorLinguistico>());
                    }
                    // Evaluamos la regla y agregamos el valor linguistico resultante.
                    Console.Write("Regla: " + r.Key);
                    ValorLinguistico evaluacion = EvaluacionRegla(regla);
                    consecuentes[valorLinguistico].Add(evaluacion);
                }
            }

            return consecuentes;
        }

        /// <summary>
        /// Evalua una regla, devolviendo el valor linguistico cortado del consecuente de la regla.
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        public ValorLinguistico EvaluacionRegla(Regla regla)
        {
            List<double> valoresLinguisticos = new List<double>();
            double resultadOperador = -1;
            /* Guardamos los valores linguisticos en una lista para obtener facilmente
               el minimo o el maximo grado de pertenencia, depdendiendo del operador. */
            foreach (KeyValuePair<string, ValorLinguistico> actual in regla.Antecedente)
            {
                Console.Write(" Valor: " + actual.Value.GradoPertenencia);
                valoresLinguisticos.Add(actual.Value.GradoPertenencia);
            }

            if (regla.Operador == "y")
            {
                Console.Write(" Min");
                resultadOperador = valoresLinguisticos.Min();
            }
            else if (regla.Operador == "o")
            {
                Console.Write(" Max");
                resultadOperador = valoresLinguisticos.Max();
            }
            Console.WriteLine(" Resultado Operador: " + resultadOperador);
            return Implicacion.Ejecutar(resultadOperador, regla.Consecuente.Item2);
        }

        /// <summary>
        /// Obtiene la regla (objeto) a partir de una regla en string.
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        public Regla ObtenerRegla(string regla)
        {
            Regla resultado = null;
            string operador = ObtenerOperador(regla);
            if (operador != "")
            {
                Dictionary<string, ValorLinguistico> antecedente = new Dictionary<string, ValorLinguistico>(ObtenerAntecedente(regla));
                if (antecedente.Count > 0)
                {
                    Tuple<string, ValorLinguistico> consecuente = ObtenerConsecuente(regla);
                    if (consecuente != null)
                    {
                        resultado = new Regla(antecedente, consecuente, operador);
                    }
                }
            }

            return resultado;
        }

        /// <summary>
        /// Obtiene el operador de la regla.
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        public string ObtenerOperador(string regla)
        {
            string operador = "";
            string[] r = regla.Split(' ');
            // El tamaño minimo de una regla debe ser ocho.
            if (r.Length >= 8)
            {
                if (r[4] == "y" || r[4] == "o")
                {
                    operador = r[4];
                }
            }

            return operador;
        }

        /// <summary>
        /// Obtiene el antecedente de la regla.
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        public Dictionary<string, ValorLinguistico> ObtenerAntecedente(string regla)
        {
            string[] r = regla.Split(' ');
            Dictionary<string, ValorLinguistico> antecedente = new Dictionary<string, ValorLinguistico>();

            for (int i = 2; i < r.Length && r[i] != "entonces"; i++)
            {
                if ((r[i] == "es") && (i - 1 > 0) && (i + 1 < r.Length))
                {
                    string variable = r[i - 1];
                    string valor = r[i + 1];

                    if (VariablesLinguisticas.ContainsKey(variable))
                    {
                        ValorLinguistico valorLinguistico = VariablesLinguisticas[variable].ValorLinguistico(valor);
                        antecedente.Add(variable, valorLinguistico);
                    }
                }
            }

            return antecedente;
        }

        /// <summary>
        /// Obtiene el consecuente de la regla.
        /// </summary>
        /// <param name="regla"></param>
        /// <returns></returns>
        public Tuple<string, ValorLinguistico> ObtenerConsecuente(string regla)
        {
            string[] r = regla.Split(' ');
            Tuple<string, ValorLinguistico> consecuente = null;
            // El tamaño minimo de una regla debe ser ocho.
            if (r.Length >= 8)
            {
                string variable = r[r.Length - 3];
                string valor = r[r.Length - 1];
                // revisamos que la variable linguistica este en la inferencia.
                if (VariablesLinguisticas.ContainsKey(variable))
                {
                    ValorLinguistico valorLinguistico = VariablesLinguisticas[variable].ValorLinguistico(valor);
                    consecuente = new Tuple<string, ValorLinguistico>(variable, valorLinguistico);
                }
            }

            return consecuente;
        }

        /// <summary>
        /// Agrega una variable linguistica.
        /// </summary>
        /// <param name="variableLinguistica"></param>
        /// <returns></returns>
        public bool AgregarVariable(VariableLinguistica variableLinguistica)
        {
            if (!VariablesLinguisticas.ContainsKey(variableLinguistica.Nombre))
            {
                VariablesLinguisticas.Add(variableLinguistica.Nombre, variableLinguistica);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Elimina una variable linguistica de la inferencia.
        /// </summary>
        /// <param name="variableLinguistica"></param>
        /// <returns></returns>
        public bool EliminarVariable(string variableLinguistica)
        {
            if (!VariablesLinguisticas.ContainsKey(variableLinguistica))
            {
                VariablesLinguisticas.Remove(variableLinguistica);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Agrega una regla a la base de reglas de la inferencia.
        /// Si la regla ya existe (id), retorna false, de lo contrario se agrega la regla.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="regla"></param>
        /// <returns></returns>
        public bool AgregarRegla(string id, string regla)
        {
            if (!BaseReglas.ContainsKey(id))
            {
                BaseReglas.Add(id, regla);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Elimina una regla de la base de reglas de la inferencia.
        /// Si la regla (id) no existe retorna false, de lo contrario elimina la regla 
        /// y retorna true.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool EliminarRegla(string id)
        {
            if (!BaseReglas.ContainsKey(id))
            {
                BaseReglas.Remove(id);
                return true;
            }

            return false;
        }

        public Dictionary<string, VariableLinguistica> VariablesLinguisticas
        {
            get { return variablesLinguisticas; }
            set { variablesLinguisticas = value; }
        }

        public Dictionary<string, string> BaseReglas
        {
            get { return baseReglas; }
            set { baseReglas = value; }
        }
    }
}
