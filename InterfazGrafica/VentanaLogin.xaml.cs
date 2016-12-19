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
        private Mensajes cuadroMensajes;
        private Rutificador rutificador;
        private InteraccionBD.InteraccionUsuarios usuarios;
        private string tipoUsuario;
        string rutFormateado;
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
            usuarios = new InteraccionBD.InteraccionUsuarios();
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
            rutFormateado = string.Empty;

            if (rutificador.ValidaRut(rut.Text, digitoVerificador.Text))
            {
                rutFormateado = rutificador.formatoRut(this.rut.Text) + "-" + this.digitoVerificador.Text;
                this.rut.Text = rutificador.formatoRut(this.rut.Text);
            }
            
            /*asignacion de los datos a comprobar*/
            usuarios.IdUsuario = rutFormateado;
            usuarios.Clave = password.Password;

           /* if (password.Password.Equals("admin"))
            {
                passVerificada = true;
            }
            if (rutFormateado.Equals("17.626.128-5"))
            {
                rutVerificado = true;
            }*/
            if (usuarios.VerificacionUsuario())
            {
                usuarios.IdUsuario = rutFormateado;
                DeterminacionUsuario(usuarios.TipoUsuario());
            }
            /*if (rutVerificado && passVerificada)
                DeterminacionUsuario("Jefe de seccion");*///falta dertimanar usuario
            else
            {
                await cuadroMensajes.VerificacionUsuarioIncorrecta();
                CamposPorDefecto();
            }
                
        }


        private void DeterminacionUsuario(string tipoUsuario)
        {
            if (tipoUsuario.Equals("ADMINISTRADOR"))
            {
                /*abre ventana admin*/
                VentanaAdministrador administrador = new VentanaAdministrador();
                administrador.IdAdministrador = rutFormateado;
                administrador.Show();
                this.Hide();
            }
            else if (tipoUsuario.Equals("JEFE_SECCION"))
            {                
                VentanaJefeSeccion jefeSeccion = new VentanaJefeSeccion();
                usuarios.IdUsuario = rutFormateado;
                jefeSeccion.RetornarAdministrador = Visibility.Hidden;
                jefeSeccion.NombreJefeSeccion = usuarios.NombreJefeSeccion();
                jefeSeccion.IdJefeSeccion = rutFormateado;
                jefeSeccion.Show();
                this.Hide();
            }
            else if (tipoUsuario.Equals("PSICOLOGO"))
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
