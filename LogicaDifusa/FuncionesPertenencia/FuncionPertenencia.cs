using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaDifusa
{
    public class FuncionPertenencia : IFuncionPertenencia
    {
        public virtual bool CortarFuncion(double gradoPertenencia)
        {
            return false;
        }

        public virtual double GradoPertenencia(double x)
        {
            return 0;
        }

        public virtual double LimiteInferior()
        {
            return 0;
        }
        public virtual double LimiteSuperior()
        {
            return 0;
        }
    }
}
