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

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaAsignacionHabilidades.xaml
    /// </summary>
    public partial class VentanaAsignacionHabilidades : MetroWindow
    {
        public VentanaAsignacionHabilidades()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            generadorHabilidadBlandas();
        }

        private void generadorHabilidadBlandas()
        {
            for (int i = 0; i < 10; i++)
            {
                Canvas contenedor_habilidades = new Canvas();
                TextBox descripcion_habilidades = new TextBox();
                ToggleSwitch encendido_apagado = new ToggleSwitch();
                Separator delimitador = new Separator();
                contenedor_habilidades.Children.Add(encendido_apagado);
                contenedor_habilidades.Children.Add(descripcion_habilidades);
                /*contenido*/
                descripcion_habilidades.Text = "Alguna habilidad blanda que este en la base de datos.";
                /*fuentes*/
                var bc = new BrushConverter();
                System.Windows.Media.Brush color_de_letras = (System.Windows.Media.Brush)bc.ConvertFrom("Black");
                System.Windows.Media.Brush color_de_fondo = (System.Windows.Media.Brush)bc.ConvertFrom("#FFFBFBFB");
                descripcion_habilidades.Foreground = color_de_letras;
                descripcion_habilidades.FontSize = 16;
                descripcion_habilidades.Foreground = color_de_letras;
                descripcion_habilidades.IsHitTestVisible = false;
                descripcion_habilidades.IsInactiveSelectionHighlightEnabled = true;
                descripcion_habilidades.TextWrapping = TextWrapping.Wrap;
                descripcion_habilidades.Background = color_de_fondo;
                descripcion_habilidades.BorderBrush = color_de_fondo;
                /*ubicaciones*/
                Canvas.SetLeft(descripcion_habilidades, 142);
                Canvas.SetTop(descripcion_habilidades, 12);
                Canvas.SetLeft(encendido_apagado, 19);
                Canvas.SetTop(encendido_apagado, 12);
                /*tamaños*/
                descripcion_habilidades.Height = 52;
                descripcion_habilidades.Width = 477;
                delimitador.Width = 621;
                contenedor_habilidades.Height = 70;
                contenedor_habilidades.Width = 618;
                //contenedor_habilidades.Background = Brushes.AliceBlue;



                /**/
                Canvas.SetTop(contenedor_habilidades, 80 * i);
                Canvas.SetTop(delimitador, 80 * i);

                contenedor_HB.Children.Add(contenedor_habilidades);
                contenedor_HB.Children.Add(delimitador);
            }
        }

        void eventoTamanioCanvas(object sender, SizeChangedEventArgs e)
        {
            try { this.contenedor_HB.Height = 82 * 10; }
            catch { }
            try { this.contenedor_HB.Width = 622; }
            catch { }
        }
    }
}
