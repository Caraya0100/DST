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
        private string consultaEliminarSeccion;
        private string seccionEliminada;
        private string rechazarSolicitud;
        private string solicitudRechazada;
        private string confirmarSolicitud;
        private string solicitudAprobada;
        private string agregarNuevaSeccion;
        private string nuevaSeccionAgregada;
        private string camposIncompletosSeccion;
        private string agregarNuevoUsuario;
        private string nuevoUsuarioAgregado;
        private string rutInvalido;
        private string passwordInvalida;
        private string eliminarUsuario;
        private string usuarioEliminado;
        private string cambiosGuardados;
        private VentanaJefeSeccion ventanaPrincipal;
        private VentanaAgregarTrabajador ventanaTrabajador;
        private VentanaEncuesta ventanaEncuesta;
        private VentanaLogin ventanaLogin;
        private VentanaAdministrador ventanaAdministrador;
        private VentanaAgregarSeccion ventanaAgregarSeccion;
        private VentanaAgregarUsuario ventanaAgregarUsuario;
        private VentanaAsignacionHabilidades ventanaAsignacionHabilidades;


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

        public Mensajes(VentanaAdministrador ventana)
        {
            inicializacionVariables();
            this.ventanaAdministrador = ventana;
            MensajesPorDefecto();
        }

        public Mensajes(VentanaAgregarSeccion ventana)
        {
            inicializacionVariables();
            this.ventanaAgregarSeccion = ventana;
            MensajesPorDefecto();
        }

        public Mensajes(VentanaAgregarUsuario ventana)
        {
            inicializacionVariables();
            this.ventanaAgregarUsuario = ventana;
            MensajesPorDefecto();
        }

        public Mensajes(VentanaAsignacionHabilidades ventana)
        {
            inicializacionVariables();
            this.ventanaAsignacionHabilidades = ventana;
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
            this.consultaEliminarSeccion = string.Empty;
            this.seccionEliminada = string.Empty;
            this.rechazarSolicitud = string.Empty;
            this.solicitudRechazada = string.Empty;
            this.confirmarSolicitud = string.Empty;
            this.solicitudAprobada = string.Empty;
            this.agregarNuevaSeccion = string.Empty;
            this.nuevaSeccionAgregada = string.Empty;
            this.camposIncompletos = string.Empty;
            this.agregarNuevoUsuario = string.Empty;
            this.nuevoUsuarioAgregado = string.Empty;
            this.rutInvalido = string.Empty;
            this.passwordInvalida = string.Empty;
            this.eliminarUsuario = string.Empty;
            this.usuarioEliminado = string.Empty;
            this.cambiosGuardados = string.Empty;
        }


        private void MensajesPorDefecto()
        {
            this.nombreSoftware = "DST Software";
            this.trabajadorEliminado = "El trabajador se ha Eliminado Exitosamente.";
            this.consultaEliminarTrabajador = "¿Está seguro de que desea eliminar al trabajador del software?";
            this.consultaSolicitudTrabajador = "¿Está seguro que desea solicitar la reubicación del trabajador?";
            this.trabajadorSolicitado = "La solicitud se ha realizado Exitosamente.";
            this.camposIncompletos = "Debe completar todos los Campos correctamente para ingresar al Trabajador.";
            this.trabajadorAgregado = "Los datos del Trabajador han sido Agregados Correctamente.";
            this.claveIncorrecta = "Contraseña incorrecta, Vuelva a intentarlo";
            this.trabajadorNoSeleccionado = "Debe seleccionar algun Trabajador para iniciar la evaluación.";
            this.encuestaFinalizadaGuardarYSalir = "Ha respondido toda la evaluación, ¿Desea Guardar y salir?.";
            this.encuestaGuardadaExitosamente = "La Evaluación ha sido Guardada exitosamente.";
            this.cerrarSesion = "¿Está seguro que desea cerrar DST software?";
            this.cerrarEncuesta = "Antes de salir debe completar la evaluación.";
            this.guardarCambiosPerfilAlSalir = "¿Desea Guardar los cambios realizados en el Perfil antes de salir?.";
            this.guardarCambiosPerfil = "¿Desea Guardar los cambios realizados?.";
            this.verificacionUsuarioIncorrecta = "Usuario o Contraseña incorrectos, Vuelva a intentarlo.";
            this.consultaEliminarSeccion = "¿Está seguro que desea eliminar la sección?.";
            this.seccionEliminada = "La sección ha sido Eliminada Exitosamente.";
            this.rechazarSolicitud = "¿Está seguro que desea rechazar la solicitud?.";
            this.solicitudRechazada = "La solicitud ha sido Descartada.";
            this.confirmarSolicitud = "¿Desea confirmar la solicitud?.";
            this.solicitudAprobada = "El trabajador ha sido reubicado exitosamente.";
            this.agregarNuevaSeccion = "¿Está seguro que desea guardar los datos de la nueva sección?.";
            this.nuevaSeccionAgregada = "Los datos de la sección han sido agregados correctamente.";
            this.camposIncompletosSeccion = "Debe completar todos los campos para guardar.";
            this.agregarNuevoUsuario = "¿Está seguro que desea guardar los datos del nuevo usuario?";
            this.nuevoUsuarioAgregado = "El nuevo usuario ha sido agregado exitosamente.";
            this.rutInvalido = "Debe ingresar un rut valido.";
            this.passwordInvalida = "Ingrese contraseña correctamente.";
            this.eliminarUsuario = "¿Está seguro que desea eliminar la cuenta de usuario?.";
            this.usuarioEliminado = "La cuenta del usuario ha sido eliminada exitosamente.";
            this.cambiosGuardados = "Los cambios han sido guardados correctamente.";
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

        async public Task<MessageDialogResult> CerrarSesionAdministrador()
        {
            MessageDialogResult resultado = await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, cerrarSesion, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public Task<MessageDialogResult> ConsultaEliminarTrabajadorAdmin()
        {
            MessageDialogResult resultado = await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, consultaEliminarTrabajador, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }
        async public void TrabajadorEliminadoAdmin()
        {
            await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, trabajadorEliminado);
        }

        async public Task<MessageDialogResult> ConsultaEliminarSeccion()
        {
            MessageDialogResult resultado = await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, consultaEliminarSeccion, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public void SeccionEliminada()
        {
            await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, seccionEliminada);
        }

        async public Task<MessageDialogResult> RechazarSolicitud()
        {
            MessageDialogResult resultado = await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, rechazarSolicitud, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public void SolicitudRechazada()
        {
            await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, solicitudRechazada);
        }

        async public Task<MessageDialogResult> ConfirmarSolicitud()
        {
            MessageDialogResult resultado = await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, confirmarSolicitud, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public void SolicitudConfirmada()
        {
            await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, solicitudAprobada);
        }

        async public Task<MessageDialogResult> AgregarNuevaSeccion()
        {
            MessageDialogResult resultado = await this.ventanaAgregarSeccion.ShowMessageAsync(nombreSoftware, agregarNuevaSeccion, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public void NuevaSeccionAgregada()
        {
            await this.ventanaAgregarSeccion.ShowMessageAsync(nombreSoftware, nuevaSeccionAgregada);
        }

        async public void CamposIncompletosSeccion()
        {
            await this.ventanaAgregarSeccion.ShowMessageAsync(nombreSoftware, camposIncompletosSeccion);
        }

        async public Task<MessageDialogResult> AgregarNuevoUsuario()
        {
            MessageDialogResult resultado = await this.ventanaAgregarUsuario.ShowMessageAsync(nombreSoftware, agregarNuevoUsuario, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public void NuevoUsuarioAgregado()
        {
            await this.ventanaAgregarUsuario.ShowMessageAsync(nombreSoftware, nuevoUsuarioAgregado);
        }

        async public void CamposIncompletosUsuario()
        {
            await this.ventanaAgregarUsuario.ShowMessageAsync(nombreSoftware, camposIncompletosSeccion);
        }

        async public void RutInvalido()
        {
            await this.ventanaAgregarUsuario.ShowMessageAsync(nombreSoftware, rutInvalido);
        }

        async public void PasswordInvalida()
        {
            await this.ventanaAgregarUsuario.ShowMessageAsync(nombreSoftware, passwordInvalida);
        }

        async public Task<MessageDialogResult> EliminarUsuario()
        {
            MessageDialogResult resultado = await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, eliminarUsuario, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public void UsuarioEliminado()
        {
            await this.ventanaAdministrador.ShowMessageAsync(nombreSoftware, usuarioEliminado);
        }

        async public Task<MessageDialogResult> GuardarCambiosAsignacionHabilidades()
        {
            MessageDialogResult resultado = await this.ventanaAsignacionHabilidades.ShowMessageAsync(nombreSoftware, guardarCambiosPerfil, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        /// <summary>
        /// Muestra un dialogo para confirmar (botones si/no).
        /// </summary>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        async public Task<MessageDialogResult> Confirmar(string mensaje, string subMensaje)
        {
            MessageDialogResult resultado = await this.ventanaAdministrador.ShowMessageAsync(mensaje, subMensaje, MessageDialogStyle.AffirmativeAndNegative);
            return resultado;
        }

        async public void CambiosGuardados()
        {
            await this.ventanaPrincipal.ShowMessageAsync(nombreSoftware, cambiosGuardados);
        }
    }
}
