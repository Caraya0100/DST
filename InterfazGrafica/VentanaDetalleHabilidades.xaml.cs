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
using DST;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaDetalleHabilidades.xaml
    /// </summary>
    public partial class VentanaDetalleHabilidades : MetroWindow, INotifyPropertyChanged
    {
        private GraficoRadar grafico;
        private double capacidadTrabajador;
        private double nivelCapacidad;
        private Perfil perfilSeccion;
        private List<double> puntajesSeccionHB;
        private List<double> puntajesSeccionHD;
        private List<double> puntajesSeccionCF;
        /*variables locales*/
        double[] hbTrabajadorPuntajes;
        double[] hbSeccionPuntajes;
        double[] hdTrabajadorPuntajes;
        double[] hdSeccionPuntajes;
        double[] cfTrabajadorPuntajes;
        double[] cfSeccionPuntajes;
        double[] generalPuntajesTrabajador;
        double[] generalPuntajesSeccion;
        string[] habilidadesBlandas;
        string[] habilidadesDuras;
        string[] caracteristicasFisicas;

        public VentanaDetalleHabilidades()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            IniciarComponentes();

        }

        private void IniciarComponentes()
        {
            puntajesSeccionHB = new List<double>();
            puntajesSeccionHD = new List<double>();
            puntajesSeccionCF = new List<double>();
        }

        private void seleccionHabilidades(object sender, SelectionChangedEventArgs e)
        {
            InformacionPerfil();
            if (tabGeneral.IsSelected)
            {
                string[] habilidades = { "CF", "HB", "HD" };
                if ( habilidades.Length != generalPuntajesSeccion.Length || habilidades.Length!= generalPuntajesTrabajador.Length  || generalPuntajesSeccion.Length != generalPuntajesTrabajador.Length)
                    PuntajesGeneralesEnCero(habilidades);
                Console.WriteLine("ptje g seccion: " + generalPuntajesSeccion.Length+ " trab " + generalPuntajesTrabajador.Length);
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine(generalPuntajesSeccion[i]+" - "+generalPuntajesTrabajador[i]+" - "+habilidades[i]);                    
                }
                this.grafico = new GraficoRadar(habilidades, generalPuntajesSeccion, generalPuntajesTrabajador, this.GraficoGeneral);
                this.grafico.TipoGrafico = "Line";
                this.grafico.constructorGrafico();
                /*valores grafico circular*/
                Value = capacidadTrabajador;
                //Value = new Random().NextDouble();
                Formatter = x => x.ToString("P");
                DataContext = this;
            }
            else if (tabHB.IsSelected)
            {
                Console.WriteLine("NO CUMPLE1-" + puntajesSeccionHB.Count + " - " + habilidadesBlandas.Length + " - " + hbTrabajadorPuntajes.Length);
                if (puntajesSeccionHB.Count != habilidadesBlandas.Length || hbTrabajadorPuntajes.Length!=HabilidadesBlandas.Length || puntajesSeccionHB.Count != hbTrabajadorPuntajes.Length)
                    PuntajesHBEnCero();
                else Console.WriteLine("NO CUMPLE2");         
                this.grafico = new GraficoRadar(habilidadesBlandas, puntajesSeccionHB.ToArray(), hbTrabajadorPuntajes, this.GraficoHB);
                this.grafico.TipoGrafico = "Area";
                this.grafico.constructorGrafico();
                /*valores grafico circular*/
                Value = ConvertidorPuntaje(PuntajesGeneralesTrabajador[1]);
                Formatter = x => x.ToString("P");
                DataContext = this;
            }
            else if (tabHD.IsSelected)
            {
                Console.WriteLine("NO CUMPLE1" + puntajesSeccionHD.Count + " - " + habilidadesDuras.Length + " - " + hdTrabajadorPuntajes.Length);
                if (puntajesSeccionHD.Count != habilidadesDuras.Length || hdTrabajadorPuntajes.Length != HabilidadesDuras.Length || puntajesSeccionHD.Count != hdTrabajadorPuntajes.Length)
                    PuntajesHDEnCero();
                else Console.WriteLine("NO CUMPLE3");
                this.grafico = new GraficoRadar(habilidadesDuras, puntajesSeccionHD.ToArray(), hdTrabajadorPuntajes, this.GraficoHD);
                this.grafico.TipoGrafico = "Area";
                this.grafico.constructorGrafico();
                /*valores grafico circular*/
                Value = ConvertidorPuntaje(PuntajesGeneralesTrabajador[2]);
                Formatter = x => x.ToString("P");
                DataContext = this;
            }
            else if (tabCF.IsSelected)
            {
                Console.WriteLine("NO CUMPLE1" + puntajesSeccionCF.Count + " - " + caracteristicasFisicas.Length + " - " + cfTrabajadorPuntajes.Length);
                if (puntajesSeccionCF.Count != caracteristicasFisicas.Length || cfTrabajadorPuntajes.Length != caracteristicasFisicas.Length || puntajesSeccionCF.Count != cfTrabajadorPuntajes.Length)
                    PuntajesCFEnCero();
                       
                this.grafico = new GraficoRadar(caracteristicasFisicas, puntajesSeccionCF.ToArray(), cfTrabajadorPuntajes, this.GraficoCF);
                this.grafico.TipoGrafico = "Area";
                this.grafico.constructorGrafico();
                /*valores grafico circular*/
                Value = ConvertidorPuntaje(PuntajesGeneralesTrabajador[0]);                
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

       public double[] PuntajesHbTrabajador
       {
           get { return hbTrabajadorPuntajes; }
           set { hbTrabajadorPuntajes = value; }
       }
       public double[] PuntajesHbSeccion
       {
           get { return hbSeccionPuntajes; }
           set { hbSeccionPuntajes = value; }
       }

       public double[] PuntajesHdTrabajador
       {
           get { return hdTrabajadorPuntajes; }
           set { hdTrabajadorPuntajes = value; }
       }
       public double[] PuntajesHdSeccion
       {
           get { return hdSeccionPuntajes; }
           set { hdSeccionPuntajes = value; }
       }
       public double[] PuntajesCfTrabajador
       {
           get { return cfTrabajadorPuntajes; }
           set { cfTrabajadorPuntajes = value; }
       }
       public double[] PuntajesCfSeccion
       {
           get { return cfSeccionPuntajes; }
           set { cfSeccionPuntajes = value; }
       }
       public string[] HabilidadesBlandas
       {
           get { return habilidadesBlandas; }
           set { habilidadesBlandas = value; }
       }

       public string[] HabilidadesDuras
       {
           get { return habilidadesDuras; }
           set { habilidadesDuras = value; }
       }
       public string[] CaracteristicasFisicas
       {
           get { return caracteristicasFisicas; }
           set { caracteristicasFisicas = value; }
       }

       public double[] PuntajesGeneralesTrabajador
       {
           get { return generalPuntajesTrabajador; }
           set { generalPuntajesTrabajador = value; }
       }

       public double[] PuntajesGeneralesSeccion
       {
           get { return generalPuntajesSeccion; }
           set { generalPuntajesSeccion = value; }
       }

       public Perfil PerfilSeccion
       {
           get { return perfilSeccion; }
           set { perfilSeccion = value; }
       }

       public double ConvertidorPuntaje(double num)
       {           
           double puntaje = num * 0.01;
           return puntaje;
       }

       public double CapacidadTrabajdor
       {
           get { return capacidadTrabajador; }
           set { capacidadTrabajador = value; }
       }

       public void InformacionPerfil()
       {
           puntajesSeccionHB.Clear();
           puntajesSeccionHD.Clear();
           puntajesSeccionCF.Clear();
           Console.WriteLine("CANTIDAD HB: "+perfilSeccion.Blandas.Count+"  PTJE "+puntajesSeccionHB.Count+" ptjeh"+hbTrabajadorPuntajes.Length);
           Console.WriteLine("CANTIDAD HD: " + perfilSeccion.Duras.Count);
           Console.WriteLine("CANTIDAD CF: " + perfilSeccion.Fisicas.Count);

           foreach (KeyValuePair<string, Componente> puntajesPerfil in perfilSeccion.Blandas)
           {              
               puntajesSeccionHB.Add(puntajesPerfil.Value.Puntaje);
           }
           foreach (KeyValuePair<string, Componente> puntajesPerfil in perfilSeccion.Duras)
           {             
               puntajesSeccionHD.Add(puntajesPerfil.Value.Puntaje);
           }
           foreach (KeyValuePair<string, Componente> puntajesPerfil in perfilSeccion.Fisicas)
           {               
               puntajesSeccionCF.Add(puntajesPerfil.Value.Puntaje);
           }
       }

       public void PuntajesHBEnCero()
       {
           hbTrabajadorPuntajes = new double[0];
           puntajesSeccionHB.Clear();
           habilidadesDuras = new string[0];

       }
       public void PuntajesHDEnCero()
       {         
           hdTrabajadorPuntajes = new double[0];
           puntajesSeccionHD.Clear();
           habilidadesDuras = new string[0];           
       }

       public void PuntajesCFEnCero()
       {
           cfTrabajadorPuntajes = new double[0];
           puntajesSeccionCF.Clear();
           caracteristicasFisicas = new string[0];           
       }

       public void PuntajesGeneralesEnCero(string [] habilidad)
       {
           habilidad = new string[0];
           generalPuntajesTrabajador = new double[0];
           generalPuntajesSeccion = new double[0];
           Console.WriteLine("ENTRO GENERAL");
          /* for (int i = 0; i < 3; i++)
           {
               generalPuntajesTrabajador[i] = 0.0;
               generalPuntajesSeccion[i] = 0.0;
           }*/
       }
    }
}
