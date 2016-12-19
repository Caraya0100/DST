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
    /// Lógica de interacción para VentanaEvaluadores.xaml
    /// </summary>
    public partial class VentanaEvaluadores : MetroWindow
    {
        private DatosDePrueba datosRandom;
        private InteraccionBD.InteraccionTrabajadores datosTrabajador;
        private int idSeccion;
        private string rutEvaluado;
        private string nombreEvaluado;        
        List<Trabajador> encuestados;
        List<string> rutEvaluadores;
        bool cerrarVentana = false;
        public VentanaEvaluadores( string rutEvaluado)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            datosTrabajador = new InteraccionBD.InteraccionTrabajadores();
            rutEvaluadores = new List<string>();
            datosTrabajador.IdTrabajador = rutEvaluado;
            this.rutEvaluado = rutEvaluado;
            GeneraListaEvaluadores();
            datosRandom = new DatosDePrueba();          
        }

        private void CerrarVentana(object sender, CancelEventArgs e)
        {
            if (!cerrarVentana)
            {
                this.Hide();
                VentanaLogin login = new VentanaLogin();
                login.Show();
            }
            
        }
        private void GeneraListaEvaluadores()
        {
            this.panelEvaluadores.Children.Clear();            
          
            encuestados = datosTrabajador.TrabajadoresEncuestados();
            int indice = 0;
            foreach (Trabajador trabajador in datosTrabajador.TrabajadoresEmpresa())
            {
                VisorEncuestados encuestado = new VisorEncuestados(SeleccionEncuestado);
                encuestado.Nombre = trabajador.Nombre;
                encuestado.Apellido = trabajador.ApellidoPaterno;
                encuestado.Estado = "No Encuestado/a";
                if (trabajador.Sexo.Equals("Masculino"))
                    encuestado.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                else encuestado.DireccionImagen = @"..\..\Iconos\User-Female.png";                
                foreach(Trabajador trab in encuestados)
                {
                    if (trab.Rut.Equals(trabajador.Rut))
                    {
                        encuestado.Habilitado = false;
                        encuestado.Estado = "Encuestado/a";
                        encuestado.ColorDeshabilitado();
                    }
                }
                rutEvaluadores.Add(trabajador.Rut);
                encuestado.IdentificadorBoton = "I" + indice;
                this.panelEvaluadores.Children.Add(encuestado.ConstructorInfo());
                indice++;
            }
        }

        private void SeleccionEncuestado(object sender, EventArgs e)
        {
            /*habilita encuesta*/
            cerrarVentana = true;
            int indice = IdentificaTrabajador(sender);/*identificar con el id desde mainwindow*/
            this.Close();
            VentanaEncuesta encuesta = new VentanaEncuesta(idSeccion);
            encuesta.Preguntas = datosRandom.Preguntas;
            encuesta.NombreTrabajador = nombreEvaluado;
            encuesta.IdTrabajador = rutEvaluado;
            encuesta.IdEvaluador = rutEvaluadores[indice];
            encuesta.InicioEncuesta();
            encuesta.ShowDialog();
            
        }
        /// <summary>
        /// Metodo que transforma el nombre del objeto en id asociada al trabajador.
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        private int IdentificaTrabajador(Object sender)
        {
            System.Windows.Controls.Button ver = sender as System.Windows.Controls.Button;
            string id = ver.Name as string;

            string[] indiceEtiqueta = id.Split('I');
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
            return indice;
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }

        public string RutEvaluado
        {
            get { return rutEvaluado; }
            set { rutEvaluado = value; }
        }

        public string NombreEvaluado
        {
            get { return nombreEvaluado; }
            set { nombreEvaluado = value; }
        }
    }
}
