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
        private InteraccionBD.InteraccionTrabajadores datosTrabajador;
        private InteraccionBD.InteraccionDesempeno datosDesempeno;
        private List<Estructuras.PreguntasYRespuestas> preguntasRespuestas;       
        private List<string> preguntas;
        private List<int> respuestas;
        private List<int> frecuencias;
        //private string nombreTrabajador;
        private string idTrabajador;
        private string idEvaluador;
        private int idSeccion;
        private int indice;
        private bool retroceso;
        private int numeroPregunta;
        private int indiceFrecuencias;
        private bool frecuenciaSeleccionada;
        private bool respuestasSeleccionada;
        private Mensajes cuadroMensajes;
        private bool encuestaLista = false;
        private bool encuestaSeccion;
        private bool encuestaJefeSeccion =false;
        private Perfil perfilseccion;
        private string idJefe;
        /*variables*/
        private List<Encuesta.DatosPregunta> listaPreguntas;
        public VentanaEncuesta(int idseccion)
        {
            encuestaSeccion = false;
            this.idSeccion = idseccion;            
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            inicializacionVariables();
            cuadroMensajes = new Mensajes(this);
            
        }

        public VentanaEncuesta(int idseccion, bool evaluacionSeccion)
        {
            this.idSeccion = idseccion;            
            this.encuestaSeccion = evaluacionSeccion;            
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            inicializacionVariables();
            cuadroMensajes = new Mensajes(this);

        }

        public VentanaEncuesta(bool jefeSeccion, int idseccion)
        {
            this.idSeccion = idseccion;
            this.encuestaJefeSeccion = jefeSeccion;
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
            datosTrabajador = new InteraccionBD.InteraccionTrabajadores();
            datosDesempeno = new InteraccionBD.InteraccionDesempeno();
            preguntasRespuestas = new List<Estructuras.PreguntasYRespuestas>();
            this.indice = 0;
            this.numeroPregunta = 0;
            this.indiceFrecuencias = 0;
            this.preguntas = new List<string>();
            this.respuestas = new List<int>();
            this.frecuencias = new List<int>();
            this.idTrabajador = string.Empty;
            //this.nombreTrabajador = string.Empty;
            this.botonDerecho.IsEnabled = true;//false
            this.botonIzquierdo.IsEnabled = true;//false
            this.frecuenciaSeleccionada = true;//false
            this.respuestasSeleccionada = true;//false
            this.retroceso = false;
            //ListaPreguntas();
            if(!encuestaSeccion)
            {
                listaPreguntas = datosEncuesta.TodasLasPreguntas();
                Console.WriteLine("FALSO");
            }
               
            else if(encuestaSeccion)
            {
                Console.WriteLine("VERDADERO");
                listaPreguntas = datosEncuesta.PreguntasDelPerfil(idSeccion);
            }    
        }
        /// <summary>
        /// Controlador que ejecuta el despliegue de la pregunta anterior en la lista de preguntas,
        /// con la respectiva respuesta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovimientoIzquierda(object sender, EventArgs e)
        {
            retroceso = true;
            numeroPregunta--;
            DesbloqueaFrecuencias();
            if (indice > 0)
            {
                indice--;
                indiceFrecuencias--;
                textoPregunta.Text = (numeroPregunta+1)+". "+listaPreguntas[indice].Pregunta;
                //habilidad.Text = listaPreguntas[indice].Habilidad;
                //tipoHabilidad.Content = TipoHabilidad(listaPreguntas[indice].TipoHabilidad);
                HabilitarPanelRespuestas(listaPreguntas[indice].TipoPregunta);
                /*alternativas*/
                //int retroceso = respuestas[indice];
                //AlternativaYaSeleccionada(retroceso);
                /*frecuencias*/
               // int retrocesoFrecuencias = frecuencias[indiceFrecuencias];                
                GeneraPanelAlternativa(indice);
                AlternativaYaSeleccionada(indice);
                FrecuenciaYaSeleccionada(indice);
                Console.WriteLine("INDICE RETRO: "+indice+" lista:" +listaPreguntas.Count);
                if (indice < listaPreguntas.Count)
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
            datosSeccion.IdSeccion = idSeccion;
            perfilseccion = datosSeccion.PerfilSeccion();

            Console.WriteLine("ESTADO INDICE: "+indice+" LIST: "+listaPreguntas.Count+" RESP: "+respuestas.Count);
            if (listaPreguntas.Count == respuestas.Count+1)
            {
                if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.EncuestaFinalizadaGuardarYSalir()))
                {
                    
                    /*Ejecutar el almacenamiento de datos*/
                    //AsignarRespuestas();
                    foreach (Encuesta.DatosPregunta pregunta in listaPreguntas)
                    {                        
                        if (pregunta.TipoPregunta.Equals("360"))
                        {
                            double valorAlternativa=0;
                            double valorFrecuencia=0;
                            double resultadoRespuesta=0;
                            valorAlternativa = datosEncuesta.ValorAlternativa(pregunta.Id, pregunta.Respuesta360);
                            valorFrecuencia = datosEncuesta.ValorFrecuencia(pregunta.Id, pregunta.Frecuencia);
                            if (valorAlternativa != -1)
                                resultadoRespuesta = (valorAlternativa * valorFrecuencia) / 100;
                            else resultadoRespuesta = valorAlternativa;
                            
                            if (encuestaSeccion)
                            {
                                datosEncuesta.AgregarRespuestaSeccion(idSeccion,resultadoRespuesta,pregunta.Id,pregunta.idHabilidad,pregunta.TipoHabilidad);
                            }
                            else
                            {                               
                                datosEncuesta.AsignarRespuesta360
                                (
                                    pregunta.Id,
                                    idTrabajador,
                                    idEvaluador,
                                    pregunta.Respuesta360,
                                    pregunta.Frecuencia,
                                    resultadoRespuesta
                                );  
                            }
                        }
                        else if (pregunta.TipoPregunta.Equals("normal"))
                        {
                            datosEncuesta.AgregarRespuestaNormal(idSeccion, idTrabajador, idEvaluador, pregunta.RespuestaNormal);
                        }
                        else if (pregunta.TipoPregunta.Equals("datos") && encuestaJefeSeccion)
                        {
                            datosTrabajador.ActualizarPuntajesTrabajador(idTrabajador, pregunta.idHabilidad, Convert.ToDouble(pregunta.RespuestaIngresoDatos));
                            
                        }
                    }
                    if (encuestaSeccion)/*actualiza los valores de la encuesta para evaluar seccion*/
                    {
                        foreach (KeyValuePair<string, Componente> habilidadPuntaje in perfilseccion.Blandas)
                        {

                            double puntaje = datosEncuesta.Puntaje(habilidadPuntaje.Value.ID);
                            datosSeccion.ActualizacionPuntajeHabilidad(idSeccion, habilidadPuntaje.Value.ID, puntaje);
                        }
                        foreach (KeyValuePair<string, Componente> habilidadPuntaje in perfilseccion.Duras)
                        {

                            double puntaje = datosEncuesta.Puntaje(habilidadPuntaje.Value.ID);
                            datosSeccion.ActualizacionPuntajeHabilidad(idSeccion, habilidadPuntaje.Value.ID, puntaje);
                        }
                        foreach (KeyValuePair<string, Componente> habilidadPuntaje in perfilseccion.Fisicas)
                        {

                            double puntaje = datosEncuesta.Puntaje(habilidadPuntaje.Value.ID);
                            datosSeccion.ActualizacionPuntajeHabilidad(idSeccion, habilidadPuntaje.Value.ID, puntaje);
                        }   
                    }
                    else
                    {
                        datosEncuesta.ActualizarEstadoEncuestados(idTrabajador, idEvaluador);
                        /*actualiza los puntajes del trabajador por todas las encuestas*/
                        foreach (KeyValuePair<string, double> habilidades in datosEncuesta.PuntajesGeneralesPorHabilidad(idTrabajador))
                        {
                            datosTrabajador.ActualizarPuntajesTrabajador(idTrabajador, habilidades.Key, habilidades.Value);
                        }
                        /*actualiza los puntajes generales del trabajador*/
                        foreach (KeyValuePair<string, double> habilidades in datosDesempeno.CalcularPuntajesGeneralesTrabajador(idTrabajador))
                        {
                            datosTrabajador.ActualizarPuntajesTrabajador(idTrabajador, habilidades.Key, habilidades.Value);
                        }
                    }

                    encuestaLista = true;
                    this.Close();
                    VentanaLogin login = new VentanaLogin();
                    login.Show();
                    
                    
                }
            }
            else if (indice <= (listaPreguntas.Count - 1))
            {
                numeroPregunta++;
                indice++;
                indiceFrecuencias++;
                if (!encuestaJefeSeccion && !listaPreguntas[indice].TipoPregunta.Equals("datos"))
                {
                    Console.WriteLine("no es DATO "+indice+" largo: "+listaPreguntas.Count);
                    textoPregunta.Text = (numeroPregunta + 1) + ". " + listaPreguntas[indice].Pregunta;
                    //habilidad.Text = listaPreguntas[indice].Habilidad;
                    //tipoHabilidad.Content = TipoHabilidad(listaPreguntas[indice].TipoHabilidad);
                    HabilitarPanelRespuestas(listaPreguntas[indice].TipoPregunta);
                    
                }
                else
                {
                    respuestas.Add(indice);
                    indice++;
                    indiceFrecuencias++;
                    Console.WriteLine("si es DATO "+indice+" largo: "+listaPreguntas.Count);
                    textoPregunta.Text = (numeroPregunta) + ". " + listaPreguntas[indice].Pregunta;
                    //habilidad.Text = listaPreguntas[indice].Habilidad;
                    //tipoHabilidad.Content = TipoHabilidad(listaPreguntas[indice].TipoHabilidad);
                    HabilitarPanelRespuestas(listaPreguntas[indice].TipoPregunta);
                   
                }
                

                if (indice < respuestas.Count - 1 && retroceso)//cuidado
                {
                    GeneraPanelAlternativa(indice);
                    AlternativaYaSeleccionada(indice);
                    FrecuenciaYaSeleccionada(indice);
                    botonDerecho.IsEnabled = true;
                }
                else 
                {
                    GeneraPanelAlternativa(indice);
                    respuestas.Add(indice);
                }
                    
                    
            }
            else
            {
                
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
       
        private void SeleccionAlternativa(object sender, EventArgs e)
        {
            RadioButton seleccion = sender as RadioButton;
            string alternativa = seleccion.Name.Replace("_"," ");
            //Console.WriteLine("ANTES: "+ listaPreguntas[indice].Pregunta+" - "+listaPreguntas[indice].Respuesta360);
            Encuesta.DatosPregunta pregunta = listaPreguntas[indice];
            pregunta.Respuesta360 = alternativa;
            listaPreguntas.RemoveAt(indice);
            listaPreguntas.Insert(indice,pregunta);
            //Console.WriteLine("DESPUES: " + listaPreguntas[indice].Pregunta + " - " + listaPreguntas[indice].Respuesta360);
            //respuestas.Add(indice);
            respuestasSeleccionada = true;            
            if (listaPreguntas[indice].Respuesta360.Equals("No puede ser evaluado"))
            {
                botonDerecho.IsEnabled = true;
            }
            if (respuestasSeleccionada && frecuenciaSeleccionada)
                botonDerecho.IsEnabled = true;
        }

        private void SeleccionFrecuencias(object sender, EventArgs e)
        {
            RadioButton seleccion = sender as RadioButton;
            string alternativa = seleccion.Name.Replace("_", " ");
            //Console.WriteLine("ANTESF: " + listaPreguntas[indice].Pregunta + " - " + listaPreguntas[indice].Frecuencia);
            Encuesta.DatosPregunta pregunta = listaPreguntas[indice];
            pregunta.Frecuencia = alternativa;
            listaPreguntas.RemoveAt(indice);
            listaPreguntas.Insert(indice, pregunta);
            //Console.WriteLine("DESPUESF: " + listaPreguntas[indice].Pregunta + " - " + listaPreguntas[indice].Frecuencia);
            //frecuencias.Add(indice);
            frecuenciaSeleccionada = true;
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
            if (!encuestaLista)
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
            get { return idTrabajador; }
            set { idTrabajador = value; }
        }

        public string IdEvaluador
        {
            get { return idEvaluador; }
            set { idEvaluador = value; }
        }

        public void InicioEncuesta()
        {
            if (!encuestaJefeSeccion)
            {
                textoPregunta.Text = (indice + 1) + ". " + listaPreguntas[0].Pregunta;
                //tipoHabilidad.Content = TipoHabilidad(listaPreguntas[0].TipoHabilidad);
                //habilidad.Text = listaPreguntas[0].Habilidad;
                HabilitarPanelRespuestas(listaPreguntas[0].TipoPregunta);
                GeneraPanelAlternativa(0);
            }
            
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }
        /***********************************************************************
         *                              METODOS 
         * *********************************************************************/
        /// <summary>
        /// Distingue entre tipo de pregunta 360, normal, ingreso de datos
        /// </summary>
        /// <param name="index"></param>
        private void GeneraPanelAlternativa(int index)
        {           
            if (listaPreguntas[index].TipoPregunta.Equals("360"))
            {
                this.panelAlternativas360.Children.Clear();
                this.panelFrecuencias.Children.Clear();
                HabilitarPanelRespuestas("360");
                Label grado = new Label(); grado.Content = "              GRADO"; //grado.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                grado.Height = 40; grado.FontWeight = FontWeights.Bold;
                Label frecuencia = new Label(); frecuencia.Content = "FRECUENCIA"; //frecuencia.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                frecuencia.Height = 40; frecuencia.FontWeight = FontWeights.Bold;
                this.panelAlternativas360.Children.Add(grado);
                this.panelFrecuencias.Children.Add(frecuencia);
                foreach (Encuesta.DatosAlternativa alternativa in datosEncuesta.Alternativas(listaPreguntas[index].Id, "grado"))
                {
                    AgregarAlternativa360(alternativa.Alternativa);
                }
                foreach (Encuesta.DatosAlternativa alternativa in datosEncuesta.Alternativas(listaPreguntas[index].Id, "frecuencia"))
                {
                    AgregarFrecuencia360(alternativa.Alternativa);
                }
            }
            else if (listaPreguntas[index].TipoPregunta.Equals("normal"))
            {
                HabilitarPanelRespuestas("normal");
                this.panelNormal.Children.Clear();                
                foreach (Encuesta.DatosAlternativa alternativa in datosEncuesta.Alternativas(listaPreguntas[index].Id, "grado"))
                {
                    AgregarAlternativa360(alternativa.Alternativa);
                }
            }
            else if (listaPreguntas[index].TipoPregunta.Equals("datos"))
            {
                HabilitarPanelRespuestas("datos");
            }
        }

        private void AlternativaYaSeleccionada(int index)
        {
            RadioButton alternativa = (System.Windows.Controls.RadioButton)
                                                LogicalTreeHelper.FindLogicalNode
                                                (
                                                    panelAlternativas360,
                                                    listaPreguntas[index].Respuesta360.Replace(" ","_")
                                                );
            if(alternativa != null)
                alternativa.IsChecked = true;
            Console.WriteLine("NO PUEDE: "+listaPreguntas[index].Respuesta360);
            if (listaPreguntas[index].Respuesta360.Equals("No puede ser evaluado"))
            {
                botonDerecho.IsEnabled = true;
            }
        }

        private void FrecuenciaYaSeleccionada(int index)
        {
            RadioButton alternativa = (System.Windows.Controls.RadioButton)
                                                LogicalTreeHelper.FindLogicalNode
                                                (
                                                    panelFrecuencias,
                                                    listaPreguntas[index].Frecuencia.Replace(" ", "_")
            
                                                );
            if(alternativa != null)
                alternativa.IsChecked = true;
        }

       

        private string TipoHabilidad(string habilidad)
        {
            string tipo = string.Empty;
            if (habilidad.Equals("hd"))
                tipo = "Habilidad Dura";
            else if (habilidad.Equals("hb"))
                tipo = "Habilidad Blanda";
            else if (habilidad.Equals("cf"))
                tipo = "Características Fisicas";
            return tipo;
        }

        private void HabilitarPanelRespuestas(string tipoPregunta)
        {
            if (tipoPregunta.Equals("360"))
            {
                this.panel360.Visibility = System.Windows.Visibility.Visible;
                this.panelDatos.Visibility = System.Windows.Visibility.Hidden;
                this.panelNormal.Visibility = System.Windows.Visibility.Hidden;
            }

            else if (tipoPregunta.Equals("normal"))
            {
                this.panel360.Visibility = System.Windows.Visibility.Hidden;
                this.panelDatos.Visibility = System.Windows.Visibility.Hidden;
                this.panelNormal.Visibility = System.Windows.Visibility.Visible;
            }
            else if (tipoPregunta.Equals("datos"))
            {
                this.panel360.Visibility = System.Windows.Visibility.Hidden;
                this.panelDatos.Visibility = System.Windows.Visibility.Visible;
                this.panelNormal.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void AgregarAlternativa360(string nombre)
        {            
            RadioButton alternativa = new RadioButton();
            alternativa.Content = nombre;
            alternativa.Name = nombre.Replace(" ","_");
            alternativa.Click += SeleccionAlternativa;
            alternativa.Width = 370;
            alternativa.Height = 30;
            alternativa.FontSize = 16;
            this.panelAlternativas360.Children.Add(alternativa);
            

        }

        private void AgregarFrecuencia360(string nombre)
        {
            RadioButton alternativa = new RadioButton();
            alternativa.Content = nombre;
            alternativa.Name = nombre.Replace(" ", "_");
            alternativa.Click += SeleccionFrecuencias;
            //alternativa.Width = 370;
            alternativa.Height = 30;
            alternativa.FontSize = 16;
            this.panelFrecuencias.Children.Add(alternativa);
        }

        
        private void IngresoDeDatos(object sender, TextChangedEventArgs e)
        {            
            botonDerecho.IsEnabled = true;
            Encuesta.DatosPregunta pregunta = listaPreguntas[indice];
            pregunta.RespuestaIngresoDatos = respuestaDato.Text;
            listaPreguntas.RemoveAt(indice);
            listaPreguntas.Insert(indice, pregunta);
        }

        public bool EncuestaJefeSeccion
        {
            get { return encuestaJefeSeccion; }
            set { encuestaJefeSeccion = value; }
        }
    }
}
