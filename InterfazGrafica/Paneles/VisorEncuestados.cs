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
    class VisorEncuestados
    {
        private Canvas contenedorInfo;
        private Rectangle imagen;
        private string direccionImagen;
        private StackPanel contenedorNombre;
        private TextBlock nombre;
        private TextBlock apellido;
        private TextBlock estado;
        private Button habilitarEncuesta;
        private Separator delimitador;


        public VisorEncuestados(System.Windows.RoutedEventHandler controladorEventos)
        {
            this.contenedorInfo = new Canvas();
            this.imagen     = new Rectangle();
            this.direccionImagen = string.Empty;
            this.contenedorNombre = new StackPanel();
            this.nombre     = new TextBlock();
            this.apellido   = new TextBlock();
            this.estado     = new TextBlock();
            this.habilitarEncuesta  = new Button();
            this.delimitador        = new Separator();
            this.habilitarEncuesta.Click += controladorEventos;
        }

        /// <summary>
        /// Metodo que construye un panel con la informacion requerida y lo retorna.
        /// </summary>
        /// <returns></returns>
        public Canvas ConstructorInfo()
        {
            asignacionContenedores();
            Dimensiones();
            FuentesAlineaciones();
            Ubicaciones();
            adicionChildren();
            this.habilitarEncuesta.Content = "Habilitar Encuesta";
            return this.contenedorInfo;
        }

        /// <summary>
        /// Metodo que agrega los elementos al panel.
        /// </summary>
        private void adicionChildren()
        {
            this.contenedorInfo.Children.Add(imagen);
            this.contenedorInfo.Children.Add(contenedorNombre);
            this.contenedorInfo.Children.Add(delimitador);
        }

        /// <summary>
        /// Metodo que determina las propiedades de Ancho y altura de los elementos en el panel.
        /// </summary>
        private void Dimensiones()
        {
            this.imagen.Width = 93; this.imagen.Height = 90;
            this.contenedorNombre.Width = 139;
            this.nombre.Width = 139;
            this.apellido.Width = 139;
            this.estado.Width = 139;
            this.delimitador.Width = 274;
            this.habilitarEncuesta.Width = 139;
            //this.habilitarEncuesta.Height = 20;
            this.contenedorInfo.Width = 247; this.contenedorInfo.Height = 100;
        }

        /// <summary>
        /// Metodo que determina propiedades de fuente, alineacion y color.
        /// </summary>
        private void FuentesAlineaciones()
        {
            this.nombre.TextAlignment = System.Windows.TextAlignment.Center;
            this.apellido.TextAlignment = System.Windows.TextAlignment.Center;
            this.estado.TextAlignment = System.Windows.TextAlignment.Center;
            this.imagen.Fill = new ImageBrush(new BitmapImage(new Uri(direccionImagen, UriKind.Relative)));
        }

        /// <summary>
        ///  Metodo que determina las propiedades de ubicacion de los elementos en panel.
        /// </summary>
        private void Ubicaciones()
        {
            Canvas.SetTop(imagen, 6); Canvas.SetLeft(imagen, 10);
            Canvas.SetTop(contenedorNombre, 10); Canvas.SetLeft(contenedorNombre, 108);
            Canvas.SetTop(delimitador, 90);
        }

        /// <summary>
        /// Metodo que agrupa los elementos por contenedores.
        /// </summary>
        private void asignacionContenedores()
        {
            contenedorNombre.Children.Add(nombre);
            contenedorNombre.Children.Add(apellido);
            contenedorNombre.Children.Add(estado);
            contenedorNombre.Children.Add(habilitarEncuesta);
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

        public string DireccionImagen
        {
            get { return direccionImagen; }
            set { direccionImagen = value; }
        }

        public string Estado
        {
            get { return estado.Text; }
            set { estado.Text = value; }
        }

        public string IdentificadorBoton
        {
            get { return habilitarEncuesta.Name; }
            set { habilitarEncuesta.Name = value; }
        }

        public bool Habilitado
        {
            get { return habilitarEncuesta.IsEnabled; }
            set { habilitarEncuesta.IsEnabled = value; }
        }

        public void ColorDeshabilitado()
        {
            this.estado.Foreground = Brushes.DarkGreen;
            this.nombre.Foreground = Brushes.DarkGreen;
            this.apellido.Foreground = Brushes.DarkGreen;
            this.nombre.FontWeight = System.Windows.FontWeights.Bold;
            this.apellido.FontWeight = System.Windows.FontWeights.Bold;
            this.estado.FontWeight = System.Windows.FontWeights.Bold;
        }
    }
}