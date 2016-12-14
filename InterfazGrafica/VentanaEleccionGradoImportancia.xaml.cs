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
    /// Lógica de interacción para VentanaEleccionGradoImportancia.xaml
    /// </summary>
    public partial class VentanaEleccionGradoImportancia : MetroWindow
    {
        private VentanaJefeSeccion ventanaJefe;
        private int idSeccion;
        public VentanaEleccionGradoImportancia(VentanaJefeSeccion ventana)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen; 
            this.ventanaJefe = ventana;
        }

        private void Manual(object sender, RoutedEventArgs e)
        {
            HabilitaEdicion();
            HabilitaItemHabilidades();
            this.Close();
        }

        private void Automatico(object sender, RoutedEventArgs e)
        {           
            DeshabilitaEdicion();
            this.Close();
            VentanaEncuesta encuesta = new VentanaEncuesta(idSeccion);
            encuesta.ShowDialog();
            /*agregar las preguntas*/
        }

        private void DeshabilitaItemHabilidades()
        {
            this.ventanaJefe.pestania_CF.IsEnabled = false;
            this.ventanaJefe.pestania_HB.IsEnabled = false;
            this.ventanaJefe.pestania_HD.IsEnabled = false;            
        }

        private void HabilitaItemHabilidades()
        {
            this.ventanaJefe.pestania_CF.IsEnabled = true;
            this.ventanaJefe.pestania_HB.IsEnabled = true;
            this.ventanaJefe.pestania_HD.IsEnabled = true;
        }

        private void HabilitaEdicion()
        {
            this.ventanaJefe.scroll_CF.IsEnabled = true;
            this.ventanaJefe.scroll_HB.IsEnabled = true;
            this.ventanaJefe.scroll_HD.IsEnabled = true;
        }

        private void DeshabilitaEdicion()
        {
            this.ventanaJefe.scroll_CF.IsEnabled = false;
            this.ventanaJefe.scroll_HB.IsEnabled = false;
            this.ventanaJefe.scroll_HD.IsEnabled = false;
        }

        public int IdSeccion
        {
            get { return idSeccion; }
            set { idSeccion = value; }
        }
    }
}
