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
        private string idEvaluador;
        private int idSeccion;
        private int indice;
        private int indiceFrecuencias;
        private bool frecuenciaSeleccionada;
        private bool respuestasSeleccionada;
        private Mensajes cuadroMensajes;
        private bool encuestaLista = false;
        private bool encuestaSeccion;
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
            this.botonDerecho.IsEnabled = true;//false
            this.botonIzquierdo.IsEnabled = true;//false
            this.frecuenciaSeleccionada = true;//false
            this.respuestasSeleccionada = true;//false
            //ListaPreguntas();
            if(!encuestaSeccion)
            {
                listaPreguntas = datosEncuesta.TodasLasPreguntas();
                Console.WriteLine("FALSO");
            }
               
            else 
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
            DesbloqueaFrecuencias();
            if (indice > 0)
            {
                indice--;
                indiceFrecuencias--;
                textoPregunta.Text = (indice+1)+". "+listaPreguntas[indice].Pregunta;
                habilidad.Text = listaPreguntas[indice].Habilidad;
                tipoHabilidad.Content = TipoHabilidad(listaPreguntas[indice].TipoHabilidad);
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
                        
            if (listaPreguntas.Count == respuestas.Count+1)
            {
                if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.EncuestaFinalizadaGuardarYSalir()))
                {
                    
                    /*Ejecutar el almacenamiento de datos*/
                    //AsignarRespuestas();
                    foreach (Encuesta.DatosPregunta pregunta in listaPreguntas)
                    {
                        /*Console.WriteLine("hab: "+pregunta.Habilidad);
                        Console.WriteLine("habti: " + pregunta.TipoHabilidad);
                        Console.WriteLine("prtip: " + pregunta.TipoPregunta);
                        Console.WriteLine("pr: " + pregunta.Pregunta);
                        Console.WriteLine("res: "+pregunta.Respuesta360);
                        Console.WriteLine("id: " + pregunta.Id);*/
                        if (pregunta.TipoPregunta.Equals("360"))
                        {

                           


                            /*Console.WriteLine("TIPO 360"); Console.WriteLine("__________________");
                            Console.WriteLine("PREGUNTA: " + pregunta.Pregunta);
                            Console.WriteLine("RESPUESTA: " + pregunta.Respuesta360);
                            Console.WriteLine("FRECUENCIA: " + pregunta.Frecuencia);
                            Console.WriteLine("ID: " + pregunta.Id);*/
                            double valorAlternativa=0;
                            double valorFrecuencia=0;
                            double resultadoRespuesta=0;
                            valorAlternativa = datosEncuesta.ValorAlternativa(pregunta.Id, pregunta.Respuesta360);
                            valorFrecuencia = datosEncuesta.ValorFrecuencia(pregunta.Id, pregunta.Frecuencia);
                            if (valorAlternativa != -1)
                                resultadoRespuesta = (valorAlternativa * valorFrecuencia) / 100;
                            else resultadoRespuesta = valorAlternativa;
                            Console.WriteLine("VALOR: "+resultadoRespuesta+" VALOR REPARADO: "+Convert.ToDouble(resultadoRespuesta.ToString("0.0")));
                            if (encuestaSeccion)
                            {
                                Console.WriteLine("RESPUESTA PARA ENCUESTA");
                                //datosEncuesta.AsignarRespuesta360(pregunta.Id, "14.389.967-5", "10.785.114-3", pregunta.Respuesta360, pregunta.Frecuencia, resultadoRespuesta);
                                datosEncuesta.AgregarRespuestaSeccion(idSeccion,resultadoRespuesta,pregunta.Id,pregunta.idHabilidad,pregunta.TipoHabilidad);
                                Console.WriteLine("RESPUETA: "+pregunta.Respuesta360+" - FRECUENCIA: "+pregunta.Frecuencia);

                                
                            }
                            else
                            {
                                Console.WriteLine("RESPUESTA PARA TRABAJADOR");
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
                                                      
                            Console.WriteLine("ALTERNATIVA: "+pregunta.Respuesta360+" - VALOR: "+valorAlternativa);
                            Console.WriteLine("FRECUENCIA: " + pregunta.Frecuencia + " - VALOR: " + valorFrecuencia);
                           /* foreach (Encuesta.DatosAlternativa alternativa in datosEncuesta.Alternativas(pregunta.Id, "frecuencia"))
                            {
                                if (alternativa.Alternativa.Equals(pregunta.Frecuencia))
                                {
                                    Console.WriteLine("ALTERNATIVA: "+pregunta.Frecuencia+" VALOR:"+alternativa.Valor);
                                    
                                }
                            }*/
                        }
                    }
                    if (encuestaSeccion)/*actualiza los valores*/
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
                        
                       /* VentanaJefeSeccion jefe = new VentanaJefeSeccion();
                        jefe.IdSeccion = idSeccion;
                        jefe.IdAdmin = "Administrador";
                        jefe.Show();*/
                    }
                    else
                        datosEncuesta.ActualizarEstadoEncuestados(idTrabajador, idEvaluador);
                    encuestaLista = true;
                    this.Close();
                    VentanaLogin login = new VentanaLogin();
                    login.Show();
                    
                    
                }
            }
            else if (indice <= (listaPreguntas.Count - 1))
            {
                indice++;
                indiceFrecuencias++;
                textoPregunta.Text = (indice+1)+". "+listaPreguntas[indice].Pregunta;
                habilidad.Text = listaPreguntas[indice].Habilidad;
                tipoHabilidad.Content = TipoHabilidad(listaPreguntas[indice].TipoHabilidad);
                HabilitarPanelRespuestas(listaPreguntas[indice].TipoPregunta);

                Console.WriteLine("INDICE: "+indice+" RESPUEST: "+respuestas.Count);
                if (indice < respuestas.Count - 1)//cuidado
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
            Console.WriteLine("la cantidad PREGUNTAS:"+listaPreguntas.Count+" CANTIDAD RESPUESTAS: "+respuestas.Count);
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
       /* private void AlternativaYaSeleccionada(int alternativa)
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
        }*/
        /// <summary>
        /// Metodo que setea una alternativa seleccionada, ya almacenada, al retroceder
        /// en la lista de frecuencias.
        /// </summary>
        /// <param name="alternativa"></param>
       /* private void FrecuenciaYaSeleccionada(int alternativa)
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
            
        }*/
        


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
            textoPregunta.Text = (indice+1)+". "+listaPreguntas[0].Pregunta;
            tipoHabilidad.Content = TipoHabilidad(listaPreguntas[0].TipoHabilidad);
            habilidad.Text = listaPreguntas[0].Habilidad;
            HabilitarPanelRespuestas(listaPreguntas[0].TipoPregunta);
            GeneraPanelAlternativa(0);
            
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }
        /***********************************************************************
         *                              METODOS 
         * *********************************************************************/
        private void GeneraPanelAlternativa(int index)
        {
            Console.WriteLine("TIPO PREGUNTA: " + listaPreguntas[index].TipoPregunta);
            if (listaPreguntas[index].TipoPregunta.Equals("360"))
            {
                this.panelAlternativas360.Children.Clear();
                this.panelFrecuencias.Children.Clear();
                foreach (Encuesta.DatosAlternativa alternativa in datosEncuesta.Alternativas(listaPreguntas[index].Id, "grado"))
                {
                    AgregarAlternativa360(alternativa.Alternativa);
                }
                foreach (Encuesta.DatosAlternativa alternativa in datosEncuesta.Alternativas(listaPreguntas[index].Id, "frecuencia"))
                {
                    AgregarFrecuencia360(alternativa.Alternativa);
                }
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

            else if (tipoPregunta.Equals("Normal"))
            {
                this.panel360.Visibility = System.Windows.Visibility.Hidden;
                this.panelDatos.Visibility = System.Windows.Visibility.Hidden;
                this.panelNormal.Visibility = System.Windows.Visibility.Visible;
            }
            else if (tipoPregunta.Equals("Ingreso_Datos"))
            {
                this.panel360.Visibility = System.Windows.Visibility.Hidden;
                this.panelDatos.Visibility = System.Windows.Visibility.Visible;
                this.panelNormal.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void AgregarAlternativa360(string nombre)
        {            
            RadioButton alternativa = new RadioButton();
            alternativa.Content = nombre+"XX";
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
            alternativa.Content = nombre + "XX";
            alternativa.Name = nombre.Replace(" ", "_");
            alternativa.Click += SeleccionFrecuencias;
            //alternativa.Width = 370;
            alternativa.Height = 30;
            alternativa.FontSize = 16;
            this.panelFrecuencias.Children.Add(alternativa);
           
            
        }

        /*****************************************************************
         * DESECHADAS
         * ******************************************************************/
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
            if (respuestasSeleccionada && frecuenciaSeleccionada)
                botonDerecho.IsEnabled = true;
        }

        private void AsignarRespuestas()
        {
            Console.WriteLine("LARGO respuestas: " + respuestas.Count);
            int indice = 0;
            foreach (int respuesta in respuestas)
            {
                Estructuras.PreguntasYRespuestas pr = new Estructuras.PreguntasYRespuestas();
                pr.AlternativaRespuesta = "" + respuesta;
                pr.Pregunta = preguntas[indice];
                pr.Frecuencia = "" + frecuencias[indice];
                preguntasRespuestas.Add(pr);
                indice++;
            }
            Console.WriteLine("LARGO respuestas2: " + respuestas.Count + " - " + preguntasRespuestas.Count);
            foreach (Estructuras.PreguntasYRespuestas a in preguntasRespuestas)
            {
                Console.WriteLine("PREGUNTA: " + a.AlternativaRespuesta + " FRECUENCIA: " + a.Frecuencia + " largo: " + preguntasRespuestas.Count);
            }
            Console.WriteLine("LARGO PR: " + preguntasRespuestas.Count);
        }

       
        

      /*  async private void MovimientoDerecha(object sender, EventArgs e)
        {
            botonDerecho.IsEnabled = false;
            frecuenciaSeleccionada = false;
            respuestasSeleccionada = false;
            botonIzquierdo.IsEnabled = true;
            DesbloqueaFrecuencias();
            if (indice <= (listaPreguntas.Count - 2))
            {
                indice++;
                indiceFrecuencias++;
                textoPregunta.Text = (indice + 1) + ". " + listaPreguntas[indice].Pregunta;
                habilidad.Text = listaPreguntas[indice].Habilidad;
                tipoHabilidad.Content = TipoHabilidad(listaPreguntas[indice].TipoHabilidad);
                HabilitarPanelRespuestas(listaPreguntas[indice].TipoPregunta);
                GeneraPanelAlternativa(indice);

                if (indice <= respuestas.Count - 2)
                {   
                    int avance = respuestas[indice];
                    AlternativaYaSeleccionada(avance);
                    
                    int avanceFrecuencias = frecuencias[indice];
                    FrecuenciaYaSeleccionada(avanceFrecuencias);
                }
                else
                    DeshabilitaAlternativas();
            }
            else
            {
                if (listaPreguntas.Count == respuestas.Count)
                {
                    if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.EncuestaFinalizadaGuardarYSalir()))
                    {
                        await cuadroMensajes.EncuestaGuardadaExitosamente();
                       
                        AsignarRespuestas();
                        this.Close();
                    }
                }
            }
        }*/

        public bool EvaluacionSeccion
        {
            get { return encuestaSeccion; }
            set { encuestaSeccion = value; }
        }

        public Perfil PerfilSeccion
        {
            get { return perfilseccion; }
            set { perfilseccion = value; }
        }
    }
}
