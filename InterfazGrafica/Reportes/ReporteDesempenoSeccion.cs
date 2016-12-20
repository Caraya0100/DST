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
    public class ReporteDesempenoSeccion
    {
        private string rutaFichero;
        private Document reporte;
        private Fuentes fuente;
        private MemoryStream buffer;
        private Seccion seccion;
        private int anio;
        Tuple<double, double> desempeno;

        public ReporteDesempenoSeccion(Seccion seccion, int anio)
        {
            this.seccion = seccion;
            this.anio = anio;
            IniciarComponentes();
        }

        private void IniciarComponentes()
        {
            this.rutaFichero = string.Empty;
            fuente = new Fuentes();
        }

        private void DesempenoVentasSeccion()
        {
            desempeno = EvaluacionDesempeno.Ejecutar(seccion.VentasActuales, seccion.VentasAnioAnterior, seccion.VentasPlan);
        }

        public void GenerarReporte()
        {
            this.reporte = CrearPDF();
            reporte.Open();
            FormatoPortada();
            if (seccion.Tipo.ToLower() == "ventas")
            {
                DesempenoVentasSeccion();
                TablaDesempenoVentas();
            }
            if (seccion.Tipo.ToLower() == "gqm")
            {
                TablaDesempenoGqm();
            }
            reporte.Close();
        }
        /// <summary>
        /// Crea el archivo PDF a utilizar en la ruta especificada.
        /// </summary>
        /// <param name="rutaFichero"></param>
        /// <returns></returns>
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


        private void TablaDesempenoVentas()
        {
            AdminDesempeño ad = new AdminDesempeño();
            List<int> meses = ad.ObtenerMesesAnio(seccion.IdSeccion, anio, seccion.Tipo);

            reporte.NewPage();
            string textoTitulo = "DESEMPEÑO " + anio + " SECCION: " + seccion.Nombre;
            Paragraph titulo = new Paragraph(textoTitulo, fuente.Subrayado(20));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            titulo.Alignment = Element.ALIGN_CENTER;

            PdfPTable table = new PdfPTable(6);
            PdfPCell cell = new PdfPCell(new Phrase("Ventas Sección"));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            table.AddCell("Mes");
            table.AddCell("Ventas mes ($)");
            table.AddCell("Ventas año anterior($)");
            table.AddCell("Ventas plan($)");
            table.AddCell("Desempeño mes/anterior(&)");
            table.AddCell("Desempeño mes/plan(%)");

            foreach (int mes in meses)
            {
                AgregarVentasMes(mes, table);
            }

            reporte.Add(table);
            reporte.NewPage();

            table = new PdfPTable(6);
            cell = new PdfPCell(new Phrase("Datos Trabajadores"));
            cell.Colspan = 6;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            table.AddCell("Mes");
            table.AddCell("Total");
            table.AddCell("No capacitados");
            table.AddCell("Capacitados");
            //table.AddCell("Capacitados (%)");
            table.AddCell("Desempeño mes/anterior(&)");
            table.AddCell("Desempeño mes/plan(%)");

            foreach (int mes in meses)
            {
                AgregarTrabajadoresMes(mes, table);
            }

            reporte.Add(table);
        }

        private void AgregarVentasMes(int mes, PdfPTable tabla)
        {
            AdminDesempeño ad = new AdminDesempeño();
            Tuple<double, double, double> ventas = ad.ObtenerVentasMes(seccion.IdSeccion, mes, anio);

            tabla.AddCell(mes.ToString());
            tabla.AddCell(ventas.Item1.ToString());
            tabla.AddCell(ventas.Item2.ToString());
            tabla.AddCell(ventas.Item3.ToString());
            tabla.AddCell(desempeno.Item1.ToString());
            tabla.AddCell(desempeno.Item2.ToString());
        }

        private void AgregarTrabajadoresMes(int mes, PdfPTable tabla)
        {
            AdminDesempeño ad = new AdminDesempeño();
            int reubicaciones = ad.ObtenerReubicacionesMes(seccion.IdSeccion, mes, anio);
            int total = ad.ObtenerTotalEmpleadosMes(seccion.IdSeccion, mes, anio, seccion.Tipo);
            int capacitados = ad.ObtenerEmpleadosCapacitadosMes(seccion.IdSeccion, mes, anio, seccion.Tipo);
            int noCapacitados = ad.ObtenerEmpleadosNoCapacitadosMes(seccion.IdSeccion, mes, anio, seccion.Tipo);
            //double porcentajeCapacitados = (capacitados / total) * 100;

            tabla.AddCell(mes.ToString());
            tabla.AddCell(total.ToString());
            tabla.AddCell(noCapacitados.ToString());
            tabla.AddCell(capacitados.ToString());
            //tabla.AddCell(porcentajeCapacitados.ToString());
            tabla.AddCell(desempeno.Item1.ToString());
            tabla.AddCell(desempeno.Item2.ToString());
        }

        private void TablaDesempenoGqm()
        {
            AdminDesempeño ad = new AdminDesempeño();
            List<int> meses = ad.ObtenerMesesAnio(seccion.IdSeccion, anio, seccion.Tipo);

            reporte.NewPage();
            string textoTitulo = "DESEMPEÑO " + anio + " SECCION: " + seccion.Nombre;
            Paragraph titulo = new Paragraph(textoTitulo, fuente.Subrayado(20));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            titulo.Alignment = Element.ALIGN_CENTER;

            PdfPTable table = new PdfPTable(2);
            PdfPCell cell = new PdfPCell(new Phrase("Ventas Sección"));
            cell.Colspan = 2;
            cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(cell);

            table.AddCell("Mes");
            table.AddCell("Desempeno (%)");

            foreach (int mes in meses)
            {
                AgregarDesempenoGqmMes(mes, table);
            }

            reporte.Add(table);
            reporte.NewPage();

            table = new PdfPTable(4);
            cell = new PdfPCell(new Phrase("Datos Trabajadores"));
            cell.Colspan = 4;
            cell.HorizontalAlignment = 1;
            table.AddCell(cell);

            table.AddCell("Mes");
            table.AddCell("Total");
            table.AddCell("No capacitados");
            table.AddCell("Capacitados");
            //table.AddCell("Capacitados (%)");

            foreach (int mes in meses)
            {
                AgregarTrabajadoresMesGqm(mes, table);
            }

            reporte.Add(table);
        }

        private void AgregarDesempenoGqmMes(int mes, PdfPTable tabla)
        {
            AdminDesempeño ad = new AdminDesempeño();
            double desempeno = ad.ObtenerDesempenoGqm(seccion.IdSeccion, mes, anio);

            tabla.AddCell(mes.ToString());
            tabla.AddCell(desempeno.ToString());
        }

        private void AgregarTrabajadoresMesGqm(int mes, PdfPTable tabla)
        {
            AdminDesempeño ad = new AdminDesempeño();
            int reubicaciones = ad.ObtenerReubicacionesMes(seccion.IdSeccion, mes, anio);
            int total = ad.ObtenerTotalEmpleadosMes(seccion.IdSeccion, mes, anio, seccion.Tipo);
            int capacitados = ad.ObtenerEmpleadosCapacitadosMes(seccion.IdSeccion, mes, anio, seccion.Tipo);
            int noCapacitados = ad.ObtenerEmpleadosNoCapacitadosMes(seccion.IdSeccion, mes, anio, seccion.Tipo);
            double porcentajeCapacitados = (capacitados / total) * 100;

            tabla.AddCell(mes.ToString());
            tabla.AddCell(total.ToString());
            tabla.AddCell(noCapacitados.ToString());
            tabla.AddCell(capacitados.ToString());
            //tabla.AddCell(porcentajeCapacitados.ToString());
        }

        public string RutaFichero
        {
            get { return rutaFichero; }
            set { rutaFichero = value; }
        }

        public MemoryStream Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public Seccion Seccion
        {
            get { return seccion; }
            set { seccion = value; }
        }
    }
}
