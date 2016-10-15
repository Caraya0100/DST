using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    class FuncionTrapezoidal : FuncionPertenencia
    {
        double _valorIzqAbajo, _valorIzqArriba, _valorDerchArriba, _valorDerchAbajo, valorCorte;
        private bool cortada;


        /// <summary>
        /// Constructor vacio.
        /// </summary>
        public FuncionTrapezoidal()
        { }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="valorIzqAbajo">Point 1</param>
        /// <param name="valorIzqArriba">Point 2</param>
        /// <param name="valorDerchArriba">Point 3</param>
        /// <param name="valorDerchAbajo">Point 4</param>
        public FuncionTrapezoidal(double valorIzqAbajo, double valorIzqArriba, double valorDerchArriba, double valorDerchAbajo)
        {
            if (!(valorIzqAbajo <= valorIzqArriba && valorIzqArriba <= valorDerchArriba && valorDerchArriba <= valorDerchAbajo))
            {
                throw new ArgumentException();
            }

            _valorIzqAbajo = valorIzqAbajo;
            _valorIzqArriba = valorIzqArriba;
            _valorDerchArriba = valorDerchArriba;
            _valorDerchAbajo = valorDerchAbajo;
            Cortada = false;
            ValorCorte = 0;
        }

        /// <summary>
        /// Devuelve el valor fusificado.
        /// </summary>
        /// <param name="x">Argument (valor eje x)</param>
        /// <returns></returns>
        public override double GradoPertenencia(double x)
        {
            double resultado = 0;

            if (x == _valorIzqAbajo && x == _valorIzqArriba)
            {
                resultado = 1.0;
            }
            else if (x == _valorDerchArriba && x == _valorDerchAbajo)
            {
                resultado = 1.0;
            }
            else if (x <= _valorIzqAbajo || x >= _valorDerchAbajo)
            {
                resultado = 0;
            }
            else if ((x >= _valorIzqArriba) && (x <= _valorDerchArriba))
            {
                resultado = 1;
            }
            else if ((x > _valorIzqAbajo) && (x < _valorIzqArriba))
            {
                resultado = (x / (_valorIzqArriba - _valorIzqAbajo)) - (_valorIzqAbajo / (_valorIzqArriba - _valorIzqAbajo));
            }
            else
            {
                resultado = (-x / (_valorDerchAbajo - _valorDerchArriba)) + (_valorDerchAbajo / (_valorDerchAbajo - _valorDerchArriba));
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
            if (gradoPertenencia >= 0 && gradoPertenencia <= 1)
            {
                ValorCorte = gradoPertenencia;
                Cortada = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Valor más a la izquierda.
        /// </summary>
        public double ValorIzqAbajo
        {
            get { return _valorIzqAbajo; }
            set { _valorIzqAbajo = value; }
        }

        /// <summary>
        /// Valor izquierda superior.
        /// </summary>
        public double ValorIzqArriba
        {
            get { return _valorIzqArriba; }
            set { _valorIzqArriba = value; }
        }

        /// <summary>
        /// Valor derecha superior.
        /// </summary>
        public double ValorDerchArriba
        {
            get { return _valorDerchArriba; }
            set { _valorDerchArriba = value; }
        }

        /// <summary>
        /// Valor más a la derecha
        /// </summary>
        public double ValorDerchAbajo
        {
            get { return _valorDerchAbajo; }
            set { _valorDerchAbajo = value; }
        }

        public override double LimiteInferior()
        {
            return ValorIzqAbajo;
        }

        public override double LimiteSuperior()
        {
            return ValorDerchAbajo;
        }

        public bool Cortada
        {
            get { return cortada; }
            set { cortada = value; }
        }

        public double ValorCorte
        {
            get { return valorCorte; }
            set { valorCorte = value; }
        }
    }
}
