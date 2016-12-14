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
using System.Windows.Threading;
using LogicaDifusa;
using LiveCharts;
using System.Collections.ObjectModel;
using System.Diagnostics;
using LiveCharts.Wpf;
using OxyPlot;
using DST;

namespace InterfazGrafica
{
    /// <summary>
    /// Lógica de interacción para VentanaAdministrador.xaml
    /// </summary>
    public partial class VentanaAdministrador : MetroWindow
    {
        private Mensajes cuadroMensajes;
        private DatosDePrueba datosPrueba;//datos de prueba
        private AnimacionScroll animadorTrabajadores;
        private Reportes.ReporteSolicitudes reporteSolicitud;
        /*INTERACCION CON BD*/
        private InteraccionBD.InteraccionUsuarios datosUsuario;
        private InteraccionBD.InteraccionSecciones datosSeccion;
        private InteraccionBD.InteraccionTrabajadores datosTrabajador;
        private InteraccionBD.InteraccionSolicitudes datosSolicitudes;
        private string idTrabajador;
        /*Estructuras de datos*/
        List<Seccion> listaDeSecciones;
        List<Trabajador> listaDeTrabajadores;
        List<Solicitud> listaDeSolicitudes;
        int trabajadorSeleccionado;

        private Dictionary<string, Seccion> secciones;
        private double valorGraficoDAnterior;
        private double valorGraficoDPlan;
        private Dictionary<string, Componente> hb;
        private Dictionary<string, Componente> hd;
        private Dictionary<string, Componente> cf;

        public VentanaAdministrador()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            IniciarComponentes();
        }

        private void EventosIniciales(object sender, RoutedEventArgs e)
        {
            animadorTrabajadores.Visualizador = this.scrollTrabajadores;
        }
        private void IniciarComponentes()
        {
            trabajadorSeleccionado = 0;
            cuadroMensajes = new Mensajes(this);
            datosPrueba = new DatosDePrueba();
            animadorTrabajadores = new AnimacionScroll();
            idTrabajador = string.Empty;
            datosUsuario = new InteraccionBD.InteraccionUsuarios();
            datosSeccion = new InteraccionBD.InteraccionSecciones();
            datosTrabajador = new InteraccionBD.InteraccionTrabajadores();
            datosSolicitudes = new InteraccionBD.InteraccionSolicitudes();
            listaDeSecciones = datosSeccion.TodasLasSecciones();
            listaDeTrabajadores = datosTrabajador.TrabajadoresEmpresa();
            listaDeSolicitudes = datosSolicitudes.ListaDeSolicitudes();
            GeneraListaTrabajadores();

            ObtenerSecciones();
            IniciarTablaDesempeno();
            IniciarPanelComponentes();
            IniciarPanelReglas();


        }
        /// <summary>
        /// Controlador que se acciona al presionar boton cerrar ventana.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void CerrarSesion(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.CerrarSesionAdministrador()))
            {
                App.Current.Shutdown();
            }
        }
        /// <summary>
        /// Controlador que se acciona al seleccionar las pestañas de disponibles (tabControl)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seleccionItem(object sender, SelectionChangedEventArgs e)
        {
            if (itemDesempenio.IsSelected)
            {
                animadorTrabajadores.detenerAnimacionHorizontal();
            }
            else if (itemTrabajadores != null && itemTrabajadores.IsSelected)
            {
                animadorTrabajadores.comenzarAnimacionHorizontal();
            }
            else if (itemSecciones != null && itemSecciones.IsSelected)
            {
                animadorTrabajadores.detenerAnimacionHorizontal();
                GeneraListaSecciones();
            }
            else if (itemEvaluacion != null && itemEvaluacion.IsSelected)
            {
                animadorTrabajadores.detenerAnimacionHorizontal();
            }
            else if (itemSolicitudes != null && itemSolicitudes.IsSelected)
            {
                animadorTrabajadores.detenerAnimacionHorizontal();
                GeneraListaSolicitudes();
            }
            else if (itemReglas != null && itemReglas.IsSelected)
            {
                animadorTrabajadores.detenerAnimacionHorizontal();
            }
            else if (itemComponentes != null && itemComponentes.IsSelected)
            {
                animadorTrabajadores.detenerAnimacionHorizontal();
            }
            else if (itemUsuarios != null && itemUsuarios.IsSelected)
            {
                animadorTrabajadores.detenerAnimacionHorizontal();
                GeneraListaUsuarios();
            }
        }
        /**********************************************************************************
                                        ITEM TRABAJADORES
        *********************************************************************************/
        /// <summary>
        /// Metodo que genera los elementos que contiene el scrollTrabajador.
        /// </summary>
        private void GeneraListaTrabajadores()
        {
            this.panelTrabajadores.Children.Clear();
            int indice = 0;
            foreach (Trabajador trabajador in listaDeTrabajadores)
            {
                VisorTrabajador infoTrabajador = new VisorTrabajador(seleccionPanelTrabajador);
                infoTrabajador.Nombre = trabajador.Nombre;
                infoTrabajador.Apellido = trabajador.ApellidoPaterno;
                Console.WriteLine("SEXO: " + trabajador.Sexo);
                /*comprobar el sexo*/
                if (trabajador.Sexo.Equals("Masculino"))
                    infoTrabajador.DireccionImagen = @"..\..\Iconos\Business-Man.png";
                else
                    infoTrabajador.DireccionImagen = @"..\..\Iconos\User-Female.png";
                infoTrabajador.IdentificadorPanel = "I" + indice;
                this.panelTrabajadores.Children.Add(infoTrabajador.ConstructorInfo());
                indice++;
            }
        }
        /// <summary>
        /// Controlador que despliega la informacion del trabajador seleccionado.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void seleccionPanelTrabajador(object sender, MouseButtonEventArgs e)
        {
            eliminarTrabajador.IsEnabled = true;
            editarInformacion.IsEnabled = true;
            int indice = IdentificadorCanvas(sender);
            trabajadorSeleccionado = indice;
            /*asignacion de datos del trabajador*/
            this.nombreTrabajador.Content = listaDeTrabajadores[indice].Nombre + " " + listaDeTrabajadores[indice].ApellidoPaterno;
            this.edadTrabajador.Content = CalcularEdad(listaDeTrabajadores[indice].FechaNacimiento);
            this.sexoTrabajador.Content = listaDeTrabajadores[indice].Sexo;
            if (listaDeTrabajadores[indice].Sexo.Equals("Masculino"))
                this.imagenTrabajador.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Iconos\Business-Man.png", UriKind.Relative)));
            else
                this.imagenTrabajador.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Iconos\User-Female.png", UriKind.Relative)));
            datosSeccion.IdTrabajador = listaDeTrabajadores[indice].Rut;
            this.seccionTrabajador.Content = datosSeccion.NombreSeccionPorRutTrabajador();
        }

        async private void EliminarTrabajador(object sender, RoutedEventArgs e)
        {
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.ConsultaEliminarTrabajadorAdmin()))
            {
                cuadroMensajes.TrabajadorEliminadoAdmin();
                nombreTrabajador.Content = "";
                edadTrabajador.Content = "";
                sexoTrabajador.Content = "";
                eliminarTrabajador.IsEnabled = false;
                editarInformacion.IsEnabled = false;
                /**eliminar datos de BD*/

            }
        }

        private void AgregarTrabajador(object sender, RoutedEventArgs e)
        {
            VentanaAgregarTrabajador nuevoTrabajador = new VentanaAgregarTrabajador();
            nuevoTrabajador.ShowDialog();
        }

        private void EditarTrabajador(object sender, RoutedEventArgs e)
        {
            VentanaAgregarTrabajador nuevoTrabajador = new VentanaAgregarTrabajador();
            nuevoTrabajador.IdTrabajador = idTrabajador;//id del trabajador seleccionado
            nuevoTrabajador.NombreTrabajador = listaDeTrabajadores[trabajadorSeleccionado].Nombre;
            nuevoTrabajador.ApellidoPaterno = listaDeTrabajadores[trabajadorSeleccionado].ApellidoPaterno;
            nuevoTrabajador.ApellidoMaterno = listaDeTrabajadores[trabajadorSeleccionado].ApellidoMaterno;
            nuevoTrabajador.ApellidoPaterno = listaDeTrabajadores[trabajadorSeleccionado].ApellidoPaterno;
            nuevoTrabajador.FechaNacimiento = FechaNacimientoFormato(listaDeTrabajadores[trabajadorSeleccionado].FechaNacimiento);
            nuevoTrabajador.Rut = RutNumero(listaDeTrabajadores[trabajadorSeleccionado].Rut);
            nuevoTrabajador.DigitoVerificador = DigitoVerificador(listaDeTrabajadores[trabajadorSeleccionado].Rut);
            nuevoTrabajador.Sexo = TipoSexo(listaDeTrabajadores[trabajadorSeleccionado].Sexo);
            nuevoTrabajador.Edicion = true;
            nuevoTrabajador.RutNoModificado = listaDeTrabajadores[trabajadorSeleccionado].Rut;
            nuevoTrabajador.ShowDialog();
        }

        private void DetenerMovientoScroll(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorTrabajadores.detenerAnimacionHorizontal();
        }
        private void ComenzarMovimientoScroll(object sender, System.Windows.Input.MouseEventArgs e)
        {
            animadorTrabajadores.comenzarAnimacionHorizontal();
        }

        private void eventoMovimientoDerecha(object sender, EventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollTrabajadores.HorizontalOffset);
            double aumentaEspacio = (double)estadoAvance + 40.0;
            animadorTrabajadores.detenerAnimacionHorizontal();
            animadorTrabajadores.Contador = (int)aumentaEspacio;
            scrollTrabajadores.ScrollToHorizontalOffset(aumentaEspacio);
        }

        private void eventoMovimientoIzquierda(object sender, EventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollTrabajadores.HorizontalOffset);
            double aumentaEspacio = (double)estadoAvance - 40.0;
            animadorTrabajadores.detenerAnimacionHorizontal();
            animadorTrabajadores.Contador = (int)aumentaEspacio;
            scrollTrabajadores.ScrollToHorizontalOffset(aumentaEspacio);
        }
        /********************************************************************************
         *                                  ITEM SECCIONES
        *********************************************************************************/

        private void GeneraListaSecciones()
        {
            this.panelSecciones.Children.Clear();
            int identificador = 0;
            foreach (Seccion unaSeccion in listaDeSecciones)
            {
                datosSeccion.NombreSeccion = unaSeccion.Nombre;  //especifica la seccion actual
                VisorSecciones seccion = new VisorSecciones();
                seccion.NombreSeccion = unaSeccion.Nombre;
                seccion.JefeSeccion = datosSeccion.NombreJefeSeccion();
                seccion.CantidadTrabajadores = "" + unaSeccion.Trabajadores.Count;
                seccion.IdentificadorEditar = "I" + identificador;
                seccion.IdentificadorEliminar = "I" + identificador;
                seccion.IdenficadorVer = "I" + identificador;
                seccion.ControladorVer(VerSeccion);
                seccion.ControladorEliminar(EliminarSeccion);
                seccion.ControladorEditar(EditarSeccion);
                this.panelSecciones.Children.Add(seccion.ConstructorInfo());
                identificador++;
            }
        }

        private void VerSeccion(object sender, RoutedEventArgs e)
        {
            int indice = IdentificadorBoton(sender);
            VentanaJefeSeccion seccion = new VentanaJefeSeccion();
            datosSeccion.NombreSeccion = listaDeSecciones[indice].Nombre;//identifica la seccion seleccionada
            seccion.NombreSeccion = listaDeSecciones[indice].Nombre;
            //seccion.NombreJefeSeccion = datosSeccion.NombreJefeSeccion();
            seccion.NombreJefeSeccion = "Administrador";
            seccion.ListaTrabajadores = listaDeSecciones[indice].Trabajadores;
            seccion.IdSeccion = listaDeSecciones[indice].IdSeccion;
            this.Hide();
            seccion.Show();
        }

        private void EditarSeccion(object sender, RoutedEventArgs e)
        {
            int indice = IdentificadorBoton(sender);
            datosSeccion.NombreSeccion = listaDeSecciones[indice].Nombre; //indica seccion actual
            VentanaAgregarSeccion seccionNueva = new VentanaAgregarSeccion();
            seccionNueva.Seccion = listaDeSecciones[indice].Nombre;
            int item = 0;
            foreach (string nombre in seccionNueva.NombreJefeSeccion)
            {
                if (nombre.Equals(datosSeccion.NombreJefeSeccion()))
                {
                    seccionNueva.JefeSeccion = item;
                }
                item++;
            }
            seccionNueva.ShowDialog();
        }

        private void AgregarSeccion(object sender, RoutedEventArgs e)
        {
            VentanaAgregarSeccion seccionNueva = new VentanaAgregarSeccion();
            seccionNueva.ShowDialog();
            listaDeSecciones = datosSeccion.TodasLasSecciones();
            GeneraListaSecciones();

        }

        async private void EliminarSeccion(object sender, RoutedEventArgs e)
        {
            int indice = IdentificadorBoton(sender);
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.ConsultaEliminarSeccion()))
            {
                datosSeccion.IdSeccion = listaDeSecciones[indice].IdSeccion;
                datosSeccion.EliminarSeccion();
                listaDeSecciones = datosSeccion.TodasLasSecciones();
                GeneraListaSecciones();
                cuadroMensajes.SeccionEliminada();
                listaDeTrabajadores = datosTrabajador.TrabajadoresEmpresa();
                GeneraListaTrabajadores();
            }
        }

        private void MovimientoArriba(object sender, RoutedEventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollSecciones.VerticalOffset);
            double aumentaEspacio = (double)estadoAvance - 100.0;
            scrollSecciones.ScrollToVerticalOffset(aumentaEspacio);
        }

        private void MovimientoAbajo(object sender, RoutedEventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollSecciones.VerticalOffset);
            double aumentaEspacio = (double)estadoAvance + 100.0;
            scrollSecciones.ScrollToVerticalOffset(aumentaEspacio);
        }

        /*********************************************************************************
        *                               ITEM USUARIOS
        *********************************************************************************/

        private void GeneraListaUsuarios()
        {
            panelUsuarios.Children.Clear();

            int identificador = 0;
            foreach (Estructuras.Usuario usuario in datosUsuario.UsuariosExistentes())
            {

                VisorUsuarios usuarios = new VisorUsuarios();
                usuarios.Nombre = usuario.Nombre;
                string nombreSeccion = datosUsuario.SeccionAsociada(usuario.Rut);

                if (!nombreSeccion.Equals(""))
                    usuarios.Seccion = nombreSeccion;
                else usuarios.Seccion = "Sección no asignada";
                usuarios.IdenficadorEditar = "I" + identificador;
                usuarios.IdenficadorEliminar = "I" + identificador;
                usuarios.ControladorEditar(EditarUsuario);
                usuarios.ControladorEliminar(EliminarUsuario);
                panelUsuarios.Children.Add(usuarios.ConstructorInfo());
                identificador++;
            }
        }

        private void EditarUsuario(object sender, RoutedEventArgs e)
        {
            int id = IdentificadorBoton(sender);//verificar
            int identificador = 0;
            foreach (Estructuras.Usuario infoUsuario in datosUsuario.UsuariosExistentes())
            {
                if (id == identificador)
                {
                    VentanaAgregarUsuario usuario = new VentanaAgregarUsuario();
                    usuario.Edicion = true;
                    usuario.NombreUsuario = infoUsuario.Nombre;
                    string[] rut = SeparadorRut(infoUsuario.Rut);
                    usuario.Rut = rut[0];
                    usuario.DigitoVerificador = rut[1];
                    if (infoUsuario.TipoUsuario.Equals("JEFE_SECCION"))
                        usuario.TipoUsuario = 0;
                    else usuario.TipoUsuario = 1;
                    usuario.Password = infoUsuario.Clave;
                    usuario.ShowDialog();
                }
                identificador++;
            }

        }

        async private void EliminarUsuario(object sender, RoutedEventArgs e)
        {
            int id = IdentificadorBoton(sender);
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.EliminarUsuario()))
            {
                int identificador = 0;
                foreach (Estructuras.Usuario infoUsuario in datosUsuario.UsuariosExistentes())
                {
                    if (id == identificador)
                    {
                        datosUsuario.IdUsuario = infoUsuario.Rut;
                        datosUsuario.EliminarUsuario();
                    }
                    identificador++;
                }
                cuadroMensajes.UsuarioEliminado();
                panelUsuarios.Children.Clear();
                GeneraListaUsuarios();
            }
        }

        private void AgregarUsuario(object sender, RoutedEventArgs e)
        {
            VentanaAgregarUsuario usuario = new VentanaAgregarUsuario();
            usuario.ShowDialog();
            panelUsuarios.Children.Clear();
            GeneraListaUsuarios();
        }

        private void MovimientoArribaUsuarios(object sender, RoutedEventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollUsuarios.VerticalOffset);
            double aumentaEspacio = (double)estadoAvance - 100.0;
            scrollUsuarios.ScrollToVerticalOffset(aumentaEspacio);
        }

        private void MovimientoAbajoUsuarios(object sender, RoutedEventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollUsuarios.VerticalOffset);
            double aumentaEspacio = (double)estadoAvance + 100.0;
            scrollUsuarios.ScrollToVerticalOffset(aumentaEspacio);
        }


        /*********************************************************************************
         *                                  ITEM SOLICITUDES
        /*********************************************************************************/
        private void GeneraListaSolicitudes()
        {
            panelSolicitudes.Children.Clear();
            int identificador = 0;
            foreach (Solicitud datosSolicitud in listaDeSolicitudes)
            {
                VisorSolicitudes solicitud = new VisorSolicitudes();
                solicitud.SeccionActual = datosSeccion.NombreSeccionPorId(datosSolicitud.IdSeccionActual);
                solicitud.SeccionNueva = datosSeccion.NombreSeccionPorId(datosSolicitud.IdSeccionSolicitada);
                solicitud.Trabajador = "un trabajador";//datos de prueba
                solicitud.CapacidadActualSeccion = "90.0%";//datos de prueba
                solicitud.CapacidadNuevaSeccion = "92.0%";//datos de prueba
                solicitud.IdentificadorAceptar = "I" + identificador;
                solicitud.IdentificadorRechazar = "I" + identificador;
                solicitud.ControladorRechazar(RechazarSolicitud);
                solicitud.ControladorAceptar(AceptarSolicitud);
                panelSolicitudes.Children.Add(solicitud.ConstructorInfo());
                identificador++;
            }
        }

        async private void AceptarSolicitud(object sender, RoutedEventArgs e)
        {
            /*actualiza datos en el panel y BD*/
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.ConfirmarSolicitud()))
            {
                cuadroMensajes.SolicitudConfirmada();
                panelSolicitudes.Children.Clear();
                GeneraListaSolicitudes();
            }

        }

        async private void RechazarSolicitud(object sender, RoutedEventArgs e)
        {
            /*actualiza datos en el panel y BD*/
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.RechazarSolicitud()))
            {
                cuadroMensajes.SolicitudRechazada();
                panelSolicitudes.Children.Clear();
                GeneraListaSolicitudes();
            }
        }

        private void GenerarReportesSolicitudes(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.SaveFileDialog explorador = new System.Windows.Forms.SaveFileDialog();
            explorador.Filter = "Pdf Files|*.pdf";
            explorador.ShowDialog();
            if (explorador.FileName != "")
            {
                reporteSolicitud = new Reportes.ReporteSolicitudes();
                reporteSolicitud.RutaFichero = explorador.FileName;
                reporteSolicitud.GenerarReporte();
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = explorador.FileName;
                proc.Start();
                proc.Close();
            }


        }

        private void MovimientoArribaSolicitudes(object sender, RoutedEventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollSolicitudes.VerticalOffset);
            double aumentaEspacio = (double)estadoAvance - 100.0;
            scrollSolicitudes.ScrollToVerticalOffset(aumentaEspacio);
        }

        private void MovimientoAbajoSolicitudes(object sender, RoutedEventArgs e)
        {
            int estadoAvance = Convert.ToInt32(scrollSolicitudes.VerticalOffset);
            double aumentaEspacio = (double)estadoAvance + 100.0;
            scrollSolicitudes.ScrollToVerticalOffset(aumentaEspacio);
        }

        /***********************************************************/
        private int IdentificadorBoton(Object sender)
        {
            var ver = sender as System.Windows.Controls.Button;
            string id = ver.Name as string;

            string[] indiceEtiqueta = id.Split('I');
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
            return indice;
        }

        private string[] SeparadorRut(string rut)
        {
            string[] rutSeparado = rut.Split('-');
            return rutSeparado;
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

        private int IdentificadorCanvas(Object sender)
        {
            var ver = sender as System.Windows.Controls.Canvas;
            string id = ver.Name as string;

            string[] indiceEtiqueta = id.Split('I');
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
            return indice;
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

        /* ----------------------------------------------------------------------
         *                          PD. PANEL DESEMPEÑO
         * ----------------------------------------------------------------------*/

        public void ObtenerSecciones()
        {
            AdminSeccion adminSeccion = new AdminSeccion();
            List<Seccion> s = adminSeccion.ObtenerSecciones();
            secciones = new Dictionary<string, Seccion>();

            foreach (Seccion seccion in s)
            {
                Tuple<double, double> desempeno = EvaluacionDesempeno.Ejecutar(seccion.VentasActuales, seccion.VentasAnioAnterior, seccion.VentasPlan);

                seccion.ActualAnterior = desempeno.Item1;
                seccion.ActualPlan = desempeno.Item2;
                secciones.Add(seccion.Nombre, seccion);
            }
        }

        /// <summary>
        /// Inicializa la tabla resumen con el desempeño de todas las secciones.
        /// </summary>
        private void IniciarTablaDesempeno()
        {
            foreach (KeyValuePair<string, Seccion> seccion in secciones)
            {
                tablaDesempeno.Items.Add(seccion.Value);
            }
        }

        /// <summary>
        /// Inicia los graficos de desempeño de una seccion.
        /// </summary>
        /// <param name="actualAnterior">Desempeño ventas actual/ventas año anterior</param>
        /// <param name="actualPlan">Desempeño ventas actual/ventas plan</param>
        private void IniciarGraficosDesempeno(double actualAnterior, double actualPlan)
        {
            ValorGraficoDAnterior = 0;
            ValorGraficoDPlan = 0;
            ValorGraficoDAnterior = actualAnterior;
            ValorGraficoDPlan = actualPlan;
            FormatoPorcentaje = x => x.ToString("P");
            
        }

        /// <summary>
        /// Inicia el grafico de las ventas anuales de una seccion.
        /// <param name="seccion">Nombre de la seccion</param>
        /// </summary>
        private void IniciarGraficoVentasAnuales(string seccion)
        {
            SeriesVentasAnuales = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Ventas Año Actual",
                    Values = new ChartValues<double> {
                        1150000, 1820000, 1052000, 1292000,
                        1352000, 1152000, 1252000, 1102000,
                        2012000, 2462000, 1252000, 1109000,
                    },
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Title = "Ventas Año Anterior",
                    Values = new ChartValues<double> {
                        1050000, 1220000, 1050000, 1092000,
                        1152000, 1052000, 1200100, 1302000,
                        2612000, 2062000, 1152000, 1009000,
                    },
                    Fill = Brushes.Transparent
                    //PointGeometry = null
                },
                new LineSeries
                {
                    Title = "Ventas Plan",
                    Values = new ChartValues<double> {
                        2150000, 2820000, 1052000, 3292000,
                        2352000, 2152000, 2252000, 2102000,
                        2012000, 2462000, 2252000, 3109000,
                    },
                    Fill = Brushes.Transparent
                    //PointGeometry = DefaultGeometries.Square,
                    //PointGeometrySize = 15
                }
            };

            LabelsAnioFiscal = new[] { "Ene", "Feb", "Mar", "Abr", "May", "Jun", "Jul", "Ago", "Oct", "Nov", "Dic" };
            YFormatoVentasAnuales = value => value + "$";
        }

        private void IniciarGraficoReubicacionesAnuales(string seccion)
        {
            SeriesReubicacionesAnuales = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Reubicaciones",
                    Values = new ChartValues<double> {
                        1, 5, 3, 2, 
                        1, 1, 3, 4, 
                        1, 2, 3, 3
                    },
                    MaxColumnWidth = 10
                }
            };

            FormatoReubicaciones = value => value.ToString("N");
        }

        private void IniciarGraficoComparacionRAnuales(string seccion)
        {
            SeriesComparacionRAnuales = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Trabajadores",
                    Values = new ChartValues<double> {
                        6, 5, 5, 5,
                        6, 5, 5, 5,
                        6, 5, 6, 6,
                    }, 
                    MaxColumnWidth = 10
                },
                new ColumnSeries
                {
                    Title = "Capacitados",
                    Values = new ChartValues<double> {
                        1, 2, 3, 4,
                        4, 4, 4, 4,
                        4, 4, 3, 3
                    },
                    MaxColumnWidth = 10
                }
            };

            FormatoReubicaciones = value => value.ToString("N");
        }

        /* ----------------------------------------------------------------------
         *                          PD.1 EVENTOS
         * ----------------------------------------------------------------------*/

        /// <summary>
        /// Muestra los detalles de la seccion seleccinada en la tabla de secciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetallesDesempenoSeccion(object sender, RoutedEventArgs e)
        {
            Seccion seccion = (Seccion)tablaDesempeno.SelectedItem;
            // iniciamos los graficos.
            boxWrapDDetalles.SelectedIndex = 0;
            boxWrapDDetalles.SelectedValue = "VentasAnuales";
            IniciarGraficosDesempeno((seccion.ActualAnterior/100), (seccion.ActualPlan/100));
            IniciarGraficoVentasAnuales(seccion.Nombre);
            IniciarGraficoReubicacionesAnuales(seccion.Nombre);
            IniciarGraficoComparacionRAnuales(seccion.Nombre);
            DataContext = this;
            // hacemos visible el panel del desempeño de la seccion.
            panelDResumen.Visibility = Visibility.Hidden;
            lblNombreSeccion.Content = seccion.Nombre;
            ValorGraficoDAnterior = seccion.ActualAnterior;
            ValorGraficoDPlan = seccion.ActualPlan;
            panelDDetalles.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Muestra el resumen del desempeño de las secciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResumenDesempenoSecciones(object sender, RoutedEventArgs e)
        {
            panelDDetalles.Visibility = Visibility.Hidden;
            panelDResumen.Visibility = Visibility.Visible;
        }

        private void TDSeleccionSeccion(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("Doble Click");
        }

        /* ----------------------------------------------------------------------
         *                      PD.2 PROPIEDADES GRAFICOS
         * ----------------------------------------------------------------------*/

        public double ValorGraficoDAnterior
        {
            get { return valorGraficoDAnterior; }
            set
            {
                valorGraficoDAnterior = value;
                OnPropertyChanged("ValorGraficoDAnterior");
            }
        }

        public double ValorGraficoDPlan
        {
            get { return valorGraficoDPlan; }
            set
            {
                valorGraficoDPlan = value;
                OnPropertyChanged("ValorGraficoDPlan");
            }
        }

        public string[] LabelsAnioFiscal { get; set; }
        public SeriesCollection SeriesVentasAnuales { get; set; }
        public SeriesCollection SeriesReubicacionesAnuales { get; set; }
        public SeriesCollection SeriesComparacionRAnuales { get; set; }
        public Func<double, string> FormatoReubicaciones { get; set; }
        public Func<double, string> YFormatoVentasAnuales { get; set; }
        public Func<double, string> FormatoPorcentaje { get; set; }

        public event PropertyChangedEventHandler PropiedadCambiada;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropiedadCambiada != null)
                PropiedadCambiada(this, new PropertyChangedEventArgs(propertyName));
        }

        /* ----------------------------------------------------------------------
         *                          PP. PANEL PREGUNTAS
         * ----------------------------------------------------------------------*/

        private void IniciarPanelPreguntas()
        {

        }

        /* ----------------------------------------------------------------------
         *                          PP.1 EVENTOS BOTONES
         * ----------------------------------------------------------------------*/

        /// <summary>
        /// Muestra los detalles de la seccion seleccinada en la tabla de secciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarPregunta(object sender, RoutedEventArgs e)
        {
            panelPreguntas.Visibility = Visibility.Hidden;
            // Limpiamos los campos y hacemos visible el panel del componente.
            
            panelPregunta.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Muestra los detalles de la seccion seleccinada en la tabla de secciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditarPregunta(object sender, RoutedEventArgs e)
        {

        }

        private void IrPanelAlternativa(object sender, RoutedEventArgs e)
        {
            panelPregunta.Visibility = Visibility.Hidden;
            panelAlternativa.Visibility = Visibility.Visible;
        }

        private void RemoverAlternativa(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Muestra el panel para agregar una nueva alternativa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarAlternativa(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Muestra el panel para editar una nueva alternativa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EliminarAlternativa(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Vuelve al panel de preguntas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolverPanelPregunta(object sender, RoutedEventArgs e)
        {
            panelAlternativa.Visibility = Visibility.Hidden;
            panelPregunta.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Vuelve al panel de preguntas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolverPanelPreguntas(object sender, RoutedEventArgs e)
        {
            panelPregunta.Visibility = Visibility.Hidden;
            panelPreguntas.Visibility = Visibility.Visible;
        }

        /* ----------------------------------------------------------------------
         *                          PC. PANEL COMPONENTES
         * ----------------------------------------------------------------------*/

        private Componente componenteActual;
        private VariableLinguistica variableActual;
        private ValorLinguistico valorActual;

        /// <summary>
        /// Inicializa los valores linguisticos por defecto de un componente.
        /// </summary>
        /// <returns></returns>
        private void ValoresDefectoComponente()
        {
            ValoresMatching val = new ValoresMatching();
            List<ValorLinguistico> valores = new List<ValorLinguistico>();
            variableActual.AgregarValorLinguistico("Muy_baja", val.MuyBajas.Fp);
            variableActual.AgregarValorLinguistico("Baja", val.Bajas.Fp);
            variableActual.AgregarValorLinguistico("Promedio", val.Promedio.Fp);
            variableActual.AgregarValorLinguistico("Alta", val.Altas.Fp);
            variableActual.AgregarValorLinguistico("Muy_alta", val.MuyAltas.Fp);
        }

        private void ObtenerComponentes()
        {
            AdminPerfil adminPerfil = new AdminPerfil();
            hb = new Dictionary<string, Componente>();
            hd = new Dictionary<string, Componente>();
            cf = new Dictionary<string, Componente>();
            hb = adminPerfil.ObtenerComponentesPorTipo(PerfilConstantes.HB);
            hd = adminPerfil.ObtenerComponentesPorTipo(PerfilConstantes.HD);
            cf = adminPerfil.ObtenerComponentesPorTipo(PerfilConstantes.CF);
        }

        /// <summary>
        /// Insertar un componente en la base de datos.
        /// </summary>
        /// <param name="componente">El componente a insertar</param>
        /// <param name="variable">La variable linguistica del componente</param>
        private void InsertarComponente(Componente componente, VariableLinguistica variable)
        {
            AdminLD adminLD = new AdminLD();
            AdminPerfil adminPerfil = new AdminPerfil();

            adminPerfil.InsertarComponente(componente.ID, componente.Nombre, componente.Descripcion, componente.Tipo, true);
            adminLD.InsertarVariableLinguistica(variable.Nombre, variable.Min, variable.Max);

            foreach (KeyValuePair<string, ValorLinguistico> valor in variable.Valores)
            {
                Type tipoFuncion = valor.Value.Fp.GetType();
                if (tipoFuncion.Equals(typeof(FuncionTrapezoidal)))
                {
                    FuncionTrapezoidal fp = (FuncionTrapezoidal)valor.Value.Fp;
                    adminLD.InsertarValorLinguistico(valor.Key, variable.Nombre, "trapezoidal");
                    adminLD.InsertarFuncionTrapezoide(variable.Nombre, valor.Key, fp.ValorIzqAbajo, fp.ValorIzqArriba, fp.ValorDerchArriba, fp.ValorDerchAbajo);
                }
                else if (tipoFuncion.Equals(typeof(FuncionTriangular)))
                {
                    FuncionTriangular fp = (FuncionTriangular)valor.Value.Fp;
                    adminLD.InsertarValorLinguistico(valor.Key, variable.Nombre, "triangular");
                    adminLD.InsertarFuncionTriangular(variable.Nombre, valor.Key, fp.ValorIzq, fp.ValorCentro, fp.ValorDerch);
                }

            }
        }

        /// <summary>
        /// Llena la tabla de componentes.
        /// </summary>
        /// <param name="componentes"></param>
        private void LlenarTablaComponentes(Dictionary<string, Componente> componentes)
        {
            tablaComponentes.Items.Clear();
            foreach (KeyValuePair<string, Componente> componente in componentes)
            {
                tablaComponentes.Items.Add(componente.Value);
            }
        }

        /// <summary>
        /// Inicia el panel de componentes, mostrando las habilidades blandas 
        /// por defecto.
        /// </summary>
        private void IniciarPanelComponentes()
        {
            ObtenerComponentes();
            LlenarTablaComponentes(hb);
            boxComponentes.SelectedValue = "Habilidades blandas";

        }

        /// <summary>
        /// Inicia el grafico de las funciones de pertenencia.
        /// </summary>
        private void IniciarGraficoFP()
        {
            
        }

        /* ----------------------------------------------------------------------
         *                          PC.1 EVENTOS BOTONES
         * ----------------------------------------------------------------------*/

        /// <summary>
        /// Muestra los detalles de la seccion seleccinada en la tabla de secciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IrPanelComponente(object sender, RoutedEventArgs e)
        {
            string boton = (sender as Button).Content.ToString();

            panelComponentes.Visibility = Visibility.Hidden;

            if (boton == "Agregar Componente")
            {
                IniciarNuevoComponente();
            } else if (boton == "Editar Componente")
            {
                IniciarComponente();
            }

            panelComponente.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Iniciar los valores para un nuevo componente.
        /// </summary>
        private void IniciarNuevoComponente()
        {
            componenteActual = new Componente("", "", "", "");
            variableActual = new VariableLinguistica("defecto", 0, 100);
            ValoresDefectoComponente();
            lblComponente.Content = "Nuevo Componente";
            txtIdComponente.Text = "";
            txtNombreComponente.Text = "";
            txtDescripcionComponente.Text = "";
            boxTipoComponente.SelectedValue = "Habilidad blanda";
            boxValoresComponente.SelectedValue = "Muy baja, Baja, Promedio, Alta, Muy alta";
            txtLimiteInferior.Text = "" + variableActual.Min;
            txtLimiteSuperior.Text = "" + variableActual.Max;
            // Iniciamos los valores linguisticos por defecto.
            foreach (KeyValuePair<string, ValorLinguistico> valor in variableActual.Valores)
            {
                boxFP.Items.Add(valor.Key);
            }
            boxFP.Items.Add("+ Agregar nueva función");
            boxFP.SelectedValue = "Muy_baja";
            FuncionTrapezoidal fp = (FuncionTrapezoidal)variableActual.Valores["Muy_baja"].Fp;
            boxTipoFP.SelectedValue = "Trapezoidal";
            txtValorIzquierdaAbajo.Text = "" + fp.ValorIzqAbajo;
            txtValorIzquierdaArriba.Text = "" + fp.ValorIzqArriba;
            txtValorDerechaArriba.Text = "" + fp.ValorDerchAbajo;
            txtValorDerechaAbajo.Text = "" + fp.ValorDerchArriba;
            txtValorIzquierda.Text = "";
            txtValorCentro.Text = "";
            txtValorDerecha.Text = "";
            wrapValoresTriangular.Visibility = Visibility.Hidden;
            wrapValoresTrapezoidal.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Inicia los valores de un componente existente.
        /// </summary>
        private void IniciarComponente()
        {
            AdminLD adminLD = new AdminLD();
            componenteActual = (Componente)tablaComponentes.SelectedItem;
            variableActual = adminLD.ObtenerVariable(componenteActual.ID);
            lblComponente.Content = "Componente";
            txtIdComponente.Text = componenteActual.ID;
            txtNombreComponente.Text = componenteActual.Nombre;
            txtDescripcionComponente.Text = componenteActual.Descripcion;
            txtLimiteInferior.Text = "" + variableActual.Min;
            txtLimiteSuperior.Text = "" + variableActual.Max;

            if (componenteActual.Tipo == PerfilConstantes.HB)
                boxTipoComponente.SelectedValue = "Habilidad blanda";
            else if (componenteActual.Tipo == PerfilConstantes.HD)
                boxTipoComponente.SelectedValue = "Habilidad dura";
            else if (componenteActual.Tipo == PerfilConstantes.CF)
                boxTipoComponente.SelectedValue = "Caracteristica fisica";

            foreach (KeyValuePair<string, ValorLinguistico> valor in variableActual.Valores)
            {
                boxFP.Items.Add(valor.Key);
            }
            string funcion = (string)boxFP.Items[0];
            Type tipoFuncion = variableActual.Valores[funcion].Fp.GetType();

            if (tipoFuncion.Equals(typeof(FuncionTrapezoidal)))
            {
                FuncionTrapezoidal fp = (FuncionTrapezoidal)variableActual.Valores[funcion].Fp;
                boxTipoFP.SelectedValue = "Trapezoidal";
                txtValorIzquierdaAbajo.Text = "" + fp.ValorIzqAbajo;
                txtValorIzquierdaArriba.Text = "" + fp.ValorIzqArriba;
                txtValorDerechaArriba.Text = "" + fp.ValorDerchAbajo;
                txtValorDerechaAbajo.Text = "" + fp.ValorDerchArriba;
                wrapValoresTriangular.Visibility = Visibility.Hidden;
                wrapValoresTrapezoidal.Visibility = Visibility.Visible;
            }
            else if (tipoFuncion.Equals(typeof(FuncionTriangular)))
            {
                FuncionTriangular fp = (FuncionTriangular)variableActual.Valores[funcion].Fp;
                boxTipoFP.SelectedValue = "Triangular";
                txtValorIzquierda.Text = "" + fp.ValorIzq;
                txtValorCentro.Text = "" + fp.ValorCentro;
                txtValorDerecha.Text = "" + fp.ValorDerch;
                wrapValoresTrapezoidal.Visibility = Visibility.Hidden;
                wrapValoresTriangular.Visibility = Visibility.Visible;
            }
        }
        
        /// <summary>
        /// Muestra los detalles de la seccion seleccinada en la tabla de secciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditarComponente(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Realiza los cambios correspondientes al componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarComponente(object sender, RoutedEventArgs e)
        {
            AdminLD adminLD = new AdminLD();
            AdminPerfil adminPerfil = new AdminPerfil();
            string idViejo = componenteActual.ID;
            bool insertar = false;
            if (componenteActual.ID == "")
            {
                insertar = true;
            }
            componenteActual.ID = txtIdComponente.Text;
            variableActual.Nombre = txtIdComponente.Text;
            componenteActual.Nombre = txtNombreComponente.Text;
            componenteActual.Descripcion = txtDescripcionComponente.Text;
            string tipo = (string)boxTipoComponente.SelectedValue;
            if (tipo == "Habilidad blanda")
            {
                componenteActual.Tipo = PerfilConstantes.HB;
            }
            else if (tipo == "Habilidad dura")
            {
                componenteActual.Tipo = PerfilConstantes.HD;
            }
            else if (tipo == "Caracteristica fisica")
            {
                componenteActual.Tipo = PerfilConstantes.CF;
            }

            if (componenteActual.ID != "" && componenteActual.Nombre != "")
            {
                if (insertar)
                {
                    if (adminPerfil.ObtenerComponentePorNombre(componenteActual.Nombre) != null)
                        MostrarError(lblErrorComponente, " El Nombre del componente ya existe.");
                    else if (adminLD.ObtenerVariable(componenteActual.ID) != null)
                        MostrarError(lblErrorComponente, " El Id del componente ya existe.");
                    else
                    {
                        InsertarComponente(componenteActual, variableActual);
                        ObtenerComponentes();
                        LlenarTablaComponentes(hb);
                        lblErrorComponente.Visibility = Visibility.Collapsed;
                        VolverPanelComponentes(null, null);
                    }
                }
                else if (!insertar)
                {
                    if (idViejo != componenteActual.ID && adminLD.ObtenerVariable(componenteActual.ID) == null)
                    {
                        adminPerfil.ActualizarComponente(idViejo, componenteActual.ID, componenteActual.Nombre, componenteActual.Descripcion, componenteActual.Tipo);
                        ObtenerComponentes();
                        LlenarTablaComponentes(hb);
                        lblErrorComponente.Visibility = Visibility.Collapsed;
                        VolverPanelComponentes(null, null);
                    }
                    else
                        MostrarError(lblErrorComponente, " El Id del componente ya existe.");
                }
            } else
            {
                MostrarError(lblErrorComponente, " El Id y el Nombre del componente no pueden ser vacios.");
            }
        }

        /// <summary>
        /// Realiza los cambios en las opciones avanzadas de la variable linguistica.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarOpcionesAvanzadas(object sender, RoutedEventArgs e)
        {
            AdminLD adminLD = new AdminLD();

            if (variableActual.Min.ToString() != txtLimiteInferior.Text || variableActual.Max.ToString() != txtLimiteSuperior.Text)
            {
                variableActual.Min = Convert.ToDouble(txtLimiteInferior.Text);
                variableActual.Max = Convert.ToDouble(txtLimiteSuperior.Text);
                adminLD.ActualizarVariableLinguistica(
                    variableActual.Nombre,
                    variableActual.Nombre,
                    variableActual.Min,
                    variableActual.Max
                );
            }

            panelOpcionesAvanzadas.Visibility = Visibility.Hidden;
            panelComponente.Visibility = Visibility.Visible;
        }

        private void MostrarError(Label label, string mensaje)
        {
            label.Content += mensaje;
            lblErrorComponente.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Muestra las opciones avanzadas de un componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpcionesAvanzadasComponente(object sender, RoutedEventArgs e)
        {
            panelComponentes.Visibility = Visibility.Hidden;
            panelComponente.Visibility = Visibility.Hidden;
            panelOpcionesAvanzadas.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Agrega una nueva funcion de pertenencia y 
        /// vuelve a las opciones avanzadas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarFP(object sender, RoutedEventArgs e)
        {
            AdminLD ald = new AdminLD();
            string tipoFuncion = "triangular";
            //boxFP.SelectedValue = "Muy_baja";

            if ((string)boxTipoFP.SelectedValue == "triangular")
            {
                FuncionTriangular fp = (FuncionTriangular)valorActual.Fp;
                string izq = txtValorIzquierda.Text;
                string centro = txtValorCentro.Text;
                string derch = txtValorDerecha.Text;

                if (izq != fp.ValorIzq.ToString() || centro != fp.ValorCentro.ToString() || derch != fp.ValorDerch.ToString())
                {
                    fp.ValorIzq = Convert.ToDouble(izq);
                    fp.ValorCentro = Convert.ToDouble(centro);
                    fp.ValorDerch = Convert.ToDouble(derch);
                    ald.ActualizarValoresTriangular(
                        variableActual.Nombre, 
                        valorActual.Nombre,
                        fp.ValorIzq, fp.ValorCentro, fp.ValorDerch
                    );
                }
            }
            else if ((string)boxTipoFP.SelectedValue == "trapezoidal")
            {
                tipoFuncion = "trapezoidal";
                FuncionTrapezoidal fp = (FuncionTrapezoidal)valorActual.Fp;
                string izqAbajo = txtValorIzquierdaAbajo.Text;
                string izqArriba = txtValorIzquierdaArriba.Text;
                string derchArriba = txtValorDerechaArriba.Text;
                string derchAbajo = txtValorDerechaAbajo.Text;

                if (izqAbajo != fp.ValorIzqAbajo.ToString() || izqArriba != fp.ValorIzqAbajo.ToString() || derchArriba != fp.ValorDerchArriba.ToString() || derchAbajo != fp.ValorDerchAbajo.ToString())
                {
                    fp.ValorIzqAbajo = Convert.ToDouble(izqAbajo);
                    fp.ValorIzqArriba = Convert.ToDouble(izqArriba);
                    fp.ValorDerchArriba = Convert.ToDouble(derchArriba);
                    fp.ValorDerchAbajo = Convert.ToDouble(derchAbajo);
                    ald.ActualizarValoresTriangular(
                        variableActual.Nombre,
                        valorActual.Nombre,
                        fp.ValorIzqAbajo, fp.ValorIzqArriba, 
                        fp.ValorDerchArriba, fp.ValorDerchAbajo
                    );
                }
            }

            if (valorActual.Nombre != txtNombreFP.Text)
            {
                ald.ActualizarNombreValor(txtNombreFP.Text, valorActual.Nombre, variableActual.Nombre, tipoFuncion);
                valorActual.Nombre = txtNombreFP.Text;
            }

            panelFP.Visibility = Visibility.Hidden;
            panelOpcionesAvanzadas.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Muestra las opciones para editar la funcion de pertenencia.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IrPanelFP(object sender, RoutedEventArgs e)
        {
            string valor = (string)boxFP.SelectedValue;
            valorActual = variableActual.Valores[valor];
            txtNombreFP.Text = valor;

            Type tipoFuncion = valorActual.Fp.GetType();
            if (tipoFuncion.Equals(typeof(FuncionTriangular)))
            {
                FuncionTriangular fp = (FuncionTriangular)valorActual.Fp;
                boxTipoFP.SelectedValue = "Triangular";
                txtValorIzquierda.Text = "" + fp.ValorIzq;
                txtValorCentro.Text = "" + fp.ValorCentro;
                txtValorDerecha.Text = "" + fp.ValorDerch;
                wrapValoresTrapezoidal.Visibility = Visibility.Hidden;
                wrapValoresTriangular.Visibility = Visibility.Visible;
            } else if (tipoFuncion.Equals(typeof(FuncionTrapezoidal))) {
                FuncionTrapezoidal fp = (FuncionTrapezoidal)valorActual.Fp;
                boxTipoFP.SelectedValue = "Trapezoidal";
                txtValorIzquierdaAbajo.Text = "" + fp.ValorIzqAbajo;
                txtValorIzquierdaArriba.Text = "" + fp.ValorIzqArriba;
                txtValorDerechaArriba.Text = "" + fp.ValorDerchAbajo;
                txtValorDerechaAbajo.Text = "" + fp.ValorDerchArriba;
                wrapValoresTriangular.Visibility = Visibility.Hidden;
                wrapValoresTrapezoidal.Visibility = Visibility.Visible;
            }


            panelOpcionesAvanzadas.Visibility = Visibility.Hidden;
            panelFP.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Vuelve a las opciones avanzadas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolverOpcionesAvanzadas(object sender, RoutedEventArgs e)
        {
            boxFP.SelectedValue = "Muy_baja";
            panelFP.Visibility = Visibility.Hidden;
            panelOpcionesAvanzadas.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Ejecuta las acciones para las opciones seleccionadas en el comboBox 
        /// de las funciones de pertenencias.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CambiosBoxFP(object sender, SelectionChangedEventArgs e)
        {
            if ((string)boxFP.SelectedValue == "+ Agregar nueva función")
            {
                panelOpcionesAvanzadas.Visibility = Visibility.Hidden;
                panelFP.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Cambia los campos segun el tipo de funcion de pertenencia seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionTipoFP(object sender, SelectionChangedEventArgs e)
        {
            if ((string)boxTipoFP.SelectedValue == "Triangular")
            {
                wrapValoresTrapezoidal.Visibility = Visibility.Collapsed;
                wrapValoresTriangular.Visibility = Visibility.Visible;
            } else if ((string)boxTipoFP.SelectedValue == "Trapezoidal")
            {
                wrapValoresTriangular.Visibility = Visibility.Collapsed;
                wrapValoresTrapezoidal.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Llena la tabla de componentes segun el tipo de componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionTipoComponentes(object sender, SelectionChangedEventArgs e)
        {
            
            if ((string)boxComponentes.SelectedValue == "Habilidades blandas")
            {
                LlenarTablaComponentes(hb);
            }
            else if ((string)boxComponentes.SelectedValue == "Habilidades duras")
            {
                LlenarTablaComponentes(hd);
            }
            else if ((string)boxComponentes.SelectedValue == "Caracteristicas fisicas")
            {
                LlenarTablaComponentes(cf);
            }
        }

        /// <summary>
        /// Selecciona el tipo del componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionTipoComponente(object sender, SelectionChangedEventArgs e)
        {
            if ((string)boxTipoComponente.SelectedValue == "Habilidad blanda")
            {
                componenteActual.Tipo = PerfilConstantes.HB;
            }
            else if ((string)boxTipoComponente.SelectedValue == "Habilidad dura")
            {
                componenteActual.Tipo = PerfilConstantes.HD;
            }
            else if ((string)boxTipoComponente.SelectedValue == "Caracteristica fisica")
            {
                componenteActual.Tipo = PerfilConstantes.CF;
            }
        }

        /// <summary>
        /// Selecciona los valores de un componente a partir del valor seleccionado 
        /// en un comboBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionValoresComponente(object sender, SelectionChangedEventArgs e)
        {
            if ((string)boxTipoFP.SelectedValue == "Baja, Alta")
            {
                wrapValoresTrapezoidal.Visibility = Visibility.Collapsed;
                wrapValoresTriangular.Visibility = Visibility.Visible;
            }
            else if ((string)boxTipoFP.SelectedValue == "Baja, Promedio, Alta")
            {
                wrapValoresTriangular.Visibility = Visibility.Collapsed;
                wrapValoresTrapezoidal.Visibility = Visibility.Visible;
            }
            else if ((string)boxTipoFP.SelectedValue == "Muy baja, Baja, Alta, Muy alta")
            {
                wrapValoresTriangular.Visibility = Visibility.Collapsed;
                wrapValoresTrapezoidal.Visibility = Visibility.Visible;
            }
            else if ((string)boxTipoFP.SelectedValue == "Muy baja, Baja, Promedio, Alta, Muy alta")
            {
                wrapValoresTriangular.Visibility = Visibility.Collapsed;
                wrapValoresTrapezoidal.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Elimina la funcion de pertenencia que se este mostrando en la edición 
        /// de un componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EliminarFP(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Muestra las opciones de un componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolverPanelComponente(object sender, RoutedEventArgs e)
        {
            panelOpcionesAvanzadas.Visibility = Visibility.Hidden;
            panelComponentes.Visibility = Visibility.Hidden;
            panelComponente.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Muestra las opciones avanzadas de un componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VolverPanelComponentes(object sender, RoutedEventArgs e)
        {
            panelOpcionesAvanzadas.Visibility = Visibility.Hidden;
            panelComponente.Visibility = Visibility.Hidden;
            panelComponentes.Visibility = Visibility.Visible;
        }

        public IList<DataPoint> PuntosFP { get; private set; }

        /* ----------------------------------------------------------------------
         *                          PR. PANEL REGLAS
         * ----------------------------------------------------------------------*/

        private Dictionary<string, Regla> reglasActuales;
        private Seccion reglasSeccionActual;
        private Regla reglaActual;

        private void IniciarPanelReglas()
        {
            GeneraListaSeccionesReglas();
            reglasSeccionActual = secciones["Cajas"];
            ObtenerReglasSeccion(reglasSeccionActual.IdSeccion.ToString(), PerfilConstantes.HB);
            LlenarTablaReglas(reglasActuales);
        }

        private void GeneraListaSeccionesReglas()
        {
            reglasSeccionActual = secciones["Cajas"];

            this.panelReglasSecciones.Children.Clear();
            for (int i = 0; i < 10; i++)
            {
                VisorSecciones seccion = new VisorSecciones();
                seccion.NombreSeccion = "Atencion a Clientes";
                seccion.JefeSeccion = "Un jefe";
                seccion.CantidadTrabajadores = "" + i;
                seccion.IdenficadorVer = "I" + i;
                seccion.IdentificadorEditar = "I" + i;
                seccion.IdentificadorEliminar = "I" + i;
                seccion.ControladorVer(VerSeccionReglas);
                this.panelReglasSecciones.Children.Add(seccion.ConstructorInfo());
            }
        }

        /// <summary>
        /// Obtiene las reglas de la seccion.
        /// </summary>
        /// <param name="idSeccion"></param>
        /// <param name="tipoComponente"></param>
        private void ObtenerReglasSeccion(string idSeccion, string tipoComponente)
        {
            AdminReglas adminReglas = new AdminReglas();
            reglasActuales = adminReglas.ObjetoReglasSeccion(Convert.ToInt32(idSeccion), tipoComponente);
        }

        /// <summary>
        /// Llena la tabla de reglas de la seccion.
        /// </summary>
        /// <param name="reglas"></param>
        private void LlenarTablaReglas(Dictionary<string, Regla> reglas)
        {
            tablaReglas.Items.Clear();
            foreach (KeyValuePair<string, Regla> regla in reglas)
            {
                tablaReglas.Items.Add(regla.Value);
            }
        }

        /* ----------------------------------------------------------------------
         *                          PR.1 EVENTOS
         * ----------------------------------------------------------------------*/

        private void VerSeccionReglas(object sender, RoutedEventArgs e)
        {
            reglasSeccionActual = secciones["Cajas"];
        }

        /// <summary>
        /// Agrega una regla vacia a la tabla de reglas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarReglaVacia(object sender, RoutedEventArgs e)
        {
            AdminReglas adminReglas = new AdminReglas();
            int id = adminReglas.ObtenerUltimoID() + 1;
            tablaReglas.Items.Add(new Regla(id.ToString()));
        }

        private void EliminarRegla(object sender, RoutedEventArgs e)
        {
            AdminReglas ar = new AdminReglas();
            Regla regla = (Regla)tablaReglas.SelectedItem;
            ar.EliminarRegla(Convert.ToInt32(regla.ID));
        }

        /// <summary>
        /// Llena la tabla de componentes segun el tipo de componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionTipoReglas(object sender, SelectionChangedEventArgs e)
        {

            if ((string)boxTipoReglas.SelectedValue == "Habilidades blandas")
            {
                
            }
            else if ((string)boxTipoReglas.SelectedValue == "Habilidades duras")
            {
                
            }
            else if ((string)boxTipoReglas.SelectedValue == "Caracteristicas fisicas")
            {
                
            }
        }

        private void SeleccionRegla(object sender, RoutedEventArgs e)
        {
            reglaActual = (Regla)tablaReglas.SelectedItem;
            Perfil perfil = reglasSeccionActual.Perfil;
            Dictionary<string, Componente> componentes = perfil.Blandas;

            boxAntecedente.Items.Clear();
            foreach (KeyValuePair<string, ValorLinguistico> valor in reglaActual.Antecedente)
            {
                boxAntecedente.Items.Add(valor.Key);
            }

            if ((string)boxTipoReglas.SelectedValue == "Habilidades duras")
                componentes = perfil.Duras;
            else if ((string)boxTipoReglas.SelectedValue == "Caracteristicas fisicas")
                componentes = perfil.Fisicas;

            boxNoAntecedente.Items.Clear();
            foreach (KeyValuePair<string, Componente> componente in componentes)
            {
                boxNoAntecedente.Items.Add(componente.Value.ID);
            }
        }

        /// <summary>
        /// Muestra los valores correspondientes al componente seleccionado en el 
        /// antecedente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionAntecedente(object sender, SelectionChangedEventArgs e)
        {
            AdminLD adminLD = new AdminLD();
            string antecedente = (string)boxAntecedente.SelectedValue;
            
            if (antecedente != null)
            {
                string valorActual = reglaActual.Antecedente[antecedente].Nombre;
                VariableLinguistica variable = adminLD.ObtenerVariable(antecedente);

                boxValoresAntecedente.Items.Clear();
                foreach (KeyValuePair<string, ValorLinguistico> valor in variable.Valores)
                {
                    boxValoresAntecedente.Items.Add(valor.Key);
                }

                boxValoresAntecedente.SelectedValue = valorActual;
            }
        }

        /// <summary>
        /// Muestra las opciones de un componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeleccionNoAntecedente(object sender, SelectionChangedEventArgs e)
        {
            AdminLD adminLD = new AdminLD();
            Debug.WriteLine("Selected" + boxNoAntecedente.SelectedValue);
            string componente = (string)boxNoAntecedente.SelectedValue;
            VariableLinguistica variable = adminLD.ObtenerVariable(componente);

            boxValoresNoAntecedente.Items.Clear();
            foreach (KeyValuePair<string, ValorLinguistico> valor in variable.Valores)
            {
                boxValoresNoAntecedente.Items.Add(valor.Key);
            }

            boxValoresNoAntecedente.SelectedIndex = 0;
        }

        private void SeleccionOperadorRegla(object sender, SelectionChangedEventArgs e)
        {
            string operador = (string)boxOperador.SelectedValue;

            if (reglaActual != null && reglaActual.Operador != operador)
            {
                reglaActual.Operador = operador;
                ActualizarTxtRegla(reglaActual);
            }
        }

        private void IrPanelRegla(object sender, RoutedEventArgs e)
        {
            AdminReglas adminReglas = new AdminReglas();
            Perfil perfil = reglasSeccionActual.Perfil;
            Dictionary<string, Componente> componentes;
            VariablesMatching vm = new VariablesMatching();
            VariableLinguistica consecuente;
            componentes = perfil.Blandas;
            consecuente = vm.HBPerfil;

            if ((string)((Button)sender).Content == "Agregar")
            {
                int id = adminReglas.ObtenerUltimoID() + 1;
                reglaActual = new Regla(
                    id.ToString(),
                    new Dictionary<string, ValorLinguistico>(),
                    new Tuple<string, ValorLinguistico>(
                        consecuente.Nombre, 
                        consecuente.Valores["promedio"]
                    ),
                    "y"
                );
                vm = new VariablesMatching();
                txtConsecuente.Text = consecuente.Nombre;
                txtRegla.Text = "";
            } else if ((string)((Button)sender).Content == "Modificar")
            {
                reglaActual = (Regla)tablaReglas.SelectedItem;
                txtRegla.Text = reglaActual.Texto;
            }

            boxAntecedente.Items.Clear();
            foreach (KeyValuePair<string, ValorLinguistico> valor in reglaActual.Antecedente)
            {
                boxAntecedente.Items.Add(valor.Key);
            }

            if ((string)boxTipoReglas.SelectedValue == "Habilidades duras")
            {
                componentes = perfil.Duras;
                consecuente = vm.HDPerfil;
            }
            else if ((string)boxTipoReglas.SelectedValue == "Caracteristicas fisicas")
            {
                componentes = perfil.Fisicas;
                consecuente = vm.CFPerfil;
            }
            // Llenamos el comboBox del consecuente.
            boxValoresConsecuente.Items.Clear();
            txtConsecuente.Text = consecuente.Nombre;
            foreach (KeyValuePair<string, ValorLinguistico> valor in consecuente.Valores)
            {
                boxValoresConsecuente.Items.Add(valor.Key);
            }
            // Seleccionamos el valor del consecuente.
            string[] r = reglaActual.Texto.Split(' ');
            boxValoresConsecuente.SelectedValue = r[r.Length - 1];
            // Llenamos el comboBox con los componentes de la seccion.
            boxNoAntecedente.Items.Clear();
            foreach (KeyValuePair<string, Componente> componente in componentes)
            {
                boxNoAntecedente.Items.Add(componente.Value.ID);
            }
            // Hacemos visible el panel de la regla.
            panelReglasSeccion.Visibility = Visibility.Hidden;
            panelRegla.Visibility = Visibility.Visible;
        }

        private void AceptarRegla(object sender, RoutedEventArgs e)
        {
            AdminReglas ar = new AdminReglas();
            string idSeccion = reglasSeccionActual.IdSeccion.ToString();
            string tipo = txtConsecuente.Text.ToLower();
            string regla = ar.ObtenerRegla(Convert.ToInt32(reglaActual.ID));

            if (regla != "")
                ar.ActualizarRegla(reglaActual, reglasSeccionActual.IdSeccion);
            else
                ar.InsertarRegla(reglaActual.ID, reglaActual.Texto, idSeccion, tipo);

            ObtenerReglasSeccion(idSeccion, tipo);
            LlenarTablaReglas(reglasActuales);
            VolverPanelReglas(null, null);
        }

        private void VolverPanelReglas(object sender, RoutedEventArgs e)
        {
            panelRegla.Visibility = Visibility.Hidden;
            panelReglasSeccion.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Cambia el valor de una variable del antecedente en la regla seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CambiarValorAntecedente(object sender, RoutedEventArgs e)
        {
            AdminLD adminLD = new AdminLD();
            string nombreVariable = (string)boxAntecedente.SelectedValue;
            string nombreValor = (string)boxValoresAntecedente.SelectedValue;
            ValorLinguistico valor = adminLD.ObtenerValor(nombreValor, nombreVariable);
            reglaActual.Antecedente[nombreVariable] = valor;
            ActualizarTxtRegla(reglaActual);
        }

        private void EliminarValorAntecedente(object sender, RoutedEventArgs e)
        {
            AdminReglas ar = new AdminReglas();
            string nombreVariable = (string)boxAntecedente.SelectedValue;
            // Eliminamos la variable de la base de datos.
            ar.EliminarVariable(Convert.ToInt32(reglaActual.ID), nombreVariable);
            // Removemos la variable del antecedente de la regla.
            reglaActual.Antecedente.Remove(nombreVariable);
            // Actualizamos el textBox de la regla.
            ActualizarTxtRegla(reglaActual);
            // Eliminamos la variable del comboBox del antecedente.
            boxAntecedente.Items.Remove(boxAntecedente.SelectedValue);
            boxValoresAntecedente.Items.Clear();
        }

        /// <summary>
        /// Cambia el valor de la variable del consecuente en la regla seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CambiarValorConsecuente(object sender, RoutedEventArgs e)
        {
            AdminLD adminLD = new AdminLD();
            VariablesMatching vm = new VariablesMatching();
            string nombreVariable = txtConsecuente.Text;
            string nombreValor = (string)boxValoresConsecuente.SelectedValue;
            VariableLinguistica consecuente = vm.HBPerfil;

            if (nombreVariable == PerfilConstantes.HD)
                consecuente = vm.HDPerfil;
            else if (nombreVariable == PerfilConstantes.CF)
                consecuente = vm.CFPerfil;

            Tuple<string, ValorLinguistico> nuevoConsecuente = new Tuple<string, ValorLinguistico>(
                consecuente.Nombre,
                consecuente.Valores[nombreValor]
              );

            reglaActual.Consecuente = nuevoConsecuente;
            ActualizarTxtRegla(reglaActual);
        }

        /// <summary>
        /// Muestra las opciones de un componente.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarComponenteAntecedente(object sender, RoutedEventArgs e)
        {
            AdminLD adminLD = new AdminLD();
            AdminReglas adminReglas = new AdminReglas();
            string nombreVariable = (string)boxNoAntecedente.SelectedValue;
            string nombreValor = (string)boxValoresNoAntecedente.SelectedValue;
            ValorLinguistico valor = adminLD.ObtenerValor(nombreValor, nombreVariable);
            // Agregamos la variable al antecedente.
            reglaActual.AgregarAntecendente(nombreVariable, valor);
            // Actualizamos el textBox de la regla.
            ActualizarTxtRegla(reglaActual);
            //Actualizamos las variables del antecedente de la regla.
            boxAntecedente.Items.Clear();
            foreach (KeyValuePair<string, ValorLinguistico> variable in reglaActual.Antecedente)
            {
                boxAntecedente.Items.Add(variable.Key);
            }
        }

        /// <summary>
        /// Actualiza el textBox de la regla.
        /// </summary>
        /// <param name="regla"></param>
        private void ActualizarTxtRegla(Regla regla)
        {
            reglaActual = new Regla(
                    reglaActual.ID,
                    reglaActual.Antecedente,
                    reglaActual.Consecuente,
                    reglaActual.Operador
                );
            txtRegla.Text = reglaActual.Texto;
        }
    }
}
