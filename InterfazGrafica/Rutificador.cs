using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace InterfazGrafica
{
    class Rutificador
    {
        /// <summary>
        /// Metodo que comprueba si el digito verificador es correcto.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        public static string Digito(int rut)
        {
            int suma = 0;
            int multiplicador = 1;
            while (rut != 0)
            {
                multiplicador++;
                if (multiplicador == 8)
                    multiplicador = 2;
                suma += (rut % 10) * multiplicador;
                rut = rut / 10;
            }
            suma = 11 - (suma % 11);
            if (suma == 11)
                return "0";
            else if (suma == 10)
                return "K";
            else
            {
                return suma.ToString();
            }
        }

        /// <summary>
        /// Metodo que valida si una cadena de texto corresponde al formato de rut especificado.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        public bool ValidaRut(string rut)
        {
            rut = rut.Replace(".", "").ToUpper();
            Regex expresion = new Regex("^([0-9]+-[0-9K])$");
            string dv = rut.Substring(rut.Length - 1, 1);
            if (!expresion.IsMatch(rut))
            {
                return false;
            }
            char[] charCorte = { '-' };
            string[] rutTemp = rut.Split(charCorte);
            if (dv != Digito(int.Parse(rutTemp[0])))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Metodo que valida una cadena de texto como rut con el digito verificador por separado.
        /// </summary>
        /// <param name="rut"></param>
        /// <param name="dv"></param>
        /// <returns></returns>
        public bool ValidaRut(string rut, string dv)
        {
            return ValidaRut(rut + "-" + dv);
        }

        /// <summary>
        /// Metodo que asigna el formato rut correcto a una cadena de texto.
        /// </summary>
        /// <param name="rut"></param>
        /// <returns></returns>
        public string formatoRut(string rut)
        {
            string rutFormateado = rut;
            if (!rut.Contains("."))
            {
                int multiplo = 3;
                int indice = 0;
                string nuevoFormato = string.Empty;
                for (int i = (rut.Length - 1); i >= 0; i--)
                {
                    if (indice == multiplo)
                    {
                        nuevoFormato += ".";
                        multiplo = multiplo + 3;
                        nuevoFormato += rut[i];
                    }
                    else
                        nuevoFormato += rut[i];
                    indice++;
                }
                rutFormateado = string.Empty;
                for (int i = (nuevoFormato.Length - 1); i >= 0; i--)
                    rutFormateado += nuevoFormato[i];
            }
            return rutFormateado;
        }
    }
}