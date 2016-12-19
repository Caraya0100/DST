using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using DST;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para GraficoLineas.xaml
    /// </summary>
    public partial class GraficoVentasAnuales : UserControl
    {
        public GraficoVentasAnuales()
        {
            InitializeComponent();
        }

        public void VentasAnuales(int idSeccion, int inicioAnioFiscal, int anio)
        {
            AdminDesempeño ad = new AdminDesempeño();
            ChartValues<double> anioActual = new ChartValues<double>();
            ChartValues<double> anioAnterior = new ChartValues<double>();
            ChartValues<double> ventasPlan = new ChartValues<double>();
            Dictionary<string, Tuple<double, double, double>> ventas = ad.ObtenerVentasAnuales(idSeccion, inicioAnioFiscal, anio);
            string[] meses = new string[ventas.Count];

            int mes = 0;
            foreach (KeyValuePair<string, Tuple<double, double, double>> venta in ventas)
            {
                string[] m = venta.Key.Split('/');
                meses[mes] = m[1];
                mes += 1;
                anioActual.Add(venta.Value.Item1);
                anioAnterior.Add(venta.Value.Item2);
                ventasPlan.Add(venta.Value.Item3);
            }

            if (SeriesCollection == null)
            {
                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Ventas Año Actual",
                        Values = new ChartValues<double>(),
                        Fill = Brushes.Transparent
                    },
                    new LineSeries
                    {
                        Title = "Ventas Año Anterior",
                        Values = new ChartValues<double>(),
                        Fill = Brushes.Transparent
                    },
                    new LineSeries
                    {
                        Title = "Ventas Plan",
                        Values = new ChartValues<double>(),
                        Fill = Brushes.Transparent
                    }
                };
            }

            SeriesCollection[0].Values.Clear();
            foreach (double ventaActual in anioActual)
            {
                SeriesCollection[0].Values.Add(ventaActual);
            }
            SeriesCollection[1].Values.Clear();
            foreach (double ventaAnterior in anioAnterior)
            {
                SeriesCollection[1].Values.Add(ventaAnterior);
            }
            SeriesCollection[2].Values.Clear();
            foreach (double ventaPlan in ventasPlan)
            {
                SeriesCollection[2].Values.Add(ventaPlan);
            }

            Labels = meses;
            YFormatter = value => value + "$";

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
