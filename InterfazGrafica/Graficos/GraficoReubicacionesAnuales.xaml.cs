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
    /// Lógica de interacción para GraficoReubicacionesAnuales.xaml
    /// </summary>
    public partial class GraficoReubicacionesAnuales : UserControl
    {
        public GraficoReubicacionesAnuales()
        {
            InitializeComponent();
        }

        public void ReubicacionesAnuales(int idSeccion, int inicioAnioFiscal, int anio)
        {
            AdminDesempeño ad = new AdminDesempeño();
            ChartValues<double> reubicaciones = new ChartValues<double>();
            Dictionary<string, int> r = ad.ObtenerReubicacionesAnuales(idSeccion, inicioAnioFiscal, anio);
            string[] meses = new string[r.Count];
            int mes = 0;

            foreach (KeyValuePair<string, int> reubicacion in r)
            {
                string[] m = reubicacion.Key.Split('/');
                meses[mes] = m[1];
                mes += 1;
                reubicaciones.Add(Convert.ToDouble(reubicacion.Value));
            }

            if (SeriesCollection == null)
            {
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                {
                    Title = "Reubicaciones",
                    Values = new ChartValues<double>(),
                    MaxColumnWidth = 10
                }
                };
            }

            SeriesCollection[0].Values.Clear();
            foreach (double reubicacionesMes in reubicaciones)
            {
                SeriesCollection[0].Values.Add(reubicacionesMes);
            }

            Labels = meses;
            Formatter = value => value.ToString("N");

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }
    }
}
