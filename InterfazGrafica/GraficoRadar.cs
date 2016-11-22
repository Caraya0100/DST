using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;

namespace InterfazGrafica
{
    class GraficoRadar
    {
        private string[] habilidades;
        private double[] puntajesSeccion;
        private double[] puntajesTrabajador;
        private string serieTrabajador;
        private string serieSeccion;
        private string tipoGrafico;
        private Chart grafico;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="habilidades"></param>
        /// <param name="puntajesSeccion"></param>
        /// <param name="puntajesTrabajador"></param>
        /// <param name="grafico"></param>

        public GraficoRadar(string[] habilidades, double[] puntajesSeccion, double[] puntajesTrabajador, Chart grafico)
        {
            this.habilidades = habilidades;
            this.puntajesSeccion = puntajesSeccion;
            this.puntajesTrabajador = puntajesTrabajador;
            this.grafico = grafico;
            this.serieTrabajador = "Trabajador";
            this.serieSeccion = "Seccion";
        }
        public string[] Habilidades
        {
            get { return habilidades; }
            set { habilidades = value; }
        }

        public double[] PuntajesSeccion
        {
            get { return puntajesSeccion; }
            set { puntajesSeccion = value; }
        }

        public double[] PuntajesTrabajador
        {
            get { return puntajesTrabajador; }
            set { puntajesTrabajador = value; }
        }

        public Chart Grafico
        {
            get { return grafico; }
            set { grafico = value; }
        }

        public string SerieTrabajador
        {
            get { return serieTrabajador; }
            set { serieTrabajador = value; }
        }

        public string SerieSeccion
        {
            get { return serieSeccion; }
            set { serieSeccion = value; }
        }

        public string TipoGrafico
        {
            get { return tipoGrafico; }
            set { tipoGrafico = value; }
        }

        public void constructorGrafico()
        {
            grafico.Series.Clear();
            grafico.Series.Add(this.serieTrabajador);
            grafico.Series.Add(this.SerieSeccion);

            grafico.ChartAreas["ChartArea1"].AxisY.Maximum = 200;
            grafico.ChartAreas["ChartArea1"].AxisY.Minimum = 0;
            grafico.ChartAreas["ChartArea1"].AxisY2.Maximum = 200;
            grafico.ChartAreas["ChartArea1"].AxisY2.Minimum = 0;

            grafico.Series[this.serieTrabajador].Points.DataBindXY(this.habilidades, this.puntajesSeccion);
            grafico.Series[this.serieSeccion].Points.DataBindXY(this.habilidades, this.puntajesTrabajador);

            // Set radar chart type
            grafico.Series[serieTrabajador].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            grafico.Series[serieSeccion].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Radar;
            // Set radar chart style (Area, Line or Marker)
            grafico.Series[serieTrabajador]["RadarDrawingStyle"] = tipoGrafico;
            grafico.Series[serieSeccion]["RadarDrawingStyle"] = tipoGrafico;
            // Set circular area drawing style (Circle or Polygon)
            grafico.Series[serieTrabajador]["AreaDrawingStyle"] = "Polygon";
            grafico.Series[serieSeccion]["AreaDrawingStyle"] = "Polygon";
            // Set labels style (Auto, Horizontal, Circular or Radial)
            grafico.Series[serieTrabajador]["CircularLabelsStyle"] = "Horizontal";
            grafico.Series[serieSeccion]["CircularLabelsStyle"] = "Horizontal";
            // Show as 3D
            grafico.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
        }

    }
}
