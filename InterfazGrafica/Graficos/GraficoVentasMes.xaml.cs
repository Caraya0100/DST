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
    /// Lógica de interacción para GraficoVentasMes.xaml
    /// </summary>
    public partial class GraficoVentasMes : UserControl
    {
        public GraficoVentasMes()
        {
            InitializeComponent();
        }

        public void VentasMes(int idSeccion, int mes, int anio)
        {
            AdminDesempeño ad = new AdminDesempeño();
            Tuple<double, double, double> ventas = ad.ObtenerVentasMes(idSeccion, mes, anio);

            if (SeriesCollection == null)
            {
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Ventas Actuales",
                        Values = new ChartValues<double>() { ventas.Item2, ventas.Item1, ventas.Item3 },
                        MaxColumnWidth = 10
                    }
                };
            }

            SeriesCollection[0].Values.Clear();
            SeriesCollection[0].Values.Add(ventas.Item2);
            SeriesCollection[0].Values.Add(ventas.Item1);
            SeriesCollection[0].Values.Add(ventas.Item3);

            Labels = new string[] { "Anterior", "Actual", "Plan" };
            YFormatter = value => value + "$";

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
