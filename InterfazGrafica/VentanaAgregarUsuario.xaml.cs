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

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaAgregarUsuario.xaml
    /// </summary>
    public partial class VentanaAgregarUsuario : MetroWindow
    {
        private Rutificador rutificador;
        private Mensajes cuadroMensajes;
        private Estructuras.Usuario nuevoUsuario;
        private InteraccionBD.InteraccionUsuarios datosUsuario;//dato de prueba
        private bool edicion;
        public VentanaAgregarUsuario()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            IniciarComponentes();
        }

        private void IniciarComponentes()
        {
            rutificador = new Rutificador();
            cuadroMensajes = new Mensajes(this);
            nuevoUsuario = new Estructuras.Usuario();
            datosUsuario = new InteraccionBD.InteraccionUsuarios();
            edicion = false;
        }
        /// <summary>
        /// Agrega los datos del nuevo usuario a la BD.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void AgregaNuevoUsuario(object sender, RoutedEventArgs e)
        {
            if (CamposCompletos())
            {
                if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.AgregarNuevoUsuario()))
                {
                    if (!rutificador.ValidaRut(rutUsuario.Text, digitoVerificadorUsuario.Text))
                        cuadroMensajes.RutInvalido();
                    else if (!VerificaPassword())
                    {
                        cuadroMensajes.PasswordInvalida();
                        CamposRutPorDefecto();
                    }
                        
                    else
                    {                           
                        /*agregar datos a BD y  actualizar panel*/
                        nuevoUsuario.Nombre = nombreUsuario.Text;
                        nuevoUsuario.Rut = rutificador.formatoRut(this.rutUsuario.Text) +"-" + digitoVerificadorUsuario.Text;
                        nuevoUsuario.Clave = passUsuario.Password;                    
                        if (tipoUsuario.SelectedIndex == 0)
                            nuevoUsuario.TipoUsuario = "JEFE_SECCION";
                        else if (tipoUsuario.SelectedIndex == 1)
                            nuevoUsuario.TipoUsuario = "PSICOLOGO";
                        nuevoUsuario.Estado = true;
                        if (!edicion)
                            datosUsuario.NuevoUsuario = nuevoUsuario;//agrega nuevo
                        else datosUsuario.ActulizacionUsuarios(nuevoUsuario);//actualiza
                        
                        CamposVacios();
                        cuadroMensajes.NuevoUsuarioAgregado();

                    }
                }
            }
            else cuadroMensajes.CamposIncompletosUsuario();
        }
           

        private bool CamposCompletos()
        {
            int camposCompletos = 0;
            if (!nombreUsuario.Text.Equals(""))
                camposCompletos++;
            if (!rutUsuario.Text.Equals(""))
                camposCompletos++;
            if (!digitoVerificadorUsuario.Text.Equals(""))
                camposCompletos++;
            if (!passUsuario.Password.Equals(""))
                camposCompletos++;
            if (!passConfirmacion.Password.Equals(""))
                camposCompletos++;
            if (tipoUsuario.SelectedIndex != -1)
                camposCompletos++;

            if (camposCompletos == 6)
                return true;
            else return false;
           
        }

        private bool VerificaPassword()
        {
            if (passUsuario.Password.Equals(passConfirmacion.Password))
                return true;
            else return false;
        }

        private void CamposVacios()
        {
            nombreUsuario.Text = "";
            rutUsuario.Text = "";
            digitoVerificadorUsuario.Text = "";
            passConfirmacion.Password = "";
            passUsuario.Password = "";
            tipoUsuario.SelectedIndex = -1;
        }

        private void CamposRutPorDefecto()
        {
            passConfirmacion.Password = "";
            passUsuario.Password = "";
        }

        public string NombreUsuario
        {
            get { return nombreUsuario.Text; }
            set { nombreUsuario.Text = value; }
        }

        public string Rut
        {
            get { return rutUsuario.Text; }
            set { rutUsuario.Text = value; }
        }

        public string DigitoVerificador
        {
            get { return digitoVerificadorUsuario.Text; }
            set { digitoVerificadorUsuario.Text = value; }
        }

        public int TipoUsuario
        {
            get { return tipoUsuario.SelectedIndex; }
            set { tipoUsuario.SelectedIndex = value; }
        }

        public string Password
        {
            get { return passUsuario.Password; }
            set { 
                passUsuario.Password = value;
                passConfirmacion.Password = value;
                }
        }

        public bool Edicion
        {
            get { return edicion; }
            set { edicion = value; }
        }
    }
}
