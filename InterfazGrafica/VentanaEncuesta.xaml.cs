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
using DST;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaEncuesta.xaml
    /// </summary>
    public partial class VentanaEncuesta : MetroWindow
    {
        private InteraccionBD.InteraccionEncuesta datosEncuesta;
        private InteraccionBD.InteraccionSecciones datosSeccion;
        private List<Estructuras.PreguntasYRespuestas> preguntasRespuestas;
        private List<string> preguntas;
        private List<int> respuestas;
        private List<int> frecuencias;
        //private string nombreTrabajador;
        private string idTrabajador;
        private int idSeccion;
        private int indice;
        private int indiceFrecuencias;
        private bool frecuenciaSeleccionada;
        private bool respuestasSeleccionada;
        private Mensajes cuadroMensajes;
        public VentanaEncuesta(int idseccion)
        {
            this.idSeccion = idseccion;
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
            datosEncuesta = new InteraccionBD.InteraccionEncuesta();
            datosSeccion = new InteraccionBD.InteraccionSecciones();
            preguntasRespuestas = new List<Estructuras.PreguntasYRespuestas>();
            this.indice = 0;
            this.indiceFrecuencias = 0;
            this.preguntas = new List<string>();
            this.respuestas = new List<int>();
            this.frecuencias = new List<int>();
            this.idTrabajador = string.Empty;
            //this.nombreTrabajador = string.Empty;
            this.botonDerecho.IsEnabled = false;
            this.botonIzquierdo.IsEnabled = false;
            this.frecuenciaSeleccionada = false;
            this.respuestasSeleccionada = false;
            ListaPreguntas();
        }
        /// <summary>
        /// Controlador que ejecuta el despliegue de la pregunta anterior en la lista de preguntas,
        /// con la respectiva respuesta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovimientoIzquierda(object sender, EventArgs e)
        {
            DesbloqueaFrecuencias();
            if (indice > 0)
            {
                indice--;
                indiceFrecuencias--;
                textoPregunta.Text = preguntas[indice];
                /*alternativas*/
                int retroceso = respuestas[indice];
                AlternativaYaSeleccionada(retroceso);
                /*frecuencias*/
                int retrocesoFrecuencias = frecuencias[indiceFrecuencias];
                FrecuenciaYaSeleccionada(retrocesoFrecuencias);

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
            frecuenciaSeleccionada = false;
            respuestasSeleccionada = false;
            botonIzquierdo.IsEnabled = true;
            DesbloqueaFrecuencias();
            if (indice <= (preguntas.Count - 2))
            {
                indice++;
                indiceFrecuencias++;
                textoPregunta.Text = preguntas[indice];

                if (indice <= respuestas.Count - 2 )
                {   /*alternativas*/
                    int avance = respuestas[indice];
                    AlternativaYaSeleccionada(avance);
                    /*frecuencias*/
                    int avanceFrecuencias = frecuencias[indice];
                    FrecuenciaYaSeleccionada(avanceFrecuencias);                    
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
                        AsignarRespuestas();
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
            alternativaMR.IsChecked = false;
            alternativaAC.IsChecked = false;
            alternativaC.IsChecked = false;
            alternativaND.IsChecked = false;
            alternativaNDS.IsChecked = false;
            alternativaNO.IsChecked = false;
            /*frecuencia*/
            alternativaSiempre.IsChecked = false;
            alternativaFrecuente.IsChecked = false;
            alternativaOcasional.IsChecked = false;
            alternativaMitadTiempo.IsChecked = false;
        }

        private void DeshabilitaFrecuencias()
        {
            alternativaSiempre.IsChecked = false;
            alternativaFrecuente.IsChecked = false;
            alternativaOcasional.IsChecked = false;
            alternativaMitadTiempo.IsChecked = false;
        }

        private void BloqueaFrecuencias()
        {
            alternativaSiempre.IsEnabled = false;
            alternativaFrecuente.IsEnabled = false;
            alternativaOcasional.IsEnabled = false;
            alternativaMitadTiempo.IsEnabled = false;
        }

        private void DesbloqueaFrecuencias()
        {
            alternativaSiempre.IsEnabled = true;
            alternativaFrecuente.IsEnabled = true;
            alternativaOcasional.IsEnabled = true;
            alternativaMitadTiempo.IsEnabled = true;
        }
        /// <summary>
        /// Metodo que setea una alternativa seleccionada, ya almacenada, al retroceder
        /// en la lista de preguntas.
        /// </summary>
        /// <param name="alternativa"></param>
        private void AlternativaYaSeleccionada(int alternativa)
        {
            if (alternativa == 5)
                alternativaMR.IsChecked = true;
            else if (alternativa == 4)
                alternativaAC.IsChecked = true;
            else if (alternativa == 3)
                alternativaC.IsChecked = true;
            else if (alternativa == 2)
                alternativaND.IsChecked = true;
            else if (alternativa == 1)
                alternativaNDS.IsChecked = true;
            else if (alternativa == 0)
                alternativaNO.IsChecked = true;            
        }
        /// <summary>
        /// Metodo que setea una alternativa seleccionada, ya almacenada, al retroceder
        /// en la lista de frecuencias.
        /// </summary>
        /// <param name="alternativa"></param>
        private void FrecuenciaYaSeleccionada(int alternativa)
        {
            if (alternativa == 3)
                alternativaSiempre.IsChecked = true;
            else if (alternativa == 2)
                alternativaFrecuente.IsChecked = true;
            else if (alternativa == 1)
                alternativaMitadTiempo.IsChecked = true;
            else if (alternativa == 0)
                alternativaOcasional.IsChecked = true;
            else if (alternativa == -1)
                DeshabilitaFrecuencias();
            
        }
        /// <summary>
        /// Controlador que se acciona al seleccionar alguna alternativa
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionRespuesta(object sender, RoutedEventArgs e)
        {
            respuestasSeleccionada = true;
            if (alternativaMR.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 5);                    
                }
                else
                    respuestas.Add(5);
               // botonDerecho.IsEnabled = true;
            }
            else if (alternativaAC.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 4);
                }
                else
                    respuestas.Add(4);
                //botonDerecho.IsEnabled = true;
            }
            else if (alternativaC.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 3);
                }
                else
                    respuestas.Add(3);
                //botonDerecho.IsEnabled = true;
            }
            else if (alternativaND.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 2);
                }
                else
                    respuestas.Add(2);
                //botonDerecho.IsEnabled = true;
            }
            else if (alternativaNDS.IsChecked == true)
            {
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 1);
                }
                else
                    respuestas.Add(1);
                //botonDerecho.IsEnabled = true;
            }
            else if (alternativaNO.IsChecked == true)
            {
                frecuenciaSeleccionada = true;
                BloqueaFrecuencias();
                /*agregar valor ORIGINAL*/
                if (indice < respuestas.Count)
                {
                    respuestas.RemoveAt(indice);
                    respuestas.Insert(indice, 0);
                    frecuencias.Add(-1);
                }
                else
                {
                    respuestas.Add(0);
                    frecuencias.Add(-1);
                }
                    

            }
            if(respuestasSeleccionada && frecuenciaSeleccionada)
                botonDerecho.IsEnabled = true;
        }
      
        private void SeleccionFrecuencia(object sender, RoutedEventArgs e)
        {
            frecuenciaSeleccionada = true;
            if (alternativaSiempre.IsChecked == true)
            {
                if (indiceFrecuencias < frecuencias.Count)
                {
                    frecuencias.RemoveAt(indiceFrecuencias);
                    frecuencias.Insert(indiceFrecuencias, 3);
                }
                else
                    frecuencias.Add(3);
            }
            else if (alternativaFrecuente.IsChecked == true)
            {
                if (indiceFrecuencias < frecuencias.Count)
                {
                    frecuencias.RemoveAt(indiceFrecuencias);
                    frecuencias.Insert(indiceFrecuencias, 2);
                }
                else
                    frecuencias.Add(2);
            }
            else if (alternativaMitadTiempo.IsChecked == true)
            {
                if (indiceFrecuencias < frecuencias.Count)
                {
                    frecuencias.RemoveAt(indiceFrecuencias);
                    frecuencias.Insert(indiceFrecuencias, 1);
                }
                else
                    frecuencias.Add(1);
            }
            else if (alternativaOcasional.IsChecked == true)
            {
                if (indiceFrecuencias < frecuencias.Count)
                {
                    frecuencias.RemoveAt(indiceFrecuencias);
                    frecuencias.Insert(indiceFrecuencias, 0);
                }
                else
                    frecuencias.Add(0);
            }
            /*comprueba que respuesta y frecuencias esten seleccionadas antes de avanzar*/
            if (respuestasSeleccionada && frecuenciaSeleccionada)
                botonDerecho.IsEnabled = true;
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

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }
        /***********************************************************************
         *                              METODOS 
         * *********************************************************************/
        private void ListaPreguntas()
        {
            //agregar a lista de preguntas
            datosSeccion.IdSeccion = idSeccion;
            foreach (KeyValuePair<string, Componente> a in datosSeccion.PerfilSeccion().Blandas)
            {
                Console.WriteLine("COMPONENTE: " + a.Value.Nombre);
                datosEncuesta.NombreHabiliadad = a.Value.Nombre;
                datosEncuesta.Preguntas();
                foreach (string pregunta in datosEncuesta.Preguntas())
                {
                    Estructuras.PreguntasYRespuestas pregResp = new Estructuras.PreguntasYRespuestas();
                    pregResp.Pregunta = pregunta;
                    preguntas.Add(pregunta);
                    //preguntasRespuestas.Add(pregResp);
                    Console.WriteLine("PREGUNTA: " + pregunta);
                }
            }
            foreach (KeyValuePair<string, Componente> a in datosSeccion.PerfilSeccion().Duras)
            {
                Console.WriteLine("COMPONENTE: " + a.Value.Nombre);
                datosEncuesta.NombreHabiliadad = a.Value.Nombre;
                datosEncuesta.Preguntas();
                foreach (string pregunta in datosEncuesta.Preguntas())
                {
                    Estructuras.PreguntasYRespuestas pregResp = new Estructuras.PreguntasYRespuestas();
                    pregResp.Pregunta = pregunta;
                    preguntas.Add(pregunta);
                    //preguntasRespuestas.Add(pregResp);
                    Console.WriteLine("PREGUNTA: " + pregunta);
                }
            }         
        }

        private void AsignarRespuestas()
        {
            Console.WriteLine("LARGO respuestas: "+respuestas.Count);
            int indice = 0;
            foreach (int respuesta in respuestas)
            {
                Estructuras.PreguntasYRespuestas pr = new Estructuras.PreguntasYRespuestas();
                pr.AlternativaRespuesta = ""+respuesta;
                pr.Pregunta = preguntas[indice];
                pr.Frecuencia = ""+frecuencias[indice];
                preguntasRespuestas.Add(pr);
                indice++;
            }
            Console.WriteLine("LARGO respuestas2: " + respuestas.Count+" - "+preguntasRespuestas.Count);
            foreach (Estructuras.PreguntasYRespuestas a in preguntasRespuestas)
            {
                Console.WriteLine("PREGUNTA: "+a.AlternativaRespuesta+" FRECUENCIA: "+a.Frecuencia+" largo: "+preguntasRespuestas.Count);
            }
            Console.WriteLine("LARGO PR: "+preguntasRespuestas.Count);
        }
    }
}
