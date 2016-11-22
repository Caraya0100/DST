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
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using System.ComponentModel;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaDetalleHabilidades.xaml
    /// </summary>
    public partial class VentanaDetalleHabilidades : MetroWindow, INotifyPropertyChanged
    {
        private GraficoRadar grafico;
        private double nivelCapacidad;
        public VentanaDetalleHabilidades()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

        }

        private void seleccionHabilidades(object sender, SelectionChangedEventArgs e)
        {
            if (tabGeneral.IsSelected)
            {
                string[] habilidades = { "Hab. Blandas", "Hab. Duras", "Caract. Fisicas" };
                double[] puntajesTrabajador = { 56, 61, 10 };
                double[] puntajesSeccion = { 76, 21, 70 };
                this.grafico = new GraficoRadar(habilidades, puntajesSeccion, puntajesTrabajador, this.GraficoGeneral);
                this.grafico.TipoGrafico = "Area";
                this.grafico.constructorGrafico();
                /*valores grafico circular*/
                Value = new Random().NextDouble();
                Formatter = x => x.ToString("P");
                DataContext = this;
            }
            else if (tabHB.IsSelected)
            {
                string[] habilidades = { "Proactividad", "Responsabilidad", "Tolerancia a la presion", "Perseverancia", "Sociabilidad" };
                double[] puntajesTrabajador = { 56, 61, 10, 59, 78 };
                double[] puntajesSeccion = { 76, 21, 70, 60, 72 };
                this.grafico = new GraficoRadar(habilidades, puntajesSeccion, puntajesTrabajador, this.GraficoHB);
                this.grafico.TipoGrafico = "Area";
                this.grafico.constructorGrafico();
                /*valores grafico circular*/
                Value = new Random().NextDouble();
                Formatter = x => x.ToString("P");
                DataContext = this;
            }
            else if (tabHD.IsSelected)
            {
                string[] habilidades = { "Hab. LectoEscritura", "C. Normas Sanitaria", "C. Matematicas Basicas", "Experiencia A. clientes", "Manejo Lenguaje Formal" };
                double[] puntajesTrabajador = { 52, 55, 60, 69, 58 };
                double[] puntajesSeccion = { 50, 56, 20, 74, 78 };
                this.grafico = new GraficoRadar(habilidades, puntajesSeccion, puntajesTrabajador, this.GraficoHD);
                this.grafico.TipoGrafico = "Area";
                this.grafico.constructorGrafico();
                /*valores grafico circular*/
                Value = new Random().NextDouble();
                Formatter = x => x.ToString("P");
                DataContext = this;
            }
            else if (tabCF.IsSelected)
            {
                string[] habilidades = { "Presentacion Personal", "Discapacidad Fisica", "Altura" };
                double[] puntajesTrabajador = { 66, 61, 71 };
                double[] puntajesSeccion = { 76, 21, 70 };
                this.grafico = new GraficoRadar(habilidades, puntajesSeccion, puntajesTrabajador, this.GraficoCF);
                this.grafico.TipoGrafico = "Area";
                this.grafico.constructorGrafico();
                /*valores grafico circular*/
                Value = new Random().NextDouble();
                Formatter = x => x.ToString("P");
                DataContext = this;
            }
        }

        public double Value
        {
            get { return nivelCapacidad; }
            set
            {
                nivelCapacidad = value;
                OnPropertyChanged("Value");
            }
        }

        public Func<double, string> Formatter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
