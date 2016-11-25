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
using System.ComponentModel;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaEncuesta.xaml
    /// </summary>
    public partial class VentanaEncuesta : MetroWindow
    {
        private List<string> preguntas;
        private List<int> respuestas;
        //private string nombreTrabajador;
        private string idTrabajador;
        private int indice;
        private Mensajes cuadroMensajes;
        public VentanaEncuesta()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            inicializacionVariables();
            cuadroMensajes = new Mensajes(this);
        }
        /// <summary>
        /// Metodo que inicializa todas las variables.
        /// </summary>
        private void inicializacionVariables()
        {
            this.indice = 0;
            this.preguntas = new List<string>();
            this.respuestas = new List<int>();
            this.idTrabajador = string.Empty;
            //this.nombreTrabajador = string.Empty;
            this.botonDerecho.IsEnabled = false;
            this.botonIzquierdo.IsEnabled = false;
        }
        /// <summary>
        /// Controlador que ejecuta el despliegue de la pregunta anterior en la lista de preguntas,
        /// con la respectiva respuesta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovimientoIzquierda(object sender, EventArgs e)
        {
            if (indice > 0)
            {
                indice--;
                textoPregunta.Text = preguntas[indice];
                int retroceso = respuestas[indice];
                AlternativaYaSeleccionada(retroceso);

                if (indice < respuestas.Count)
                    botonDerecho.IsEnabled = true;

                else
                    botonDerecho.IsEnabled = false;

                if (indice == 0)
                    botonIzquierdo.IsEnabled = false;
            }
        }
        /// <summary>
        /// Controlador que ejecuta el despliegue de la siguiente pregunta en la lista,
        /// no se ejecuta si no se ha elegido una respuesta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void MovimientoDerecha(object sender, EventArgs e)
        {
            botonDerecho.IsEnabled = false;
            botonIzquierdo.IsEnabled = true;
            if (indice <= (preguntas.Count - 2))
            {
                indice++;
                textoPregunta.Text = preguntas[indice];

                if (indice < respuestas.Count - 1)
                {
                    int avance = respuestas[indice];
                    AlternativaYaSeleccionada(avance);
                    botonDerecho.IsEnabled = true;
                }
                else
                    DeshabilitaAlternativas();
            }
            else
            {
                if (preguntas.Count == respuestas.Count)
                {
                    if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.EncuestaFinalizadaGuardarYSalir()))
                    {
                        await cuadroMensajes.EncuestaGuardadaExitosamente();
                        /*Ejecutar el almacenamiento de datos*/
                        this.Close();
                    }
                }
            }
        }
        /// <summary>
        /// Metodo que desabilita la alternativa seleccionada.
        /// </summary>
        private void DeshabilitaAlternativas()
        {
            alternativaTDA.IsChecked = false;
            alternativaDA.IsChecked = false;
            alternativaNDND.IsChecked = false;
            alternativaEDS.IsChecked = false;
            alternativaTDS.IsChecked = false;
        }
        /// <summary>
        /// Metodo que selecciona una alternativa seleccionada, ya almacenada, al retroceder
        /// en la lista de preguntas.
        /// </summary>
        /// <param name="alternativa"></param>
        private void AlternativaYaSeleccionada(int alternativa)
        {
            if (alternativa == 4)
                alternativaTDA.IsChecked = true;
            else if (alternativa == 3)
                alternativaDA.IsChecked = true;
            else if (alternativa == 2)
                alternativaNDND.IsChecked = true;
            else if (alternativa == 1)
                alternativaEDS.IsChecked = true;
            else if (alternativa == 0)
                alternativaTDS.IsChecked = true;
        }
        /// <summary>
        /// Controlador que se acciona al seleccionar alguna alternativa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionRespuesta(object sender, RoutedEventArgs e)
        {
            if (alternativaTDA.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 4);
                }
                else
                    respuestas.Add(4);
                botonDerecho.IsEnabled = true;
            }
            else if (alternativaDA.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 3);
                }
                else
                    respuestas.Add(3);
                botonDerecho.IsEnabled = true;
            }
            else if (alternativaNDND.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 2);
                }
                else
                    respuestas.Add(2);
                botonDerecho.IsEnabled = true;
            }
            else if (alternativaEDS.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 1);
                }
                else
                    respuestas.Add(1);
                botonDerecho.IsEnabled = true;
            }
            else if (alternativaTDS.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 0);
                }
                else
                    respuestas.Add(0);
                botonDerecho.IsEnabled = true;
            }
        }
        /// <summary>
        /// Controlador que cierra la ventana si se han respondido todas las preguntas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void CerrarEncuesta(object sender, CancelEventArgs e)
        {
            if (respuestas.Count != preguntas.Count)
            {
                e.Cancel = true;
                if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.CerrarEncuesta()))
                {
                    //this.Close();
                }
            }


        }
        public List<string> Preguntas
        {
            get { return preguntas; }
            set { preguntas = value; }
        }

        public string NombreTrabajador
        {
            get { return nombreTrabajador.Content as string; }
            set { nombreTrabajador.Content = value; }
        }

        public string IdTrabajador
        {
            get { return IdTrabajador; }
            set { IdTrabajador = value; }
        }

        public void InicioEncuesta()
        {
            textoPregunta.Text = preguntas[indice];
        }

    }
}
