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
        private InteraccionBD.InteraccionDesempeno  datosDesempeno;
        private string idTrabajador;
        private string idAdministrador;
        /*Estructuras de datos*/
        List<Seccion> listaDeSecciones;
        List<Trabajador> listaDeTrabajadores;
        List<Solicitud> listaDeSolicitudes;
        int trabajadorSeleccionado;

        private Dictionary<string, Seccion> secciones;
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

            datosDesempeno = new InteraccionBD.InteraccionDesempeno();


            IniciarPanelReglasSecciones();
            IniciarPanelPreguntas("encuesta");

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
            /*actualizacion de trabajador a eliminar*/
            datosTrabajador.IdTrabajador = listaDeTrabajadores[indice].Rut;
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
                datosTrabajador.EliminarTrabajador();
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
            seccion.IdAdmin = idAdministrador;
            this.Hide();
            seccion.Show();
        }

        private void EditarSeccion(object sender, RoutedEventArgs e)
        {
            int indice = IdentificadorBoton(sender);
            datosSeccion.NombreSeccion = listaDeSecciones[indice].Nombre; //indica seccion actual
            VentanaAgregarSeccion seccionNueva = new VentanaAgregarSeccion();
            seccionNueva.Edicion = true;
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
            seccionNueva.Edicion = false;
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
                solicitud.Trabajador = datosTrabajador.NombreTrabajadorPorRut(datosSolicitud.RutSolicitud);
                solicitud.CapacidadActualSeccion = (datosDesempeno.CapacidadGeneralTrabajadorRanking(datosSolicitud.IdSeccionActual, datosSolicitud.RutSolicitud) * 100) + "%"; ;//datos de prueba
                datosDesempeno.IdTrabajador = datosSolicitud.RutSolicitud;
                solicitud.CapacidadNuevaSeccion = ""+(datosDesempeno.CapacidadGeneralTrabajadorRanking(datosSolicitud.IdSeccionSolicitada,datosSolicitud.RutSolicitud)*100)+"%";
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
            var boton = sender as Button;
            string[] etiqueta = boton.Name.Split('I');
            int indice = Convert.ToInt32(etiqueta[1]);
            /*actualiza datos en el panel y BD*/
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.ConfirmarSolicitud()))
            {
                Solicitud solicitud = listaDeSolicitudes[indice];
                //datosDesempeno.ActualizacionSolicitud("ACEPTADA", solicitud.IdSeccionActual, solicitud.IdSeccionSolicitada, solicitud.RutSolicitud);
                cuadroMensajes.SolicitudConfirmada();
                datosDesempeno.ReubicarTrabajador(solicitud.RutSolicitud, solicitud.IdSeccionActual, solicitud.IdSeccionSolicitada, solicitud.FechaSolicitud.Replace(" 0:00:00",""));
                listaDeSolicitudes = datosSolicitudes.ListaDeSolicitudes();
                panelSolicitudes.Children.Clear();
                GeneraListaSolicitudes();
            }

        }

        async private void RechazarSolicitud(object sender, RoutedEventArgs e)
        {
            var boton = sender as Button;
            string[] etiqueta = boton.Name.Split('I');           
            int indice = Convert.ToInt32(etiqueta[1]);
            /*actualiza datos en el panel y BD*/
            if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.RechazarSolicitud()))
            {
                Solicitud solicitud = listaDeSolicitudes[indice];
                datosDesempeno.ActualizacionSolicitud("RECHAZADA", solicitud.IdSeccionActual, solicitud.IdSeccionSolicitada, solicitud.RutSolicitud);                
                cuadroMensajes.SolicitudRechazada();
                panelSolicitudes.Children.Clear();
                listaDeSolicitudes = datosSolicitudes.ListaDeSolicitudes();
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

        // Mes inicial del año fiscal de la empresa.
        int inicioAnioFiscal = 1;
        Seccion desempenoSeccionActual;
        private double valorGraficoDAnterior;
        private double valorGraficoDPlan;
        private double valorGraficoObjetivoMes;
        private double valorGraficoCapacitados;
        Pregunta preguntaSeccionActual;
        Dictionary<string, string> respuestasSeccionActual; // pregunta, alternativa

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
            wrapGraficosObjetivo.Visibility = Visibility.Collapsed;
            wrapGraficosVentas.Visibility = Visibility.Visible;
            graficoDAnterior.Value = actualAnterior;
            graficoDPlan.Value = actualPlan;
        }

        /// <summary>
        /// Inicia los graficos de desempeño de una seccion.
        /// </summary>
        /// <param name="objetivo">El desempeño de la seccion segun los objetivos.</param>
        private void IniciarGraficoObjetivo(double desempenoGqm)
        {
            wrapGraficosVentas.Visibility = Visibility.Collapsed;
            wrapGraficosObjetivo.Visibility = Visibility.Visible;
            graficoDObjetivo.Value = desempenoGqm;
        }

        public void IniciarPanelVentasMes(int idSeccion, string tipoSeccion, int mes, int año)
        {
            AdminDesempeño ad = new AdminDesempeño();
            int reubicaciones = ad.ObtenerReubicacionesMes(idSeccion, mes, año);
            int capacitados = ad.ObtenerEmpleadosCapacitadosMes(idSeccion, mes, año, tipoSeccion);
            int noCapacitados = ad.ObtenerEmpleadosNoCapacitadosMes(idSeccion, mes, año, tipoSeccion);

            if (tipoSeccion.ToLower() == "ventas")
                IniciarGraficoVentasMes(idSeccion, mes, año);
            else if (tipoSeccion.ToLower() == "gqm")
                IniciarGraficoMensualObjetivo(idSeccion, mes, año);

            IniciarMensajesDesempeno();
            //IniciarGraficoMensualObjetivo(idSeccion, mes, año);
            txtNumeroReubicaciones.Text = reubicaciones.ToString();
            txtNumeroCapacitados.Text = capacitados.ToString();
            txtNumeroNoCapacitados.Text = noCapacitados.ToString();
            IniciarGraficoDCapacitados(idSeccion, mes, año, tipoSeccion);
            panelDatosAnuales.Visibility = Visibility.Collapsed;
            panelDatosMensuales.Visibility = Visibility.Visible;
        }

        public void IniciarMensajesDesempeno()
        {
            string tipo = desempenoSeccionActual.Tipo.ToLower();
            SolidColorBrush fondo = Brushes.OrangeRed;
            lblDMensajeActualPlan.Background = Brushes.OrangeRed;
            SolidColorBrush texto = Brushes.White;

            if (tipo == "ventas")
            {
                ChartValues<double> ventas = (ChartValues<double>)graficoVentasMes.SeriesCollection[0].Values;
                double ventasActual = ventas[1];
                double ventasAnterior = ventas[0];
                double ventasPlan = ventas[2];
                string mensajeActualAnterior = "No cumple con superar las ventas del año anterior";
                string mensajeActualPlan = "No cumple con superar las ventas del plan";

                if (ventasActual > ventasAnterior)
                {
                    mensajeActualAnterior = "Supera las ventas del año anterior";
                    fondo = Brushes.LimeGreen;
                } else if (ventasActual == ventasAnterior)
                {
                    mensajeActualAnterior = "Alcanza las ventas del año anterior";
                    fondo = Brushes.BlueViolet;
                }
                if (ventasActual > ventasPlan)
                {
                    mensajeActualPlan = "Supera las ventas del plan";
                    lblDMensajeActualPlan.Background = Brushes.LimeGreen;
                }
                else if (ventasActual == ventasPlan)
                {
                    mensajeActualPlan = "Alcanza las ventas del plan";
                    lblDMensajeActualPlan.Background = Brushes.BlueViolet;
                }

                lblDMensajeActualAnterior.Content = mensajeActualAnterior;
                lblDMensajeActualAnterior.Foreground = texto;
                lblDMensajeActualAnterior.Background = fondo;
                lblDMensajeActualPlan.Content = mensajeActualPlan;
                lblDMensajeActualPlan.Foreground = texto;
                lblDMensajeObjetivos.Visibility = Visibility.Collapsed;
                lblDMensajeActualAnterior.Visibility = Visibility.Visible;
                lblDMensajeActualPlan.Visibility = Visibility.Visible;
            } else if (tipo == "gqm")
            {
                double desempeno = graficoMensualObjetivo.Value * 100;
                string mensajeObjetivos = "No cumple con superar los objetivos";

                if (desempeno > 100)
                {
                    mensajeObjetivos = "Cumple con superar los objetivos";
                    fondo = Brushes.LimeGreen;
                }
                else if (desempeno == 100)
                {
                    mensajeObjetivos = "Alcanza los objetivos";
                    fondo = Brushes.BlueViolet;
                }

                lblDMensajeObjetivos.Content = mensajeObjetivos;
                lblDMensajeObjetivos.Foreground = texto;
                lblDMensajeObjetivos.Background = fondo;
                lblDMensajeActualAnterior.Visibility = Visibility.Collapsed;
                lblDMensajeActualPlan.Visibility = Visibility.Collapsed;
                lblDMensajeObjetivos.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Inicia el grafico de las ventas anuales de una seccion.
        /// <param name="seccion">Id de la seccion</param>
        /// <param name="anio">Anio de las ventas</param>
        /// </summary>
        private void IniciarGraficoObjetivosAnuales(int idSeccion, int anio)
        {
            GraficoVentasAnuales.Visibility = Visibility.Collapsed;
            graficoObjetivosAnual.Visibility = Visibility.Visible;
            graficoObjetivosAnual.ObjetivosAnuales(desempenoSeccionActual.IdSeccion, inicioAnioFiscal, anio);
        }

        /// <summary>
        /// Inicia el grafico de las ventas anuales de una seccion.
        /// <param name="seccion">Id de la seccion</param>
        /// <param name="anio">Anio de las ventas</param>
        /// </summary>
        private void IniciarGraficoVentasAnuales(int idSeccion, int anio)
        {
            graficoObjetivosAnual.Visibility = Visibility.Collapsed;
            GraficoVentasAnuales.Visibility = Visibility.Visible;
            GraficoVentasAnuales.VentasAnuales(desempenoSeccionActual.IdSeccion, inicioAnioFiscal, anio);
        }

        private void IniciarGraficoReubicacionesAnuales(int idSeccion, int anio)
        {
            GraficoReubicacionesAnuales.ReubicacionesAnuales(desempenoSeccionActual.IdSeccion, inicioAnioFiscal, anio, desempenoSeccionActual.Tipo);
        }

        private void IniciarGraficoComparacionRAnuales(int idSeccion, int anio)
        {
            GraficoCAnualEmpleados.ComparativaAnual(desempenoSeccionActual.IdSeccion, inicioAnioFiscal, anio, desempenoSeccionActual.Tipo);
        }

        public void IniciarPanelDatosAnuales(int idSeccion, int anio)
        {
            string tipo = desempenoSeccionActual.Tipo.ToLower();
            if (tipo == "ventas")
            {
                IniciarGraficoVentasAnuales(desempenoSeccionActual.IdSeccion, anio);
            } else if (tipo == "gqm")
            {
                IniciarGraficoObjetivosAnuales(desempenoSeccionActual.IdSeccion, anio);
            }
            
            IniciarGraficoReubicacionesAnuales(desempenoSeccionActual.IdSeccion, anio);
            IniciarGraficoComparacionRAnuales(desempenoSeccionActual.IdSeccion, anio);
            panelDatosMensuales.Visibility = Visibility.Collapsed;
            panelDatosAnuales.Visibility = Visibility.Visible;
        }

        public void IniciarGraficoVentasMes(int idSeccion, int mes, int anio)
        {
            graficoVentasMes.VentasMes(idSeccion, mes, anio);
            graficoMensualObjetivo.Visibility = Visibility.Collapsed;
            graficoVentasMes.Visibility = Visibility.Visible;
        }

        public void IniciarGraficoMensualObjetivo(int idSeccion, int mes, int anio)
        {
            AdminDesempeño ad = new AdminDesempeño();
            double desempenoGqm = ad.ObtenerDesempenoGqm(idSeccion, mes, anio);

            graficoMensualObjetivo.Value = desempenoGqm / 100;

            graficoVentasMes.Visibility = Visibility.Collapsed;
            graficoMensualObjetivo.Visibility = Visibility.Visible;
        }

        public void IniciarGraficoDCapacitados (int idSeccion, int mes, int anio, string tipoSeccion)
        {
            AdminDesempeño ad = new AdminDesempeño();
            double total = ad.ObtenerTotalEmpleadosMes(idSeccion, mes, anio, tipoSeccion);
            double capacitados = ad.ObtenerEmpleadosCapacitadosMes(idSeccion, mes, anio, tipoSeccion);

            capacitados = Math.Round((capacitados / total) * 100, 0);

            graficoDCapacitados.Value =  capacitados / 100;
        }

        private void IniciarPanelIngresoObjetivos()
        {
            respuestasSeccionActual = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Pregunta> pregunta in desempenoSeccionActual.Preguntas)
            {
                listObjetivos.Items.Add(pregunta.Value);
            }
        }

        private void IniciarPanelIngresoVentas()
        {
            AdminDesempeño ad = new AdminDesempeño();
            int anio = DateTime.Now.Year;
            int mes = DateTime.Now.Month;
            int anioAnterior = anio - 1;
            double ventasAnterior = ad.ObtenerVentasAnioAnterior(desempenoSeccionActual.IdSeccion, mes, anioAnterior);

            txtVentasAnterior.Text = "";
            txtVentasActuales.Text = "";
            txtVentasPlan.Text = "";

            // si no hay un año anterior, mostrar el textBox para su ingreso
            if (ventasAnterior == -1)
            {
                lblVentasAnterior.Visibility = Visibility.Visible;
                txtVentasAnterior.Visibility = Visibility.Visible;
            } else
            {
                lblVentasAnterior.Visibility = Visibility.Collapsed;
                txtVentasAnterior.Visibility = Visibility.Collapsed;
            }
        }

        public void MostrarAvisoReubicacion()
        {
            AdminDesempeño ad = new AdminDesempeño();
            int idSeccion = desempenoSeccionActual.IdSeccion;
            int reubicaciones = ad.ObtenerReubicacionMes(idSeccion, DateTime.Now.Month);

            if (desempenoSeccionActual.Tipo.ToLower() == "ventas")
            {
                Dictionary<string, Tuple<double, double, double>> ventas = ad.ObtenerVentasAnuales(idSeccion, inicioAnioFiscal, DateTime.Now.Year);
                int numeroMeses = ventas.Count;

                if (numeroMeses >= 3)
                {
                    string mes1 = ventas.Keys.ElementAt(numeroMeses - 1);
                    string mes2 = ventas.Keys.ElementAt(numeroMeses - 2);
                    string mes3 = ventas.Keys.ElementAt(numeroMeses - 3);

                    if ((ventas[mes1].Item1 < ventas[mes1].Item2) && (ventas[mes2].Item1) < (ventas[mes2].Item2) && (ventas[mes3].Item1 < ventas[mes3].Item2) && (reubicaciones == 0))
                    {
                        lblAvisoReubicacion.Visibility = Visibility.Visible;
                    } else
                    {
                        lblAvisoReubicacion.Visibility = Visibility.Collapsed;
                    }
                }
            } else if (desempenoSeccionActual.Tipo.ToLower() == "gqm")
            {
                Dictionary<string, double> desempenos = ad.ObtenerDesempenoGqmAnual(idSeccion, inicioAnioFiscal, DateTime.Now.Year);
                int numeroMeses = desempenos.Count;

                if (numeroMeses >= 3)
                {
                    string mes1 = desempenos.Keys.ElementAt(numeroMeses - 1);
                    string mes2 = desempenos.Keys.ElementAt(numeroMeses - 2);
                    string mes3 = desempenos.Keys.ElementAt(numeroMeses - 3);

                    if ((desempenos[mes1] < desempenos[mes1]) && (desempenos[mes2] < desempenos[mes2]) && (desempenos[mes3] < desempenos[mes3]) && (reubicaciones == 0))
                    {
                        lblAvisoReubicacion.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        lblAvisoReubicacion.Visibility = Visibility.Collapsed;
                    }
                }
            }
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
            desempenoSeccionActual = (Seccion)tablaDesempeno.SelectedItem;
            AdminDesempeño ad = new AdminDesempeño();
            DateTime fechaActual = DateTime.Now;
            List<int> anios = ad.ObtenerAnios(desempenoSeccionActual.IdSeccion, desempenoSeccionActual.Tipo);

            // Iniciamos los comboBox con los años y los meses.
            boxWrapDDetalles.Items.Clear();
            foreach (int anio in anios)
            {
                boxWrapDDetalles.Items.Add(anio.ToString());
            }

            string tipoSeccion = desempenoSeccionActual.Tipo.ToLower();
            // Iniciamos los graficos.
            if (tipoSeccion == "ventas")
            {
                IniciarGraficosDesempeno((desempenoSeccionActual.ActualAnterior / 100), (desempenoSeccionActual.ActualPlan / 100));
            }
            else if (tipoSeccion == "gqm")
            {
                IniciarGraficoObjetivo(desempenoSeccionActual.DesempenoGqm / 100);
            }

            //boxWrapDDetalles.SelectedValue = fechaActual.Year.ToString();
            IniciarPanelDatosAnuales(desempenoSeccionActual.IdSeccion, fechaActual.Year);

            // Habilitamos el ingreso de datos si no se han ingresado antes.
            double ventasAnterior = ad.ObtenerVentasAnioAnterior(desempenoSeccionActual.IdSeccion, fechaActual.Month, fechaActual.Year);

            if (ventasAnterior == -1)
                btnDIngreso.IsEnabled = true;
            else
                btnDIngreso.IsEnabled = false;

            // Se muestra o esconde el aviso para realizar reubicaciones.
            MostrarAvisoReubicacion();

            // Hacemos visible el panel del desempeño de la seccion.
            panelDResumen.Visibility = Visibility.Hidden;
            lblNombreSeccion.Content = desempenoSeccionActual.Nombre;
            //ValorGraficoDAnterior = desempenoSeccionActual.ActualAnterior;
            //ValorGraficoDPlan = desempenoSeccionActual.ActualPlan;
            panelDDetalles.Visibility = Visibility.Visible;
        }

        public void SeleccionAnioDDetalles(object sender, RoutedEventArgs e)
        {
            string anio = (string)boxWrapDDetalles.SelectedValue;
            AdminDesempeño ad = new AdminDesempeño();
            List<int> meses = ad.ObtenerMesesAnio(desempenoSeccionActual.IdSeccion, Convert.ToInt32(anio), desempenoSeccionActual.Tipo);

            boxWrapMes.Items.Clear();
            boxWrapMes.Items.Add("Anual");
            foreach (int mes in meses)
            {
                boxWrapMes.Items.Add(mes.ToString());
            }
            boxWrapMes.SelectedValue = "Anual";

            IniciarPanelDatosAnuales(desempenoSeccionActual.IdSeccion, Convert.ToInt32(anio));
        }

        public void SeleccionMesDDetalles(object sender, RoutedEventArgs e)
        {
            string anio = (string)boxWrapDDetalles.SelectedValue;
            string mes = (string)boxWrapMes.SelectedValue;

            if (mes != null)
            {
                if (mes != "Anual")
                {
                    IniciarPanelVentasMes(desempenoSeccionActual.IdSeccion, desempenoSeccionActual.Tipo, Convert.ToInt32(mes), Convert.ToInt32(anio));
                }
                else
                {
                    IniciarPanelDatosAnuales(desempenoSeccionActual.IdSeccion, Convert.ToInt32(anio));
                }
            }
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

        private void GenerarReporteDesempeno(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Forms.SaveFileDialog explorador = new System.Windows.Forms.SaveFileDialog();
            explorador.Filter = "Pdf Files|*.pdf";
            explorador.ShowDialog();
            if (explorador.FileName != "")
            {
                Reportes.ReporteDesempenoSeccion reporte = new Reportes.ReporteDesempenoSeccion(desempenoSeccionActual, Convert.ToInt32(boxWrapDDetalles.SelectedValue));

                reporte.RutaFichero = explorador.FileName;
                reporte.GenerarReporte();
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = explorador.FileName;
                proc.Start();
                proc.Close();
            }


        }

        private void IrPanelIngresoDesempeno(object sender, RoutedEventArgs e)
        {
            string tipoSeccion = desempenoSeccionActual.Tipo;
            if (tipoSeccion.ToLower() == "gqm")
            {
                IniciarPanelIngresoObjetivos();
                panelDDetalles.Visibility = Visibility.Hidden;
                panelDIngreso.Visibility = Visibility.Visible;
                wrapIngresoVentas.Visibility = Visibility.Collapsed;
                wrapIngresoObjetivos.Visibility = Visibility.Visible;
            } else if (tipoSeccion.ToLower() == "ventas")
            {
                IniciarPanelIngresoVentas();
                panelDDetalles.Visibility = Visibility.Hidden;
                panelDIngreso.Visibility = Visibility.Visible;
                wrapIngresoObjetivos.Visibility = Visibility.Collapsed;
                wrapIngresoVentas.Visibility = Visibility.Visible;
            }
        }

        private void IrPanelPreguntasGqm(object sender, RoutedEventArgs e)
        {
            IniciarPanelPreguntas("gqm");
            itemEvaluacion.IsSelected = true;
        }

        private void VolverPanelDDetalles(object sender, RoutedEventArgs e)
        {
            panelDIngreso.Visibility = Visibility.Hidden;
            panelDDetalles.Visibility = Visibility.Visible;
        }

        private void SeleccionPreguntaObjetivo(object sender, SelectionChangedEventArgs e)
        {
            preguntaSeccionActual = (Pregunta)listObjetivos.SelectedItem;

            if (preguntaSeccionActual != null)
            {
                boxRespuestaObjetivo.Items.Clear();
                foreach (KeyValuePair<string, Alternativa> alternativa in preguntaSeccionActual.Alternativas)
                {
                    boxRespuestaObjetivo.Items.Add(alternativa.Key);
                }
            }

        }

        private void SeleccionRepuestaObjetivo(object sender, SelectionChangedEventArgs e)
        {
            string idPregunta = preguntaSeccionActual.ID.ToString();

            if (idPregunta != null)
            {
                if (respuestasSeccionActual.ContainsKey(idPregunta))
                {
                    respuestasSeccionActual[idPregunta] = (string)boxRespuestaObjetivo.SelectedValue;
                }
                else
                {
                    respuestasSeccionActual.Add(idPregunta, (string)boxRespuestaObjetivo.SelectedValue);
                }
            }
        }

        public int EmpleadosCapacitados()
        {
            AdminTrabajador at = new AdminTrabajador();
            int idSeccion = desempenoSeccionActual.IdSeccion;
            Dictionary<string, Trabajador> trabajadores = desempenoSeccionActual.Trabajadores;
              int capacitados = 0;

            foreach (KeyValuePair<string, Trabajador> trabajador in trabajadores)
            {
                double capacidad = at.ObtenerCapacidad(trabajador.Value.Rut, idSeccion);

                if (capacitados >= 0)
                {
                    if (EsCapacitado(capacidad))
                        capacitados += 1;
                }
            }

            return capacitados;
        }

        /// <summary>
        /// Evalua si un trabajador es capacitado para la sección.
        /// </summary>
        /// <param name="capacidad"></param>
        /// <returns></returns>
        public bool EsCapacitado(double capacidad)
        {
            VariablesMatching vm = new VariablesMatching();
            VariableLinguistica trabajador = vm.Trabajador;

            trabajador.Fuzzificar(capacidad);

            if (trabajador.Valores["sobre_capacitado"].GradoPertenencia > 0)
            {
                return true;
            }
            else if (trabajador.Valores["sobre_capacitado"].GradoPertenencia <= 0 && trabajador.Valores["capacitado"].GradoPertenencia >= 0.5)
            {
                return true;
            }

            return false;
        }

        private void AceptarIngresoVentas(object sender, RoutedEventArgs e)
        {
            AdminDesempeño ad = new AdminDesempeño();
            int totalEmpleados = desempenoSeccionActual.Trabajadores.Count;
            int capacitados = EmpleadosCapacitados();
            int noCapacitados = totalEmpleados - capacitados;
            double ventasActuales = Convert.ToDouble(txtVentasActuales.Text);
            double ventasPlan = Convert.ToDouble(txtVentasPlan.Text);
            int anio = DateTime.Now.Year;
            int mes = DateTime.Now.Month;
            int anioAnterior = anio - 1;
            string fecha = anio + "-" + mes + "-01";
            double ventasAnterior = ad.ObtenerVentasAnioAnterior(desempenoSeccionActual.IdSeccion, mes, anioAnterior);

            // si no existe ventas del año anterior, las agregamos.
            if (ventasAnterior == -1)
            {
                ventasAnterior = Convert.ToDouble(txtVentasAnterior.Text);
                ad.InsertarVentasAñoAnterior(
                    desempenoSeccionActual.IdSeccion,
                    mes.ToString(),
                    anioAnterior.ToString(),
                    ventasAnterior
                );
            }

            // Tambien debemos ingresar las ventas actuales como anteriores (para el proximo año).
            ad.InsertarVentasAñoAnterior(
                desempenoSeccionActual.IdSeccion,
                mes.ToString(),
                anio.ToString(),
                ventasActuales
            );
            ad.InsertarVentasPlan(
                desempenoSeccionActual.IdSeccion,
                mes.ToString(),
                anio.ToString(),
                ventasPlan
            );
            ad.InsertarDesempeñoMensual(
                desempenoSeccionActual.IdSeccion,
                fecha,
                ventasActuales,
                ventasAnterior,
                ventasPlan,
                ad.ObtenerReubicacionMes(desempenoSeccionActual.IdSeccion, mes),
                totalEmpleados,
                capacitados,
                noCapacitados
            );
            // Actualizamos las secciones y volvemos al inicio del panel de desempeño.
            ObtenerSecciones();
            IniciarTablaDesempeno();

            btnDIngreso.IsEnabled = false;
            panelDIngreso.Visibility = Visibility.Hidden;
            panelDResumen.Visibility = Visibility.Visible;
        }

        private void AceptarRespuestasObjetivo(object sender, RoutedEventArgs e)
        {
            AdminEncuesta ae = new AdminEncuesta();
            AdminDesempeño ad = new AdminDesempeño();
            int idSeccion = desempenoSeccionActual.IdSeccion;
            int totalEmpleados = desempenoSeccionActual.Trabajadores.Count;
            int capacitados = EmpleadosCapacitados();
            int noCapacitados = totalEmpleados - capacitados;
            List<double> respuestas = new List<double>();
            double desempeno = -1;
            string anio = DateTime.Now.Year.ToString();
            string mes = DateTime.Now.Month.ToString();
            string fecha = anio + "-" + mes + "-01";

            foreach (KeyValuePair<string, string> respuesta in respuestasSeccionActual)
            {
                int idPregunta = Convert.ToInt32(respuesta.Key);
                Alternativa alternativa = ae.ObtenerRespuestaSeccion(idSeccion, idPregunta, respuesta.Value);

                if (alternativa == null)
                {
                    ae.InsertarRespuestaSeccion(idSeccion, idPregunta, respuesta.Value);
                    alternativa = ae.ObtenerRespuestaSeccion(idSeccion, idPregunta, respuesta.Value);
                } else
                {
                    ae.ActualizarRespuestaSeccion(idSeccion, idPregunta, respuesta.Value);
                }

                // Agregamos la respuesta, para usarla en la evaluación.
                respuestas.Add(alternativa.Valor);
            }

            desempeno = EvaluacionDesempeno.EjecutarGqm(respuestas);
            ad.InsertarDesempenoGqmMes(
                idSeccion, 
                fecha, 
                desempeno,
                ad.ObtenerReubicacionMes(desempenoSeccionActual.IdSeccion, Convert.ToInt32(mes)), 
                totalEmpleados,
                capacitados,
                noCapacitados
            );

            // Actualizamos las secciones y volvemos al inicio del panel de desempeño.
            ObtenerSecciones();
            IniciarTablaDesempeno();

            btnDIngreso.IsEnabled = false;
            panelDIngreso.Visibility = Visibility.Hidden;
            panelDResumen.Visibility = Visibility.Visible;
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

        public double ValorGraficoObjetivoMes
        {
            get { return valorGraficoObjetivoMes; }
            set
            {
                valorGraficoObjetivoMes = value;
                OnPropertyChanged("ValorGraficoObjetivoMes");
            }
        }

        public double ValorGraficoCapacitados
        {
            get { return valorGraficoCapacitados; }
            set
            {
                valorGraficoCapacitados = value;
                OnPropertyChanged("ValorGraficoCapacitados");
            }
        }

        public string[] LabelsAnioFiscal { get; set; }
        public string[] LabelsVentasMes { get; set; }
        public SeriesCollection SeriesVentasAnuales { get; set; }
        public SeriesCollection SeriesVentasMes { get; set; }
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

        Dictionary<string, Pregunta> preguntas;
        Pregunta preguntaActual;
        Alternativa alternativaActual;
        string tipoActualPreguntas;

        public void ObtenerPreguntas()
        {
            AdminEncuesta ae = new AdminEncuesta();
            preguntas = ae.ObtenerPreguntas();
        }

        public void LlenarTablaPreguntas()
        {
            tablaPreguntas.Items.Clear();
            foreach (KeyValuePair<string, Pregunta> pregunta in preguntas)
            {
                if (pregunta.Value.Tipo.ToLower() != "gqm")
                    tablaPreguntas.Items.Add(pregunta.Value);
            }
        }

        public void LlenarTablaPreguntasGQM()
        {
            tablaPreguntas.Items.Clear();
            foreach (KeyValuePair<string, Pregunta> pregunta in preguntas)
            {
                if (pregunta.Value.Tipo.ToLower() == "gqm")
                    tablaPreguntas.Items.Add(pregunta.Value);
            }
        }

        /// <summary>
        /// Inicia el panel de preguntas.
        /// </summary>
        /// <param name="tipo">"encuesta" (evaluacion competencias) u 
        /// "gqm" (evaluación desempeño sección)</param>
        private void IniciarPanelPreguntas(string tipo)
        {
            ObtenerPreguntas();
            tipoActualPreguntas = tipo;
            if (tipo == "encuesta") LlenarTablaPreguntas();
            else if (tipo == "gqm") LlenarTablaPreguntasGQM();
        }

        private void IniciarPanelPregunta(Pregunta pregunta)
        {
            AdminPerfil ap = new AdminPerfil();
            Dictionary<string, Componente> componentes = new Dictionary<string, Componente>();
            txtPregunta.Text = pregunta.ToString();

            // Si es una pregunta nueva, deshabilitamos las alternativas.
            if (preguntaActual.ID == -1)
            {
                wrapAlternativas.IsEnabled = false;
                wrapFrecuencias.IsEnabled = false;
            } else
            {
                wrapAlternativas.IsEnabled = true;
                wrapFrecuencias.IsEnabled = true;
            }

            if (pregunta.Tipo.ToLower() == "gqm")
            {
                lblTipoComponentes.Visibility = Visibility.Collapsed;
                boxTipoComponentes.Visibility = Visibility.Collapsed;
                lblComponentePregunta.Visibility = Visibility.Collapsed;
                listComponentes.Visibility = Visibility.Collapsed;
                boxTipoPregunta.Items.Clear();
                boxTipoPregunta.Items.Add("Desempeño");
                boxTipoPregunta.SelectedValue = "Desempeño";
                SeleccionTipoPregunta(null,null);
                boxTipoPregunta.IsEnabled = false;
            }
            else
            {
                lblTipoComponentes.Visibility = Visibility.Visible;
                boxTipoComponentes.Visibility = Visibility.Visible;
                lblComponentePregunta.Visibility = Visibility.Visible;
                listComponentes.Visibility = Visibility.Visible;
                boxTipoComponentes.SelectedValue = "Habilidades blandas";
                SeleccionPreguntaTipoComponente(null,null);
                boxTipoPregunta.IsEnabled = true;
                boxTipoPregunta.Items.Clear();
                boxTipoPregunta.Items.Add("Evaluacion 360");
                boxTipoPregunta.Items.Add("Normal");
                boxTipoPregunta.Items.Add("Ingreso de datos");

                if (pregunta.Tipo.ToLower() == "normal")
                        boxTipoPregunta.SelectedValue = "Normal";
                else if (pregunta.Tipo.ToLower() == "datos")
                    boxTipoPregunta.SelectedValue = "Ingreso de datos";
                else
                    boxTipoPregunta.SelectedValue = "Evaluacion 360";

                SeleccionTipoPregunta(null, null);
            }
        }

        private void MarcarComponentesPregunta(Pregunta pregunta, ListBox lista)
        {
            foreach (string componente in pregunta.Componentes)
            {
                lista.SelectedItems.Add(componente);
            }
        }

        private void LlenarListaAlternativas(Dictionary<string, Alternativa> alternativas, ListBox lista)
        {
            lista.Items.Clear();
            foreach (KeyValuePair<string, Alternativa> alternativa in alternativas)
            {
                lista.Items.Add(alternativa.Key);
            }
        }

        private void LlenarBoxAlternativa(string tipoAlternativa)
        {
            AdminEncuesta ae = new AdminEncuesta();
            Dictionary<string, Alternativa> alternativas = ae.ObtenerAlternativasPorTipo(tipoAlternativa);

            boxAlternativa.Items.Clear();
            foreach (KeyValuePair<string, Alternativa> al in alternativas)
            {
                boxAlternativa.Items.Add(al.Key);
            }
            boxAlternativa.Items.Add("+ Nueva alternativa");
        }

        /* ----------------------------------------------------------------------
         *                          PP.1 EVENTOS
         * ----------------------------------------------------------------------*/

        /// <summary>
        /// Muestra los detalles de la seccion seleccinada en la tabla de secciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AgregarPregunta(object sender, RoutedEventArgs e)
        {
            preguntaActual = new Pregunta();

            if (tipoActualPreguntas == "gqm")
                preguntaActual.Tipo = "gqm";
            else if (tipoActualPreguntas == "encuesta")
                preguntaActual.Tipo = "360";

            IniciarPanelPregunta(preguntaActual);
            panelPreguntas.Visibility = Visibility.Hidden;
            panelPregunta.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Muestra los detalles de la seccion seleccinada en la tabla de secciones.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditarPregunta(object sender, RoutedEventArgs e)
        {
            preguntaActual = (Pregunta)tablaPreguntas.SelectedItem;
            IniciarPanelPregunta(preguntaActual);
            panelPreguntas.Visibility = Visibility.Hidden;
            panelPregunta.Visibility = Visibility.Visible;
        }

        private void EliminarPregunta(object sender, RoutedEventArgs e)
        {
            
        }

        private void AceptarPregunta(object sender, RoutedEventArgs e)
        {
            AdminEncuesta ae = new AdminEncuesta();
            string tipoPregunta = (string)boxTipoPregunta.SelectedValue;

            // Obtenemos la pregunta y su tipo.
            preguntaActual.Descripcion = txtPregunta.Text;
            if (tipoPregunta == "Evaluacion 360") preguntaActual.Tipo = "360";
            if (tipoPregunta == "Desempeño") preguntaActual.Tipo = "gqm";
            if (tipoPregunta == "Normal") preguntaActual.Tipo = "normal";
            if (tipoPregunta == "Ingreso de datos") preguntaActual.Tipo = "datos";

            if (preguntaActual.ID == -1)
            {
                // Insertamos la nueva pregunta.
                ae.InsertarPreguntaSeccion(desempenoSeccionActual.IdSeccion, preguntaActual.ToString(), preguntaActual.Tipo);
            } else
            {
                // Primero borramos todas las componentes y alternativas de la pregunta.
                ae.EliminarAlternativasPregunta(preguntaActual.ID);
                ae.EliminarComponentesPregunta(preguntaActual.ID);

                // Actualizamos la pregunta.
                if (preguntaActual.Tipo.ToString() != "gqm")
                {
                    List<string> nuevosComponentes = new List<string>();

                    foreach (var componente in listComponentes.SelectedItems)
                    {
                        nuevosComponentes.Add((string)componente);
                    }

                    preguntaActual.Componentes = nuevosComponentes;
                }

                if (preguntaActual.Tipo.ToString() != "datos")
                {
                    Dictionary<string, Alternativa> nuevasAlternativas = new Dictionary<string, Alternativa>();
                    Dictionary<string, Alternativa> nuevasFrecuencias = new Dictionary<string, Alternativa>();

                    foreach (var alternativa in listAlternativas.SelectedItems)
                    {
                        nuevasAlternativas.Add(
                            (string)alternativa,
                            ae.ObtenerAlternativa((string)alternativa)
                        );
                    }
                    // Solo las preguntas 360 tienen frecuencias.
                    if (preguntaActual.Tipo.ToString() == "360")
                    {
                        foreach (var frecuencia in listFrecuencias.Items)
                        {
                            nuevasFrecuencias.Add(
                                (string)frecuencia,
                                ae.ObtenerAlternativa((string)frecuencia)
                            );
                        }
                    }

                    preguntaActual.Alternativas = nuevasAlternativas;
                    preguntaActual.Frecuencias = nuevasFrecuencias;
                }

                ae.ActualizarPregunta(preguntaActual);
            }
            
            IniciarPanelPreguntas(tipoActualPreguntas);
            VolverPanelPreguntas(null, null);
        }

        private void IrPanelAlternativa(object sender, RoutedEventArgs e)
        {
            AdminEncuesta ae = new AdminEncuesta();
            string nombreBoton = (sender as Button).Name;
            object item = null;

            if (nombreBoton == "btnConfigurarAlternativas")
                item = listAlternativas.SelectedValue;
            else if (nombreBoton == "btnConfigurarFrecuencias")
                item = listFrecuencias.SelectedValue;

            alternativaActual = ae.ObtenerAlternativa((string)item);

            if (alternativaActual == null)
            {
                alternativaActual = new Alternativa();
                alternativaActual.Tipo = preguntaActual.Tipo;
            }

            LlenarBoxAlternativa(alternativaActual.Tipo);

            if (alternativaActual.Nombre == "")
                boxAlternativa.SelectedValue = "+ Nueva alternativa";
            else
                boxAlternativa.SelectedValue = alternativaActual.Nombre;

            panelPregunta.Visibility = Visibility.Hidden;
            panelAlternativa.Visibility = Visibility.Visible;
        }

        private void RemoverAlternativa(object sender, RoutedEventArgs e)
        {
            preguntaActual.Alternativas.Remove((string)listAlternativas.SelectedValue);
            listAlternativas.Items.Remove(listAlternativas.SelectedItem);
            LlenarListaAlternativas(preguntaActual.Alternativas, listAlternativas);
        }

        private void RemoverFrecuencia(object sender, RoutedEventArgs e)
        {
            preguntaActual.Frecuencias.Remove((string)listFrecuencias.SelectedValue);
            listFrecuencias.Items.Remove(listFrecuencias.SelectedItem);
            LlenarListaAlternativas(preguntaActual.Frecuencias, listFrecuencias);
        }

        /// <summary>
        /// Muestra el panel para agregar una nueva alternativa.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AceptarAlternativa(object sender, RoutedEventArgs e)
        {
            AdminEncuesta ae = new AdminEncuesta();
            string nombreActual = alternativaActual.Nombre;
            bool actualizar = false;

            if (alternativaActual.Nombre != "")
                actualizar = true;

            alternativaActual.Nombre = txtNombreAlternativa.Text;
            alternativaActual.Descripcion = txtDescAlternativa.Text;
            alternativaActual.Valor = Convert.ToDouble(txtValorAlternativa.Text);
            alternativaActual.Tipo = preguntaActual.Tipo;

            if (actualizar)
            {
                ae.ActualizarAlternativa(
                    nombreActual,
                    alternativaActual.Nombre,
                    alternativaActual.Descripcion,
                    alternativaActual.Valor,
                    alternativaActual.Tipo
                );
                // Actualizamos los datos de la pregunta.
                preguntaActual = ae.ObtenerPregunta(preguntaActual.ID);
                // Si la variable actualizada no esta en la pregunta, la agregamos.
                if (alternativaActual.Tipo.ToLower() == "frecuencia" && !preguntaActual.Frecuencias.ContainsKey(alternativaActual.Nombre))
                {
                    preguntaActual.Frecuencias.Add(alternativaActual.Nombre, alternativaActual);
                    ae.InsertarAlternativaPregunta(preguntaActual.ID, alternativaActual.Nombre);
                }
                else if (!preguntaActual.Alternativas.ContainsKey(alternativaActual.Nombre))
                {
                    preguntaActual.Alternativas.Add(alternativaActual.Nombre, alternativaActual);
                    ae.InsertarAlternativaPregunta(preguntaActual.ID, alternativaActual.Nombre);
                }
            } else
            {
                ae.InsertarAlternativa(
                    alternativaActual.Nombre,
                    alternativaActual.Descripcion,
                    alternativaActual.Valor,
                    alternativaActual.Tipo
                );
                ae.InsertarAlternativaPregunta(
                    preguntaActual.ID, 
                    alternativaActual.Nombre
                );
                // Actualizamos los datos de la pregunta.
                preguntaActual = ae.ObtenerPregunta(preguntaActual.ID);
            }

            IniciarPanelPregunta(preguntaActual);
            VolverPanelPregunta(null, null);
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

        private void SeleccionPreguntaTipoComponente(object sender, RoutedEventArgs e)
        {
            AdminPerfil ap = new AdminPerfil();
            string tipoComponente = (string)boxTipoComponentes.SelectedValue;
            Dictionary<string, Componente> componentes = new Dictionary<string, Componente>();

            if (tipoComponente == "Habilidades blandas")
                componentes = ap.ObtenerComponentesPorTipo(PerfilConstantes.HB);
            else if (tipoComponente == "Habilidades duras")
                componentes = ap.ObtenerComponentesPorTipo(PerfilConstantes.HD);
            else if (tipoComponente == "Caracteristicas fisicas")
                componentes = ap.ObtenerComponentesPorTipo(PerfilConstantes.CF);

            listComponentes.Items.Clear();
            foreach (KeyValuePair<string, Componente> componente in componentes)
            {
                listComponentes.Items.Add(componente.Key);
            }

            MarcarComponentesPregunta(preguntaActual, listComponentes);
        }

        private void SeleccionTipoPregunta(object sender, RoutedEventArgs e)
        {
            AdminPerfil ap = new AdminPerfil();
            string tipoPregunta = (string)boxTipoPregunta.SelectedValue;
            Dictionary<string, Componente> alternativas = new Dictionary<string, Componente>();

            if (tipoPregunta == "Evaluacion 360")
            {
                preguntaActual.Tipo = "360";
                LlenarListaAlternativas(preguntaActual.Alternativas, listAlternativas);
                LlenarListaAlternativas(preguntaActual.Frecuencias, listFrecuencias);
                wrapAlternativa.Visibility = Visibility.Visible;
                wrapFrecuencias.Visibility = Visibility.Visible;
            } else if (tipoPregunta == "Normal" || tipoPregunta == "Desempeño")
            {
                if (tipoPregunta == "Normal") preguntaActual.Tipo = "normal";
                if (tipoPregunta == "Desempeño") preguntaActual.Tipo = "gqm";
                LlenarListaAlternativas(preguntaActual.Alternativas, listAlternativas);
                wrapAlternativa.Visibility = Visibility.Visible;
                wrapFrecuencias.Visibility = Visibility.Collapsed;
            } else if (tipoPregunta == "Ingreso de datos")
            {
                preguntaActual.Tipo = "datos";
                wrapAlternativa.Visibility = Visibility.Collapsed;
                wrapFrecuencias.Visibility = Visibility.Collapsed;
            }
        }

        private void SeleccionAlternativa(object sender, RoutedEventArgs e)
        {
            string alternativa = (string)boxAlternativa.SelectedItem;

            if (alternativa != null)
            {
                if (alternativa == "+ Nueva alternativa")
                {
                    string tipo = alternativaActual.Tipo; // guardamos para no pierdala.
                    alternativaActual = new Alternativa();
                    alternativaActual.Tipo = tipo;
                }
                else
                {
                    AdminEncuesta ae = new AdminEncuesta();
                    alternativaActual = ae.ObtenerAlternativa(alternativa);
                }

                txtNombreAlternativa.Text = alternativaActual.Nombre;
                txtDescAlternativa.Text = alternativaActual.Descripcion;
                txtValorAlternativa.Text = alternativaActual.Valor.ToString();
            }
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
            btnOpcionesAvanzadas.IsEnabled = false;
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
            boxFP.Items.Clear();
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
            btnOpcionesAvanzadas.IsEnabled = true;
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

            boxFP.Items.Clear();
            foreach (KeyValuePair<string, ValorLinguistico> valor in variableActual.Valores)
            {
                boxFP.Items.Add(valor.Key);
            }
            boxFP.Items.Add("+ Agregar nueva función");
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

            if (valorActual.Nombre == "")
            {
                string nombreValor = txtNombreFP.Text;
                
                if ((string)boxTipoFP.SelectedValue == "Triangular")
                {
                    double valorIzq = Convert.ToDouble(txtValorIzquierda.Text);
                    double valorCentro = Convert.ToDouble(txtValorCentro.Text);
                    double valorDerch = Convert.ToDouble(txtValorDerecha.Text);
                    ald.InsertarValorLinguistico(nombreValor, variableActual.Nombre, tipoFuncion);
                    ald.InsertarFuncionTriangular(variableActual.Nombre, nombreValor, valorIzq, valorCentro, valorDerch);
                } else if ((string)boxTipoFP.SelectedValue == "Trapezoidal")
                {
                    tipoFuncion = "trapezoidal";
                    double valorIzqAbajo = Convert.ToDouble(txtValorIzquierdaAbajo.Text);
                    double valorIzqArriba = Convert.ToDouble(txtValorIzquierdaArriba.Text);
                    double valorDerchArriba = Convert.ToDouble(txtValorDerechaArriba.Text);
                    double valorDerchAbajo = Convert.ToDouble(txtValorDerechaAbajo.Text);
                    ald.InsertarValorLinguistico(nombreValor, variableActual.Nombre, tipoFuncion);
                    ald.InsertarFuncionTrapezoide(variableActual.Nombre, nombreValor, valorIzqAbajo, valorIzqArriba, valorDerchArriba, valorDerchAbajo);
                }
            } else
            {
                if ((string)boxTipoFP.SelectedValue == "Triangular")
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
                else if ((string)boxTipoFP.SelectedValue == "Trapezoidal")
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
            if (valor != null)
            {
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
                }
                else if (tipoFuncion.Equals(typeof(FuncionTrapezoidal)))
                {
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
                valorActual = new ValorLinguistico("", new FuncionTriangular(0, 0, 50));
                txtValorIzquierda.Text = "0";
                txtValorCentro.Text = "0";
                txtValorDerecha.Text = "50";
                boxTipoFP.SelectedValue = "Triangular";
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
            AdminLD al = new AdminLD();
            string tipoFuncion = ((string)boxTipoFP.SelectedValue).ToLower();

            al.EliminarValor(variableActual.Nombre, valorActual.Nombre, tipoFuncion);

            VolverOpcionesAvanzadas(null,null);
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

        private void IniciarPanelReglasSecciones()
        {
            tablaReglasSecciones.Items.Clear();
            foreach (KeyValuePair<string, Seccion> seccion in secciones)
            {
                tablaReglasSecciones.Items.Add(seccion.Value);
            }
        }

        private void IniciarPanelReglas()
        {
            if (reglasSeccionActual != null)
            {
                ObtenerReglasSeccion(reglasSeccionActual.IdSeccion.ToString(), PerfilConstantes.HB);
                LlenarTablaReglas(reglasActuales);
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
            reglasSeccionActual = (Seccion)tablaReglasSecciones.SelectedItem;
            IniciarPanelReglas();
            panelReglasSecciones.Visibility = Visibility.Hidden;
            panelReglasSeccion.Visibility = Visibility.Visible;
        }

        private void VolverReglasSecciones(object sender, RoutedEventArgs e)
        {
            panelReglasSeccion.Visibility = Visibility.Hidden;
            panelReglasSecciones.Visibility = Visibility.Visible;
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
            if (reglasSeccionActual != null)
            {
                if ((string)boxTipoReglas.SelectedValue == "Habilidades blandas")
                {
                    ObtenerReglasSeccion(reglasSeccionActual.IdSeccion.ToString(), PerfilConstantes.HB);
                    LlenarTablaReglas(reglasActuales);
                }
                else if ((string)boxTipoReglas.SelectedValue == "Habilidades duras")
                {
                    ObtenerReglasSeccion(reglasSeccionActual.IdSeccion.ToString(), PerfilConstantes.HD);
                    LlenarTablaReglas(reglasActuales);
                }
                else if ((string)boxTipoReglas.SelectedValue == "Caracteristicas fisicas")
                {
                    ObtenerReglasSeccion(reglasSeccionActual.IdSeccion.ToString(), PerfilConstantes.CF);
                    LlenarTablaReglas(reglasActuales);
                }
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
            string componente = (string)boxNoAntecedente.SelectedValue;
            VariableLinguistica variable = adminLD.ObtenerVariable(componente);

            if (variable != null)
            {
                boxValoresNoAntecedente.Items.Clear();
                foreach (KeyValuePair<string, ValorLinguistico> valor in variable.Valores)
                {
                    boxValoresNoAntecedente.Items.Add(valor.Key);
                }

                boxValoresNoAntecedente.SelectedIndex = 0;
            }
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

            if (nombreVariable.ToLower() == PerfilConstantes.HD)
                consecuente = vm.HDPerfil;
            else if (nombreVariable.ToLower() == PerfilConstantes.CF)
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

        public string IdAdministrador
        {
            get { return idAdministrador; }
            set { idAdministrador = value; }
        }

        private void IniciarEvaluacion(object sender, RoutedEventArgs e)
        {
            AdminPerfil ap = new AdminPerfil();
            AdminMatching am = new AdminMatching();
            Trabajador trabajador = listaDeTrabajadores[trabajadorSeleccionado];
            EvaluacionTrabajador et = new EvaluacionTrabajador(trabajador);

            Debug.WriteLine("Trabajador: " + trabajador.Rut);

            foreach (KeyValuePair<string, Seccion> seccion in secciones)
            {
                Perfil perfilEvaluado = EvaluacionPerfil.Ejecutar(seccion.Value.Perfil, seccion.Value.IdSeccion);

                perfilEvaluado.HB.Importancia = ap.ObtenerComponentePerfilSeccion(seccion.Value.IdSeccion, "HB").Importancia;
                perfilEvaluado.HD.Importancia = ap.ObtenerComponentePerfilSeccion(seccion.Value.IdSeccion, "HD").Importancia;
                perfilEvaluado.CF.Importancia = ap.ObtenerComponentePerfilSeccion(seccion.Value.IdSeccion, "CF").Importancia;

                Debug.WriteLine("Seccion : " + seccion.Value.Nombre);
                Debug.WriteLine("HB: " + perfilEvaluado.HB.Puntaje + " " + perfilEvaluado.HB.Importancia);
                Debug.WriteLine("HD: " + perfilEvaluado.HD.Puntaje + " " + perfilEvaluado.HD.Importancia);
                Debug.WriteLine("CF: " + perfilEvaluado.CF.Puntaje + " " + perfilEvaluado.CF.Importancia);

                // En caso de no existir en la bd insertamos las generales (HB, HD, CF)
                am.InsertarComponentes();
                // Insertamos tambien en caso de que no existan en la sección.
                am.InsertarComponente(seccion.Value.IdSeccion, "HB", perfilEvaluado.HB.Puntaje, perfilEvaluado.HB.Importancia);
                am.InsertarComponente(seccion.Value.IdSeccion, "HD", perfilEvaluado.HD.Puntaje, perfilEvaluado.HD.Importancia);
                am.InsertarComponente(seccion.Value.IdSeccion, "CF", perfilEvaluado.CF.Puntaje, perfilEvaluado.CF.Importancia);


                et.EvaluarCapacidad(perfilEvaluado, seccion.Value.IdSeccion);
                
                // Guardamos o actualizamos la capacidad del trabajador.
                am.InsertarCapacidad(trabajador.Rut, seccion.Value.IdSeccion, et.IgualdadHB, et.IgualdadHD, et.IgualdadCF, et.Capacidad);

                
                Debug.WriteLine("Igualdad HB: " + et.IgualdadHB);
                Debug.WriteLine("Igualdad HD: " + et.IgualdadHD);
                Debug.WriteLine("Igualdad CF: " + et.IgualdadCF);
                Debug.WriteLine("Capacidad: " + et.Capacidad);
            }
        }
    }
}
