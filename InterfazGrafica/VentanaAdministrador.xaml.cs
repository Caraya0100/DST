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
using System.ComponentModel;
using System.Windows.Threading;
using DST;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaAdministrador.xaml
    /// </summary>
    public partial class VentanaAdministrador : MetroWindow
    {
        private Mensajes cuadroMensajes;
        private DatosDePrueba datosPrueba;
        private AnimacionScroll animadorTrabajadores;
        public VentanaAdministrador()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            IniciarComponentes();
        }

        private void EventosIniciales(object sender, RoutedEventArgs e)
        {
            animadorTrabajadores.Visualizador = this.scrollTrabajadores;
        }
        private void IniciarComponentes()
        {
            cuadroMensajes  = new Mensajes(this);
            datosPrueba     = new DatosDePrueba();            
            animadorTrabajadores = new AnimacionScroll();
            GeneraListaTrabajadores();
            
            
        }
        /// <summary>
        /// Controlador que se acciona al presionar boton cerrar ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void CerrarSesion(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.CerrarSesionAdministrador()))
            {
                App.Current.Shutdown();
            }
        }
        /// <summary>
        /// Controlador que se acciona al seleccionar las pestañas de disponibles (tabControl)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seleccionItem(object sender, SelectionChangedEventArgs e)
        {
            if (itemDesempenio.IsSelected)
            {

            }
            else if (itemTrabajadores.IsSelected)
            {
                //animadorTrabajadores.detenerAnimacionHorizontal();
                animadorTrabajadores.comenzarAnimacionHorizontal();                
            }
            else if (itemSecciones.IsSelected)
            {

            }
            else if (itemEvaluacion.IsSelected)
            {

            }
            else if (itemSolicitudes.IsSelected)
            {

            }
            else if(itemReglas.IsSelected)
            {

            }
            else if (itemComponentes.IsSelected)
            {

            }
            else if (itemUsuarios.IsSelected)
            {

            }
        }
        /// <summary>
        /// Metodo que genera los elementos que contiene el scrollTrabajador.
        /// </summary>
        private void GeneraListaTrabajadores()
        {
            this.panelTrabajadores.Children.Clear();
            int indice = 0;
            foreach (Trabajador datos in datosPrueba.Trabajadores)
            {
                VisorTrabajador infoTrabajador = new VisorTrabajador(seleccionPanelTrabajador);
                infoTrabajador.Nombre = datos.Nombre;
                infoTrabajador.Apellido = datos.ApellidoPaterno;                
                /*comprobar el sexo*/
                if (datos.Sexo.Equals("M"))
                    infoTrabajador.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                else
                    infoTrabajador.DireccionImagen = @"..\..\Iconos\User-Female.png";
                infoTrabajador.IdentificadorPanel = "I" + indice;
                this.panelTrabajadores.Children.Add(infoTrabajador.ConstructorInfo());
                indice++;
            }
        }
        /// <summary>
        /// Controlador que despliega la informacion del trabajador seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seleccionPanelTrabajador(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
