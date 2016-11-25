using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Behaviours;

namespace InterfazGrafica
{
    class Mensajes : MetroWindow
    {
        private string nombreSoftware;
        private string trabajadorEliminado;
        private string consultaEliminarTrabajador;
        private string consultaSolicitudTrabajador;
        private string trabajadorSolicitado;
        private string camposIncompletos;
        private string trabajadorAgregado;
        private string claveIncorrecta;
        private string trabajadorNoSeleccionado;
        private string encuestaFinalizadaGuardarYSalir;
        private string encuestaGuardadaExitosamente;
        private string cerrarSesion;
        private string cerrarEncuesta;
        private string guardarCambiosPerfilAlSalir;
        private string guardarCambiosPerfil;
        private string verificacionUsuarioIncorrecta;
        private VentanaJefeSeccion ventanaPrincipal;
        private VentanaAgregarTrabajador ventanaTrabajador;
        private VentanaEncuesta ventanaEncuesta;
        private VentanaLogin ventanaLogin;

        public Mensajes(VentanaJefeSeccion ventana)
        {
            inicializacionVariables();
            this.ventanaPrincipal = ventana;
            MensajesPorDefecto();
        }

        public Mensajes(VentanaAgregarTrabajador ventana)
        {
            inicializacionVariables();
            this.ventanaTrabajador = ventana;
            MensajesPorDefecto();
        }

        public Mensajes(VentanaEncuesta ventana)
        {
            inicializacionVariables();
            this.ventanaEncuesta = ventana;
            MensajesPorDefecto();
        }
        public Mensajes(VentanaLogin ventana)
        {
            inicializacionVariables();
            this.ventanaLogin = ventana;
            MensajesPorDefecto();
        }
        private void inicializacionVariables()
        {
            this.nombreSoftware = string.Empty;
            this.trabajadorEliminado = string.Empty;
            this.consultaEliminarTrabajador = string.Empty;
            this.consultaSolicitudTrabajador = string.Empty;
            this.trabajadorSolicitado = string.Empty;
            this.camposIncompletos = string.Empty;
            this.trabajadorAgregado = string.Empty;
            this.claveIncorrecta = string.Empty;
            this.trabajadorNoSeleccionado = string.Empty;
            this.encuestaFinalizadaGuardarYSalir = string.Empty;
            this.encuestaGuardadaExitosamente = string.Empty;
            this.cerrarSesion = string.Empty;
            this.cerrarEncuesta = string.Empty;
            this.guardarCambiosPerfilAlSalir = string.Empty;
            this.guardarCambiosPerfil = string.Empty;
            this.verificacionUsuarioIncorrecta = string.Empty;
        }


        private void MensajesPorDefecto()
        {
            this.nombreSoftware = "DST Software";
            this.trabajadorEliminado = "El trabajador se ha Eliminado Exitosamente.";
            this.consultaEliminarTrabajador = "¿Está seguro de que desea eliminar al trabajador del software?";
            this.consultaSolicitudTrabajador = "¿Está seguro que desea solicitar la reubicación del trabajador?";
            this.trabajadorSolicitado = "La solicitud se ha realizado Exitosamente.";
            this.camposIncompletos = "Debe completar todos los Campos correctamente para ingresar al Trabajador.";
            this.trabajadorAgregado = "Los datos del nuevo Trabajador han sido Agregados Correctamente.";
            this.claveIncorrecta = "Contraseña incorrecta, Vuelva a intentarlo";
            this.trabajadorNoSeleccionado = "Debe seleccionar algun Trabajador para iniciar la evaluación.";
            this.encuestaFinalizadaGuardarYSalir = "Ha respondido toda la evaluación, ¿Desea Guardar y salir?.";
            this.encuestaGuardadaExitosamente = "La Evaluación ha sido Guardada exitosamente.";
            this.cerrarSesion = "¿Está seguro que desea cerrar DST software?";
            this.cerrarEncuesta = "Antes de salir debe completar la evaluación.";
            this.guardarCambiosPerfilAlSalir = "¿Desea Guardar los cambios realizados en el Perfil antes de salir?.";
            this.guardarCambiosPerfil = "¿Desea Guardar los cambios realizados?.";
            this.verificacionUsuarioIncorrecta = "Usuario o Contraseña incorrectos, Vuelva a intentarlo.";
        }

        async public Task<MessageDialogResult> ConsultaEliminarTrabajador()
        {
            MessageDialogResult resultado = await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, consultaEliminarTrabajador, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }
        async public void TrabajadorEliminado()
        {
            await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, trabajadorEliminado);
        }

        async public Task<MessageDialogResult> ConsultaSolicitudTrabajador()
        {
            MessageDialogResult resultado = await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, consultaSolicitudTrabajador, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public void TrabajadorSolicitado()
        {
            await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, trabajadorSolicitado);
        }

        async public void CamposIncompletos()
        {
            await this.ventanaTrabajador.ShowMessageAsync(nombreSoftware, camposIncompletos);
        }
        async public void NuevoTrabajadorAgregado()
        {
            await this.ventanaTrabajador.ShowMessageAsync(nombreSoftware, trabajadorAgregado);
        }
        async public void ClaveIncorrecta()
        {
            await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, claveIncorrecta);
        }

        async public void TrabajadorNoSeleccionado()
        {
            await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, trabajadorNoSeleccionado);
        }

        async public Task<MessageDialogResult> EncuestaFinalizadaGuardarYSalir()
        {
            MessageDialogResult resultado = await this.ventanaEncuesta.ShowMessageAsync(nombreSoftware, encuestaFinalizadaGuardarYSalir, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public Task<MessageDialogResult> EncuestaGuardadaExitosamente()
        {
            MessageDialogResult resultado = await this.ventanaEncuesta.ShowMessageAsync(nombreSoftware, encuestaGuardadaExitosamente, MessageDialogStyle.Affirmative);
            return resultado;
        }

        async public Task<MessageDialogResult> CerrarSesion()
        {
            MessageDialogResult resultado = await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, cerrarSesion, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public Task<MessageDialogResult> CerrarEncuesta()
        {
            MessageDialogResult resultado = await this.ventanaEncuesta.ShowMessageAsync(nombreSoftware, cerrarEncuesta, MessageDialogStyle.Affirmative);
            return resultado;
        }

        async public Task<MessageDialogResult> GuardarCambiosPerfilAlSalir()
        {
            MessageDialogResult resultado = await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, guardarCambiosPerfilAlSalir, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public Task<MessageDialogResult> GuardarCambiosPerfil()
        {
            MessageDialogResult resultado = await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, guardarCambiosPerfil, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public Task<MessageDialogResult> VerificacionUsuarioIncorrecta()
        {
            MessageDialogResult resultado = await this.ventanaLogin.ShowMessageAsync(nombreSoftware, verificacionUsuarioIncorrecta, MessageDialogStyle.Affirmative);
            return resultado;
        }
    }
}
