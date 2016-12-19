using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfazGrafica
{
    /// <summary>
    /// Clase utilizada para el comboBox del datagrid de las preguntas 
    /// de desempeño de una sección.
    /// </summary>
    public class RespuestasGqm : List<string>
    {
        public RespuestasGqm(List<string> respuestas)
        {
            foreach (string respuesta in respuestas)
            {
                this.Add(respuesta);
            }
        }
    }
}
