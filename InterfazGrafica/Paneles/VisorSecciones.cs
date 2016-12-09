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

    class VisorSecciones
    {

        private Canvas contenedorInfo;
        private Button botonVer;
        private Button botonEliminar;
        private Button botonEditar;
        private StackPanel contenedorEtiquetas;
        private StackPanel contenedorDatos;
        private Label nombreSeccion;
        private Label jefeSeccion;
        private Label cantidadTrabajadores;
        private Label etiquetaNombreSeccion;
        private Label etiquetaJefeSeccion;
        private Label etiquetaCantidadTrabajadores;
        private Separator delimitador;

        public VisorSecciones()
        {
            this.contenedorInfo = new Canvas();
            this.botonVer = new Button();
            this.botonEliminar = new Button();
            this.botonEditar = new Button();
            this.contenedorEtiquetas = new StackPanel();
            this.contenedorDatos = new StackPanel();
            this.nombreSeccion = new Label();
            this.jefeSeccion = new Label();
            this.cantidadTrabajadores = new Label();
            this.etiquetaNombreSeccion = new Label();
            this.etiquetaJefeSeccion = new Label();
            this.etiquetaCantidadTrabajadores = new Label();           
            this.etiquetaNombreSeccion.Content = "Nombre Sección                        :";
            this.etiquetaJefeSeccion.Content ="Jefe Sección                               :";
            this.etiquetaCantidadTrabajadores.Content = "Cantidad de Trabajadores         :";
            this.botonVer.Content = "Ver";
            this.botonEditar.Content = "Ed";
            this.botonEliminar.Content = "X";
            this.delimitador = new Separator();
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
            this.contenedorInfo.Children.Add(botonVer);
            this.contenedorInfo.Children.Add(delimitador);
        }

        /// <summary>
        /// Metodo que determina las propiedades de Ancho y altura de los elementos en el panel.
        /// </summary>
        private void Dimensiones()
        {
            this.contenedorEtiquetas.Width = 191; this.contenedorEtiquetas.Height = 86;
            this.contenedorDatos.Width = 191; this.contenedorDatos.Height = 86;
            this.botonVer.Width = 41; this.botonVer.Height = 41;
            this.botonEliminar.Width = 41; this.botonEliminar.Height = 41;
            this.botonEditar.Width = 41; this.botonEditar.Height = 41;
            this.contenedorInfo.Width = 521; this.contenedorInfo.Height = 159;
            this.delimitador.Width = 501; this.delimitador.Height = 12;
        }

        /// <summary>
        /// Metodo que determina propiedades de fuente, alineacion y color.
        /// </summary>
        private void FuentesAlineaciones()
        {            
            botonVer.Style = System.Windows.Application.Current.Resources["MetroCircleButtonStyle"] as System.Windows.Style;
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
            Canvas.SetTop(contenedorEtiquetas, 44); Canvas.SetLeft(contenedorEtiquetas, 30);
            Canvas.SetTop(contenedorDatos, 44); Canvas.SetLeft(contenedorDatos, 221);
            Canvas.SetTop(botonEliminar, 10); Canvas.SetLeft(botonEliminar, 437);
            Canvas.SetTop(botonEditar, 50); Canvas.SetLeft(botonEditar, 437);
            Canvas.SetTop(botonVer, 90); Canvas.SetLeft(botonVer, 437);
            Canvas.SetTop(delimitador, 150); Canvas.SetLeft(delimitador, 10);
        }

        private void asignacionContenedores()
        {
            contenedorEtiquetas.Children.Add(etiquetaNombreSeccion);
            contenedorEtiquetas.Children.Add(etiquetaJefeSeccion);
            contenedorEtiquetas.Children.Add(etiquetaCantidadTrabajadores);
            contenedorDatos.Children.Add(nombreSeccion);
            contenedorDatos.Children.Add(jefeSeccion);
            contenedorDatos.Children.Add(cantidadTrabajadores);
        }

        public string NombreSeccion
        {
            get { return nombreSeccion.Content as string; }
            set { nombreSeccion.Content = value; }
        }

        public string JefeSeccion
        {
            get { return jefeSeccion.Content as string; }
            set { jefeSeccion.Content = value; }
        }

        public string CantidadTrabajadores
        {
            get { return cantidadTrabajadores.Content as string; }
            set { cantidadTrabajadores.Content = value; }
        }

        public string IdentificadorEditar
        {
            get { return botonEditar.Name; }
            set { botonEditar.Name = value; }
        }

        public string IdentificadorEliminar
        {
            get { return botonEliminar.Name; }
            set { botonEliminar.Name = value; }
        }

        public string IdenficadorVer
        {
            get { return botonVer.Name; }
            set { botonVer.Name = value; }
        }

        public void ControladorEliminar(System.Windows.RoutedEventHandler controladorEventos)
        {
            botonEliminar.Click += controladorEventos;
        }

        public void ControladorEditar(System.Windows.RoutedEventHandler controladorEventos)
        {
            botonEditar.Click += controladorEventos;
        }

        public void ControladorVer(System.Windows.RoutedEventHandler controladorEventos)
        {
            botonVer.Click += controladorEventos;
        }

    }

     
}
