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
    /// Lógica de interacción para GraficoObjetivosAnual.xaml
    /// </summary>
    public partial class GraficoObjetivosAnual : UserControl
    {
        public GraficoObjetivosAnual()
        {
            InitializeComponent();
        }

        public void ObjetivosAnuales(int idSeccion, int inicioAnioFiscal, int anio)
        {
            AdminDesempeño ad = new AdminDesempeño();
            ChartValues<double> valoresDesempenos = new ChartValues<double>();
            Dictionary<string, double> desempenos = ad.ObtenerDesempenoGqmAnual(idSeccion, inicioAnioFiscal, anio);
            string[] meses = new string[desempenos.Count];

            int mes = 0;
            foreach (KeyValuePair<string, double> desempeno in desempenos)
            {
                string[] m = desempeno.Key.Split('/');
                meses[mes] = m[1];
                mes += 1;
                valoresDesempenos.Add(desempeno.Value);
            }

            if (SeriesCollection == null)
            {
                SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Desempeños",
                        Values = new ChartValues<double>(),
                        Fill = Brushes.Transparent
                    }
                };
            }

            SeriesCollection[0].Values.Clear();
            foreach (double valorDesempeno in valoresDesempenos)
            {
                SeriesCollection[0].Values.Add(valorDesempeno);
            }

            Labels = meses;
            YFormatter = value => value + "%";

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
