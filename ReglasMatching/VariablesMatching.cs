using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDifusa;

namespace DST
{
    /// <summary>
    /// Clase que contiene las variables linguisticas para la inferencia del matching.
    /// </summary>
    public class VariablesMatching
    {
        private VariableLinguistica hbPerfil;
        private VariableLinguistica hdPerfil;
        private VariableLinguistica cfPerfil;
        private VariableLinguistica hb;
        private VariableLinguistica hd;
        private VariableLinguistica cf;
        private VariableLinguistica trabajador;

        /// <summary>
        /// Constructor, inicializa las variables para el matching.
        /// </summary>
        public VariablesMatching()
        {
            /*ValorLinguistico muy_bajas = new ValorLinguistico("muy_bajas", new FuncionTrapezoidal(0.0, 0.0, 0.1, 0.3));
            ValorLinguistico bajas = new ValorLinguistico("bajas", new FuncionTriangular(0.1, 0.3, 0.5));
            ValorLinguistico promedio = new ValorLinguistico("promedio", new FuncionTriangular(0.3, 0.5, 0.7));
            ValorLinguistico altas = new ValorLinguistico("altas", new FuncionTriangular(0.5, 0.7, 0.9));
            ValorLinguistico muy_altas = new ValorLinguistico("muy_altas", new FuncionTrapezoidal(0.7, 0.9, 1.0, 1.0));
            ValorLinguistico no_compatibles = new ValorLinguistico("no_compatibles", new FuncionTriangular(0.0, 0.0, 1));
            ValorLinguistico medianamente_compatibles = new ValorLinguistico("medianamente_compatibles", new FuncionTriangular(0.0, 0.5, 1.0));
            ValorLinguistico compatibles = new ValorLinguistico("compatibles", new FuncionTriangular(0.5, 1.0, 1.5));
            ValorLinguistico exceden = new ValorLinguistico("exceden", new FuncionTriangular(1.0, 2.0, 2.0));*/

            ValoresMatching valores = new ValoresMatching();

            hbPerfil = new VariableLinguistica("HB", 0.0, 100.0);
            hbPerfil.AgregarValorLinguistico(valores.MuyBajas);
            hbPerfil.AgregarValorLinguistico(valores.Bajas);
            hbPerfil.AgregarValorLinguistico(valores.Promedio);
            hbPerfil.AgregarValorLinguistico(valores.Altas);
            hbPerfil.AgregarValorLinguistico(valores.MuyAltas);

            hdPerfil = new VariableLinguistica("HD", 0.0, 100.0);
            hdPerfil.AgregarValorLinguistico(valores.MuyBajas);
            hdPerfil.AgregarValorLinguistico(valores.Bajas);
            hdPerfil.AgregarValorLinguistico(valores.Promedio);
            hdPerfil.AgregarValorLinguistico(valores.Altas);
            hdPerfil.AgregarValorLinguistico(valores.MuyAltas);

            cfPerfil = new VariableLinguistica("CF", 0.0, 100.0);
            cfPerfil.AgregarValorLinguistico(valores.MuyBajas);
            cfPerfil.AgregarValorLinguistico(valores.Bajas);
            cfPerfil.AgregarValorLinguistico(valores.Promedio);
            cfPerfil.AgregarValorLinguistico(valores.Altas);
            cfPerfil.AgregarValorLinguistico(valores.MuyAltas);

            hb = new VariableLinguistica("HB", 0.0, 200.0);
            hb.AgregarValorLinguistico(valores.Inferiores);
            //hb.AgregarValorLinguistico(medianamente_compatibles);
            hb.AgregarValorLinguistico(valores.Iguales);
            hb.AgregarValorLinguistico(valores.Superiores);

            hd = new VariableLinguistica("HD", 0.0, 200.0);
            hd.AgregarValorLinguistico(valores.Inferiores);
            //hd.AgregarValorLinguistico(medianamente_compatibles);
            hd.AgregarValorLinguistico(valores.Iguales);
            hd.AgregarValorLinguistico(valores.Superiores);

            cf = new VariableLinguistica("CF", 0.0, 200.0);
            cf.AgregarValorLinguistico(valores.Inferiores);
            //cf.AgregarValorLinguistico(medianamente_compatibles);
            cf.AgregarValorLinguistico(valores.Iguales);
            cf.AgregarValorLinguistico(valores.Superiores);

            trabajador = new VariableLinguistica("trabajador", 0.0, 200.0);
            trabajador.AgregarValorLinguistico(valores.NoCapacitado);
            trabajador.AgregarValorLinguistico(valores.MedianamenteCapacitado);
            trabajador.AgregarValorLinguistico(valores.Capacitado);
            trabajador.AgregarValorLinguistico(valores.SobreCapacitado);
        }

        public VariableLinguistica HBPerfil
        {
            get { return hbPerfil; }
        }

        public VariableLinguistica HDPerfil
        {
            get { return hdPerfil; }
        }

        public VariableLinguistica CFPerfil
        {
            get { return cfPerfil; }
        }
        public VariableLinguistica HB
        {
            get { return hb; }
        }

        public VariableLinguistica HD
        {
            get { return hd; }
        }

        public VariableLinguistica CF
        {
            get { return cf; }
        }

        public VariableLinguistica Trabajador
        {
            get { return trabajador; }
        }
    }
}
