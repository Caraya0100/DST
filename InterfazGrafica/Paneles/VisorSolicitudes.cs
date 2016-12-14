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
    class VisorSolicitudes
    {

        private Canvas contenedorInfo;
        private Button botonAceptar;
        private Button botonEliminar;        
        private StackPanel contenedorEtiquetas;
        private StackPanel contenedorDatos;
        private Label etiquetaSeccionActual;
        private Label etiquetaSeccionNueva;
        private Label etiquetaTrabajador;
        private Label etiquetaCapacidadSeccionActual;
        private Label etiquetaCapacidadSeccionNueva;
        private Label seccionActual;
        private Label seccionNueva;
        private Label trabajador;
        private Label capacidadSeccionActual;
        private Label capacidadSeccionNueva;
        private Separator delimitador;

        public VisorSolicitudes()
        {
            this.contenedorInfo = new Canvas();
            this.botonAceptar = new Button();
            this.botonEliminar = new Button();
            this.contenedorEtiquetas = new StackPanel();
            this.contenedorDatos = new StackPanel();
            this.etiquetaSeccionActual = new Label();
            this.etiquetaSeccionNueva = new Label();
            this.etiquetaTrabajador = new Label();
            this.etiquetaCapacidadSeccionActual = new Label();
            this.etiquetaCapacidadSeccionNueva = new Label();
            this.seccionActual = new Label();
            this.seccionNueva = new Label();
            this.trabajador = new Label();
            this.capacidadSeccionActual = new Label();
            this.capacidadSeccionNueva = new Label();
            this.delimitador = new Separator();
            this.etiquetaSeccionActual.Content = "Sección Actual                     :";
            this.etiquetaSeccionNueva.Content = "Sección Solicitante              :";
            this.etiquetaTrabajador.Content = "Trabajador                           :";
            this.etiquetaCapacidadSeccionActual.Content = "Capacidad Sección Actual   :";
            this.etiquetaCapacidadSeccionNueva.Content = "Capacidad Sección Nueva   :";
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
            this.contenedorInfo.Children.Add(botonAceptar); 
            this.contenedorInfo.Children.Add(delimitador);
        }

        /// <summary>
        /// Metodo que determina las propiedades de Ancho y altura de los elementos en el panel.
        /// </summary>
        private void Dimensiones()
        {
            this.contenedorEtiquetas.Width = 191; this.contenedorEtiquetas.Height = 150;
            this.contenedorDatos.Width = 191; this.contenedorDatos.Height = 150;            
            this.botonEliminar.Width = 41; this.botonEliminar.Height = 41;
            this.botonAceptar.Width = 41; this.botonAceptar.Height = 41;
            this.contenedorInfo.Width = 521; this.contenedorInfo.Height = 170;
            this.delimitador.Width = 501; this.delimitador.Height = 12;
        }

        /// <summary>
        /// Metodo que determina propiedades de fuente, alineacion y color.
        /// </summary>
        private void FuentesAlineaciones()
        {
            botonAceptar.Style = System.Windows.Application.Current.Resources["MetroCircleButtonStyle"] as System.Windows.Style;
            botonEliminar.Style = System.Windows.Application.Current.Resources["MetroCircleButtonStyle"] as System.Windows.Style;            
            string direccionImagenEliminar = @"..\..\Iconos\eliminarRojo.png";
            string direccionImagenEditar = @"..\..\Iconos\aceptar.png";
            ImageBrush imagenEliminar = new ImageBrush(new BitmapImage(new Uri(direccionImagenEliminar, UriKind.Relative)));
            ImageBrush imagenEditar = new ImageBrush(new BitmapImage(new Uri(direccionImagenEditar, UriKind.Relative)));
            DockPanel panelEliminar = new DockPanel();
            DockPanel panelAceptar = new DockPanel();
            panelEliminar.Height = 21; panelEliminar.Width = 21;
            panelAceptar.Height = 21; panelAceptar.Width = 21;
            panelEliminar.Background = imagenEliminar;
            panelAceptar.Background = imagenEditar;
            botonEliminar.Content = panelEliminar;
            botonAceptar.Content = panelAceptar;
        }

        /// <summary>
        /// Metodo que determina las propiedades de ubicacion de los elementos en panel.
        /// </summary>
        private void Ubicaciones()
        {
            Canvas.SetTop(contenedorEtiquetas, 24); Canvas.SetLeft(contenedorEtiquetas, 30);
            Canvas.SetTop(contenedorDatos, 24); Canvas.SetLeft(contenedorDatos, 221);
            Canvas.SetTop(botonEliminar, 40); Canvas.SetLeft(botonEliminar, 437);
            Canvas.SetTop(botonAceptar, 80); Canvas.SetLeft(botonAceptar, 437);           
            Canvas.SetTop(delimitador, 165); Canvas.SetLeft(delimitador, 10);
        }

        private void asignacionContenedores()
        {
            contenedorEtiquetas.Children.Add(etiquetaSeccionActual);
            contenedorEtiquetas.Children.Add(etiquetaSeccionNueva);
            contenedorEtiquetas.Children.Add(etiquetaTrabajador);
            contenedorEtiquetas.Children.Add(etiquetaCapacidadSeccionActual);
            contenedorEtiquetas.Children.Add(etiquetaCapacidadSeccionNueva);

            contenedorDatos.Children.Add(seccionActual);
            contenedorDatos.Children.Add(seccionNueva);
            contenedorDatos.Children.Add(trabajador);
            contenedorDatos.Children.Add(capacidadSeccionActual);
            contenedorDatos.Children.Add(capacidadSeccionNueva);
        }

        public string SeccionActual
        {
            get { return seccionActual.Content as string; }
            set { seccionActual.Content = value; }
        }

        public string SeccionNueva
        {
            get { return seccionNueva.Content as string; }
            set { seccionNueva.Content = value; }
        }

        public string Trabajador
        {
            get { return trabajador.Content as string; }
            set { trabajador.Content= value; }
        }

        public string CapacidadActualSeccion
        {
            get { return capacidadSeccionActual.Content as string; }
            set { capacidadSeccionActual.Content = value; }
        }

        public string CapacidadNuevaSeccion
        {
            get { return capacidadSeccionNueva.Content as string; }
            set { capacidadSeccionNueva.Content = value; }
        }

        public string IdentificadorAceptar
        {
            get { return botonAceptar.Name; }
            set { botonAceptar.Name = value; }
        }

        public string IdentificadorRechazar
        {
            get { return botonEliminar.Name; }
            set { botonEliminar.Name = value; }
        }

        public void ControladorRechazar(System.Windows.RoutedEventHandler controladorEventos)
        {
            botonEliminar.Click += controladorEventos;
        }

        public void ControladorAceptar(System.Windows.RoutedEventHandler controladorEventos)
        {
            botonAceptar.Click += controladorEventos;
        }
    }
}
