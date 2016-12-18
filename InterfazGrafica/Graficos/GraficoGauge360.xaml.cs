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
using System.ComponentModel;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para GraficoGauge360.xaml
    /// </summary>
    public partial class GraficoGauge360 : UserControl, INotifyPropertyChanged
    {
        private double _value;

        public GraficoGauge360()
        {
            InitializeComponent();

            Formatter = x => x.ToString("P");
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
                DataContext = this;
            }
        }

        public Func<double, string> Formatter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
