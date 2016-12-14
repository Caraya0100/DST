using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDifusa;

namespace DST
{
    public class ValoresMatching
    {
        ValorLinguistico muyBajas;
        ValorLinguistico bajas;
        ValorLinguistico promedio;
        ValorLinguistico altas;
        ValorLinguistico muyAltas;
        ValorLinguistico inferiores;
        ValorLinguistico iguales;
        ValorLinguistico superiores;
        ValorLinguistico noCapacitado;
        ValorLinguistico medianamenteCapacitado;
        ValorLinguistico capacitado;
        ValorLinguistico sobreCapacitado;

        public ValoresMatching()
        {
            muyBajas = new ValorLinguistico("muy_bajas", new FuncionTrapezoidal(0.0, 0.0, 10.0, 30.0));
            bajas = new ValorLinguistico("bajas", new FuncionTriangular(10.0, 30.0, 50.0));
            promedio = new ValorLinguistico("promedio", new FuncionTriangular(30.0, 50.0, 70.0));
            altas = new ValorLinguistico("altas", new FuncionTriangular(50.0, 70.0, 90.0));
            muyAltas = new ValorLinguistico("muy_altas", new FuncionTrapezoidal(70.0, 90.0, 100.0, 100.0));

            inferiores = new ValorLinguistico("inferiores", new FuncionTriangular(0.0, 0.0, 100.0));
            iguales = new ValorLinguistico("iguales", new FuncionTriangular(50.0, 100.0, 150.0));
            superiores = new ValorLinguistico("superiores", new FuncionTriangular(100.0, 200.0, 200.0));

            noCapacitado = new ValorLinguistico("no_capacitado", new FuncionTriangular(0.0, 0.0, 100.0));
            medianamenteCapacitado = new ValorLinguistico("medianamente_capacitado", new FuncionTriangular(0.0, 50.0, 100.0));
            capacitado = new ValorLinguistico("capacitado", new FuncionTriangular(50.0, 100.0, 150.0));
            sobreCapacitado = new ValorLinguistico("sobre_capacitado", new FuncionTriangular(100.0, 200.0, 200.0));
        }

        public ValorLinguistico MuyBajas
        {
            get { return muyBajas; }
        }

        public ValorLinguistico Bajas
        {
            get { return bajas; }
        }

        public ValorLinguistico Promedio
        {
            get { return promedio; }
        }

        public ValorLinguistico Altas
        {
            get { return altas; }
        }

        public ValorLinguistico MuyAltas
        {
            get { return muyAltas; }
        }

        public ValorLinguistico Inferiores
        {
            get { return inferiores; }
        }

        public ValorLinguistico Iguales
        {
            get { return iguales; }
        }

        public ValorLinguistico Superiores
        {
            get { return superiores; }
        }

        public ValorLinguistico NoCapacitado
        {
            get { return noCapacitado; }
        }

        public ValorLinguistico MedianamenteCapacitado
        {
            get { return medianamenteCapacitado; }
        }

        public ValorLinguistico Capacitado
        {
            get { return capacitado; }
        }

        public ValorLinguistico SobreCapacitado
        {
            get { return sobreCapacitado; }
        }

    }
}
