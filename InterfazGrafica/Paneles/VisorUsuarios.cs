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
    class VisorUsuarios
    {
        private Canvas contenedorInfo;        
        private Button botonEliminar;
        private Button botonEditar;
        private StackPanel contenedorEtiquetas;
        private StackPanel contenedorDatos;
        private Label nombre;
        private Label seccion;  
        private Label etiquetaNombre;
        private Label etiquetaSeccion;  
        private Separator delimitador;

        public VisorUsuarios()
        {
            this.contenedorInfo = new Canvas();            
            this.botonEliminar = new Button();
            this.botonEditar = new Button();
            this.contenedorEtiquetas = new StackPanel();
            this.contenedorDatos = new StackPanel();
            this.nombre = new Label();
            this.seccion = new Label();            
            this.etiquetaNombre = new Label();
            this.etiquetaSeccion = new Label();
            this.delimitador = new Separator();
            this.etiquetaNombre.Content = "Nombre Usuario   :";
            this.etiquetaSeccion.Content = "Sección                 :";
        }

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
        /// Metodo que agrega los elementos al panel.
        /// </summary>
        private void adicionChildren()
        {
            this.contenedorInfo.Children.Add(contenedorEtiquetas);
            this.contenedorInfo.Children.Add(contenedorDatos);
            this.contenedorInfo.Children.Add(botonEliminar);
            this.contenedorInfo.Children.Add(botonEditar);            
            this.contenedorInfo.Children.Add(delimitador);
        }

        /// <summary>
        /// Metodo que determina las propiedades de Ancho y altura de los elementos en el panel.
        /// </summary>
        private void Dimensiones()
        {
            this.contenedorEtiquetas.Width = 191; this.contenedorEtiquetas.Height = 86;
            this.contenedorDatos.Width = 191; this.contenedorDatos.Height = 86;            
            this.botonEliminar.Width = 41; this.botonEliminar.Height = 41;
            this.botonEditar.Width = 41; this.botonEditar.Height = 41;
            this.contenedorInfo.Width = 450; this.contenedorInfo.Height = 120;
            this.delimitador.Width = 380; this.delimitador.Height = 12;
        }

        /// <summary>
        /// Metodo que determina propiedades de fuente, alineacion y color.
        /// </summary>
        private void FuentesAlineaciones()
        {          
            botonEliminar.Style = System.Windows.Application.Current.Resources["MetroCircleButtonStyle"] as System.Windows.Style;
            botonEditar.Style = System.Windows.Application.Current.Resources["MetroCircleButtonStyle"] as System.Windows.Style;
            string direccionImagenEliminar = @"..\..\Iconos\eliminarRojo.png";
            string direccionImagenEditar = @"..\..\Iconos\editar.png";
            ImageBrush imagenEliminar = new ImageBrush(new BitmapImage(new Uri(direccionImagenEliminar, UriKind.Relative)));
            ImageBrush imagenEditar = new ImageBrush(new BitmapImage(new Uri(direccionImagenEditar, UriKind.Relative)));
            DockPanel panelEliminar = new DockPanel();
            DockPanel panelEditar = new DockPanel();
            panelEliminar.Height = 21; panelEliminar.Width = 21;
            panelEditar.Height = 21; panelEditar.Width = 21;
            panelEliminar.Background = imagenEliminar;
            panelEditar.Background = imagenEditar;
            botonEliminar.Content = panelEliminar;
            botonEditar.Content = panelEditar;
        }

        /// <summary>
        /// Metodo que determina las propiedades de ubicacion de los elementos en panel.
        /// </summary>
        private void Ubicaciones()
        {
            Canvas.SetTop(contenedorEtiquetas, 14); Canvas.SetLeft(contenedorEtiquetas, 20);
            Canvas.SetTop(contenedorDatos, 14); Canvas.SetLeft(contenedorDatos, 150);
            Canvas.SetTop(botonEliminar, 1); Canvas.SetLeft(botonEliminar, 330);
            Canvas.SetTop(botonEditar, 40); Canvas.SetLeft(botonEditar, 330);           
            Canvas.SetTop(delimitador, 90); Canvas.SetLeft(delimitador, 10);
        }

        private void asignacionContenedores()
        {            
            contenedorEtiquetas.Children.Add(etiquetaNombre);
            contenedorEtiquetas.Children.Add(etiquetaSeccion);   
            contenedorDatos.Children.Add(nombre);
            contenedorDatos.Children.Add(seccion);           
        }

        public string Nombre
        {
            get { return nombre.Content as string; }
            set { nombre.Content = value; }
        }

        public string Seccion
        {
            get { return seccion.Content as string; }
            set { seccion.Content = value; }
        }

        public string IdenficadorEliminar
        {
            get { return botonEliminar.Name; }
            set { botonEliminar.Name = value; }
        }
        public string IdenficadorEditar
        {
            get { return botonEditar.Name; }
            set { botonEditar.Name = value; }
        }

        public void ControladorEliminar(System.Windows.RoutedEventHandler controladorEventos)
        {
            botonEliminar.Click += controladorEventos;
        }

        public void ControladorEditar(System.Windows.RoutedEventHandler controladorEventos)
        {
            botonEditar.Click += controladorEventos;
        }
    }
}
