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
    /// Lógica de interacción para VentanaAgregarSeccion.xaml
    /// </summary>
    public partial class VentanaAgregarSeccion : MetroWindow
    {
        private Mensajes cuadroMensajes;
        private InteraccionBD.InteraccionUsuarios datosUsuario;
        private InteraccionBD.InteraccionSecciones datosSeccion;
        /*Variables*/
        private bool edicion;
        public VentanaAgregarSeccion()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            IniciarComponentes();
            GeneraListaJefes();
        }

        private void IniciarComponentes()
        {
            this.cuadroMensajes = new Mensajes(this);
            this.datosUsuario = new InteraccionBD.InteraccionUsuarios();
            this.datosSeccion = new InteraccionBD.InteraccionSecciones();
            this.edicion = false;
        }
        /// <summary>
        /// Metodo que despliega ventana para ingresar un nuevo usuario.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregaNuevoUsuario(object sender, RoutedEventArgs e)
        {
            VentanaAgregarUsuario nuevoUsuario = new VentanaAgregarUsuario();
            nuevoUsuario.ShowDialog();
            GeneraListaJefes();

        }
        /// <summary>
        /// Metodo que agrega los datos de la nueva seccion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void AgregaNuevaSeccion(object sender, RoutedEventArgs e)
        {
            if (CamposCompletos())
            {
                if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.AgregarNuevaSeccion()))
                {
                    /*Agregar datos a BD*/
                    string jefe = this.listaJefeSeccion.SelectedItem as string;
                    //string seccion = this.nombreSeccion.Text;
                    datosSeccion.NombreSeccion = this.nombreSeccion.Text;
                    datosSeccion.RutJefeSeccion = datosUsuario.RutUsuario(jefe); 
                    datosSeccion.NuevaSeccion();                   
                    CamposVacios();
                    cuadroMensajes.NuevaSeccionAgregada();
                    //this.Close();
                }
            }
            else 
                cuadroMensajes.CamposIncompletosSeccion();
            
            
        }

        private void GeneraListaJefes()
        {
            this.listaJefeSeccion.Items.Clear();
            foreach (string jefeSeccion in datosUsuario.ListaJefesSeccion())
            {
                this.listaJefeSeccion.Items.Add(jefeSeccion);
            }
        }

        private bool CamposCompletos()
        {            
            int camposCompletos = 0;
            if (!nombreSeccion.Text.Equals(""))
                camposCompletos++;
            if (listaJefeSeccion.SelectedIndex != -1)
                camposCompletos++;
            if (!descripcionSeccion.Text.Equals(""))
                camposCompletos++;

            if (camposCompletos == 2)//3 CON DESCRIPCION
                return true;
            else return false;
        }

        private void CamposVacios()
        {
            nombreSeccion.Text = "";
            listaJefeSeccion.SelectedIndex = -1;
            descripcionSeccion.Text = "";
        }

        public string Seccion
        {
            get { return nombreSeccion.Text; }
            set { nombreSeccion.Text = value; }
        }

        public string Descripcion
        {
            get { return descripcionSeccion.Text; }
            set { descripcionSeccion.Text = value; }
        }

        public int JefeSeccion
        {
            get { return listaJefeSeccion.SelectedIndex; }
            set { listaJefeSeccion.SelectedIndex = value; }
        }

        public ItemCollection NombreJefeSeccion
        {
            get { return listaJefeSeccion.Items; }
            set { listaJefeSeccion.SelectedItem = value; }
        }

        public bool Edicion
        {
            get { return edicion; }
            set { edicion = value; }
        }
    }
}
