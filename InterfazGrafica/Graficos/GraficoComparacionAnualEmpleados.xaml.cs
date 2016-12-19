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
    /// Lógica de interacción para GraficoComparacionReubicacionesAnuales.xaml
    /// </summary>
    public partial class GraficoComparacionAnualEmpleados : UserControl
    {
        public GraficoComparacionAnualEmpleados()
        {
            InitializeComponent();
        }

        public void ComparativaAnual(int idSeccion, int inicioAnioFiscal, int anio, string tipoSeccion)
        {
            AdminDesempeño ad = new AdminDesempeño();
            ChartValues<double> totalEmpleados = new ChartValues<double>();
            ChartValues<double> empleadosCapacitados = new ChartValues<double>();
            Dictionary<string, int> te = ad.ObtenerTotalEmpleadosAnuales(idSeccion, inicioAnioFiscal, anio, tipoSeccion);
            Dictionary<string, int> ec = ad.ObtenerEmpleadosCapacitadosAnuales(idSeccion, inicioAnioFiscal, anio, tipoSeccion);
            string[] meses = new string[te.Count];
            int mes = 0;

            foreach (KeyValuePair<string, int> total in te)
            {
                string[] m = total.Key.Split('/');
                meses[mes] = m[1];
                mes += 1;
                totalEmpleados.Add(Convert.ToDouble(total.Value));
            }

            foreach (KeyValuePair<string, int> empleados in ec)
            {
                empleadosCapacitados.Add(Convert.ToDouble(empleados.Value));
            }

            if (SeriesCollection == null)
            {
                SeriesCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Trabajadores",
                        Values = new ChartValues<double>(),
                        MaxColumnWidth = 10
                    },
                    new ColumnSeries
                    {
                        Title = "Capacitados",
                        Values = new ChartValues<double>(),
                        MaxColumnWidth = 10
                    }
                };
            }

            SeriesCollection[0].Values.Clear();
            foreach (double totalMes in totalEmpleados)
            {
                SeriesCollection[0].Values.Add(totalMes);
            }
            SeriesCollection[1].Values.Clear();
            foreach (double capacitadosMes in empleadosCapacitados)
            {
                SeriesCollection[1].Values.Add(capacitadosMes);
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
