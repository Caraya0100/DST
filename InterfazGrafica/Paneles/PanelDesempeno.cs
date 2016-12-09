using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;
using System.ComponentModel;

namespace DST
{
    /// <summary>
    /// Clase para el panel de desempeño de una seccion. 
    /// Manipula todos los elementos correspondientes para mostrar el desempeño 
    /// de la seccion.
    /// </summary>
    public class PanelDesempeno
    {
        private MetroWindow ventana;
        private Seccion seccion;
        private double valorDAnterior;
        private double valorDPlan;

        /// <summary>
        /// Constructor, recibe la ventana y la seccion.
        /// </summary>
        /// <param name="ventana"></param>
        /// <param name="seccion"></param>
        public PanelDesempeno(Seccion seccion)
        {
            this.seccion = seccion;
            GraficoDesempenoAnual();
            GraficoDesempenoAnterior();
            GraficoDesempenoPlan();
        }

        public void GraficoDesempenoAnterior()
        {
            DesempenoAnterior(.1);
        }

        public void GraficoDesempenoPlan()
        {
            DesempenoPlan(.1);
        }

        /// <summary>
        /// Asigna y actuliza el valor de los graficos Ranking y Trabajadores.
        /// </summary>
        /// <param name="doble"></param>
        public void DesempenoAnterior(double doble)
        {
            ValorDAnterior = doble;
            FormatoPorcentaje = x => x.ToString("P");
        }

        /// Asigna y actuliza el valor de los graficos Ranking y Trabajadores.
        /// </summary>
        /// <param name="doble"></param>
        public void DesempenoPlan(double doble)
        {
            ValorDAnterior = doble;
            FormatoPorcentaje = x => x.ToString("P");
        }

        public void GraficoDesempenoAnual()
        {
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Ventas Año Actual",
                    Values = new ChartValues<double> {
                        1150000,
                        1820000, 
                        1052000,
                        1292000,
                        1352000,
                        1152000,
                        1252000,
                        1102000,
                        2012000,
                        2462000,
                        1252000,
                        1109000,
                    }, 
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Ventas Año Anterior",
                    Values = new ChartValues<double> {
                        1050000,
                        1220000,
                        1050000,
                        1092000,
                        1152000,
                        1052000,
                        1200100,
                        1302000,
                        2612000,
                        2062000,
                        1152000,
                        1009000,
                    },
                    Fill = Brushes.Transparent
                    //PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Ventas Plan",
                    Values = new ChartValues<double> {
                        2150000,
                        2820000,
                        1052000,
                        3292000,
                        2352000,
                        2152000,
                        2252000,
                        2102000,
                        2012000,
                        2462000,
                        2252000,
                        3109000,
                    },
                    Fill = Brushes.Transparent
                    //PointGeometry = DefaultGeometries.Square,
                    //PointGeometrySize = 15
                }
            };

            Labels = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Oct", "Nov", "Dic" };
            YFormatter = value => value.ToString("C");

            //modifying any series values will also animate and update the chart
            //SeriesCollection[3].Values.Add(5d);
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public double ValorDAnterior
        {
            get { return valorDAnterior; }
            set
            {
                valorDAnterior = value;
                OnPropertyChanged("ValorDAnterior");
            }
        }

        public double ValorDPlan
        {
            get { return valorDPlan; }
            set
            {
                valorDPlan = value;
                OnPropertyChanged("ValorDPlan");
            }
        }

        public Func<double, string> FormatoPorcentaje { get; set; }

        public event PropertyChangedEventHandler PropiedadCambiada;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropiedadCambiada != null)
                PropiedadCambiada(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
