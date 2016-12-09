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
    class VisorEvaluacion
    {
        private Canvas contenedorInfo;
        private Rectangle imagen;
        private Rectangle estadoEvaluacion;
        private string direccionImagen;
        private string direccionEstado;
        private StackPanel contenedorNombre;
        private TextBlock nombre;
        private TextBlock apellido;


        public VisorEvaluacion(System.Windows.Input.MouseButtonEventHandler controladorEventos)
        {
            this.contenedorInfo = new Canvas();
            this.imagen = new Rectangle();
            this.estadoEvaluacion = new Rectangle();
            this.direccionImagen = string.Empty;
            this.contenedorNombre = new StackPanel();
            this.nombre = new TextBlock();
            this.apellido = new TextBlock();
            this.contenedorInfo.MouseDown += controladorEventos;
        }

        /// <summary>
        /// Metodo que contruye el panel con la informacion requerida y luego lo retorna.
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
        /// Metoto que agrega los elementos al panel.
        /// </summary>
        private void adicionChildren()
        {
            this.contenedorInfo.Children.Add(imagen);
            this.contenedorInfo.Children.Add(estadoEvaluacion);
            this.contenedorInfo.Children.Add(contenedorNombre);
        }

        /// <summary>
        /// Metodo que determina las propiedades de Ancho y altura de los elementos en el panel.
        /// </summary>
        private void Dimensiones()
        {
            this.imagen.Width = 116; this.imagen.Height = 98;
            this.estadoEvaluacion.Width = 25; this.estadoEvaluacion.Height = 25;
            this.contenedorNombre.Width = 116;
            this.nombre.Width = 116;
            this.apellido.Width = 116;
            this.contenedorInfo.Width = 132; this.contenedorInfo.Height = 112;
        }

        /// <summary>
        /// Metodo que determina propiedades de fuente, alineacion y color.
        /// </summary>
        private void FuentesAlineaciones()
        {
            this.nombre.TextAlignment = System.Windows.TextAlignment.Center;
            this.apellido.TextAlignment = System.Windows.TextAlignment.Center;
            this.imagen.Fill = new ImageBrush(new BitmapImage(new Uri(direccionImagen, UriKind.Relative)));
            this.estadoEvaluacion.Fill = new ImageBrush(new BitmapImage(new Uri(direccionEstado, UriKind.Relative)));
        }

        /// <summary>
        /// Metodo que determina las propiedades de ubicacion de los elementos en panel.
        /// </summary>
        private void Ubicaciones()
        {
            Canvas.SetTop(imagen, 6); Canvas.SetLeft(imagen, 10);
            Canvas.SetTop(contenedorNombre, 79); Canvas.SetLeft(contenedorNombre, 10);
            Canvas.SetTop(estadoEvaluacion, 62); Canvas.SetLeft(estadoEvaluacion, 91);
        }

        /// <summary>
        /// Metodo que agrupa los elementos por contenedores.
        /// </summary>
        private void asignacionContenedores()
        {
            contenedorNombre.Children.Add(nombre);
            contenedorNombre.Children.Add(apellido);
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

        public string DireccionEstado
        {
            get { return direccionEstado; }
            set { direccionEstado = value; }
        }

        public string IdentificadorPanel
        {
            get { return contenedorInfo.Name; }
            set { contenedorInfo.Name = value; }
        }
    }
}