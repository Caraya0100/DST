using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    /// <summary>
    /// Intefaz para las funciones de pertenencia.
    /// </summary>
    public interface IFuncionPertenencia
    {
        /// <summary>
        /// Calcula el grado de pertenencia.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        double GradoPertenencia(double x);

        /// <summary>
        /// Escala la funcion de pertenencia al grado de pertenencia pasado.
        /// </summary>
        /// <param name="gradoPertenencia"></param>
        bool CortarFuncion(double gradoPertenencia);

        /// <summary>
        /// Indica el limite inferior (donde comienza) de la funcion de pertenencia.
        /// </summary>
        /// <returns></returns>
        double LimiteInferior();

        /// <summary>
        /// Indica el limite inferior (donde termina) de la funcion de pertenencia.
        /// </summary>
        /// <returns></returns>
        double LimiteSuperior();

        double ValorCorte
        {
            get;
            set;
        }
    }
}
