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
    class VisorRanking
    {
        private Canvas contenedorInfo;
        private Rectangle imagen;
        private string direccionImagen;
        private StackPanel contenedorNombre;
        private TextBlock nombre;
        private TextBlock apellido;
        private StackPanel contenedorSeccion;
        private Label seccion;
        private Label posicionRanking;
        private Button botonVer;

        public VisorRanking(System.Windows.RoutedEventHandler controladorEventos)
        {
            this.contenedorInfo = new Canvas();
            this.imagen = new Rectangle();
            this.direccionImagen = string.Empty;
            this.contenedorNombre = new StackPanel();
            this.nombre = new TextBlock();
            this.apellido = new TextBlock();
            this.contenedorSeccion = new StackPanel();
            this.seccion = new Label();
            this.posicionRanking = new Label();
            this.botonVer = new Button();
            this.botonVer.Content = "Ver";
            this.botonVer.Click += controladorEventos;
        }

        /// <summary>
        ///  Metodo que construye un panel con la informacion requerida y lo retorna.
        /// </summary>
        /// <returns></returns>
        public Canvas ConstructorInfo()
        {
            asignacionContenedores();
            Dimensiones();
            FuentesAlineaciones();
            Ubicaciones();
            adicionChildren();
            return this.contenedorInfo;
        }

        /// <summary>
        /// Metodo que agrupa los elementos por contenedores.
        /// </summary>
        private void asignacionContenedores()
        {
            contenedorNombre.Children.Add(nombre);
            contenedorNombre.Children.Add(apellido);
            contenedorSeccion.Children.Add(seccion);
        }

        /// <summary>
        /// Metodo que determina las propiedades de Ancho y altura de los elementos en el panel.
        /// </summary>
        private void Dimensiones()
        {
            this.imagen.Width = 93; this.imagen.Height = 84;
            this.contenedorNombre.Width = 98;
            this.contenedorSeccion.Width = 114;
            this.posicionRanking.Width = 74; this.posicionRanking.Height = 87;
            this.botonVer.Width = 41; this.botonVer.Height = 41;
            this.contenedorInfo.Width = 280; this.contenedorInfo.Height = 97;
        }

        /// <summary>
        /// Metodo que determina propiedades de fuente, alineacion y color.
        /// </summary>
        private void FuentesAlineaciones()
        {
            this.nombre.TextAlignment = System.Windows.TextAlignment.Center;
            this.apellido.TextAlignment = System.Windows.TextAlignment.Center;
            this.seccion.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.posicionRanking.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            this.posicionRanking.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            this.posicionRanking.FontSize = 36;
            this.posicionRanking.FontStyle = System.Windows.FontStyles.Italic;
            this.imagen.Fill = new ImageBrush(new BitmapImage(new Uri(direccionImagen, UriKind.Relative)));
            botonVer.Style = System.Windows.Application.Current.Resources["MetroCircleButtonStyle"] as System.Windows.Style;
        }

        /// <summary>
        /// Metodo que determina las propiedades de ubicacion de los elementos en panel.
        /// </summary>
        private void Ubicaciones()
        {
            Canvas.SetTop(imagen, 3); Canvas.SetLeft(imagen, 74);
            Canvas.SetTop(contenedorNombre, 27); Canvas.SetLeft(contenedorNombre, 172);
            Canvas.SetTop(contenedorSeccion, 62); Canvas.SetLeft(contenedorSeccion, 64);
            Canvas.SetTop(posicionRanking, 10); Canvas.SetLeft(posicionRanking, 10);
            Canvas.SetTop(botonVer, 56); Canvas.SetLeft(botonVer, 199);
        }

        /// <summary>
        /// Metodo que agrega los elementos al panel.
        /// </summary>
        private void adicionChildren()
        {
            this.contenedorInfo.Children.Add(imagen);
            this.contenedorInfo.Children.Add(contenedorNombre);
            this.contenedorInfo.Children.Add(contenedorSeccion);
            this.contenedorInfo.Children.Add(posicionRanking);
            this.contenedorInfo.Children.Add(botonVer);
        }
        public string BotonVer
        {
            get { return botonVer.Name; }
            set { botonVer.Name = value; }
        }

        public string Nombre
        {
            get { return nombre.Text; }
            set { nombre.Text = value; }
        }

        public string Apellido
        {
            get { return apellido.Text; }
            set { apellido.Text = value; }
        }

        public string Seccion
        {
            get { return seccion.Content as string; }
            set { seccion.Content = value; }
        }

        public string PosicionRanking
        {
            get { return posicionRanking.Content as string; }
            set { posicionRanking.Content = value; }
        }

        public Canvas ContenedorInfo
        {
            get { return contenedorInfo; }
            set { }
        }

        public string DireccionImagen
        {
            get { return direccionImagen; }
            set { direccionImagen = value; }
        }
    }
}
