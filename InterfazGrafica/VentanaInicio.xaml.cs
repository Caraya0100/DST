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
using DST;
using System.IO;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaInicio.xaml
    /// </summary>
    public partial class VentanaInicio : MetroWindow
    {
        public VentanaInicio()
        {
            // Si existe la base de datos, mostramos el login.
            if (ExisteBD())
            {
                MostrarPanelLogin();
            } else
            {
                InitializeComponent();
            }
        }

        /// <summary>
        /// Comprueba que exista la base de datos.
        /// </summary>
        /// <returns></returns>
        private bool ExisteBD()
        {
            try
            {
                // Los datos de la base de datos se guardar en el archivo dataDST.
                string[] lines = File.ReadAllLines("dataDST");

                if (lines.Length >= 1)
                {
                    try
                    {
                        BaseDeDatos bd = new BaseDeDatos();
                        bd.Open();
                        bd.Close();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            } catch
            {
                return false;
            }
            return false;
        }

        private void Aceptar(object sender, RoutedEventArgs e)
        {
            string usuarioBD = txtNombreUserBD.Text;
            string passBD = txtPassUserBD.Text;
            string rutAdmin = txtRutAdmin.Text;
            string nombreAdmin = txtNombreAdmin.Text;
            string passAdmin = txtPassAdmin.Text;

            if (usuarioBD != "" && rutAdmin != "" && nombreAdmin != "" && passAdmin != "")
            {
                string datos = usuarioBD + Environment.NewLine;
                datos += passBD;
                File.WriteAllText("dataDST", datos);
                CreacionBD creacionBD = new CreacionBD(usuarioBD, passBD);
                AdminUsuario au = new AdminUsuario();
                au.InsertarUsuario(nombreAdmin, rutAdmin, passAdmin, "ADMINISTRADOR", true);
                MostrarPanelLogin();
            }
        }

        private void MostrarPanelLogin()
        {
            VentanaLogin login = new VentanaLogin();
            login.Show();
            this.Hide();
        }
    }
}
