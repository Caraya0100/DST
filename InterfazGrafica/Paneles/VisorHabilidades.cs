using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;


namespace InterfazGrafica
{
    class VisorHabilidades
    {
        Canvas contenedorInfo;
        TextBlock descripcionHabilidades;
        ToggleSwitch encendidoApagado;
        Separator delimitador;

        public VisorHabilidades(System.EventHandler<RoutedEventArgs> controladorEventos)
        {
            this.contenedorInfo = new Canvas();
            this.descripcionHabilidades = new TextBlock();
            this.encendidoApagado = new ToggleSwitch();
            this.delimitador = new Separator();
            this.encendidoApagado.Click += controladorEventos;

        }
        public Canvas ConstructorInfo()
        {           
            Dimensiones();
            FuentesAlineaciones();
            Ubicaciones();
            adicionChildren();            
            return this.contenedorInfo;
        }

        private void adicionChildren()
        {
            this.contenedorInfo.Children.Add(descripcionHabilidades);
            this.contenedorInfo.Children.Add(encendidoApagado);
            this.contenedorInfo.Children.Add(delimitador);
        }

        private void Dimensiones()
        {
            descripcionHabilidades.Height = 52;
            descripcionHabilidades.Width = 477;
            delimitador.Width = 621;
            contenedorInfo.Height = 70;
            contenedorInfo.Width = 618;
        }

        private void FuentesAlineaciones()
        {
            var bc = new BrushConverter();
            System.Windows.Media.Brush color_de_letras = (System.Windows.Media.Brush)bc.ConvertFrom("Black");
            System.Windows.Media.Brush color_de_fondo = (System.Windows.Media.Brush)bc.ConvertFrom("#FFFBFBFB");
            descripcionHabilidades.Foreground = color_de_letras;
            descripcionHabilidades.FontSize = 16;
            descripcionHabilidades.Foreground = color_de_letras;
            descripcionHabilidades.IsHitTestVisible = false;           
            descripcionHabilidades.TextWrapping = TextWrapping.Wrap;
            descripcionHabilidades.Background = color_de_fondo;
          
        }

        private void Ubicaciones()
        {
            Canvas.SetLeft(descripcionHabilidades, 142);
            Canvas.SetTop(descripcionHabilidades, 12);
            Canvas.SetLeft(encendidoApagado, 19);
            Canvas.SetTop(encendidoApagado, 12);
        }

        public string DescripcionHabilidades
        {
            get { return descripcionHabilidades.Text; }
            set { descripcionHabilidades.Text = value; }
        }

        public bool? Encendido
        {
            get { return encendidoApagado.IsChecked; }
            set { encendidoApagado.IsChecked = value; }
        }

        public string Identificador
        {
            get { return encendidoApagado.Name; }
            set { encendidoApagado.Name = value; }
        }

        public string IdentificadorHabilidad
        {
            get { return descripcionHabilidades.Name; }
            set { descripcionHabilidades.Name = value; }
        }

    }
}
