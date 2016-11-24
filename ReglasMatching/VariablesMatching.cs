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

        public VariablesMatching()
        {
            ValorLinguistico muy_bajas = new ValorLinguistico("muy_bajas", new FuncionTrapezoidal(0.0, 0.0, 0.1, 0.3));
            ValorLinguistico bajas = new ValorLinguistico("bajas", new FuncionTriangular(0.1, 0.3, 0.5));
            ValorLinguistico promedio = new ValorLinguistico("promedio", new FuncionTriangular(0.3, 0.5, 0.7));
            ValorLinguistico altas = new ValorLinguistico("altas", new FuncionTriangular(0.5, 0.7, 0.9));
            ValorLinguistico muy_altas = new ValorLinguistico("muy_altas", new FuncionTrapezoidal(0.7, 0.9, 1.0, 1.0));
            ValorLinguistico no_compatibles = new ValorLinguistico("no_compatibles", new FuncionTriangular(0.0, 0.0, 1));
            ValorLinguistico medianamente_compatibles = new ValorLinguistico("medianamente_compatibles", new FuncionTriangular(0.0, 0.5, 1.0));
            ValorLinguistico compatibles = new ValorLinguistico("compatibles", new FuncionTriangular(0.5, 1.0, 1.5));
            ValorLinguistico exceden = new ValorLinguistico("exceden", new FuncionTriangular(1.0, 2.0, 2.0));

            hbPerfil = new VariableLinguistica("HB", 0.0, 1.0);
            hbPerfil.AgregarValorLinguistico(muy_bajas);
            hbPerfil.AgregarValorLinguistico(bajas);
            hbPerfil.AgregarValorLinguistico(promedio);
            hbPerfil.AgregarValorLinguistico(altas);
            hbPerfil.AgregarValorLinguistico(muy_altas);

            hdPerfil = new VariableLinguistica("HD", 0.0, 1.0);
            hdPerfil.AgregarValorLinguistico(muy_bajas);
            hdPerfil.AgregarValorLinguistico(bajas);
            hdPerfil.AgregarValorLinguistico(promedio);
            hdPerfil.AgregarValorLinguistico(altas);
            hdPerfil.AgregarValorLinguistico(muy_altas);

            cfPerfil = new VariableLinguistica("CF", 0.0, 1.0);
            cfPerfil.AgregarValorLinguistico(muy_bajas);
            cfPerfil.AgregarValorLinguistico(bajas);
            cfPerfil.AgregarValorLinguistico(promedio);
            cfPerfil.AgregarValorLinguistico(altas);
            cfPerfil.AgregarValorLinguistico(muy_altas);

            hb = new VariableLinguistica("HB", 0.0, 2.0);
            hb.AgregarValorLinguistico(no_compatibles);
            //hb.AgregarValorLinguistico(medianamente_compatibles);
            hb.AgregarValorLinguistico(compatibles);
            hb.AgregarValorLinguistico(exceden);

            hd = new VariableLinguistica("HD", 0.0, 2.0);
            hd.AgregarValorLinguistico(no_compatibles);
            //hd.AgregarValorLinguistico(medianamente_compatibles);
            hd.AgregarValorLinguistico(compatibles);
            hd.AgregarValorLinguistico(exceden);

            cf = new VariableLinguistica("CF", 0.0, 2.0);
            cf.AgregarValorLinguistico(no_compatibles);
            //cf.AgregarValorLinguistico(medianamente_compatibles);
            cf.AgregarValorLinguistico(compatibles);
            cf.AgregarValorLinguistico(exceden);

            trabajador = new VariableLinguistica("trabajador", 0.0, 2.0);
            trabajador.AgregarValorLinguistico("no_capacitado", no_compatibles.Fp);
            trabajador.AgregarValorLinguistico("medianamente_capacitado", medianamente_compatibles.Fp);
            trabajador.AgregarValorLinguistico("capacitado", compatibles.Fp);
            trabajador.AgregarValorLinguistico("sobre_capacitado", exceden.Fp);
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
