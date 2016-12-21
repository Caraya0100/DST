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
using System.IO;//reporte
using iTextSharp.text.pdf;//reporte
using iTextSharp.text;//reporte

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class VentanaJefeSeccion : MetroWindow, INotifyPropertyChanged
    {
        VentanaAsignacionHabilidades asignacionHabilidades;
        StackPanel panelHabilidadesSeleccionado;   
        private double  valorCapacidad;        
        private bool cambiosPanelHB = false;
        private bool cambiosPanelHD = false;
        private bool cambiosPanelCF = false;
        Dictionary<string, double> diccionarioHabilidades;
        Dictionary<string, double> diccionarioHD;
        Dictionary<string, double> diccionarioHB;        
        Dictionary<string, double> diccionarioCF;
        List<Perfil> habilidadesPerfilActulizado;
        AnimacionScroll animadorRanking;
        AnimacionScroll animadorTrabajadores;
        AnimacionScroll animadorEvaluacion;
        Mensajes        cuadroMensajes;
        /*datos de prueba*/
        DatosDePrueba datosTrabajador; 
        List<Trabajador> trabajadores;
        List<Trabajador> trabajadoresRanking;
        List<string> nombres;//dato de prueba      
        Reportes.ReportesTrabajador reporte;
        /*interaccion con BD*/
        private InteraccionBD.InteraccionTrabajadores datosTrabajadores;
        private InteraccionBD.InteraccionSecciones datosSeccion;
        private InteraccionBD.InteraccionSolicitudes datosSolicitudes;
        private InteraccionBD.InteraccionEncuesta datosEncuesta;
        private InteraccionBD.InteraccionDesempeno datosDesempeno;
        /*variables*/
        private int idSeccion;
        private string idJefeSeccion;
        //private string nombreSeccion;
        //private string nombreJefeSeccion;
        private Dictionary<string, Trabajador> listaTrabajadores;
        /*estructuras con puntajes y habilidades*/
        Perfil perfilSeccionActual;
        List<string> HB = new List<string>();
        List<string> HD = new List<string>();
        List<string> CF = new List<string>();
        List<double> HBPuntajesTrabajador = new List<double>();
        List<double> HDPuntajesTrabajador = new List<double>();
        List<double> CFPuntajesTrabajador = new List<double>();
        List<double> HBPuntajesSeccion = new List<double>();
        List<double> HDPuntajesSeccion = new List<double>();
        List<double> CFPuntajesSeccion = new List<double>();
        List<double> puntajesGeneralesSeccion = new List<double>();
        List<double> puntajesGeneralesTrabajador = new List<double>();
        /*estructuras ranking*/
        List<Perfil> perfilRanking;
        List<Trabajador> trabajadorRanking;
        string trabajadorSeleccionado;
        string rutTrabajadorRanking;
        /*configuracion perfil*/
        string tipoHabilidad;
        /*evaluacion*/
        string rutEvaluado;
        List<string> rutEvaluados;
       /*General*/
        string itemReporte;
        Trabajador trabajadorActual;
        string rutAdministrador;
        

        public VentanaJefeSeccion()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;            
            this.datosTrabajador = new DatosDePrueba();  
            /*Inicializacion de datos de prueba*/
            this.datosTrabajador = new DatosDePrueba();
            nombres     = datosTrabajador.Nombres;                       

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
            /**/
            panelHabilidadesSeleccionado = contenedor_HB;
            /*datos trabajadores*/
            datosTrabajadores = new InteraccionBD.InteraccionTrabajadores();
            datosSeccion = new InteraccionBD.InteraccionSecciones();
            datosSolicitudes = new InteraccionBD.InteraccionSolicitudes();
            datosEncuesta = new InteraccionBD.InteraccionEncuesta();
            datosDesempeno = new InteraccionBD.InteraccionDesempeno();
            /*datos ranking*/
            perfilRanking = new List<Perfil>(); ;
            trabajadorRanking = new List<Trabajador>(); ;


            /*inicio diccionarios de prueba*/
            diccionarioHabilidades = new Dictionary<string, double>();
            diccionarioHD = new Dictionary<string, double>();
            diccionarioHB = new Dictionary<string, double>();
            diccionarioCF = new Dictionary<string, double>();
          
            
            /*Inicia graficos en cero*/
            AsignacionValoresGraficoCircular(0.0);
            /*Interaccion de mensajes a usario*/
            cuadroMensajes = new Mensajes(this);     
           
            /*inicializa los movimientos del scroll*/
            animadorRanking         = new AnimacionScroll();
            animadorTrabajadores    = new AnimacionScroll();
            animadorEvaluacion      = new AnimacionScroll();
            /*especificacion de seccion*/
            datosTrabajadores.IdSeccion = idSeccion;   
            /*evaluacion*/
            rutEvaluados = new List<string>();
            itemReporte = "Trabajadores";
            idJefeSeccion = string.Empty;
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
                    {
                        Console.WriteLine("HB: " + habilidadPuntaje.Key + " Puntaje: " + habilidadPuntaje.Value);
                        datosSeccion.ActualizacionImportanciasHabilidades(habilidadPuntaje.Key, habilidadPuntaje.Value);
                    }

                    foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioHD)
                    {
                        Console.WriteLine("HD: " + habilidadPuntaje.Key + " Puntaje: " + habilidadPuntaje.Value);
                        datosSeccion.ActualizacionImportanciasHabilidades(habilidadPuntaje.Key, habilidadPuntaje.Value);
                    }

                    foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioCF)
                    {
                        Console.WriteLine("CF: " + habilidadPuntaje.Key + " Puntaje: " + habilidadPuntaje.Value);
                        datosSeccion.ActualizacionImportanciasHabilidades(habilidadPuntaje.Key, habilidadPuntaje.Value);
                    }
                    
                    datosSeccion.ActualizacionImportanciasHabilidades("CF", Convert.ToDouble(this.slider_CF.Value.ToString("0.0")));
                    datosSeccion.ActualizacionImportanciasHabilidades("HD", Convert.ToDouble(this.slider_HD.Value.ToString("0.0")));
                    datosSeccion.ActualizacionImportanciasHabilidades("HB", Convert.ToDouble(this.slider_HB.Value.ToString("0.0")));                
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

        private void GeneracionReportes(object sender, RoutedEventArgs e)
        {
            if (itemReporte.Equals("Trabajadores"))
            {
                GenerarReporteTrabajador();
            }
            else if (itemReporte.Equals("Evaluacion"))
            {

            }
            else if (itemReporte.Equals("Perfil"))
            {

            }
            else if(itemReporte.Equals("Ranking"))
            {

            }
        }
        /// <summary>
        /// Metodo que controla la opcion seleccionada por el usuario, en el panel general.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seleccionItem(object sender, SelectionChangedEventArgs e)
        {
            e.Handled = true;
            var tabElegido = sender as System.Windows.Controls.TabControl;
            if (e.OriginalSource == this.tabGeneral)
            {                
                if (itemTrabajadores.IsSelected)
                {
                    IniciarDatosSeccion();
                    GeneraListaTrabajadores();
                    this.hostRanking.Visibility = Visibility.Hidden;
                    deshabilitaEtiquetasTrabajador();
                    animadorRanking.detenerAnimacionVertical();
                    animadorEvaluacion.detenerAnimacionHorizontal();
                    animadorTrabajadores.comenzarAnimacionHorizontal();
                    itemReporte = "Trabajadores";
                }
                else if (itemEvaluacion.IsSelected)
                {
                    deshabilitaEtiquetasRanking();
                    deshabilitaEtiquetasTrabajador();
                    animadorRanking.detenerAnimacionVertical();
                    animadorTrabajadores.detenerAnimacionHorizontal();
                    animadorEvaluacion.comenzarAnimacionHorizontal();
                    GeneraListaEvaluados();
                    itemReporte = "Evaluacion";
                    
                }
                else if (itemPerfil.IsSelected)
                {                   
                    PanelesVacios();
                    GeneradorPanelHabilidadesGenerales();
                    GeneradorPanelHabilidades();
                    deshabilitaEtiquetasRanking();
                    deshabilitaEtiquetasTrabajador();
                    animadorRanking.detenerAnimacionVertical();
                    animadorTrabajadores.detenerAnimacionHorizontal();
                    animadorEvaluacion.detenerAnimacionHorizontal();
                    itemReporte = "Perfil";
                }
                else if (itemRanking.IsSelected)
                {
                    GeneradorRankingPrueba();
                    this.hostTrabajadores.Visibility = Visibility.Hidden;
                    deshabilitaEtiquetasRanking();
                    animadorTrabajadores.detenerAnimacionHorizontal();
                    animadorEvaluacion.detenerAnimacionHorizontal();
                    animadorRanking.comenzarAnimacionVertical();
                    itemReporte = "Ranking";
                }
            }
            
        }

        private void VolverVentanaAdministrador(object sender, RoutedEventArgs e)
        {
            VentanaAdministrador admin = new VentanaAdministrador();
            this.Hide();
            admin.Show();
            
        }
        /***********************************************************************************************
         *                                      ITEM TRABAJADORES
        /***********************************************************************************************/
        public void IniciarDatosSeccion()
        {
            if (!nombreJefeSeccion.Content.Equals("Administrador"))
            {               
                datosSeccion.RutJefeSeccion = idJefeSeccion;
                seccion.Content = datosSeccion.NombreSeccionPorRutJefe();
                idSeccion = datosSeccion.IdSeccionPorRutJefeSeccion();
                datosTrabajadores.IdSeccion = idSeccion;
                listaTrabajadores = datosTrabajadores.TrabajadoresSeccion();
            }
        }
        /// <summary>
        /// Metodo que genera la lista de trabajadores contenida en el scroll Trabajadores.
        /// </summary>
        public void GeneraListaTrabajadores()
        {
            this.panelTrabajadores.Children.Clear();            
            int indice = 0; 
            foreach (KeyValuePair<string, Trabajador> trabajador in listaTrabajadores)
            {                
                VisorTrabajador infoTrabajador = new VisorTrabajador(seleccionPanelTrabajador);
                infoTrabajador.Nombre = trabajador.Value.Nombre;
                infoTrabajador.Apellido = trabajador.Value.ApellidoPaterno;                
                if(trabajador.Value.Sexo.Equals("Masculino"))
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
            PuntajesEnCero();
            habilitaEtiquetasTrabajador();
            botonEliminarTrabajador.IsEnabled = true;
            botonDetalle.IsEnabled = true;
            botonEditar.IsEnabled = true;
            this.hostTrabajadores.Visibility = Visibility.Visible;
            System.Windows.Controls.Canvas ver = sender as System.Windows.Controls.Canvas;
            string id = ver.Name as string;

            string[] indiceEtiqueta = id.Split('I');            
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
            //trabajadorSeleccionado = indice;
            int identificador = 0;
            /*puntajes de seccion por habilidad*/
            datosSeccion.IdSeccion = idSeccion;
            perfilSeccionActual = datosSeccion.PerfilSeccion();
            Dictionary<string, Componente> habilidadesPerfil = perfilSeccionActual.Blandas;

            foreach (KeyValuePair<string, Trabajador> infoTrabajador in listaTrabajadores)
            {                
                if (indice == identificador)
                {
                    trabajadorActual = infoTrabajador.Value;
                    this.nombreTrabajador.Content = infoTrabajador.Value.Nombre +" "+infoTrabajador.Value.ApellidoPaterno;                   
                    this.sexoTrabajador.Content = infoTrabajador.Value.Sexo;
                    if(infoTrabajador.Value.Sexo.Equals("Masculino"))
                        this.imagenTrabajador.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Iconos\Business-Man.png", UriKind.Relative)));
                    else
                        this.imagenTrabajador.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Iconos\User-Female.png", UriKind.Relative)));                                     
                    
                    this.edadTrabajador.Content = CalcularEdad(infoTrabajador.Value.FechaNacimiento);
                    /*grafico*/
                    
                    
                    foreach(KeyValuePair<string, Componente> habilidadBlanda in infoTrabajador.Value.Perfil.Blandas)
                    {
                        foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                        {
                            if (habilidadPerfil.Key.Equals(habilidadBlanda.Key))
                            {
                                HB.Add(habilidadBlanda.Value.Nombre);
                                HBPuntajesTrabajador.Add(habilidadBlanda.Value.Puntaje);
                                Console.WriteLine("KEY: " + habilidadBlanda.Key + " - KEY PERFIL: " + habilidadPerfil.Key);
                            }
                        } 
                    }
                    habilidadesPerfil = perfilSeccionActual.Duras;
                    foreach (KeyValuePair<string, Componente> habilidadDura in infoTrabajador.Value.Perfil.Duras)
                    {
                        foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                        {
                            if (habilidadPerfil.Key.Equals(habilidadDura.Key))
                            {
                                HD.Add(habilidadDura.Value.Nombre);
                                HDPuntajesTrabajador.Add(habilidadDura.Value.Puntaje);
                                //Console.WriteLine("HABILIADDES QUE NO SE MUESTRAN1: "+habilidadDura.Key+"  "+habilidadPerfil.Key);
                            }
                            //Console.WriteLine("HABILIADDES QUE NO SE MUESTRAN2: " + habilidadDura.Value.Nombre);
                        } //Console.WriteLine("HABILIADDES QUE NO SE MUESTRAN3: " + habilidadDura.Value.Nombre);                                                                   
                    }
                    habilidadesPerfil = perfilSeccionActual.Fisicas;
                    foreach (KeyValuePair<string, Componente> caractFisica in infoTrabajador.Value.Perfil.Fisicas)
                    {
                        foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                        {
                            if (habilidadPerfil.Key.Equals(caractFisica.Key))
                            {
                                CF.Add(caractFisica.Value.Nombre);
                                CFPuntajesTrabajador.Add(caractFisica.Value.Puntaje);  
                            }
                        }                                              
                    }
                    Console.WriteLine("las habiliades: "+CF.Count+" los pjes: "+CFPuntajesTrabajador.Count);
                    Console.WriteLine("las habiliades: " + HB.Count + " los pjes: " + HBPuntajesTrabajador.Count);
                    Console.WriteLine("las habiliades: " + HD.Count + " los pjes: " + HDPuntajesTrabajador.Count);
                    datosSeccion.IdSeccion = idSeccion;
                    trabajadorSeleccionado = infoTrabajador.Value.Rut;//para eliminar/editar
                    datosTrabajadores.IdTrabajador = trabajadorSeleccionado;
                    datosDesempeno.IdTrabajador = trabajadorSeleccionado;
                    datosTrabajadores.IdSeccion = IdSeccion;

                    puntajesGeneralesSeccion.Add(datosSeccion.PuntajeGeneralCF());
                    puntajesGeneralesSeccion.Add(datosSeccion.PuntajeGeneralHB());
                    puntajesGeneralesSeccion.Add(datosSeccion.PuntajeGeneralHD());                    
                    puntajesGeneralesTrabajador.Add(datosTrabajadores.PuntajeGeneralCF()); 
                    puntajesGeneralesTrabajador.Add(datosTrabajadores.PuntajeGeneralHB()); 
                    puntajesGeneralesTrabajador.Add(datosTrabajadores.PuntajeGeneralHD()); 
                                    
                }
                identificador++;
            }         
            /*grafico circular*/
            datosDesempeno.IdSeccion = idSeccion;
            AsignacionValoresGraficoCircular(datosDesempeno.CapacidadGeneralTrabajador());
            /*Grafico Radar*/
            string[] habilidades = { "CF", "HB", "HD" };          
            GraficoRadar graficoRadar = new GraficoRadar
                (
                habilidades, 
                puntajesGeneralesSeccion.ToArray(), 
                puntajesGeneralesTrabajador.ToArray(),
                this.GraficoTrabajadores
                );
            graficoRadar.TipoGrafico = "Area";
            graficoRadar.constructorGrafico();  
        }

        private void EditarTrabajador(object sender, RoutedEventArgs e)
        {
            datosTrabajadores.IdSeccion = idSeccion;
            VentanaAgregarTrabajador ventanaTrabajador = new VentanaAgregarTrabajador();
            ventanaTrabajador.NombreTrabajador = listaTrabajadores[trabajadorSeleccionado].Nombre;
            ventanaTrabajador.ApellidoPaterno = listaTrabajadores[trabajadorSeleccionado].ApellidoPaterno;
            ventanaTrabajador.ApellidoMaterno = listaTrabajadores[trabajadorSeleccionado].ApellidoMaterno;
            ventanaTrabajador.FechaNacimiento = FechaNacimientoFormato(listaTrabajadores[trabajadorSeleccionado].FechaNacimiento);
            ventanaTrabajador.Rut = RutNumero(listaTrabajadores[trabajadorSeleccionado].Rut);
            ventanaTrabajador.DigitoVerificador = DigitoVerificador(listaTrabajadores[trabajadorSeleccionado].Rut);
            ventanaTrabajador.Sexo = TipoSexo(listaTrabajadores[trabajadorSeleccionado].Sexo);
            ventanaTrabajador.Edicion = true;
            ventanaTrabajador.RutNoModificado = listaTrabajadores[trabajadorSeleccionado].Rut;
            ventanaTrabajador.IdSeccion = idSeccion;
            ventanaTrabajador.ShowDialog();
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
                /*elimina los datos de la BD*/
                datosTrabajadores.IdTrabajador = trabajadorSeleccionado; 
                datosTrabajadores.EliminarTrabajador();
                datosTrabajadores.IdSeccion = idSeccion;
                listaTrabajadores = datosTrabajadores.TrabajadoresSeccion();
                GeneraListaTrabajadores(); 
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
        /// Controlador que despliega una ventana(VentanaAgregarTrabajador) para el ingreso de un trabajador nuevo.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoAgregaTrabajador(object sender, RoutedEventArgs e)
        {
            datosTrabajadores.IdSeccion = idSeccion;
            VentanaAgregarTrabajador nuevoTrabajador = new VentanaAgregarTrabajador();
            nuevoTrabajador.IdSeccion = idSeccion;System.Windows.MessageBox.Show("idseccion:"+idSeccion);         
            nuevoTrabajador.ShowDialog();
            listaTrabajadores.Clear();             
            listaTrabajadores = datosTrabajadores.TrabajadoresSeccion();
            GeneraListaTrabajadores();
        }
        /// <summary>
        /// Controlador que al accionarse despliega una ventana con el detalle de calificacion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void detalleTrabajador(object sender, RoutedEventArgs e)
        {           
            datosDesempeno.IdSeccion = idSeccion;
            VentanaDetalleHabilidades detalleHabilidades = new VentanaDetalleHabilidades();
            /*capacidad general*/
            detalleHabilidades.CapacidadTrabajdor = datosDesempeno.CapacidadGeneralTrabajador();
            /*puntajes generales por habilidad de la seccion*/
            detalleHabilidades.PuntajesGeneralesSeccion = puntajesGeneralesSeccion.ToArray();
            detalleHabilidades.PuntajesGeneralesTrabajador = puntajesGeneralesTrabajador.ToArray();
            /*puntajes hab blandas seccion*/
            detalleHabilidades.PerfilSeccion = perfilSeccionActual;
            /*habilidades blandas*/
            detalleHabilidades.HabilidadesBlandas = HB.ToArray();
            detalleHabilidades.PuntajesHbTrabajador = HBPuntajesTrabajador.ToArray();
            detalleHabilidades.PuntajesHbSeccion = HBPuntajesSeccion.ToArray();
            /*habilidades duras*/
            detalleHabilidades.HabilidadesDuras = HD.ToArray();
            detalleHabilidades.PuntajesHdTrabajador = HDPuntajesTrabajador.ToArray();
            detalleHabilidades.PuntajesHdSeccion = HDPuntajesSeccion.ToArray();
            /*caracteristicas fisicas*/
            detalleHabilidades.CaracteristicasFisicas = CF.ToArray();
            detalleHabilidades.PuntajesCfSeccion = CFPuntajesSeccion.ToArray();
            detalleHabilidades.PuntajesCfTrabajador = CFPuntajesTrabajador.ToArray();
            detalleHabilidades.ShowDialog();
            

        }

        private void GenerarReporteTrabajador()
        {
            /*ruta del archivo*/
            SaveFileDialog explorador = new SaveFileDialog();
            explorador.Filter = "Pdf Files|*.pdf";
            explorador.ShowDialog();
            List<double> puntajesSeccionHB = new List<double>();
            List<double> puntajesSeccionHD = new List<double>();
            List<double> puntajesSeccionCF = new List<double>();
            reporte = new Reportes.ReportesTrabajador();

            if (explorador.FileName != "")
            {
                /*habilidades del perfil*/
                datosSeccion.IdSeccion = idSeccion;
                perfilSeccionActual = datosSeccion.PerfilSeccion();

                Dictionary<string, Componente> habilidadesPerfil = perfilSeccionActual.Blandas;
                foreach (KeyValuePair<string, Componente> habilidadBlanda in trabajadorActual.Perfil.Blandas)
                {
                    foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                    {
                        if (habilidadPerfil.Key.Equals(habilidadBlanda.Key))
                        {
                            HB.Add(habilidadBlanda.Value.Nombre);
                            HBPuntajesTrabajador.Add(habilidadBlanda.Value.Puntaje);
                            HBPuntajesSeccion.Add(habilidadPerfil.Value.Puntaje);
                            Console.WriteLine("KEY: " + habilidadBlanda.Key + " - KEY PERFIL: " + habilidadPerfil.Key);
                        }
                    }
                }
                habilidadesPerfil = perfilSeccionActual.Duras;
                foreach (KeyValuePair<string, Componente> habilidadDura in trabajadorActual.Perfil.Duras)
                {
                    foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                    {
                        if (habilidadPerfil.Key.Equals(habilidadDura.Key))
                        {
                            HD.Add(habilidadDura.Value.Nombre);
                            HDPuntajesTrabajador.Add(habilidadDura.Value.Puntaje);
                            HDPuntajesSeccion.Add(habilidadPerfil.Value.Puntaje);
                            Console.WriteLine("KEY: " + habilidadDura.Key + " - KEY PERFIL: " + habilidadPerfil.Key);
                        }
                    }
                }
                habilidadesPerfil = perfilSeccionActual.Fisicas;
                foreach (KeyValuePair<string, Componente> caracFisica in trabajadorActual.Perfil.Fisicas)
                {
                    foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                    {
                        if (habilidadPerfil.Key.Equals(caracFisica.Key))
                        {
                            CF.Add(caracFisica.Value.Nombre);
                            CFPuntajesTrabajador.Add(caracFisica.Value.Puntaje);
                            CFPuntajesSeccion.Add(habilidadPerfil.Value.Puntaje);
                            Console.WriteLine("KEY: " + caracFisica.Key + " - KEY PERFIL: " + habilidadPerfil.Key);
                        }
                    }
                } 
                /*Ingresar datos del grafico y generarlo*/
                /*Grafico Radar General*/
                MemoryStream imagenGeneral = new MemoryStream();
                MemoryStream imagenHB = new MemoryStream();
                MemoryStream imagenHD = new MemoryStream();
                MemoryStream imagenCF = new MemoryStream();                
                string[] habilidades = { "CF", "HB", "HD" };
                
                /*Grafico Radar HB*/
                GraficoRadar graficoHB = new GraficoRadar(HB.ToArray(), HBPuntajesSeccion.ToArray(), HBPuntajesTrabajador.ToArray(), this.GraficoTrabajadores);
                graficoHB.TipoGrafico = "Area";
                graficoHB.constructorGrafico();
                graficoHB.Grafico.SaveImage(imagenHB, ChartImageFormat.Png);
                reporte.ImagenGraficoHB = iTextSharp.text.Image.GetInstance(imagenHB.GetBuffer());
                /*Grafico Radar HD*/
                GraficoRadar graficoHD = new GraficoRadar(HD.ToArray(), HDPuntajesSeccion.ToArray(), HDPuntajesTrabajador.ToArray(), this.GraficoTrabajadores);
                graficoHD.TipoGrafico = "Area";
                graficoHD.constructorGrafico();
                graficoHD.Grafico.SaveImage(imagenHD, ChartImageFormat.Png);
                /*Grafico Radar CF*/
                GraficoRadar graficoCF = new GraficoRadar(CF.ToArray(), CFPuntajesSeccion.ToArray(), CFPuntajesTrabajador.ToArray(), this.GraficoTrabajadores);
                graficoCF.TipoGrafico = "Area";
                graficoCF.constructorGrafico();
                graficoCF.Grafico.SaveImage(imagenCF, ChartImageFormat.Png);
                /*Grafico General*/
                GraficoRadar graficoGeneral = new GraficoRadar
                    (
                        habilidades,
                        puntajesGeneralesSeccion.ToArray(),
                        puntajesGeneralesTrabajador.ToArray(),
                        this.GraficoTrabajadores
                    );
                graficoGeneral.TipoGrafico = "Area";
                graficoGeneral.constructorGrafico();
                graficoGeneral.Grafico.SaveImage(imagenGeneral, ChartImageFormat.Png);
                /*reporte*/

                
                reporte.RutaFichero = explorador.FileName;

                reporte.ImagenGrafico = iTextSharp.text.Image.GetInstance(imagenGeneral.GetBuffer());                
                reporte.ImagenGraficoHD = iTextSharp.text.Image.GetInstance(imagenHD.GetBuffer());
                reporte.ImagenGraficoCF = iTextSharp.text.Image.GetInstance(imagenCF.GetBuffer());
                reporte.Trabajador = trabajadorActual;
                reporte.GenerarReporte();
                /*abre el documento luego de crearlo*/
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = explorador.FileName;
                proc.Start();
                proc.Close();
            }
        }

        private void PuntajesEnCero()
        {
            puntajesGeneralesSeccion.Clear();
            puntajesGeneralesTrabajador.Clear();
            HBPuntajesTrabajador.Clear();
            HDPuntajesTrabajador.Clear();
            CFPuntajesTrabajador.Clear();
            HBPuntajesSeccion.Clear();
            HDPuntajesSeccion.Clear();
            CFPuntajesSeccion.Clear();
            HB.Clear();
            HD.Clear();
            CF.Clear();
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

        /***********************************************************************************************
         *                                      ITEM CONFIGURACION
        /***********************************************************************************************/
        /// <summary>
        /// Metodo que genera el panel de habilidades para la asignacion de importancia de manera manual.
        /// </summary>
        /// <param name="contenedorSeleccionado"></param>
        public void GeneradorPanelHabilidades()
        {            
            int indice = 0;
            datosSeccion.IdSeccion = idSeccion;
            //GeneradorPanelHabilidadesGenerales();
            Perfil perfilSeccion = datosSeccion.PerfilSeccion();              
            HabilidadesBlandas(perfilSeccion, indice);
            HabilidadesDuras(perfilSeccion, indice);
            CaracteristicasFisicas(perfilSeccion, indice);
        }

        public void HabilidadesBlandas(Perfil perfilSeccion, int indice)
        {
            foreach (KeyValuePair<string, Componente> habilidadPuntaje in perfilSeccion.Blandas)
            {                
                if (!habilidadPuntaje.Value.Nombre.Equals("hb") && !habilidadPuntaje.Value.Nombre.Equals("Habilidades Blandas"))
                {
                    PanelHabilidades panelHabilidades = new PanelHabilidades();
                    panelHabilidades.HabilidadName = habilidadPuntaje.Value.ID + "0";
                    panelHabilidades.Habilidad = habilidadPuntaje.Value.Nombre;
                    panelHabilidades.GradoImportancia = habilidadPuntaje.Value.Importancia;
                    panelHabilidades.GradoImportanciaEtiqueta = "" + habilidadPuntaje.Value.Importancia;
                    panelHabilidades.Puntaje = "" + habilidadPuntaje.Value.Puntaje.ToString("0.0");
                    panelHabilidades.GradoImportanciaIdentificador = habilidadPuntaje.Value.ID;
                    panelHabilidades.GradoImportanciaEtiquetaIdentificador = habilidadPuntaje.Value.ID + "2";
                    panelHabilidades.Controlador = AsignacionValorHabilidad;
                    this.contenedor_HB.Children.Add(panelHabilidades.ConstructorPanel(indice));
                    this.contenedor_HB.Children.Add(panelHabilidades.Delimitador);
                    indice++;
                }
            }
        }

        public void HabilidadesDuras(Perfil perfilSeccion, int indice)
        {
            foreach (KeyValuePair<string, Componente> habilidadPuntaje in perfilSeccion.Duras)
            {
                if (!habilidadPuntaje.Value.Nombre.Equals("hd") && !habilidadPuntaje.Value.Nombre.Equals("Habilidades Duras"))
                {
                    PanelHabilidades panelHabilidades = new PanelHabilidades();
                    panelHabilidades.HabilidadName = habilidadPuntaje.Value.ID + "0";
                    panelHabilidades.Habilidad = habilidadPuntaje.Value.Nombre;
                    panelHabilidades.GradoImportancia = habilidadPuntaje.Value.Importancia;
                    panelHabilidades.GradoImportanciaEtiqueta = "" + habilidadPuntaje.Value.Importancia;
                    panelHabilidades.Puntaje = "" + habilidadPuntaje.Value.Puntaje.ToString("0.0");
                    panelHabilidades.GradoImportanciaIdentificador = habilidadPuntaje.Value.ID;//cambio
                    panelHabilidades.GradoImportanciaEtiquetaIdentificador = habilidadPuntaje.Value.ID + "2";
                    panelHabilidades.Controlador = AsignacionValorHabilidad;
                    this.contenedor_HD.Children.Add(panelHabilidades.ConstructorPanel(indice));
                    this.contenedor_HD.Children.Add(panelHabilidades.Delimitador);
                    indice++;
                }
            }
        }

        private void CaracteristicasFisicas(Perfil perfilSeccion, int indice)
        {
            foreach (KeyValuePair<string, Componente> habilidadPuntaje in perfilSeccion.Fisicas)
            {
                if (!habilidadPuntaje.Value.Nombre.Equals("cf") && !habilidadPuntaje.Value.Nombre.Equals("Caracteristicas fisicas"))
                {
                    PanelHabilidades panelHabilidades = new PanelHabilidades();
                    panelHabilidades.HabilidadName = habilidadPuntaje.Value.ID + "0";
                    panelHabilidades.Habilidad = habilidadPuntaje.Value.Nombre;
                    panelHabilidades.GradoImportancia = habilidadPuntaje.Value.Importancia;
                    panelHabilidades.GradoImportanciaEtiqueta = "" + habilidadPuntaje.Value.Importancia;
                    panelHabilidades.Puntaje = "" + habilidadPuntaje.Value.Puntaje.ToString("0.0");
                    panelHabilidades.GradoImportanciaIdentificador = habilidadPuntaje.Value.ID;
                    panelHabilidades.GradoImportanciaEtiquetaIdentificador = habilidadPuntaje.Value.ID + "2";
                    panelHabilidades.Controlador = AsignacionValorHabilidad;
                    this.contenedor_CF.Children.Add(panelHabilidades.ConstructorPanel(indice));
                    this.contenedor_CF.Children.Add(panelHabilidades.Delimitador);
                    indice++;
                }
            }
        }

        private void GeneradorPanelHabilidadesGenerales() 
        {
            AdminSeccion asec = new AdminSeccion();
            List<Seccion> secciones = asec.ObtenerSecciones();
            Seccion seccion = null;
            foreach (Seccion s in secciones)
            {
                if (s.IdSeccion == IdSeccion) seccion = s;
            }

            Perfil perfilEvaluado = EvaluacionPerfil.Ejecutar(seccion.Perfil, IdSeccion);
            datosSeccion.IdSeccion = idSeccion;
            this.puntajeCF.Content = perfilEvaluado.CF.Puntaje;
            this.puntajeHB.Content = perfilEvaluado.HB.Puntaje;
            this.puntajeHD.Content = perfilEvaluado.HD.Puntaje;

            this.grado_importancia_CF.Content = "" + datosSeccion.ImportanciaCF();
            this.grado_importancia_HB.Content = "" + datosSeccion.ImportanciaHB();
            this.grado_importancia_HD.Content = "" + datosSeccion.ImportanciaHD();
            this.slider_CF.Value = datosSeccion.ImportanciaCF();
            this.slider_HD.Value = datosSeccion.ImportanciaHD();
            this.slider_HB.Value = datosSeccion.ImportanciaHB();
            
        }       
        /// <summary>
        /// Controlador que asigna el valor a la habilidad mediante slider.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AsignacionValorHabilidad(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            habilidadesPerfilActulizado = new List<Perfil>();
            var slider              = sender as Slider;
            double value            = slider.Value;
            string identificador    = slider.Name;
            System.Windows.Controls.Label grado_importancia_numerico = new System.Windows.Controls.Label();
            System.Windows.Controls.TextBlock habilidad = new System.Windows.Controls.TextBlock();
            try 
            { 
                grado_importancia_numerico =    (System.Windows.Controls.Label)
                                                LogicalTreeHelper.FindLogicalNode
                                                (
                                                    panelHabilidadesSeleccionado, 
                                                    identificador + "2"
                                                );
                habilidad = (System.Windows.Controls.TextBlock)
                                                LogicalTreeHelper.FindLogicalNode
                                                (
                                                    panelHabilidadesSeleccionado,
                                                    identificador + "0"
                                                ); 
                ActualizaEstadoDePanelHabilidades();   
            }
            catch {}
            if (grado_importancia_numerico != null)
            {
                grado_importancia_numerico.Content = "" + value.ToString("0.0");
                /*identificar diccionario y lo modifica*/
                if (panelHabilidadesSeleccionado.Name.Equals("contenedor_HB"))
                {
                    diccionarioHB[identificador] = Convert.ToDouble(value.ToString("0.0"));                    
                    //Console.WriteLine("EN EL DICCIONARIO " + identificador + " TAM: " + diccionarioHB.Count + " CONTENIDO: " + diccionarioHB.ContainsValue(Convert.ToDouble(value.ToString("0.0"))));
                }                    
                else if (panelHabilidadesSeleccionado.Name.Equals("contenedor_HD"))
                {
                    diccionarioHD[identificador] = Convert.ToDouble(value.ToString("0.0"));
                }
                else if (panelHabilidadesSeleccionado.Name.Equals("contenedor_CF"))
                {
                    diccionarioCF[identificador] = Convert.ToDouble(value.ToString("0.0"));
                }
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
            var a = sender as System.Windows.Controls.TabControl;
            
            if (pestania_HB.IsSelected)
            {
                tipoHabilidad = "hb";
                panelHabilidadesSeleccionado = contenedor_HB;   
            }
            else if (pestania_HD.IsSelected)
            {
                tipoHabilidad = "hd"; 
                panelHabilidadesSeleccionado = contenedor_HD;                            
            }
            else if (pestania_CF.IsSelected)
            {
                tipoHabilidad = "cf";
                panelHabilidadesSeleccionado = contenedor_CF;               
            }
            else if (pestaniaGeneral.IsSelected)
            {
                tipoHabilidad = "general";
            }
        }
        /// <summary>
        /// Controlador que acciona la ventana para habilitar o deshabilitar las habilidades disponibles.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eventoIrAConfiguracion(object sender, RoutedEventArgs e)
        {
            var configuracion = sender as System.Windows.Controls.Button;
            int indice = 0;            
            asignacionHabilidades = new VentanaAsignacionHabilidades(tipoHabilidad,idSeccion);
            asignacionHabilidades.IdSeccion = idSeccion;           
            asignacionHabilidades.ShowDialog();            
            Perfil perfil = datosSeccion.PerfilSeccion();
            if (configuracion.Name.Equals("configHD"))
            {                
                contenedor_HD.Children.Clear();
                HabilidadesDuras(perfil, indice);
            }
            else if (configuracion.Name.Equals("configHB"))
            {
                contenedor_HB.Children.Clear();
                HabilidadesBlandas(perfil, indice);
            }
            else if (configuracion.Name.Equals("configCF"))
            {
                contenedor_CF.Children.Clear();
                CaracteristicasFisicas(perfil, indice);
            }           
        }

        private void ConfiguracionGeneral(object sender, RoutedEventArgs e)
        {
            
            VentanaEleccionGradoImportancia tipoCalificacion = new VentanaEleccionGradoImportancia(this);
            tipoCalificacion.IdSeccion = idSeccion;
            if (!idJefeSeccion.Equals(""))
                tipoCalificacion.IdJefeSeccion = idJefeSeccion;
            else tipoCalificacion.IdJefeSeccion = rutAdministrador;
            tipoCalificacion.Show();
        }
        /// <summary>
        /// Guarda los nuevos valores del perfil por el tipo de habilidad.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GuardarConfiguracion(object sender, RoutedEventArgs e)
        {
            if (tipoHabilidad.Equals("general"))
            {
                datosSeccion.ActualizacionImportanciasHabilidades("HB", Convert.ToDouble(this.slider_HB.Value.ToString("0.0")));
                datosSeccion.ActualizacionImportanciasHabilidades("HD", Convert.ToDouble(this.slider_HD.Value.ToString("0.0")));
                datosSeccion.ActualizacionImportanciasHabilidades("CF", Convert.ToDouble(this.slider_CF.Value.ToString("0.0")));
                cuadroMensajes.CambiosGuardados();
            }
            else if (tipoHabilidad.Equals("hb"))
            {
                foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioHB)
                {
                    datosSeccion.ActualizacionImportanciasHabilidades(habilidadPuntaje.Key, habilidadPuntaje.Value);                   
                }
                cuadroMensajes.CambiosGuardados();
            }
            else if (tipoHabilidad.Equals("hd"))
            {
                foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioHD)
                {                    
                    datosSeccion.ActualizacionImportanciasHabilidades(habilidadPuntaje.Key, habilidadPuntaje.Value);                    
                }
                cuadroMensajes.CambiosGuardados();
            }
            else if (tipoHabilidad.Equals("cf"))
            {
                foreach (KeyValuePair<string, double> habilidadPuntaje in diccionarioCF)
                { 
                    datosSeccion.ActualizacionImportanciasHabilidades(habilidadPuntaje.Key, habilidadPuntaje.Value);                    
                }
                cuadroMensajes.CambiosGuardados();
            }            
        }

        private void DeshabilitaItemHabilidades()
        {            
            pestania_CF.IsEnabled = false;
            pestania_HB.IsEnabled = false;
            pestania_HD.IsEnabled = false;            
        }

        private void HabilitaItemHabilidades()
        {
            pestania_CF.IsEnabled = true;
            pestania_HB.IsEnabled = true;
            pestania_HD.IsEnabled = true;
        }

        private void PanelesVacios()
        {
            contenedor_CF.Children.Clear();
            contenedor_HB.Children.Clear();
            contenedor_HD.Children.Clear();
        }

        /***********************************************************************************************
         *                                          ITEM RANKING
        /***********************************************************************************************/
        /// <summary>
        /// Controlador que se acciona al seleccionar un trabajador en el scroll Ranking.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventoSeleccionTrabajadorRanking(object sender, EventArgs e)
        {
            PuntajesEnCero();
            habilitaEtiquetasRanking();
            detalleRanking.IsEnabled = true;
            solicitaReubicacionRanking.IsEnabled = true;
            /*identifiacacion de objeto*/
            System.Windows.Controls.Button ver = sender as System.Windows.Controls.Button;
            string id = ver.Name as string;
            string[] indiceEtiqueta = id.Split('I');
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
            /*asigna datos del trabajador*/
            nombreRanking.Text = trabajadorRanking[indice].Nombre+ " " + trabajadorRanking[indice].ApellidoPaterno;
            edadRanking.Text = ""+CalcularEdad(trabajadorRanking[indice].FechaNacimiento);
            sexoRanking.Text = trabajadorRanking[indice].Sexo;
            if (trabajadorRanking[indice].Sexo.Equals("Masculino"))
                this.imagenRanking.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Iconos\Business-Man.png", UriKind.Relative)));
            else
                this.imagenRanking.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Iconos\User-Female.png", UriKind.Relative)));
            datosSeccion.IdTrabajador = trabajadorRanking[indice].Rut;
            /*actualizacion en ranking*/
            rutTrabajadorRanking = trabajadorRanking[indice].Rut;
            seccionRanking.Text = datosSeccion.NombreSeccionPorRutTrabajador();
            /*grafico circular*/
            datosDesempeno.IdSeccion = idSeccion;
            datosDesempeno.IdTrabajador = trabajadorRanking[indice].Rut;
            double capacidadGeneral = datosDesempeno.CapacidadGeneralTrabajador();
            double doble = new Random().NextDouble(); //dato de prueba
            AsignacionValoresGraficoCircular(capacidadGeneral);
            /*Grafico araña*/
            datosSeccion.IdSeccion = idSeccion;
            perfilSeccionActual = datosSeccion.PerfilSeccion();
            datosDesempeno.IdSeccion = idSeccion;
            /*puntajes generales*/
            puntajesGeneralesSeccion.Add(datosSeccion.PuntajeGeneralCF());
            puntajesGeneralesSeccion.Add(datosSeccion.PuntajeGeneralHB());
            puntajesGeneralesSeccion.Add(datosSeccion.PuntajeGeneralHD());
            puntajesGeneralesTrabajador.Add(datosDesempeno.CapacidadGeneralCF()); 
            puntajesGeneralesTrabajador.Add(datosDesempeno.CapacidadGeneralHB()); 
            puntajesGeneralesTrabajador.Add(datosDesempeno.CapacidadGeneralHD());
            datosSeccion.IdSeccion = idSeccion;
            perfilSeccionActual = datosSeccion.PerfilSeccion();
            Dictionary<string, Componente> habilidadesPerfil = perfilSeccionActual.Blandas;           
            foreach (KeyValuePair<string, Componente> habilidadBlanda in trabajadorRanking[indice].Perfil.Blandas)
            {
                foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                {
                    if (habilidadPerfil.Key.Equals(habilidadBlanda.Key))
                    {
                        HB.Add(habilidadBlanda.Value.Nombre);
                        HBPuntajesTrabajador.Add(habilidadBlanda.Value.Puntaje);
                        Console.WriteLine("KEY: " + habilidadBlanda.Key + " - KEY PERFIL: " + habilidadPerfil.Key);
                    }
                } 
            }
            habilidadesPerfil = perfilSeccionActual.Duras;
            foreach (KeyValuePair<string, Componente> habilidadDura in trabajadorRanking[indice].Perfil.Duras)
            {
                foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                {
                    if (habilidadPerfil.Key.Equals(habilidadDura.Key))
                    {
                        HD.Add(habilidadDura.Value.Nombre);
                        HDPuntajesTrabajador.Add(habilidadDura.Value.Puntaje);
                        Console.WriteLine("KEY: " + habilidadDura.Key + " - KEY PERFIL: " + habilidadPerfil.Key);
                    }
                } 
            }
            habilidadesPerfil = perfilSeccionActual.Fisicas;
            foreach (KeyValuePair<string, Componente> caracFisica in trabajadorRanking[indice].Perfil.Fisicas)
            {
                foreach (KeyValuePair<string, Componente> habilidadPerfil in habilidadesPerfil)
                {
                    if (habilidadPerfil.Key.Equals(caracFisica.Key))
                    {
                        CF.Add(caracFisica.Value.Nombre);
                        CFPuntajesTrabajador.Add(caracFisica.Value.Puntaje);
                        Console.WriteLine("KEY: " + caracFisica.Key + " - KEY PERFIL: " + habilidadPerfil.Key);
                    }
                } 
            }          
           
            string[] habilidades = { "CF", "HD", "HB" };         
            GraficoRadar graficoRadar = new GraficoRadar
                (habilidades, 
                 puntajesGeneralesSeccion.ToArray(), 
                 puntajesGeneralesTrabajador.ToArray(), 
                 this.graficoRadarRanking
                 );
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
            datosDesempeno.IdSeccion = idSeccion;
            /*capacidad general*/
            detalleHabilidades.CapacidadTrabajdor = datosDesempeno.CapacidadGeneralTrabajador();
            /*puntajes generales por habilidad de la seccion*/
            detalleHabilidades.PuntajesGeneralesSeccion = puntajesGeneralesSeccion.ToArray();
            detalleHabilidades.PuntajesGeneralesTrabajador = puntajesGeneralesTrabajador.ToArray();
            /*puntajes hab blandas seccion*/
            detalleHabilidades.PerfilSeccion = datosSeccion.PerfilSeccion();
            /*habilidades blandas*/
            detalleHabilidades.HabilidadesBlandas = HB.ToArray();
            detalleHabilidades.PuntajesHbTrabajador = HBPuntajesTrabajador.ToArray();
            detalleHabilidades.PuntajesHbSeccion = HBPuntajesSeccion.ToArray();
            /*habilidades duras*/
            detalleHabilidades.HabilidadesDuras = HD.ToArray();
            detalleHabilidades.PuntajesHdTrabajador = HDPuntajesTrabajador.ToArray();
            detalleHabilidades.PuntajesHdSeccion = HDPuntajesSeccion.ToArray();
            /*caracteristicas fisicas*/
            detalleHabilidades.CaracteristicasFisicas = CF.ToArray();
            detalleHabilidades.PuntajesCfSeccion = CFPuntajesSeccion.ToArray();
            detalleHabilidades.PuntajesCfTrabajador = CFPuntajesTrabajador.ToArray();
            detalleHabilidades.ShowDialog();   
        }
        /// <summary>
        /// Metodo que genera la lista de trabajadores que contiene el scroll Ranking.
        /// </summary>
        public void GeneradorRankingPrueba()
        {
            panel_principal.Children.Clear();     
            datosDesempeno.IdSeccion = idSeccion;
            List<string> trabajadoresRanking = datosDesempeno.TrabajadoresRanking();
            int indice = 0;
            foreach (string nombre in trabajadoresRanking)
            {
                datosTrabajadores.IdTrabajador = nombre;
                Trabajador trabajador = datosTrabajadores.InfoTrabajador();
                Perfil perfilTrabajor = datosTrabajadores.PerfilTrabajador();
                perfilRanking.Add(perfilTrabajor);//listas generales
                trabajadorRanking.Add(trabajador);//listas generales
                VisorRanking infoTrabajador = new VisorRanking(EventoSeleccionTrabajadorRanking);
                infoTrabajador.Nombre = trabajador.Nombre;
                infoTrabajador.Apellido = trabajador.ApellidoPaterno ;
                datosSeccion.IdTrabajador = trabajador.Rut;
               // datosSeccion.RutJefeSeccion = datosSeccion.NombreSeccionPorRutTrabajador(); 
                infoTrabajador.Seccion = datosSeccion.NombreSeccionPorRutTrabajador();
                infoTrabajador.PosicionRanking = "" + (indice + 1);
                infoTrabajador.BotonVer = "I" + indice;
                if(trabajador.Sexo.Equals("Masculino"))
                infoTrabajador.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                else infoTrabajador.DireccionImagen = @"..\..\Iconos\User-Female.png";
                this.panel_principal.Children.Add(infoTrabajador.ConstructorInfo());
                indice++;
                
            }
        }

        /// <summary>
        /// Controlador que al accionarse solicita la reubicacion del trabajador seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void solicitaReubicacion(object sender, RoutedEventArgs e)
        {
            int idSeccionNueva = datosTrabajadores.IdSeccionPorRutTrabajador(rutTrabajadorRanking);
            this.hostRanking.Visibility = Visibility.Hidden;
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.ConsultaSolicitudTrabajador()))
            {
                datosSolicitudes.GeneraSolicitud(
                    rutTrabajadorRanking, 
                    idSeccion,
                    idSeccionNueva,
                    datosDesempeno.CapacidadGeneralTrabajadorRanking(idSeccion, rutTrabajadorRanking),
                    datosDesempeno.CapacidadGeneralTrabajadorRanking(idSeccionNueva, rutTrabajadorRanking)
                    );
                cuadroMensajes.TrabajadorSolicitado();
            }
            else { this.hostRanking.Visibility = Visibility.Visible; }
        }

        private void MovimientoArribaRanking(object sender, EventArgs e)
        {
            int estado_avance = Convert.ToInt32(scrollRanking.VerticalOffset);
            double aumenta_espacio = (double)estado_avance - 40.0;
            animadorRanking.detenerAnimacionHorizontal();
            animadorRanking.Contador = (int)aumenta_espacio;
            scrollRanking.ScrollToVerticalOffset(aumenta_espacio);
        }

        private void MovimientoAbajoRanking(object sender, EventArgs e)
        {
            int estado_avance = Convert.ToInt32(scrollRanking.VerticalOffset);
            double aumenta_espacio = (double)estado_avance + 40.0;
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

        /**********************************************************************************************
         *                                      ITEM EVALUACION
        /***********************************************************************************************/
        /// <summary>
        /// Metodo que genera la lista de trabajadores que responderan la encuesta.
        /// </summary>
        private void GeneraListaEvaluados()
        {
            this.panelEvaluacion.Children.Clear();
            int identificador = 0;
            List<string> evaluados = datosTrabajadores.TrabajadoresEvaluados();
            foreach (KeyValuePair<string, Trabajador> infoTrabajador in listaTrabajadores)
            {
                rutEvaluados.Add(infoTrabajador.Value.Rut);
                VisorEvaluacion trabajador = new VisorEvaluacion(seleccionPanelEvaluacion);
                trabajador.Nombre = infoTrabajador.Value.Nombre;
                trabajador.Apellido = infoTrabajador.Value.ApellidoPaterno;    

                if(infoTrabajador.Value.Sexo.Equals("Masculino"))
                    trabajador.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                else 
                    trabajador.DireccionImagen = @"..\..\Iconos\User-Female.png";
               
                if(evaluados.Contains(infoTrabajador.Value.Rut))
                    trabajador.DireccionEstado = @"..\..\Iconos\encuestaRealizada.png";
                else 
                    trabajador.DireccionEstado = @"..\..\Iconos\noEncuestado.png";
                trabajador.IdentificadorPanel = "I" + identificador;
                this.panelEvaluacion.Children.Add(trabajador.ConstructorInfo());
                /*Botones tipo evaluadores*/
                bool estadoEvaluadoPorTrabajadores = false;//dato de prueba
                bool estadoEvaluadoPorJefeSeccion = false;//dato de prueba
                int cantidadEvaluaciones = 11; //dato de prueba
                if (estadoEvaluadoPorTrabajadores)
                {
                    encuestaTrabajador.Title = "Trabajadores (" + cantidadEvaluaciones + ")";
                    //encuestaTrabajador.Content = "CONTESTADA";
                }
                else
                {
                    encuestaTrabajador.Title = "Trabajadores";
                    //encuestaTrabajador.Content = "NO CONTESTADA";
                }
                if (estadoEvaluadoPorJefeSeccion)
                    encuestaJefeSeccion.Content = "";//contestada
                else
                    encuestaJefeSeccion.Content = "";//no contestada
                identificador++;
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
            int identificador = 0;
            rutEvaluado = rutEvaluados[indice];
            foreach (KeyValuePair<string, Trabajador> infoTrabajador in listaTrabajadores)
            {
                if(indice == identificador)
                {
                    nombreEvaluado.Content = infoTrabajador.Value.Nombre+" "+infoTrabajador.Value.ApellidoPaterno;
                }
                identificador++;
            }
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
                VentanaEvaluadores evaluadores = new VentanaEvaluadores(rutEvaluado);
                this.Hide();
                evaluadores.NombreEvaluado = nombreEvaluado.Content as string;
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
                this.Hide();
                VentanaEncuesta encuesta = new VentanaEncuesta(true,idSeccion);
                //encuesta.IdSeccion = idSeccion;
                //encuesta.Preguntas = datosTrabajador.Preguntas;
                encuesta.NombreTrabajador = nombreEvaluado.Content as string;
                encuesta.IdTrabajador = rutEvaluado;
                if (!idJefeSeccion.Equals(""))//jefeseccion
                    encuesta.IdEvaluador = idJefeSeccion;
                else encuesta.IdEvaluador = rutAdministrador;//administrador
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

        /**********************************************************************************************
         *                                  METODOS ASOCIADOS A GRAFICOS
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

        /**********************************************************************************
        *                                  METODOS DE PARSEO
        * *******************************************************************************/
        private string CalcularEdad(string fechaNacimiento)
        {
            string[] soloFecha = fechaNacimiento.Split(' ');
            string[] fecha = soloFecha[0].Split('/');
            Console.WriteLine(fecha[0] + "-" + fecha[1] + "-" + fecha[2]);
            int mm, yy, dd; Int32.TryParse(fecha[0], out dd); Int32.TryParse(fecha[1], out mm); Int32.TryParse(fecha[2], out yy);
            DateTime nacimiento = new DateTime(yy, mm, dd);
            int edad = DateTime.Today.AddTicks(-nacimiento.Ticks).Year - 1;
            return "" + edad;
        }

        private string FechaNacimientoFormato(string fecha)
        {
            string[] fechaNacimiento = fecha.Split(' ');
            return fechaNacimiento[0];
        }

        private string RutNumero(string rut)
        {
            string[] soloRut = rut.Split('-');
            return soloRut[0];
        }

        private string DigitoVerificador(string rut)
        {
            string[] soloRut = rut.Split('-');
            return soloRut[1];
        }

        private int TipoSexo(string sexo)
        {
            if (sexo.Equals("Masculino"))
                return 0;
            else return 1;
        }

        /**********************************************************************************
         *                                  METODOS DE VARIABLES LOCALES
         * *******************************************************************************/
        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }

        public string NombreSeccion
        {
            get { return seccion.Content as string; }
            set { seccion.Content = value; }
        }

        public string NombreJefeSeccion
        {
            get { return nombreJefeSeccion.Content as string; }
            set { nombreJefeSeccion.Content = value; }
        }

        public Dictionary<string, Trabajador> ListaTrabajadores
        {
            get { return listaTrabajadores; }
            set { listaTrabajadores = value;}
        }

        public Visibility RetornarAdministrador
        {
            get { return Visibility.Hidden; }
            set { this.botonVolver.Visibility = value; }
        }

        public string IdJefeSeccion
        {
            get { return idJefeSeccion; }
            set { idJefeSeccion = value; }
        }

        public string IdAdmin
        {
            get { return rutAdministrador; }
            set { rutAdministrador = value; }
        }
        
    }
}
