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
using DST;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaLogin.xaml
    /// </summary>
    public partial class VentanaLogin : MetroWindow
    {
        Mensajes cuadroMensajes;
        Rutificador rutificador;
        private string tipoUsuario;
        public VentanaLogin()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            IniciarComponentes();
 
        }

        private void CerrarSesion(object sender, CancelEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void IniciarComponentes()
        {
            rutificador = new Rutificador();
            tipoUsuario = string.Empty;
            cuadroMensajes = new Mensajes(this);
        }

        private void VerificacionUsuario(object sender, RoutedEventArgs e)
        {
            flyoutLogin.IsOpen = true ;
        }

        private void CierraFlyout(object sender, RoutedEventArgs e)
        {
            if (flyoutLogin.IsOpen == false)
            {
                CamposPorDefecto();
            }
            
        }

        async private void IniciarSesion(object sender, RoutedEventArgs e)
        {
            bool rutVerificado = false;
            bool passVerificada = false;
            string rutFormateado = string.Empty;

            if (rutificador.ValidaRut(rut.Text, digitoVerificador.Text))
            {
                rutFormateado = rutificador.formatoRut(this.rut.Text) + "-" + this.digitoVerificador.Text;
                this.rut.Text = rutificador.formatoRut(this.rut.Text);
            }
            Console.WriteLine(rutFormateado);
            if (password.Password.Equals("admin"))
            {
                passVerificada = true;
            }
            if (rutFormateado.Equals("17.980.134-5"))
            {
                rutVerificado = true;
            }
            if (rutVerificado && passVerificada)
                DeterminacionUsuario("Administrador");
            else
            {
                await cuadroMensajes.VerificacionUsuarioIncorrecta();
                CamposPorDefecto();
            }
                
        }


        private void DeterminacionUsuario(string tipoUsuario)
        {
            if (tipoUsuario.Equals("Administrador"))
            {
                /*abre ventana admin*/
                VentanaAdministrador administrador = new VentanaAdministrador();
                administrador.Show();
                this.Visibility = Visibility.Hidden;
            }
            else if (tipoUsuario.Equals("JefeSeccion"))
            {                
                VentanaJefeSeccion jefeSeccion = new VentanaJefeSeccion();
                jefeSeccion.Show();
                this.Visibility = Visibility.Hidden;
            }
            else if (tipoUsuario.Equals("Psicologo"))
            {

            }
        }

        private void CamposPorDefecto()
        {
            rut.Text = "";
            digitoVerificador.Text = "";
            password.Password = "";
        }
               
    }
}
