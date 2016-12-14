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
        private string idTrabajador;
        private int idSeccion;
        private DatosDePrueba datosPrueba;//datos de prueba
        private bool edicion;
        private string rutNoModificado;
        
        /****************************/
        private InteraccionBD.InteraccionTrabajadores datosTrabajadores;
        public VentanaAgregarTrabajador()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            IniciarComponentes();
        }


        private void IniciarComponentes()
        {
            idTrabajador = string.Empty;
            rutificador = new Rutificador();
            cuadroMensajes = new Mensajes(this);
            datosTrabajadores = new InteraccionBD.InteraccionTrabajadores();
            datosPrueba = new DatosDePrueba();
            edicion = false;
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
                rut = rutificador.formatoRut(this.rut.Text) + "-" + this.digitoVerificador.Text;//agrego text
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
                Estructuras.Trabajador nuevoTrabajador = new Estructuras.Trabajador();
                nuevoTrabajador.Nombre = nombre.Text;
                nuevoTrabajador.ApellidoPaterno = apellidoPaterno.Text;
                nuevoTrabajador.ApellidoMaterno = apellidoMaterno.Text;
                nuevoTrabajador.Rut = rut;
                if(sexo.SelectedIndex == 0)
                    nuevoTrabajador.Sexo = "Masculino";
                else nuevoTrabajador.Sexo = "Femenino";
                nuevoTrabajador.FechaNacimiento = etiquetaFechaNacimiento.Content as string;
                nuevoTrabajador.IdSeccion = idSeccion;//CAMBIAR por idseccion
                nuevoTrabajador.Estado = true;
                if (!edicion)
                {
                    nuevoTrabajador.IdSeccion = idSeccion; System.Windows.MessageBox.Show("id"+idSeccion);
                    datosTrabajadores.NuevoTrabajador(nuevoTrabajador);//agrega el trabajador
                }                    
                else datosTrabajadores.ModificaTrabajador(nuevoTrabajador, rutNoModificado);//guarda los datos editados
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
            if (this.calendario.SelectedDate.HasValue)
                this.etiquetaFechaNacimiento.Content = "" + this.calendario.SelectedDate.Value.ToString("yyyy-MM-dd"); //dd/MM/yyyy
            else this.etiquetaFechaNacimiento.Content = string.Empty;
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
            digitoVerificador.Text = "";
            etiquetaFechaNacimiento.Content = "";
        }

        public string IdTrabajador
        {
            get { return idTrabajador; }
            set { idTrabajador = value; }
        }

        public void RecuperarDatos()
        {
            int indice = Convert.ToInt32(idTrabajador);
            nombre.Text = datosPrueba.Trabajadores[indice].Nombre;//datos de prueba
            apellidoPaterno.Text = datosPrueba.Trabajadores[indice].ApellidoPaterno;//datos de prueba
            apellidoMaterno.Text = datosPrueba.Trabajadores[indice].ApellidoMaterno;//datos de prueba
            string[] rutSeparado = datosPrueba.Trabajadores[indice].Rut.Split('-');
            rut.Text = rutSeparado[0];//datos de prueba
            digitoVerificador.Text = rutSeparado[1];//datos de prueba
            etiquetaFechaNacimiento.Content = datosPrueba.Trabajadores[indice].FechaNacimiento;//datos de prueba
            if (datosPrueba.Trabajadores[indice].Sexo.Equals("Masculino"))//datos de prueba
                sexo.SelectedIndex = 0;
            else
                sexo.SelectedIndex = 1;
        }

        public string NombreTrabajador
        {
            get { return nombre.Text; }
            set { nombre.Text = value; }
        }

        public string ApellidoPaterno
        {
            get { return apellidoPaterno.Text; }
            set { apellidoPaterno.Text = value; }
        }

        public string ApellidoMaterno
        {
            get { return apellidoMaterno.Text; }
            set { apellidoMaterno.Text = value; }
        }
        public string FechaNacimiento
        {
            get { return etiquetaFechaNacimiento.Content as string; }
            set { etiquetaFechaNacimiento.Content = value; }
        }

        public int Sexo
        {
            get { return sexo.SelectedIndex; }
            set { sexo.SelectedIndex = value; }
        }

        public string Rut
        {
            get { return rut.Text; }
            set { rut.Text = value; }
        }

        public string DigitoVerificador
        {
            get { return digitoVerificador.Text; }
            set { digitoVerificador.Text = value; }
        }

        public bool Edicion
        {
            get { return edicion; }
            set { edicion = value; }
        }

        public string RutNoModificado
        {
            get { return rutNoModificado; }
            set { rutNoModificado = value; }
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }
    }
}
