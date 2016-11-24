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
using System.Text.RegularExpressions;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaAgregarTrabajador.xaml
    /// </summary>
    public partial class VentanaAgregarTrabajador : MetroWindow
    {
        private Rutificador rutificador;
        private Mensajes cuadroMensajes;
        public VentanaAgregarTrabajador()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            rutificador = new Rutificador();
            cuadroMensajes = new Mensajes(this);
        }

        /// <summary>
        /// Despliega un calendario para que el usuario seleccione fecha de nacimiento.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void despliegueFechaNacimiento(object sender, RoutedEventArgs e)
        {
            despliegueCalendario.IsOpen = true;
        }

        /// <summary>
        /// Controlador que al accionarse agrega los datos de un nuevo trabajador.(Interactua con BD)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void agregarNuevoTrabajador(object sender, RoutedEventArgs e)
        {
            int camposCompletos = 0;
            string rut = string.Empty;
            if (rutificador.ValidaRut(this.rut.Text, this.digitoVerificador.Text))
            {
                rut = rutificador.formatoRut(this.rut.Text) + "-" + this.digitoVerificador;
                this.rut.Text = rutificador.formatoRut(this.rut.Text);
                camposCompletos++;                
            }
            else { }

            if (nombre.Text != "")
                camposCompletos++;
            if (apellidoPaterno.Text != "")
                camposCompletos++;
            if (apellidoMaterno.Text != "")
                camposCompletos++;
            if (!etiquetaFechaNacimiento.Content.Equals(""))
                camposCompletos++;
            if (camposCompletos != 5)
                cuadroMensajes.CamposIncompletos();
            else
            {
                /*capturar datos y almacenar en BD*/
                cuadroMensajes.NuevoTrabajadorAgregado();
                EstableceCamposVacios();
            }
        }

        /// <summary>
        /// Controlador que asigna la fecha de nacimiento a la etiqueta en el panel.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionFechaNacimiento(object sender, MouseButtonEventArgs e)
        {
            despliegueCalendario.IsOpen = false;
            this.etiquetaFechaNacimiento.Content = "" + this.calendario.SelectedDate.Value.ToString("dd/MM/yyyy"); ;
        }

        /// <summary>
        /// Restablece los campos del panel por defecto.
        /// </summary>
        private void EstableceCamposVacios()
        {
            nombre.Text = "";
            apellidoPaterno.Text = "";
            apellidoMaterno.Text = "";
            rut.Text = "";
            etiquetaFechaNacimiento.Content = "";
        }
    }
}
