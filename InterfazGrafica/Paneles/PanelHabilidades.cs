using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;


namespace InterfazGrafica
{
    class PanelHabilidades
    {
        private TextBlock habilidad;
        private Slider gradoImportancia;
        private Label gradoImportanciaEtiqueta;
        private Separator delimitador;
        private Canvas contenedorHabilidad;
        private Label key;

        public PanelHabilidades()
        {
            this.habilidad = new TextBlock();
            this.gradoImportancia = new Slider();
            this.gradoImportanciaEtiqueta = new Label();
            this.delimitador = new Separator();
            this.contenedorHabilidad = new Canvas();
            this.key = new Label();
        }

        /// <summary>
        /// Metodo que contruye el panel con la informacion requerida y luego lo retorna.
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public Canvas ConstructorPanel(int indice)
        {
            Dimensiones();
            FuentesAlineaciones();
            Ubicaciones(indice);
            AdicionChildren();
            return this.contenedorHabilidad;
        }

        /// <summary>
        /// Metodo que determina propiedades de fuente, alineacion y color.
        /// </summary>
        public void FuentesAlineaciones()
        {
            var color = new BrushConverter();
            Brush colorDeLetras = (Brush)color.ConvertFrom("Black");
            Brush colorDeLetrasNumero = (Brush)color.ConvertFrom("#CCA4C400");
            Brush colorDeFondo = (Brush)color.ConvertFrom("#FFFBFBFB");
            habilidad.Foreground = colorDeLetras;
            habilidad.FontSize = 16;
            habilidad.TextWrapping = System.Windows.TextWrapping.Wrap;
            habilidad.Background = colorDeFondo;
            gradoImportanciaEtiqueta.Foreground = colorDeLetrasNumero;
            gradoImportanciaEtiqueta.FontSize = 36;
            gradoImportanciaEtiqueta.FontStyle = System.Windows.FontStyles.Italic;
            habilidad.TextAlignment = System.Windows.TextAlignment.Center;
            habilidad.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            gradoImportanciaEtiqueta.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            gradoImportanciaEtiqueta.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
        }

        /// <summary>
        /// Metodo que determina las propiedades de ubicacion de los elementos en panel.
        /// </summary>
        /// <param name="indice"></param>
        public void Ubicaciones(int indice)
        {
            Canvas.SetTop(habilidad, 40); Canvas.SetLeft(habilidad, 10);
            Canvas.SetTop(gradoImportancia, 40); Canvas.SetLeft(gradoImportancia, 365);
            Canvas.SetTop(gradoImportanciaEtiqueta, 20); Canvas.SetLeft(gradoImportanciaEtiqueta, 580);
            Canvas.SetTop(contenedorHabilidad, 100 * indice);
            Canvas.SetTop(delimitador, 102 * indice);
        }

        /// <summary>
        /// Metodo que determina las propiedades de Ancho y altura de los elementos en el panel.
        /// </summary>
        public void Dimensiones()
        {
            contenedorHabilidad.Width = 650; contenedorHabilidad.Height = 70;
            habilidad.Width = 330; habilidad.Height = 43;
            delimitador.Width = 679; delimitador.Height = 2;
            gradoImportancia.Width = 174; gradoImportancia.Height = 22;
            gradoImportanciaEtiqueta.Width = 110; gradoImportanciaEtiqueta.Height = 55;
            key.Visibility = System.Windows.Visibility.Hidden;
        }

        /// <summary>
        /// Metoto que agrega los elementos al panel.
        /// </summary>
        public void AdicionChildren()
        {
            contenedorHabilidad.Children.Add(this.habilidad);
            contenedorHabilidad.Children.Add(this.gradoImportancia);
            contenedorHabilidad.Children.Add(this.gradoImportanciaEtiqueta);
            contenedorHabilidad.Children.Add(this.key);
        }

        /// <summary>
        /// Metodo que asigna un controlador al panel. se acciona al seleccionr el panel.
        /// </summary>
        public System.Windows.RoutedPropertyChangedEventHandler<double> Controlador
        {
            get { return null; }
            set { gradoImportancia.ValueChanged += value; }
        }
        public string Habilidad
        {
            get { return habilidad.Text; }
            set { habilidad.Text = value; }
        }

        public string HabilidadName
        {
            get { return habilidad.Name; }
            set { habilidad.Name = value; }
        }

        public double GradoImportancia
        {
            get { return gradoImportancia.Value; }
            set { gradoImportancia.Value = value; }
        }

        public string GradoImportanciaIdentificador
        {
            get { return gradoImportancia.Name; }
            set { gradoImportancia.Name = value; }
        }

        public string GradoImportanciaEtiqueta
        {
            get { return gradoImportanciaEtiqueta.Content as string; }
            set { gradoImportanciaEtiqueta.Content = value; }
        }

        public string GradoImportanciaEtiquetaIdentificador
        {
            get { return gradoImportanciaEtiqueta.Name; }
            set { gradoImportanciaEtiqueta.Name = value; }
        }

        public string Key
        {
            get { return key.Name; }
            set { key.Name = value; }
        }

        public Separator Delimitador
        {
            get { return delimitador; }
            set { }
        }        
    }
}