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
    class ReportesTrabajador
    {
        private string rutaFichero;
        private Document reporte;
        private Fuentes fuente;
        private iTextSharp.text.Image imagenGrafico;
        private iTextSharp.text.Image imagenGraficoHB;
        private iTextSharp.text.Image imagenGraficoHD;
        private iTextSharp.text.Image imagenGraficoCF;
        private MemoryStream buffer;
        private Trabajador trabajador;
        public ReportesTrabajador()
        {
            IniciarComponentes();
        }

        private void IniciarComponentes()
        {
            this.rutaFichero = string.Empty;
            fuente = new Fuentes();          
        }

        public void GenerarReporte()
        {
            this.reporte = CrearPDF();
            reporte.Open();
            FormatoPortada();
            InformacionTrabajador();
            PuntajesGenerales();
            GraficoGeneral();
            HabilidadesBlandas();
            //GraficoHB();
            HabilidadesDuras();
            //GraficoHD();
            CaracteristicasFisicas();
            //GraficoCF();
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

        public void InformacionTrabajador()
        {
            //reporte.Open();
            reporte.NewPage();

            Paragraph titulo = new Paragraph("IDENTIFICACIÓN DEL TRABAJADOR", fuente.Subrayado(20));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            titulo.Alignment = Element.ALIGN_CENTER;

            PdfPTable tablaInformacionTrabajador = new PdfPTable(2);
            tablaInformacionTrabajador.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell celdaTitulo = new PdfPCell(new Paragraph("INFORMACION GENERAL", fuente.Cursiva(12)));
            celdaTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaTitulo.Colspan = 2;
            celdaTitulo.BackgroundColor = new BaseColor(171, 196, 237);
            /*Etiquetas*/
            PdfPCell nombreTrabajadorEtiqueta = new PdfPCell(new Paragraph("Nombre ", fuente.Cursiva(12)));
            PdfPCell apellidoPaternoEtiqueta = new PdfPCell(new Paragraph("Apellido Paterno ", fuente.Cursiva(12)));
            PdfPCell apellidoMaternoEtiqueta = new PdfPCell(new Paragraph("Apellido Materno ", fuente.Cursiva(12)));
            PdfPCell fechaNacimientoEtiqueta = new PdfPCell(new Paragraph("Fecha de Nacimiento ", fuente.Cursiva(12)));
            PdfPCell edadEtiqueta = new PdfPCell(new Paragraph("Edad ", fuente.Cursiva(12)));
            PdfPCell sexoEtiqueta = new PdfPCell(new Paragraph("Sexo ", fuente.Cursiva(12)));
            PdfPCell seccionEtiqueta = new PdfPCell(new Paragraph("Sección ", fuente.Cursiva(12)));
            PdfPCell capacidadSeccionEtiqueta = new PdfPCell(new Paragraph("Capacidad en la sección ", fuente.Cursiva(12)));
            /*contenido*/
            PdfPCell nombreTrabajador = new PdfPCell(new Paragraph(trabajador.Nombre, fuente.Normal(11)));//dato de prueba
            PdfPCell apellidoPaterno = new PdfPCell(new Paragraph(trabajador.ApellidoPaterno, fuente.Normal(11)));//dato de prueba
            PdfPCell apellidoMaterno = new PdfPCell(new Paragraph(trabajador.ApellidoMaterno, fuente.Normal(11)));//dato de prueba
            PdfPCell fechaNacimiento = new PdfPCell(new Paragraph(trabajador.FechaNacimiento, fuente.Normal(11)));//dato de prueba
            PdfPCell edad = new PdfPCell(new Paragraph("28", fuente.Normal(11)));//dato de prueba
            PdfPCell sexo = new PdfPCell(new Paragraph(trabajador.Sexo, fuente.Normal(11)));//dato de prueba
            PdfPCell seccion = new PdfPCell(new Paragraph("Atención al Cliente", fuente.Normal(11)));//dato de prueba
            PdfPCell capacidad = new PdfPCell(new Paragraph("XX", fuente.Normal(11)));

            /*alineaciones etiquetas*/
            nombreTrabajadorEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            apellidoPaternoEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            apellidoMaternoEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            fechaNacimientoEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            edadEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            sexoEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            seccionEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            capacidadSeccionEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            /*alineaciones contenido*/
            nombreTrabajador.HorizontalAlignment = Element.ALIGN_CENTER;
            apellidoPaterno.HorizontalAlignment = Element.ALIGN_CENTER;
            apellidoMaterno.HorizontalAlignment = Element.ALIGN_CENTER;
            fechaNacimiento.HorizontalAlignment = Element.ALIGN_CENTER;
            edad.HorizontalAlignment = Element.ALIGN_CENTER;
            sexo.HorizontalAlignment = Element.ALIGN_CENTER;
            seccion.HorizontalAlignment = Element.ALIGN_CENTER;
            capacidad.HorizontalAlignment = Element.ALIGN_CENTER;
            /*se agregan las celdas a la tabla*/
            tablaInformacionTrabajador.AddCell(celdaTitulo);
            /*etiquetas*/
            tablaInformacionTrabajador.AddCell(nombreTrabajadorEtiqueta);
            tablaInformacionTrabajador.AddCell(nombreTrabajador);
            tablaInformacionTrabajador.AddCell(apellidoPaternoEtiqueta);
            tablaInformacionTrabajador.AddCell(apellidoPaterno);
            tablaInformacionTrabajador.AddCell(apellidoMaternoEtiqueta);
            tablaInformacionTrabajador.AddCell(apellidoMaterno);
            tablaInformacionTrabajador.AddCell(fechaNacimientoEtiqueta);
            tablaInformacionTrabajador.AddCell(fechaNacimiento);
            tablaInformacionTrabajador.AddCell(edadEtiqueta);
            tablaInformacionTrabajador.AddCell(edad);
            tablaInformacionTrabajador.AddCell(sexoEtiqueta);
            tablaInformacionTrabajador.AddCell(sexo);
            tablaInformacionTrabajador.AddCell(seccionEtiqueta);
            tablaInformacionTrabajador.AddCell(seccion);
            tablaInformacionTrabajador.AddCell(capacidadSeccionEtiqueta);
            tablaInformacionTrabajador.AddCell(capacidad);
            /*se agrega los elementos al reporte*/
            reporte.Add(titulo);
            reporte.Add(espacio);
            reporte.Add(tablaInformacionTrabajador);
            //reporte.Close();
        }

        private void PuntajesGenerales()
        {
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            PdfPTable tablaPuntajeTrabajador = new PdfPTable(2);
            tablaPuntajeTrabajador.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
            PdfPCell celdaTitulo = new PdfPCell(new Paragraph("PUNTAJES GENERALES", fuente.Cursiva(12)));
            celdaTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
            celdaTitulo.Colspan = 2;
            celdaTitulo.BackgroundColor = new BaseColor(171, 196, 237);
            /*Etiquetas*/
            PdfPCell habilidadEtiqueta = new PdfPCell(new Paragraph("HABILIDAD", fuente.Cursiva(12)));
            PdfPCell capacidadEtiqueta = new PdfPCell(new Paragraph("CAPACIDAD", fuente.Cursiva(12)));
            PdfPCell hbEtiqueta = new PdfPCell(new Paragraph("Habilidades Blandas", fuente.Cursiva(12)));
            PdfPCell hdEtiqueta = new PdfPCell(new Paragraph("Habilidades Duras", fuente.Cursiva(12)));
            PdfPCell cfEtiqueta = new PdfPCell(new Paragraph("Caracteristicas Fisicas", fuente.Cursiva(12)));
            PdfPCell generalEtiqueta = new PdfPCell(new Paragraph("GENERAL ", fuente.Negrita(12)));
            /*colores*/
            habilidadEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
            capacidadEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
            /*Contenidos*/
            PdfPCell habilidadesBlandas = new PdfPCell(new Paragraph(""+trabajador.Perfil.HB.Puntaje, fuente.Normal(11)));//dato de prueba
            PdfPCell habilidadesDuras = new PdfPCell(new Paragraph(""+trabajador.Perfil.HD.Puntaje, fuente.Normal(11)));//dato de prueba
            PdfPCell caracteristicasFisicas = new PdfPCell(new Paragraph("" + trabajador.Perfil.CF.Puntaje, fuente.Normal(11)));//dato de prueba
            PdfPCell general = new PdfPCell(new Paragraph("55.00%", fuente.Negrita(12)));//dato de prueba
            /*Alineaciones*/
            habilidadEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            capacidadEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            generalEtiqueta.HorizontalAlignment = Element.ALIGN_RIGHT;
            hbEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            hdEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            cfEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            habilidadesBlandas.HorizontalAlignment = Element.ALIGN_CENTER;
            habilidadesDuras.HorizontalAlignment = Element.ALIGN_CENTER;
            caracteristicasFisicas.HorizontalAlignment = Element.ALIGN_CENTER;
            general.HorizontalAlignment = Element.ALIGN_CENTER;

            /*se agregan los elementos a la tabla*/
            tablaPuntajeTrabajador.AddCell(celdaTitulo);
            tablaPuntajeTrabajador.AddCell(habilidadEtiqueta);
            tablaPuntajeTrabajador.AddCell(capacidadEtiqueta);
            tablaPuntajeTrabajador.AddCell(hbEtiqueta);
            tablaPuntajeTrabajador.AddCell(habilidadesBlandas);
            tablaPuntajeTrabajador.AddCell(hdEtiqueta);
            tablaPuntajeTrabajador.AddCell(habilidadesDuras);
            tablaPuntajeTrabajador.AddCell(cfEtiqueta);
            tablaPuntajeTrabajador.AddCell(caracteristicasFisicas);
            tablaPuntajeTrabajador.AddCell(generalEtiqueta);
            tablaPuntajeTrabajador.AddCell(general);
            /*se agregan los elementos al reporte*/
            reporte.Add(espacio);
            reporte.Add(tablaPuntajeTrabajador);
        }

        public void GraficoGeneral()
        {
            string contenido = "Calificación de las habilidades del trabajador a nivel general.  Se grafican "+
                            "los puntajes obtenidos en Habilidades blandas(HB), Habilidades duras (HD), y "+
                            "Caracteristicas fisicas(CF).";

            Chunk subtitulo = new Chunk("DESCRIPCION: ", fuente.Negrita(11));
            Chunk texto = new Chunk(contenido, fuente.Normal(11));
            Phrase descripcionCompleta = new Phrase();
            
            descripcionCompleta.Add(subtitulo);
            descripcionCompleta.Add(texto);

            Paragraph parrafoDescripcion = new Paragraph();
            parrafoDescripcion.Add(descripcionCompleta);
            parrafoDescripcion.Alignment = Element.ALIGN_JUSTIFIED;

            parrafoDescripcion.IndentationLeft = 60f;
            parrafoDescripcion.IndentationRight = 60f;

            //Console.WriteLine("AAA: "+buffer.GetBuffer());
            Paragraph titulo = new Paragraph("GRAFICO GENERAL", fuente.Subrayado(18));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            Paragraph unEspacio = new Paragraph("\n", fuente.Normal(11));
            Paragraph habTrabajador = new Paragraph("Hab. Trabajador: Naranjo", fuente.Normal(10));
            Paragraph habSeccion= new Paragraph("Hab. Sección: Azul", fuente.Normal(10));
            titulo.Alignment = Element.ALIGN_CENTER;
            habTrabajador.Alignment = Element.ALIGN_RIGHT;
            habSeccion.Alignment = Element.ALIGN_RIGHT;
            habTrabajador.IndentationRight = 60f;
            habSeccion.IndentationRight = 60f;            
            
            imagenGrafico.ScalePercent(90f);
            imagenGrafico.Alignment = Element.ALIGN_CENTER;
            /*se agregan los elementos al documento*/
            reporte.Add(espacio);
            reporte.Add(titulo);
            reporte.Add(unEspacio);
            reporte.Add(parrafoDescripcion);
            reporte.Add(imagenGrafico);
            reporte.Add(habSeccion);
            reporte.Add(habTrabajador);
        }

        public void GraficoHB()
        {
            string contenido = "Calificación de las habilidades del trabajador a nivel general.  Se grafican " +
                            "los puntajes obtenidos en Habilidades blandas(HB), Habilidades duras (HD), y " +
                            "Caracteristicas fisicas(CF).";

            Chunk subtitulo = new Chunk("DESCRIPCION: ", fuente.Negrita(11));
            Chunk texto = new Chunk(contenido, fuente.Normal(11));
            Phrase descripcionCompleta = new Phrase();

            descripcionCompleta.Add(subtitulo);
            descripcionCompleta.Add(texto);

            Paragraph parrafoDescripcion = new Paragraph();
            parrafoDescripcion.Add(descripcionCompleta);
            parrafoDescripcion.Alignment = Element.ALIGN_JUSTIFIED;

            parrafoDescripcion.IndentationLeft = 60f;
            parrafoDescripcion.IndentationRight = 60f;

            //Console.WriteLine("AAA: "+buffer.GetBuffer());
            Paragraph titulo = new Paragraph("GRAFICO HABILIDADES BLANDAS", fuente.Subrayado(18));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            Paragraph unEspacio = new Paragraph("\n", fuente.Normal(11));
            Paragraph habTrabajador = new Paragraph("Hab. Trabajador: Naranjo", fuente.Normal(10));
            Paragraph habSeccion = new Paragraph("Hab. Sección: Azul", fuente.Normal(10));
            titulo.Alignment = Element.ALIGN_CENTER;
            habTrabajador.Alignment = Element.ALIGN_RIGHT;
            habSeccion.Alignment = Element.ALIGN_RIGHT;
            habTrabajador.IndentationRight = 60f;
            habSeccion.IndentationRight = 60f;

            imagenGraficoHB.ScalePercent(90f);
            imagenGraficoHB.Alignment = Element.ALIGN_CENTER;
            /*se agregan los elementos al documento*/
            reporte.Add(espacio);
            reporte.Add(titulo);
            reporte.Add(unEspacio);
            reporte.Add(parrafoDescripcion);
            reporte.Add(imagenGraficoHB);
            reporte.Add(habSeccion);
            reporte.Add(habTrabajador);
        }

        public void GraficoHD()
        {
            string contenido = "Calificación de las habilidades del trabajador a nivel general.  Se grafican " +
                            "los puntajes obtenidos en Habilidades blandas(HB), Habilidades duras (HD), y " +
                            "Caracteristicas fisicas(CF).";

            Chunk subtitulo = new Chunk("DESCRIPCION: ", fuente.Negrita(11));
            Chunk texto = new Chunk(contenido, fuente.Normal(11));
            Phrase descripcionCompleta = new Phrase();

            descripcionCompleta.Add(subtitulo);
            descripcionCompleta.Add(texto);

            Paragraph parrafoDescripcion = new Paragraph();
            parrafoDescripcion.Add(descripcionCompleta);
            parrafoDescripcion.Alignment = Element.ALIGN_JUSTIFIED;

            parrafoDescripcion.IndentationLeft = 60f;
            parrafoDescripcion.IndentationRight = 60f;

            //Console.WriteLine("AAA: "+buffer.GetBuffer());
            Paragraph titulo = new Paragraph("GRAFICO HABILIDADES DURAS", fuente.Subrayado(18));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            Paragraph unEspacio = new Paragraph("\n", fuente.Normal(11));
            Paragraph habTrabajador = new Paragraph("Hab. Trabajador: Naranjo", fuente.Normal(10));
            Paragraph habSeccion = new Paragraph("Hab. Sección: Azul", fuente.Normal(10));
            titulo.Alignment = Element.ALIGN_CENTER;
            habTrabajador.Alignment = Element.ALIGN_RIGHT;
            habSeccion.Alignment = Element.ALIGN_RIGHT;
            habTrabajador.IndentationRight = 60f;
            habSeccion.IndentationRight = 60f;

            imagenGraficoHD.ScalePercent(90f);
            imagenGraficoHD.Alignment = Element.ALIGN_CENTER;
            /*se agregan los elementos al documento*/
            reporte.Add(espacio);
            reporte.Add(titulo);
            reporte.Add(unEspacio);
            reporte.Add(parrafoDescripcion);
            reporte.Add(imagenGraficoHD);
            reporte.Add(habSeccion);
            reporte.Add(habTrabajador);
        }

        public void GraficoCF()
        {
            string contenido = "Calificación de las habilidades del trabajador a nivel general.  Se grafican " +
                            "los puntajes obtenidos en Habilidades blandas(HB), Habilidades duras (HD), y " +
                            "Caracteristicas fisicas(CF).";

            Chunk subtitulo = new Chunk("DESCRIPCION: ", fuente.Negrita(11));
            Chunk texto = new Chunk(contenido, fuente.Normal(11));
            Phrase descripcionCompleta = new Phrase();

            descripcionCompleta.Add(subtitulo);
            descripcionCompleta.Add(texto);

            Paragraph parrafoDescripcion = new Paragraph();
            parrafoDescripcion.Add(descripcionCompleta);
            parrafoDescripcion.Alignment = Element.ALIGN_JUSTIFIED;

            parrafoDescripcion.IndentationLeft = 60f;
            parrafoDescripcion.IndentationRight = 60f;

            //Console.WriteLine("AAA: "+buffer.GetBuffer());
            Paragraph titulo = new Paragraph("GRAFICO CARACTERISTICAS FISICAS", fuente.Subrayado(18));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            Paragraph unEspacio = new Paragraph("\n", fuente.Normal(11));
            Paragraph habTrabajador = new Paragraph("Hab. Trabajador: Naranjo", fuente.Normal(10));
            Paragraph habSeccion = new Paragraph("Hab. Sección: Azul", fuente.Normal(10));
            titulo.Alignment = Element.ALIGN_CENTER;
            habTrabajador.Alignment = Element.ALIGN_RIGHT;
            habSeccion.Alignment = Element.ALIGN_RIGHT;
            habTrabajador.IndentationRight = 60f;
            habSeccion.IndentationRight = 60f;

            imagenGraficoCF.ScalePercent(90f);
            imagenGraficoCF.Alignment = Element.ALIGN_CENTER;
            /*se agregan los elementos al documento*/
            reporte.Add(espacio);
            reporte.Add(titulo);
            reporte.Add(unEspacio);
            reporte.Add(parrafoDescripcion);
            reporte.Add(imagenGraficoCF);
            reporte.Add(habSeccion);
            reporte.Add(habTrabajador);
        }

        private void HabilidadesBlandas()
        {
            reporte.NewPage();
            Paragraph titulo = new Paragraph("EVALUACION DE HABILIDADES BLANDAS", fuente.Subrayado(20));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            titulo.Alignment = Element.ALIGN_CENTER;

            PdfPTable tablaHabilidadesTrabajador = new PdfPTable(2);
            tablaHabilidadesTrabajador.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell habilidadEtiqueta = new PdfPCell(new Paragraph("HABILIDAD", fuente.Cursiva(12)));
            PdfPCell capacidadEtiqueta = new PdfPCell(new Paragraph("CAPACIDAD", fuente.Cursiva(12)));
            habilidadEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            capacidadEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            /*colores*/
            habilidadEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
            capacidadEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);    

            /*Agrega los elementos al reporte*/
            reporte.Add(titulo);
            reporte.Add(espacio);

            tablaHabilidadesTrabajador.AddCell(habilidadEtiqueta);
            tablaHabilidadesTrabajador.AddCell(capacidadEtiqueta);
            
            /*agrega elementos*/
            /*for (int i = 0; i < 10; i++ )
            {
                PdfPCell habilidad = new PdfPCell(new Paragraph("ALGUNA HABILIDAD BLANDA", fuente.Normal(11)));
                PdfPCell capacidad = new PdfPCell(new Paragraph("99.00%", fuente.Normal(11)));
                capacidad.HorizontalAlignment = Element.ALIGN_CENTER;
                habilidad.HorizontalAlignment = Element.ALIGN_CENTER;
                tablaHabilidadesTrabajador.AddCell(habilidad);
                tablaHabilidadesTrabajador.AddCell(capacidad);
            }*/

            foreach (KeyValuePair<string, Componente> habilidadBlanda in this.trabajador.Perfil.Blandas)
            {
                PdfPCell habilidad = new PdfPCell(new Paragraph(habilidadBlanda.Value.Nombre, fuente.Normal(11)));
                PdfPCell capacidad = new PdfPCell(new Paragraph(habilidadBlanda.Value.Puntaje+"%", fuente.Normal(11)));
                capacidad.HorizontalAlignment = Element.ALIGN_CENTER;
                habilidad.HorizontalAlignment = Element.ALIGN_CENTER;
                tablaHabilidadesTrabajador.AddCell(habilidad);
                tablaHabilidadesTrabajador.AddCell(capacidad);
            }
            reporte.Add(tablaHabilidadesTrabajador);
        }

        private void HabilidadesDuras()
        {
            reporte.NewPage();
            Paragraph titulo = new Paragraph("EVALUACION DE HABILIDADES DURAS", fuente.Subrayado(20));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            titulo.Alignment = Element.ALIGN_CENTER;

            PdfPTable tablaHabilidadesTrabajador = new PdfPTable(2);
            tablaHabilidadesTrabajador.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell habilidadEtiqueta = new PdfPCell(new Paragraph("HABILIDAD", fuente.Cursiva(12)));
            PdfPCell capacidadEtiqueta = new PdfPCell(new Paragraph("CAPACIDAD", fuente.Cursiva(12)));
            habilidadEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            capacidadEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            /*colores*/
            habilidadEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
            capacidadEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);


            /*Agrega los elementos al reporte*/
            reporte.Add(titulo);
            reporte.Add(espacio);

            tablaHabilidadesTrabajador.AddCell(habilidadEtiqueta);
            tablaHabilidadesTrabajador.AddCell(capacidadEtiqueta);

            /*agrega elementos*/            
            foreach (KeyValuePair<string, Componente> habilidadDura in this.trabajador.Perfil.Duras)
            {
                PdfPCell habilidad = new PdfPCell(new Paragraph(habilidadDura.Value.Nombre, fuente.Normal(11)));
                PdfPCell capacidad = new PdfPCell(new Paragraph(habilidadDura.Value.Puntaje+"%", fuente.Normal(11)));
                capacidad.HorizontalAlignment = Element.ALIGN_CENTER;
                habilidad.HorizontalAlignment = Element.ALIGN_CENTER;
                tablaHabilidadesTrabajador.AddCell(habilidad);
                tablaHabilidadesTrabajador.AddCell(capacidad);
            }

            reporte.Add(tablaHabilidadesTrabajador);
        }

        private void CaracteristicasFisicas()
        {
            reporte.NewPage();
            Paragraph titulo = new Paragraph("EVALUACION DE CARACTERISTICAS FISICAS", fuente.Subrayado(20));
            Paragraph espacio = new Paragraph("\n\n", fuente.Normal(11));
            titulo.Alignment = Element.ALIGN_CENTER;

            PdfPTable tablaHabilidadesTrabajador = new PdfPTable(2);
            tablaHabilidadesTrabajador.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell habilidadEtiqueta = new PdfPCell(new Paragraph("CARACTERISTICA", fuente.Cursiva(12)));
            PdfPCell capacidadEtiqueta = new PdfPCell(new Paragraph("CAPACIDAD", fuente.Cursiva(12)));
            habilidadEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            capacidadEtiqueta.HorizontalAlignment = Element.ALIGN_CENTER;
            /*colores*/
            habilidadEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);
            capacidadEtiqueta.BackgroundColor = new BaseColor(171, 196, 237);

            /*Agrega los elementos al reporte*/
            reporte.Add(titulo);
            reporte.Add(espacio);

            tablaHabilidadesTrabajador.AddCell(habilidadEtiqueta);
            tablaHabilidadesTrabajador.AddCell(capacidadEtiqueta);

            /*agrega elementos*/
            foreach (KeyValuePair<string, Componente> habilidadFisica in this.trabajador.Perfil.Fisicas)
            {
                PdfPCell habilidad = new PdfPCell(new Paragraph(habilidadFisica.Value.Nombre, fuente.Normal(11)));
                PdfPCell capacidad = new PdfPCell(new Paragraph(habilidadFisica.Value.Puntaje + "%", fuente.Normal(11)));
                capacidad.HorizontalAlignment = Element.ALIGN_CENTER;
                habilidad.HorizontalAlignment = Element.ALIGN_CENTER;
                tablaHabilidadesTrabajador.AddCell(habilidad);
                tablaHabilidadesTrabajador.AddCell(capacidad);
            }
            reporte.Add(tablaHabilidadesTrabajador);
        }
        public string RutaFichero
        {
            get { return rutaFichero; }
            set { rutaFichero = value; }
        }

        public iTextSharp.text.Image ImagenGrafico
        {
            get { return imagenGrafico; }
            set { imagenGrafico = value;}
        }

        public iTextSharp.text.Image ImagenGraficoHB
        {
            get { return imagenGraficoHB; }
            set { imagenGraficoHB = value; }
        }

        public iTextSharp.text.Image ImagenGraficoHD
        {
            get { return imagenGraficoHD; }
            set { imagenGraficoHD = value; }
        }

          public iTextSharp.text.Image ImagenGraficoCF
        {
            get { return imagenGraficoCF; }
            set { imagenGraficoCF = value; }
        }
        public MemoryStream Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }

        public Trabajador Trabajador
        {
            get { return trabajador; }
            set { trabajador = value; }
        }
    }

}
