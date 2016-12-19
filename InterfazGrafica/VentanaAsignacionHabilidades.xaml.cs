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
    /// Lógica de interacción para VentanaAsignacionHabilidades.xaml
    /// </summary>
    public partial class VentanaAsignacionHabilidades : MetroWindow
    {         
        private List<string> listaHabilidades;
        private List<Componente> listaDeTodasHabilidades;   
        private Dictionary<string, int> habilidadesSeleccionadas;
        private Dictionary<string, int> habilidadesApagadas;
        /*interaccion BD*/
        private InteraccionBD.InteraccionSecciones datosSeccion;
        /*variables*/
        private string tipoHabilidad;
        private int idSeccion;
        private Mensajes cuadroMensajes;
        private bool cambios;
        private bool confirmarCierre;
        

        public VentanaAsignacionHabilidades(string tipoHabilidad,int idSeccion)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            IniciarComponentes();
            this.tipoHabilidad = tipoHabilidad;
            this.idSeccion = idSeccion;
            GeneradorHabilidades();
        }

        private void IniciarComponentes()
        {
            cuadroMensajes = new Mensajes(this);
            datosSeccion = new InteraccionBD.InteraccionSecciones();
            listaDeTodasHabilidades = new List<Componente>();
            habilidadesSeleccionadas = new Dictionary<string, int>();
            habilidadesApagadas = new Dictionary<string, int>();
            tipoHabilidad = string.Empty;
            cambios = false;
            confirmarCierre = false;
        }
        /// <summary>
        /// Metodo que genera las habilidades referentes a la id de seccion recibida.
        /// </summary>
        private void GeneradorHabilidades()
        {
            /*todas las habilidades*/
            List<Componente> todasLasHabilidades = new List<Componente>();
            datosSeccion.IdSeccion = idSeccion;            

            int identificador = 0;
            if(tipoHabilidad.Equals("hd"))
            {                
                todasLasHabilidades.Clear();
                todasLasHabilidades = datosSeccion.HabilidadesDuras();//todas las hd
                listaHabilidades = datosSeccion.HabilidadesDurasPerfil();//hd del perfil
                foreach (Componente habilidad in todasLasHabilidades)
                {                    
                    VisorHabilidades habilidades = new VisorHabilidades( EncendidoApagado );               
                    habilidades.DescripcionHabilidades = habilidad.Nombre;                    
                    habilidades.Identificador = "I"+identificador;
                    habilidades.IdentificadorHabilidad = "I" + identificador;
                    Console.WriteLine("ESTADO1: " + habilidad.Estado);
                    if (listaHabilidades.Contains(habilidad.Nombre))
                    {
                        listaDeTodasHabilidades.Add(habilidad);
                        habilidades.Encendido = true;
                        habilidadesSeleccionadas.Add(habilidad.Nombre, idSeccion);                        
                    }
                    else
                    {
                        listaDeTodasHabilidades.Add(habilidad);
                        habilidades.Encendido = false;
                        habilidadesApagadas.Add(habilidad.Nombre, idSeccion);                       
                    } 
                    contenedorHabilidades.Children.Add(habilidades.ConstructorInfo());
                    identificador++;
                }
            }
                
            else if(tipoHabilidad.Equals("hb"))
            {
                todasLasHabilidades.Clear();                
                todasLasHabilidades = datosSeccion.HabilidadesBlandas();
                listaHabilidades = datosSeccion.HabilidadesBlandasPerfil();
                foreach (Componente habilidad in todasLasHabilidades)
                {                   
                    VisorHabilidades habilidades = new VisorHabilidades(EncendidoApagado);
                    habilidades.DescripcionHabilidades = habilidad.Nombre;                   
                    habilidades.Identificador = "I" + identificador;
                    habilidades.IdentificadorHabilidad = "I" + identificador;
                    Console.WriteLine("ESTADO2: " + habilidad.Estado);
                    if (listaHabilidades.Contains(habilidad.Nombre))
                    {
                        listaDeTodasHabilidades.Add(habilidad);
                        habilidades.Encendido = true;
                        habilidadesSeleccionadas.Add(habilidad.Nombre, idSeccion);                        
                    }

                    else
                    {
                        listaDeTodasHabilidades.Add(habilidad);
                        habilidades.Encendido = false;
                        habilidadesApagadas.Add(habilidad.Nombre, idSeccion);                       
                    }                        
                    contenedorHabilidades.Children.Add(habilidades.ConstructorInfo());
                    identificador++;
                }
            }

                
            else if (tipoHabilidad.Equals("cf"))
            {
                todasLasHabilidades.Clear();                
                todasLasHabilidades = datosSeccion.CaracteristicasFisicas();
                listaHabilidades = datosSeccion.CaracteristicasFisicasPerfil();
                foreach (Componente habilidad in todasLasHabilidades)
                {                   
                    VisorHabilidades habilidades = new VisorHabilidades(EncendidoApagado);
                    habilidades.DescripcionHabilidades = habilidad.Nombre;                    
                    habilidades.Identificador = "I" + identificador;
                    habilidades.IdentificadorHabilidad = "I" + identificador;
                    Console.WriteLine("ESTADO3: "+habilidad.Estado);
                    if (listaHabilidades.Contains(habilidad.Nombre))
                    {
                        listaDeTodasHabilidades.Add(habilidad);
                        habilidades.Encendido = true;
                        habilidadesSeleccionadas.Add(habilidad.Nombre, idSeccion);                        
                    }
                    else
                    {
                        listaDeTodasHabilidades.Add(habilidad);
                        habilidades.Encendido = false;
                        habilidadesApagadas.Add(habilidad.Nombre, idSeccion);                        
                    } 
                    contenedorHabilidades.Children.Add(habilidades.ConstructorInfo());
                    identificador++;
                }
            }
               
        }

        private void DeterminaHabilidadesHabilitadas()
        {
            
        }
        /// <summary>
        /// Controlador que activa/desactiva la habilidad seleccionada.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EncendidoApagado(object sender, EventArgs e)
        {
            cambios = true;
            var elementoSeleccionado = sender as ToggleSwitch;
            string[] indiceEtiqueta = elementoSeleccionado.Name.Split('I');            
            int indice = Convert.ToInt32(indiceEtiqueta[1]);
           
           if (elementoSeleccionado.IsChecked == true)
            {
                habilidadesSeleccionadas.Add(listaDeTodasHabilidades[indice].Nombre, idSeccion);
                habilidadesApagadas.Remove(listaDeTodasHabilidades[indice].Nombre);                
            }
           else
           {
                habilidadesApagadas.Add(listaDeTodasHabilidades[indice].Nombre, idSeccion);
                habilidadesSeleccionadas.Remove(listaDeTodasHabilidades[indice].Nombre);               
           }
               

        }
        /// <summary>
        /// Agrega o elimina las habilidades que componen el perfil de seccion, segun sea el caso.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async private void CerrarVentana(object sender, CancelEventArgs e)
        {
            if (!confirmarCierre)
            {
                e.Cancel = true;
                if (cambios)
                {
                    if (MessageDialogResult.Affirmative.Equals(await cuadroMensajes.GuardarCambiosAsignacionHabilidades()))
                    {
                        foreach (KeyValuePair<string, int> preguntas in habilidadesSeleccionadas)
                        {
                            foreach (Componente id in listaDeTodasHabilidades)
                            {
                                if (id.Nombre.Equals(preguntas.Key))
                                    datosSeccion.GuardarComponentesPerfil(preguntas.Value, id.ID);
                            }

                        }
                        foreach (KeyValuePair<string, int> preguntas in habilidadesApagadas)
                        {
                            foreach (Componente id in listaDeTodasHabilidades)
                            {
                                if (id.Nombre.Equals(preguntas.Key))
                                    datosSeccion.EliminarComponentesPerfil(preguntas.Value, id.ID);
                            }
                        }
                        confirmarCierre = true;
                        this.Close();
                    }
                    else e.Cancel = false;
                }
                else e.Cancel = false; 
            }
        }
        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }

        public string TipoHabilidad
        {
            get { return tipoHabilidad; }
            set { tipoHabilidad = value; }
        }
    }
}
