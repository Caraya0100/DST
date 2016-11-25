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
        Canvas contenedorInfo = new Canvas();
        TextBox descripcion_habilidades = new TextBox();
        ToggleSwitch encendido_apagado = new ToggleSwitch();
        Separator delimitador = new Separator();

        public Canvas ConstructorInfo()
        {
            //asignacionContenedores();
            Dimensiones();
            FuentesAlineaciones();
            Ubicaciones();
            //adicionChildren();
            return this.contenedorInfo;
        }

        private void adicionChildren()
        {
            //this.contenedorInfo.Children.Add(imagen);
            //this.contenedorInfo.Children.Add(contenedorNombre);
        }

        private void Dimensiones()
        {
            descripcion_habilidades.Height = 52;
            descripcion_habilidades.Width = 477;
            delimitador.Width = 621;
            contenedorInfo.Height = 70;
            contenedorInfo.Width = 618;
        }

        private void FuentesAlineaciones()
        {
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
        }

        private void Ubicaciones()
        {
            Canvas.SetLeft(descripcion_habilidades, 142);
            Canvas.SetTop(descripcion_habilidades, 12);
            Canvas.SetLeft(encendido_apagado, 19);
            Canvas.SetTop(encendido_apagado, 12);
        }


    }
}
