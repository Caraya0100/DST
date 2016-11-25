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
    /// Lógica de interacción para VentanaEvaluadores.xaml
    /// </summary>
    public partial class VentanaEvaluadores : MetroWindow
    {
        private DatosDePrueba datosRandom;
        public VentanaEvaluadores()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            GeneraListaEvaluadores();
            datosRandom = new DatosDePrueba();
        }

        private void GeneraListaEvaluadores()
        {
            this.panelEvaluadores.Children.Clear();
            for (int i = 0; i < 30; i++)
            {
                VisorEncuestados encuestado = new VisorEncuestados(SeleccionEncuestado);
                encuestado.Nombre = "Alejandro Santander" + i;
                encuestado.Apellido = "Atencion Clientes" + i;
                encuestado.Estado = "No Encuestado";
                encuestado.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                encuestado.IdentificadorBoton = "I" + i;
                this.panelEvaluadores.Children.Add(encuestado.ConstructorInfo());
            }
        }

        private void SeleccionEncuestado(object sender, EventArgs e)
        {
            /*habilita encuesta*/
            int indice = IdentificaTrabajador(sender);/*identificar con el id desde mainwindow*/
            this.Close();
            VentanaEncuesta encuesta = new VentanaEncuesta();
            encuesta.Preguntas = datosRandom.Preguntas;
            encuesta.NombreTrabajador = datosRandom.Nombres[indice] + " " + datosRandom.Apellido[indice];
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
    }
}
