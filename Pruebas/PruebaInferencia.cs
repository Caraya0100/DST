using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicaDifusa;
using SistemaInferencia;

namespace DST
{
    public class PruebaInferencia
    {
        public PruebaInferencia()
        {

        }

        public void PruebaPropina(double datoServicio, double datoEspera, double datoComida)
        {
            Inferencia inferencia = new Inferencia();

            VariableLinguistica servicio = new VariableLinguistica("servicio", 0.0, 10.0);
            servicio.AgregarValorLinguistico("pobre", new FuncionTriangular(0.0, 0.0, 5.0));
            servicio.AgregarValorLinguistico("bueno", new FuncionTriangular(0.0, 5.0, 10.0));
            servicio.AgregarValorLinguistico("excelente", new FuncionTriangular(5.0, 10.0, 10.0));
            inferencia.AgregarVariable(servicio);

            VariableLinguistica espera = new VariableLinguistica("espera", 0.0, 10.0);
            espera.AgregarValorLinguistico("poca", new FuncionTriangular(0.0, 0.0, 5.0));
            espera.AgregarValorLinguistico("promedio", new FuncionTriangular(2.0, 5.0, 8.0));
            espera.AgregarValorLinguistico("larga", new FuncionTriangular(6.0, 10.0, 10.0));
            inferencia.AgregarVariable(espera);

            VariableLinguistica comida = new VariableLinguistica("comida", 0.0, 10.0);
            comida.AgregarValorLinguistico("rancia", new FuncionTrapezoidal(0.0, 0.0, 1.0, 3.0));
            comida.AgregarValorLinguistico("deliciosa", new FuncionTrapezoidal(7.0, 9.0, 10.0, 10.0));
            inferencia.AgregarVariable(comida);

            VariableLinguistica tips = new VariableLinguistica("propina", 0.0, 30.0);
            tips.AgregarValorLinguistico("poca", new FuncionTriangular(0.0, 5.0, 10.0));
            tips.AgregarValorLinguistico("promedio", new FuncionTriangular(10.0, 15.0, 20.0));
            tips.AgregarValorLinguistico("generosa", new FuncionTriangular(20.0, 25.0, 30.0));
            inferencia.AgregarVariable(tips);

            inferencia.AgregarRegla("R1", "Si servicio es pobre y espera es larga y comida es rancia entonces propina es poca");
            //inferencia.AgregarRegla("R2", "Si servicio es good entonces tips es average");
            inferencia.AgregarRegla("R2", "Si servicio es excelente y comida es deliciosa entonces propina es generosa");
            inferencia.AgregarRegla("R3", "Si servicio es bueno y espera es poca o comida es deliciosa entonces propina es promedio");

            Dictionary<string, double> datos = new Dictionary<string, double>();
            datos.Add("servicio", datoServicio);
            datos.Add("espera", datoEspera);
            datos.Add("comida", datoComida);

            double salida = inferencia.Ejecutar(datos);

            Console.WriteLine(salida);
            Console.ReadLine();
        }
    }
}
