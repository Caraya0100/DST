using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    /// <summary>
    /// Funcion Triangular de Pertenencia.
    /// </summary>
    public class FuncionTriangular : FuncionPertenencia
    {
        private double _valorIzq, _valorCentro, _valorDerch, valorCorte;
        private bool cortada;

        /// <summary>
        /// Constructor vacio.
        /// </summary>
        public FuncionTriangular()
        { }

        /// <summary>
        /// Constructor, recibe los tres valores "limites" de la funcion.
        /// </summary>
        /// <param name="valorIzq">Point 1</param>
        /// <param name="valorCentro">Point 2</param>
        /// <param name="valorDerch">Point 3</param>
        public FuncionTriangular(double valorIzq, double valorCentro, double valorDerch)
        {
            if (!(valorIzq <= valorCentro && valorCentro <= valorDerch))
            {
                throw new ArgumentException();
            }

            _valorIzq = valorIzq;
            _valorCentro = valorCentro;
            _valorDerch = valorDerch;
            Cortada = false;
            ValorCorte = -1;
        }

        /// <summary>
        /// Obtiene el valor fusificado.
        /// </summary>
        /// <param name="x">Argument (valor eje x)</param>
        /// <returns></returns>
        public override double GradoPertenencia(double x)
        {
            double resultado = 0;

            if (x == _valorIzq && x == _valorCentro)
            {
                resultado = 1.0;
            }
            else if (x == _valorCentro && x == _valorDerch)
            {
                resultado = 1.0;
            }
            else if (x <= _valorIzq || x >= _valorDerch)
            {
                resultado = 0;
            }
            else if (x == _valorCentro)
            {
                resultado = 1;
            }
            else if ((x > _valorIzq) && (x < _valorCentro))
            {

                resultado = (x / (_valorCentro - _valorIzq)) - (_valorIzq / (_valorCentro - _valorIzq));
            }
            else
            {
                resultado = (-x / (_valorDerch - _valorCentro)) + (_valorDerch / (_valorDerch - _valorCentro));
            }
            // Si la funcion fue cortada, el grado de pertenencia no puede sobrepasar al valor de corte.
            if (resultado > ValorCorte && Cortada)
            {
                resultado = ValorCorte;
            }
            return resultado;
        }

        public override bool CortarFuncion(double gradoPertenencia)
        {
            if (gradoPertenencia >= 0  && gradoPertenencia <= 1)
            {
                ValorCorte = gradoPertenencia;
                Cortada = true;
                return true;
            }
            return false;
        }

        public override double LimiteInferior()
        {
            return ValorIzq;
        }

        public override double LimiteSuperior()
        {
            return ValorDerch;
        }

        /// <summary>
        /// Valor más a la izaquierda.
        /// </summary>
        public double ValorIzq
        {
            get { return _valorIzq; }
            set { _valorIzq = value; }
        }

        /// <summary>
        /// Valor del centro
        /// </summary>
        public double ValorCentro
        {
            get { return _valorCentro; }
            set { _valorCentro = value; }
        }

        public bool Cortada
        {
            get { return cortada; }
            set { cortada = value; }
        }

        public override double ValorCorte
        {
            get { return valorCorte; }
            set { valorCorte = value; }
        }

        /// <summary>
        /// Valor más a la derecha
        /// </summary>
        public double ValorDerch
        {
            get { return _valorDerch; }
            set { _valorDerch = value; }
        }
    }
}
