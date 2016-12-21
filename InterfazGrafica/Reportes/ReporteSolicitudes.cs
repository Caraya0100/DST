using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using DST;

namespace InterfazGrafica.Reportes
{
    class ReporteSolicitudes
    {
        private string rutaFichero;
        private Document reporte;
        private Fuentes fuente;
        private AdminDesempeño datosDesempeno;
        private InteraccionBD.InteraccionSecciones datosSeccion;
        private InteraccionBD.InteraccionTrabajadores datosTrabajador;
        private List<Solicitud> solicitudes;
        public ReporteSolicitudes()
        {
            IniciarComponentes();
        }

        private void IniciarComponentes()
        {
            this.rutaFichero = string.Empty;
            fuente = new Fuentes();
            datosDesempeno = new AdminDesempeño();
            solicitudes = datosDesempeno.ObtenerSolicitudes();
            datosSeccion = new InteraccionBD.InteraccionSecciones();
            datosTrabajador = new InteraccionBD.InteraccionTrabajadores();
            
        }

        public void GenerarReporte()
        {
            this.reporte = CrearPDF();
            reporte.Open();
            FormatoPortada();
            Titulo();
            InformacionSolicitud();
            reporte.Close();
        }

        public Document CrearPDF()
        {
            Document documento = new Document(iTextSharp.text.PageSize.LETTER, 10, 10, 42, 35);
            PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(rutaFichero, FileMode.Create));
            return documento;
        }

        private void FormatoPortada()
        {
            //reporte.Open();
            Paragraph espacios = new Paragraph("\n\n\n", fuente.Cursiva(24));
            espacios.Alignment = Element.ALIGN_CENTER;
            reporte.Add(espacios);
            /*Logo del software*/
            iTextSharp.text.Image Logo = iTextSharp.text.Image.GetInstance(@"..\..\Iconos\logotipo.png");
            Logo.Alignment = Element.ALIGN_CENTER;
            reporte.Add(Logo);
            /*Titulo*/
            Paragraph separador = new Paragraph("_______________________________", fuente.Delineado(24));
            separador.Alignment = Element.ALIGN_CENTER;
            reporte.Add(separador);

            Paragraph titulo = new Paragraph("D  S  T", fuente.Cursiva(24));
            titulo.Alignment = Element.ALIGN_CENTER;
            reporte.Add(titulo);

            Paragraph nombre_software = new Paragraph("Determinación de Sección de Trabajo", fuente.Cursiva(18));
            nombre_software.Alignment = Element.ALIGN_CENTER;
            reporte.Add(nombre_software);
            //reporte.Close();
        }

        private void Titulo()
        {
            reporte.NewPage();
            Paragraph titulo = new Paragraph("HISTORIAL DE SOLICITUDES", fuente.Subrayado(24));
            titulo.Alignment = Element.ALIGN_CENTER;
            reporte.Add(titulo);
        }

        private void InformacionSolicitud()
        {
            Paragraph espacios = new Paragraph("\n\n", fuente.Normal(18));
            reporte.Add(espacios);
            
            /*datos de prueba*/
            foreach (Solicitud sol in solicitudes)
            {
                PdfPTable tablaSolicitudes = new PdfPTable(2);
                tablaSolicitudes.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell seccionEtiqueta = new PdfPCell(new Paragraph("SECCION SOLICITANTE", fuente.Cursiva(12)));
                PdfPCell jefeSeccionEtiqueta = new PdfPCell(new Paragraph("JEFE DE SECCION", fuente.Cursiva(12)));
                PdfPCell fechaEtiqueta = new PdfPCell(new Paragraph("FECHA", fuente.Cursiva(12)));
                PdfPCell trabajadorEtiqueta = new PdfPCell(new Paragraph("NOMBRE TRABAJADOR", fuente.Cursiva(12)));
                PdfPCell seccionActualEtiqueta = new PdfPCell(new Paragraph("SECCION ACTUAL", fuente.Cursiva(12)));
                PdfPCell capacidadActualEtiqueta = new PdfPCell(new Paragraph("CAPACIDAD EN SECCION ACTUAL", fuente.Cursiva(12)));
                PdfPCell capacidadNuevaEtiqueta = new PdfPCell(new Paragraph("CAPACIDAD SECCION SOLICITANTE", fuente.Cursiva(12)));
                PdfPCell estadoEtiqueta = new PdfPCell(new Paragraph("ESTADO DE LA SOLICITUD", fuente.Cursiva(12)));
                /*alineaciones*/
                seccionEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
                jefeSeccionEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
                fechaEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
                trabajadorEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
                seccionActualEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
                capacidadActualEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
                capacidadNuevaEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
                estadoEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
                /*colores*/
                seccionEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
                jefeSeccionEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
                fechaEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
                trabajadorEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
                seccionActualEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
                capacidadNuevaEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
                capacidadActualEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
                estadoEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);

                PdfPCell seccion = new PdfPCell(new Paragraph(datosSeccion.NombreSeccionPorId(sol.IdSeccionSolicitada), fuente.Normal(12)));
                PdfPCell jefeSeccion = new PdfPCell(new Paragraph("", fuente.Normal(12)));
                PdfPCell fecha = new PdfPCell(new Paragraph(sol.FechaSolicitud, fuente.Normal(12)));
                PdfPCell trabajador = new PdfPCell(new Paragraph(datosTrabajador.NombreTrabajadorPorRut(sol.RutSolicitud), fuente.Normal(12)));
                PdfPCell seccionActual = new PdfPCell(new Paragraph(datosSeccion.NombreSeccionPorId(sol.IdSeccionActual), fuente.Normal(12)));
                PdfPCell capacidadActual = new PdfPCell(new Paragraph(""+sol.CapacidadSeccionActual, fuente.Normal(12)));
                PdfPCell capacidadNueva = new PdfPCell(new Paragraph(""+sol.CapacidadNuevaSeccion, fuente.Normal(12)));
                PdfPCell estado = new PdfPCell(new Paragraph(sol.EstadoSolicitud, fuente.Normal(12)));

                seccion.HorizontalAlignment = Element.ALIGN_CENTER;
                jefeSeccion.HorizontalAlignment = Element.ALIGN_CENTER;
                fecha.HorizontalAlignment = Element.ALIGN_CENTER;
                trabajador.HorizontalAlignment = Element.ALIGN_CENTER;
                seccionActual.HorizontalAlignment = Element.ALIGN_CENTER;
                capacidadActual.HorizontalAlignment = Element.ALIGN_CENTER;
                capacidadNueva.HorizontalAlignment = Element.ALIGN_CENTER;
                estado.HorizontalAlignment = Element.ALIGN_CENTER;

                /*se agregan las celdas a la tabla*/
                tablaSolicitudes.AddCell(seccionEtiqueta); tablaSolicitudes.AddCell(seccionActual);
                //tablaSolicitudes.AddCell(jefeSeccionEtiqueta); tablaSolicitudes.AddCell(jefeSeccion);
                tablaSolicitudes.AddCell(fechaEtiqueta); tablaSolicitudes.AddCell(fecha);
                tablaSolicitudes.AddCell(trabajadorEtiqueta); tablaSolicitudes.AddCell(trabajador);
                tablaSolicitudes.AddCell(seccionActualEtiqueta); tablaSolicitudes.AddCell(seccionActual);
                tablaSolicitudes.AddCell(capacidadActualEtiqueta); tablaSolicitudes.AddCell(capacidadActual);
                tablaSolicitudes.AddCell(capacidadNuevaEtiqueta); tablaSolicitudes.AddCell(capacidadNueva);
                tablaSolicitudes.AddCell(estadoEtiqueta); tablaSolicitudes.AddCell(estado);                

                reporte.Add(tablaSolicitudes);
                reporte.Add(espacios);
            }
        }

        public string RutaFichero
        {
            get { return rutaFichero; }
            set { rutaFichero = value; }
        }
    }
}
