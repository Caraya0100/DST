using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DST
{
    public class PruebaEvaluacion360
    {
        Alternativa no;
        Alternativa d;
        Alternativa c;
        Alternativa b;
        Alternativa a;
        Alternativa ne;

        public PruebaEvaluacion360()
        {
            /*Encuesta360 encuesta = new Encuesta360();
            no = new Alternativa("No desarrollada", "Necesita significativas mejoras para lograr enficiencia en esta conducta.", 0.0);
            d = new Alternativa("Necesita desarrollarse", "Necesita algunas mejoras para ser eficiente en esta conducta.", 25.0);
            c = new Alternativa("Competente", "Generalmente capacitado en esta conducta.", 50.0);
            b = new Alternativa("Altamente Competente", "Muy eficiente en esta conducta.", 75.0);
            a = new Alternativa("Modelo de rol", "Establece un estandar de excelencia en esta conducta. La conducta del evaluado es vista por los otros como un modelo a seguir", 100.0);
            ne = new Alternativa("No puede ser evaluada", "Esta conducta no puede ser evaluada.", -1);
            encuesta.AgregarPregunta("1", NuevaPregunta("Controla las emociones fuertes u otro estrés", new List<string>() { "Autocontrol" }));
            encuesta.AgregarPregunta("2", NuevaPregunta("En situaciones muy estresantes, calma a otros.", new List<string>() { "Autocontrol" }));
            encuesta.AgregarPregunta("3", NuevaPregunta("El trato hacia sus compañeros y superiores es amable y cortés.", new List<string>() { "Respeto" }));
            encuesta.AgregarPregunta("4", NuevaPregunta("Escucha a los demás antes de expresar su propio punto de vista.", new List<string>() { "Respeto" }));
        }

        private Pregunta NuevaPregunta(string descripcion, List<string> componentes)
        {
            Pregunta pregunta = new Pregunta();

            /*pregunta.Alternativas.Add("No_desarrollada", no.Valor);
            pregunta.Alternativas.Add("Necesita_desarrollarse", d.Valor);
            pregunta.Alternativas.Add("Competente", c.Valor);
            pregunta.Alternativas.Add("Altamente_competente", b.Valor);
            pregunta.Alternativas.Add("Modelo_de_rol", a.Valor);
            pregunta.Alternativas.Add("No_puede_ser_evaluado", ne.Valor);
            pregunta.Descripcion = descripcion;
            pregunta.Componentes = componentes;*/

            /*return pregunta;*/
        }
    }
}
