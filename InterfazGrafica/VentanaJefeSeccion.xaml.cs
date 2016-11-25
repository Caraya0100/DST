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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;
using System.Windows.Threading;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Forms.Integration;
using DST;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class VentanaJefeSeccion : MetroWindow, INotifyPropertyChanged
    {
        VentanaAsignacionHabilidades asignacionHabilidades;
        Canvas panelHabilidadesSeleccionado;   
        private double  valorCapacidad;
        private bool panelDefaultHB = true;
        private bool panelDefaultHD = true;
        private bool panelDefaultCF = true;
        private bool cambiosPanelHB = false;
        private bool cambiosPanelHD = false;
        private bool cambiosPanelCF = false;
        Dictionary<string, double> diccionarioHabilidades;
        Dictionary<string, double> diccionarioHD;
        Dictionary<string, double> diccionarioHB;
        Dictionary<string, double> diccionarioCF;
        AnimacionScroll animadorRanking;
        AnimacionScroll animadorTrabajadores;
        AnimacionScroll animadorEvaluacion;
        Mensajes        cuadroMensajes;
        /*datos de prueba*/
        DatosDePrueba datosTrabajador; 
        List<Trabajador> trabajadores;
        List<Trabajador> trabajadoresRanking;
        List<string> nombres;//dato de prueba
        List<string> apellidos;//dato de prueba
        List<int> edades;//dato de prueba
        List<string> sexo;//dato de prueba
        List<string> listaHab = new List<string>();//dato de prueba
        List<double> listaPje = new List<double>();//dato de prueba
        

        public VentanaJefeSeccion()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;            
            this.datosTrabajador = new DatosDePrueba();  
            /*Inicializacion de datos de prueba*/
            this.datosTrabajador = new DatosDePrueba();
            nombres     = datosTrabajador.Nombres;
            apellidos   = datosTrabajador.Apellido;
            edades      = datosTrabajador.Edad;
            sexo        = datosTrabajador.Sexo;
            listaPje    = datosTrabajador.PuntajesHB;            

            trabajadores = new List<Trabajador>();
            trabajadoresRanking = new List<Trabajador>();
            IniciarComponentes();
        }
        /// <summary>
        /// Metodo que asigna animacion a los scroll.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventosIniciales(object sender, RoutedEventArgs e)
        {
            animadorTrabajadores.Visualizador   = this.scrollTrabajadores;
            animadorRanking.Visualizador        = this.scrollRanking;
            animadorEvaluacion.Visualizador     = this.scrollEvaluacion;
        }
        /// <summary>
        /// Inicializa los componentes.
        /// </summary>
        private void IniciarComponentes()
        {
            /*inicio diccionarios de prueba*/
            diccionarioHabilidades = new Dictionary<string, double>();
            diccionarioHD = new Dictionary<string, double>();
            diccionarioHB = new Dictionary<string, double>();
            diccionarioCF = new Dictionary<string, double>();

            for (int i = 0; i < datosTrabajador.HabilidadesBlandas.Length; i++)
                diccionarioHB.Add(datosTrabajador.HabilidadesBlandas[i], datosTrabajador.PuntajesGenerales[i]);

            for (int i = 0; i < datosTrabajador.HabilidadesDuras.Length; i++)
                diccionarioHD.Add(datosTrabajador.HabilidadesDuras[i], datosTrabajador.PuntajesGenerales[i]);

            for (int i = 0; i < datosTrabajador.CaracteristicasFisicas.Length; i++)
                diccionarioCF.Add(datosTrabajador.CaracteristicasFisicas[i], datosTrabajador.PuntajesGenerales[i]);
            
            /*Inicia graficos en cero*/
            AsignacionValoresGraficoCircular(0.0);
            /*Interaccion de mensajes a usario*/
            cuadroMensajes = new Mensajes(this);
            /*Inicializa los paneles con la informacion de trabajadores*/
            GeneradorRankingPrueba();
            GeneraListaTrabajadores();
            /*inicializa los movimientos del scroll*/
            animadorRanking         = new AnimacionScroll();
            animadorTrabajadores    = new AnimacionScroll();
            animadorEvaluacion      = new AnimacionScroll();
            
        }
        /// <summary>
        /// Controlador de cierre de sesion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void CerrarSesion(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            deshabilitaEtiquetasRanking();
            deshabilitaEtiquetasTrabajador();
            if (VerificaEstadoPanelHabilidades())
            {
                if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.GuardarCambiosPerfilAlSalir()))
                {
                    Console.WriteLine("cantidad: " + contenedor_CF.Children.Count);
                    /*guardar los cambios en la BD*/
                    foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioHB)
                        Console.WriteLine("HB: "+habilidadPuntaje.Key+" Puntaje: "+habilidadPuntaje.Value);
                    foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioHD)
                        Console.WriteLine("HD: " + habilidadPuntaje.Key + " Puntaje: " + habilidadPuntaje.Value);
                    foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioCF)
                        Console.WriteLine("CF: " + habilidadPuntaje.Key + " Puntaje: " + habilidadPuntaje.Value);

                    Console.WriteLine("HBG"+this.grado_importancia_HB.Content);
                    Console.WriteLine("HDG" + this.grado_importancia_HD.Content);
                    Console.WriteLine("CFG" +this.grado_importancia_CF.Content);
                    /*cierra la aplicacion*/
                    App.Current.Shutdown();
                    
                }
                else
                    App.Current.Shutdown();
            }
            else
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.CerrarSesion()))
            {
                App.Current.Shutdown();
            }
        }
        /// <summary>
        /// Metodo que controla la opcion seleccionada por el usuario, en el panel general.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seleccionItem(object sender, SelectionChangedEventArgs e)
        {
            if (itemTrabajadores.IsSelected)
            {                
                this.hostRanking.Visibility = Visibility.Hidden;
                deshabilitaEtiquetasTrabajador();
                animadorRanking.detenerAnimacionVertical();
                animadorEvaluacion.detenerAnimacionHorizontal();
                animadorTrabajadores.comenzarAnimacionHorizontal();                
            }
            else if (itemEvaluacion.IsSelected)
            {
                deshabilitaEtiquetasRanking();
                deshabilitaEtiquetasTrabajador();
                animadorRanking.detenerAnimacionVertical();
                animadorTrabajadores.detenerAnimacionHorizontal();
                animadorEvaluacion.comenzarAnimacionHorizontal();
                GeneraListaEvaluados();
            }
            else if (itemPerfil.IsSelected)
            {
                deshabilitaEtiquetasRanking();
                deshabilitaEtiquetasTrabajador();
                animadorRanking.detenerAnimacionVertical();
                animadorTrabajadores.detenerAnimacionHorizontal();
                animadorEvaluacion.detenerAnimacionHorizontal();   
            }
            else if (itemRanking.IsSelected)
            {
                this.hostTrabajadores.Visibility = Visibility.Hidden;
                deshabilitaEtiquetasRanking();
                animadorTrabajadores.detenerAnimacionHorizontal();
                animadorEvaluacion.detenerAnimacionHorizontal();
                animadorRanking.comenzarAnimacionVertical();
            }
        }
        /**************************************ITEM TRABAJADORES****************************************/
        /***********************************************************************************************/
        /// <summary>
        /// Metodo que genera la lista de trabajadores contenida en el scroll Trabajadores.
        /// </summary>
        public void GeneraListaTrabajadores()
        {
            this.panelTrabajadores.Children.Clear();            
            int indice = 0;
            foreach (Trabajador datos in datosTrabajador.Trabajadores)
            {
                VisorTrabajador infoTrabajador = new VisorTrabajador(seleccionPanelTrabajador);
                infoTrabajador.Nombre = datos.Nombre;
                infoTrabajador.Apellido = datos.Nombre;
                /*comprobar el sexo*/
                if(sexo[indice].Equals("M"))
                    infoTrabajador.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                else
                    infoTrabajador.DireccionImagen = @"..\..\Iconos\User-Female.png";
                infoTrabajador.IdentificadorPanel = "I" + indice;
                this.panelTrabajadores.Children.Add(infoTrabajador.ConstructorInfo());
                indice++;
            }
        }
        /// <summary>
        /// Controlador que despliega la informacion del trabajador seleccionado, en item Trabajadores.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seleccionPanelTrabajador(object sender, MouseButtonEventArgs e)
        {
            habilitaEtiquetasTrabajador();
            botonEliminarTrabajador.IsEnabled = true;
            botonDetalle.IsEnabled = true;
            this.hostTrabajadores.Visibility = Visibility.Visible;
            System.Windows.Controls.Canvas ver = sender as System.Windows.Controls.Canvas;
            string id = ver.Name as string;

            string[] indiceEtiqueta = id.Split('I');            
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
            /*asignacion de datos del trabajador*/
            this.nombreTrabajador.Content = datosTrabajador.Trabajadores[indice].Nombre;//datos de prueba
            this.edadTrabajador.Content = edades[indice];//datos de prueba
            this.sexoTrabajador.Content = sexo[indice];//datos de prueba
            if(sexo[indice].Equals("M"))
                this.imagenTrabajador.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Iconos\Business-Man.png", UriKind.Relative)));
            else
                this.imagenTrabajador.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Iconos\User-Female.png", UriKind.Relative)));
            /*grafico circular*/
            double doble = new Random().NextDouble();//datos de prueba
            AsignacionValoresGraficoCircular(doble);
            /*Grafico Radar*/
            double[] trabajador = { 20, new Random().Next(60), new Random().Next(70) };//datos de prueba
            double[] seccion = { new Random().Next(100), new Random().Next(200), 50 };//datos de prueba
            string[] habilidades = { "CF", "HB", "HD" };//datos de prueba
            GraficoRadar graficoRadar = new GraficoRadar(habilidades, seccion, trabajador, this.GraficoTrabajadores);
            graficoRadar.TipoGrafico = "Area";
            graficoRadar.constructorGrafico();
        }
        /// <summary>
        /// Metodo que deshabilita las etiquetas de grafico y host WF, en item Trabajador y Ranking.
        /// </summary>
        private void deshabilitaEtiquetasTrabajador()
        {
            AsignacionValoresGraficoCircular(0.0);
            this.hostRanking.Visibility = Visibility.Hidden;
            this.colorSeccion.Visibility = Visibility.Hidden;
            this.colorTrabajador.Visibility = Visibility.Hidden;
            this.etiquetaSeccion.Visibility = Visibility.Hidden;
            this.etiquetaTrabajador.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Metodo que habilita las etiquetas de grafico y host WF, en item Trabajador y Ranking.
        /// </summary>
        private void habilitaEtiquetasTrabajador()
        {
            this.hostRanking.Visibility = Visibility.Visible;
            this.colorSeccion.Visibility = Visibility.Visible;
            this.colorTrabajador.Visibility = Visibility.Visible;
            this.etiquetaSeccion.Visibility = Visibility.Visible;
            this.etiquetaTrabajador.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Controlador de movimiento del scroll de Trabajadores (detiene movimiento horizontal).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntroScrollTrabajador(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorTrabajadores.detenerAnimacionHorizontal();
        }
        /// <summary>
        /// Controlador de movimiento del scroll de Trabajadores (comienza movimiento horizontal).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DejoScrollTrabajador(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorTrabajadores.comenzarAnimacionHorizontal();
        }
        /// <summary>
        /// Controlador que genera movimiento manual mediante Button (boton circular derecho), en scroll trabajadores.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoMovimientoDerecha(object sender, EventArgs e)
        {
            int estado_avance = Convert.ToInt32(scrollTrabajadores.HorizontalOffset);
            double aumenta_espacio = (double)estado_avance + 40.0;
            animadorTrabajadores.detenerAnimacionHorizontal();
            animadorTrabajadores.Contador = (int)aumenta_espacio;
            scrollTrabajadores.ScrollToHorizontalOffset(aumenta_espacio);
        }
        /// <summary>
        /// Controlador que genera movimiento manual mediante Button (boton circular izquierdo), en scroll trabajadores.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoMovimientoIzquierda(object sender, EventArgs e)
        {
            int estado_avance = Convert.ToInt32(scrollTrabajadores.HorizontalOffset);
            double aumenta_espacio = (double)estado_avance - 40.0;
            animadorTrabajadores.detenerAnimacionHorizontal();
            animadorTrabajadores.Contador = (int)aumenta_espacio;
            scrollTrabajadores.ScrollToHorizontalOffset(aumenta_espacio);
        }
        /// <summary>
        /// Controlador de movimiento de scroll trabajadores, se acciona al dejar el boton circular derecho.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoDejaBotonMovDerecho(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorTrabajadores.detenerAnimacionHorizontal();
            animadorTrabajadores.comenzarAnimacionHorizontal();
        }
        /// <summary>
        /// Controlador de movimiento de scroll trabajadores, se acciona al dejar el boton circular derecho.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoDejaBotonMovIzquierdo(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorTrabajadores.detenerAnimacionHorizontal();
            animadorTrabajadores.comenzarAnimacionHorizontal();
        }
        /// <summary>
        /// Controlador que al accionarse elimina los datos del trabajador seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void eventoEliminarTrabajador(object sender, RoutedEventArgs e)
        {
            this.hostTrabajadores.Visibility = Visibility.Hidden;
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.ConsultaEliminarTrabajador()))
            {
                cuadroMensajes.TrabajadorEliminado();
                nombreTrabajador.Content = "";
                edadTrabajador.Content = "";
                sexoTrabajador.Content = "";
                deshabilitaEtiquetasTrabajador();
                /*grafico circular*/
                AsignacionValoresGraficoCircular(0.0);
            }
            else { this.hostTrabajadores.Visibility = Visibility.Visible; }

        }
        /// <summary>
        /// Controlador que al accionarse solicita la reubicacion del trabajador seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void solicitaReubicacion(object sender, RoutedEventArgs e)
        {
            this.hostRanking.Visibility = Visibility.Hidden;
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.ConsultaSolicitudTrabajador()))
            {
                cuadroMensajes.TrabajadorSolicitado();
            }
            else { this.hostRanking.Visibility = Visibility.Visible; }
        }
        /// <summary>
        /// Controlador que despliega una ventana(VentanaAgregarTrabajador) para el ingreso de un trabajador nuevo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoAgregaTrabajador(object sender, RoutedEventArgs e)
        {
            VentanaAgregarTrabajador nuevoTrabajador = new VentanaAgregarTrabajador();
            nuevoTrabajador.ShowDialog();
        }
        /// <summary>
        /// Controlador que al accionarse despliega una ventana con el detalle de calificacion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detalleTrabajador(object sender, RoutedEventArgs e)
        {
            VentanaDetalleHabilidades detalleHabilidades = new VentanaDetalleHabilidades();
            detalleHabilidades.ShowDialog();
        }


        /**************************************ITEM CONFIGURACION**************************************/
        /***********************************************************************************************/
        /// <summary>
        /// Metodo que genera el panel de habilidades para la asignacion de importancia de manera manual.
        /// </summary>
        /// <param name="contenedorSeleccionado"></param>
        public void GeneradorPanelHabilidades(Canvas contenedorSeleccionado)
        {
            int indice = 0;
            foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioHabilidades)
            {
                PanelHabilidades panelHabilidades = new PanelHabilidades();

                //panelHabilidades.Key                        = habilidadPuntaje.Key;
                panelHabilidades.HabilidadName              = "I"+indice+"0";
                panelHabilidades.Habilidad                  = habilidadPuntaje.Key; //datos de prueba                
                panelHabilidades.GradoImportancia           = habilidadPuntaje.Value;//datos de prueba
                panelHabilidades.GradoImportanciaEtiqueta   = "" + habilidadPuntaje.Value;//datos de prueba
                panelHabilidades.GradoImportanciaIdentificador          = "I" + indice;
                panelHabilidades.GradoImportanciaEtiquetaIdentificador  = "I" + indice + "2";
                panelHabilidades.Controlador = AsignacionValorHabilidad;
                contenedorSeleccionado.Children.Add(panelHabilidades.ConstructorPanel(indice));
                contenedorSeleccionado.Children.Add(panelHabilidades.Delimitador);
                indice++;
            }
        }
        /// <summary>
        /// Metodo que ajusta el tamaño del panel seleccionado (habilidades).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoTamanioCanvas(object sender, SizeChangedEventArgs e)
        {
            try { this.panelHabilidadesSeleccionado.Height = 105 * diccionarioHabilidades.Count; }
            catch { }
            try { this.panelHabilidadesSeleccionado.Width = 678; }
            catch { }
        }
        /// <summary>
        /// Controlador que asigna el valor a la habilidad mediante slider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AsignacionValorHabilidad(object sender, RoutedPropertyChangedEventArgs<double> e)
        {            
            var slider              = sender as Slider;
            double value            = slider.Value;
            string identificador    = slider.Name;
            System.Windows.Controls.Label grado_importancia_numerico = new System.Windows.Controls.Label();
            System.Windows.Controls.TextBlock habilidad = new System.Windows.Controls.TextBlock();
            try { 
                grado_importancia_numerico =    (System.Windows.Controls.Label)
                                                LogicalTreeHelper.FindLogicalNode(
                                                panelHabilidadesSeleccionado, 
                                                identificador + "2");
                habilidad = (System.Windows.Controls.TextBlock)
                                                LogicalTreeHelper.FindLogicalNode(
                                                panelHabilidadesSeleccionado,
                                                identificador + "0"); 
                ActualizaEstadoDePanelHabilidades();    
                /*actualizar el diccionario*/
                
                
                }
            catch { }
            if (grado_importancia_numerico != null)
            {
                grado_importancia_numerico.Content = "" + value.ToString("0.0");
                /*identificar diccionario y lo modifica*/
                if (panelHabilidadesSeleccionado.Name.Equals("contenedor_HB"))
                    diccionarioHB[habilidad.Text] = Convert.ToDouble(value.ToString("0.0"));
                else if (panelHabilidadesSeleccionado.Name.Equals("contenedor_HD"))
                    diccionarioHD[habilidad.Text] = Convert.ToDouble(value.ToString("0.0"));
                else if (panelHabilidadesSeleccionado.Name.Equals("contenedor_CF"))
                    diccionarioCF[habilidad.Text] = Convert.ToDouble(value.ToString("0.0"));
            }
        }
        /// <summary>
        /// Metodo que asigna grado de importancia a las HB, HD y CF de manera manual, mediante slider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void asignacionValorHabilidadGeneral(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;

            double value = slider.Value;
            string identificador = slider.Name;

            if (identificador.Equals("slider_HD"))
                grado_importancia_HD.Content = "" + value.ToString("0.0");

            else if (identificador.Equals("slider_HB"))
                grado_importancia_HB.Content = "" + value.ToString("0.0");

            else if (identificador.Equals("slider_CF"))
                grado_importancia_CF.Content = "" + value.ToString("0.0");
        }
        /// <summary>
        /// Controlador que se acciona al seleccionar el panel de configuracion de cada habilidad (HB, HD y CF).        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoTabHabilidades(object sender, SelectionChangedEventArgs e)
        {
            if (pestania_HB.IsSelected)
            {
                panelHabilidadesSeleccionado = contenedor_HB;
                if (panelDefaultHB)
                {                    
                    diccionarioHabilidades.Clear();
                    diccionarioHabilidades = Diccionario(diccionarioHB);  
                    contenedor_HB.Children.Clear();
                    GeneradorPanelHabilidades(contenedor_HB);
                    panelDefaultHB = false;
                }
                
            }
            else if (pestania_HD.IsSelected)
            {
                panelHabilidadesSeleccionado = contenedor_HD;
                if (panelDefaultHD)
                {
                    diccionarioHabilidades.Clear();
                    diccionarioHabilidades = Diccionario(diccionarioHD);
                    contenedor_HD.Children.Clear();
                    GeneradorPanelHabilidades(contenedor_HD);
                    panelDefaultHD = false;
                }                
            }
            else if (pestania_CF.IsSelected)
            {
                panelHabilidadesSeleccionado = contenedor_CF;
                if (panelDefaultCF)
                {
                    diccionarioHabilidades.Clear();
                    diccionarioHabilidades = Diccionario(diccionarioCF);
                    contenedor_CF.Children.Clear();
                    GeneradorPanelHabilidades(contenedor_CF);
                    panelDefaultCF = false;
                }
            }
        }
        /// <summary>
        /// Controlador que acciona la ventana para habilitar o deshabilitar las habilidades disponibles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoIrAConfiguracion(object sender, RoutedEventArgs e)
        {
            asignacionHabilidades = new VentanaAsignacionHabilidades();
            asignacionHabilidades.ShowDialog();
        }

        /******************************************ITEM RANKING*****************************************/
        /***********************************************************************************************/
        /// <summary>
        /// Controlador que se acciona al seleccionar un trabajador en el scroll Ranking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventoSeleccionTrabajadorRanking(object sender, EventArgs e)
        {
            habilitaEtiquetasRanking();
            detalleRanking.IsEnabled = true;
            solicitaReubicacionRanking.IsEnabled = true;
            /*identifiacacion de objeto*/
            System.Windows.Controls.Button ver = sender as System.Windows.Controls.Button;
            string id = ver.Name as string;
            string[] indiceEtiqueta = id.Split('I');
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
            /*asigna datos del trabajador*/
            nombreRanking.Text = nombres[indice] + " " + apellidos[indice];//dato de prueba
            edadRanking.Text = "" + edades[indice];//dato de prueba
            sexoRanking.Text = sexo[indice];//dato de prueba
            seccionRanking.Text = "Atención Clientes";//dato de prueba
            /*grafico circular*/
            double doble = new Random().NextDouble(); //dato de prueba
            AsignacionValoresGraficoCircular(doble);
            /*Grafico araña*/
            double[] trabajador = { 20, new Random().Next(60), new Random().Next(70) };//dato de prueba
            double[] seccion = { new Random().Next(100), new Random().Next(200), 50 };//dato de prueba
            string[] habilidades = { "CF", "HD", "HB" };//dato de prueba          
            GraficoRadar graficoRadar = new GraficoRadar(habilidades, seccion, trabajador, this.graficoRadarRanking);
            graficoRadar.TipoGrafico = "Area";
            graficoRadar.constructorGrafico();
        }
        /// <summary>
        /// Controlador que despliega una Ventana con el detalle del trabajador seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetalleRanking(object sender, EventArgs e)
        {
            VentanaDetalleHabilidades detalleHabilidades = new VentanaDetalleHabilidades();
            detalleHabilidades.ShowDialog();
        }
        /// <summary>
        /// Metodo que genera la lista de trabajadores que contiene el scroll Ranking.
        /// </summary>
        public void GeneradorRankingPrueba()
        {
            panel_principal.Children.Clear();
            for (int i = 0; i < 20; i++)
            {
                VisorRanking infoTrabajador = new VisorRanking(EventoSeleccionTrabajadorRanking);
                infoTrabajador.Nombre = nombres[i];//datos de prueba
                infoTrabajador.Apellido = apellidos[i];//datos de prueba
                infoTrabajador.Seccion = "Atención Clientes";//datos de prueba
                infoTrabajador.PosicionRanking = "" + (i + 1);
                infoTrabajador.BotonVer = "I" + i;
                infoTrabajador.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                this.panel_principal.Children.Add(infoTrabajador.ConstructorInfo());
            }
        }

        private void MovimientoArribaRanking(object sender, EventArgs e)
        {
            int estado_avance = Convert.ToInt32(scrollRanking.VerticalOffset);
            double aumenta_espacio = (double)estado_avance + 40.0;
            animadorRanking.detenerAnimacionHorizontal();
            animadorRanking.Contador = (int)aumenta_espacio;
            scrollRanking.ScrollToVerticalOffset(aumenta_espacio);
        }

        private void MovimientoAbajoRanking(object sender, EventArgs e)
        {
            int estado_avance = Convert.ToInt32(scrollRanking.VerticalOffset);
            double aumenta_espacio = (double)estado_avance - 40.0;
            animadorRanking.detenerAnimacionHorizontal();
            animadorRanking.Contador = (int)aumenta_espacio;
            scrollRanking.ScrollToVerticalOffset(aumenta_espacio);
        }

        private void EntroScrollRanking(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorRanking.detenerAnimacionVertical();
        }

        private void DejoScrollRanking(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorRanking.comenzarAnimacionVertical();
        }

        /// <summary>
        /// Metodo que deshabilita las etiquetas de grafico y host WF, en item Ranking y Trabajador.
        /// </summary>
        private void deshabilitaEtiquetasRanking()
        {
            AsignacionValoresGraficoCircular(0.0);
            this.hostTrabajadores.Visibility = Visibility.Hidden;
            this.etiquetaTrabajadorRanking.Visibility = Visibility.Hidden;
            this.etiquetaSeccionRanking.Visibility = Visibility.Hidden;
            this.colorSeccionRanking.Visibility = Visibility.Hidden;
            this.colorTrabajadorRanking.Visibility = Visibility.Hidden;
        }
        /// <summary>
        /// Metodo que habilita las etiqutas de grafico y host WF, en item Ranking y Trabajador.
        /// </summary>
        private void habilitaEtiquetasRanking()
        {
            this.hostRanking.Visibility = Visibility.Visible;
            this.etiquetaTrabajadorRanking.Visibility = Visibility.Visible;
            this.etiquetaSeccionRanking.Visibility = Visibility.Visible;
            this.colorSeccionRanking.Visibility = Visibility.Visible;
            this.colorTrabajadorRanking.Visibility = Visibility.Visible;
        }

        /***************************************ITEM EVALUACION*****************************************/
        /***********************************************************************************************/
        /// <summary>
        /// Metodo que genera la lista de trabajadores que responderan la encuesta.
        /// </summary>
        private void GeneraListaEvaluados()
        {
            this.panelEvaluacion.Children.Clear();
            for (int i = 0; i < 20; i++)
            {
                VisorEvaluacion trabajador = new VisorEvaluacion(seleccionPanelEvaluacion);
                trabajador.Nombre = nombres[i];//datos de prueba
                trabajador.Apellido = apellidos[i];//datos de prueba
                trabajador.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                trabajador.DireccionEstado = @"..\..\Iconos\encuestaRealizada.png";
                trabajador.IdentificadorPanel = "I" + i;
                this.panelEvaluacion.Children.Add(trabajador.ConstructorInfo());
            }
        }
        /// <summary>
        /// Controlador que al accionarse indica en el panel, el nombre del trabajador a evaluar.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seleccionPanelEvaluacion(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Controls.Canvas ver = sender as System.Windows.Controls.Canvas;
            string id = ver.Name as string;

            string[] indiceEtiqueta = id.Split('I');
            int indice = Convert.ToInt32(indiceEtiqueta[1]);

            nombreEvaluado.Content = nombres[indice] + " " + apellidos[indice];//datos de prueba
        }
        /// <summary>
        /// Controlador que mueve los elementos contenidos en el scroll hacia la derecha.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovimientoDerechaEvaluacion(object sender, EventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollEvaluacion.HorizontalOffset);
            double aumentaEspacio = (double)estadoAvance + 40.0;
            animadorEvaluacion.detenerAnimacionHorizontal();
            animadorEvaluacion.Contador = (int)aumentaEspacio;
            scrollEvaluacion.ScrollToHorizontalOffset(aumentaEspacio);
        }
        /// <summary>
        /// Controlador que mueve los elementos contenidos en el scroll hacia la izquierda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovimientoIzquierdaEvaluacion(object sender, EventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollEvaluacion.HorizontalOffset);
            double aumentaEspacio = (double)estadoAvance - 40.0;
            animadorEvaluacion.detenerAnimacionHorizontal();
            animadorEvaluacion.Contador = (int)aumentaEspacio;
            scrollEvaluacion.ScrollToHorizontalOffset(aumentaEspacio);
        }
        /// <summary>
        /// Controlador de movimiento de scroll trabajadores, se acciona al dejar el boton circular derecho.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DejaBotonMovimientoEvaluacion(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorEvaluacion.detenerAnimacionHorizontal();
            animadorEvaluacion.comenzarAnimacionHorizontal();
        }
        /// <summary>
        /// Metodo que verifca la pass en el item evaluacion, luego de responder la encuesta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void verificarPassword(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (password.Password.Equals("clave"))//clave de prueba
                {
                    GeneraListaEvaluados();
                }
                else
                    cuadroMensajes.ClaveIncorrecta();
            }
        }
        /// <summary>
        /// Controlador que se acciona al seleccionar la opcion de trabajador para evaluar, depliega una ventana
        /// con la lista de posibles evaluadores.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionEncuestaTrabajador(object sender, RoutedEventArgs e)
        {
            if (nombreEvaluado.Content.Equals(""))
                cuadroMensajes.TrabajadorNoSeleccionado();
            else
            {
                /*pasar el id del trabajador seleccionado*/
                VentanaEvaluadores evaluadores = new VentanaEvaluadores();
                evaluadores.ShowDialog();
            }
        }
        /// <summary>
        /// Controlador que se acciona al seleccionar al jefe de seccion como evaluador, despliega la ventana de encuesta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionEncuestaJefeSeccion(object sender, RoutedEventArgs e)
        {
            if (nombreEvaluado.Content.Equals(""))
                cuadroMensajes.TrabajadorNoSeleccionado();
            else
            {
                //int indice = IdentificaTrabajador(sender); IDENTIFICA  
                VentanaEncuesta encuesta = new VentanaEncuesta();
                encuesta.Preguntas = datosTrabajador.Preguntas;
                encuesta.NombreTrabajador = nombres[0];
                encuesta.InicioEncuesta();
                encuesta.ShowDialog();
            }
        }
        /// <summary>
        /// Controlador que se acciona al entrar al scroll de Evaluacion. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EntroScrollEvaluacion(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorEvaluacion.detenerAnimacionHorizontal();
        }
        /// <summary>
        /// Controlador que se acciona al dejar el scroll de Evaluacion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DejoScrollEvaluacion(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorEvaluacion.comenzarAnimacionHorizontal();
        }

        /****************************METODOS ASOCIADOS A GRAFICOS****************************************/
        /***********************************************************************************************/
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Metodo que genera el formato del grafico (%).
        /// </summary>  
        public Func<double, string> Formato { get; set; }
        /// <summary>
        /// Metodo Asociado al Grafico Circular, actuliza los valores.
        /// </summary>
        public double ValorGraficoCircular
        {
            get { return valorCapacidad; }
            set
            {
                valorCapacidad = value;
                OnPropertyChanged("ValorGraficoCircular");
            }
        }
        /// <summary>
        /// Metodo asociado al Grafico circular.
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Asigna y actuliza el valor de los graficos Ranking y Trabajadores.
        /// </summary>
        /// <param name="doble"></param>
        private void AsignacionValoresGraficoCircular(double doble)
        {
            ValorGraficoCircular = doble;
            Formato = x => x.ToString("P");
            DataContext = this;
        }

        private int IdentificaTrabajador(Object sender)
        {
            System.Windows.Controls.Canvas ver = sender as System.Windows.Controls.Canvas;
            string id = ver.Name as string;

            string[] indiceEtiqueta = id.Split('I');
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
            return indice;
        }

        /**pruebas**/
        public List<Trabajador> Trabajadores
        {
            get { return trabajadores; }
            set { trabajadores = value; }
        }

        public void Visible()
        {
            this.ShowDialog();
        }

        async private void GuardarCambiosPerfil(object sender, RoutedEventArgs e)
        {
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.GuardarCambiosPerfil()))
            {
                /*guardar cambios en la base de datos*/
            }

        }

        private Dictionary<string, double> Diccionario( Dictionary<string, double> diccionario)
        {
            Dictionary<string, double> nuevoDiccionario = new Dictionary<string, double>();
            foreach (KeyValuePair<string, double> habilidades in diccionario)
            {
                nuevoDiccionario.Add(habilidades.Key, habilidades.Value);
            }
            return nuevoDiccionario;
        }
        /// <summary>
        /// Captura si se realizo algun cambio en los paneles de perfil
        /// </summary>
        private void ActualizaEstadoDePanelHabilidades()
        {
            if (panelHabilidadesSeleccionado.Name.Equals("contenedor_HB"))
                cambiosPanelHB = true;
            else if (panelHabilidadesSeleccionado.Name.Equals("contenedor_HD"))
                cambiosPanelHD = true;
            else if (panelHabilidadesSeleccionado.Name.Equals("contenedor_CF"))
                cambiosPanelCF = true;
        }
        /// <summary>
        /// Verifica si se ha realizado cambios en los paneles de perfil
        /// </summary>
        /// <returns></returns>
        private bool VerificaEstadoPanelHabilidades()
        {
            if (cambiosPanelHB || cambiosPanelHD || cambiosPanelCF)
                return true;
            else return false;
        }
    }
}
