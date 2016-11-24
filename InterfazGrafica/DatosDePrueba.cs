using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DST;

namespace InterfazGrafica
{
    public class DatosDePrueba
    {
        private List<string> nombres;
        private List<string> apellido;
        private List<int>    edad;
        private List<string> sexo;
        private List<string> preguntas;
        private List<Trabajador> trabajador;
        private Componente  componente;
        private Perfil      perfil;
        private string[] habilidadB = { "Proactividad", "Respeto", "Motivacion", "Tolerancia a la frustracion", "Honestidad", "Integridad" };
        private string[] habilidadD = { "C. Atencion Clientes", "C. Normas Higiene", "C. Sist. informatico de la Empresa", "C. Normativa Empresa", "C. Documentos Mercantiles" };
        private string[] caracteristicaF = { "Altura", "Presentacion Personal", "Discapacidad Fisica" };
        
        private List<double> puntajesHB;
        private List<double> puntajesHD;
        private List<double> puntajesCF;
        private List<double> puntajesGenerales;

        public DatosDePrueba()
        {
            nombres     = new List<string>();
            apellido    = new List<string>();
            edad        = new List<int>();
            sexo        = new List<string>();
            preguntas   = new List<string>();
            trabajador  = new List<Trabajador>();            
            puntajesHB  = new List<double>();
            puntajesHD  = new List<double>();
            puntajesCF  = new List<double>();
            puntajesGenerales = new List<double>();
            componente  = new Componente("","","");
            perfil      = new Perfil();
            datosIniciales();
        }

        private void datosIniciales()
        {
            Random random = new Random(DateTime.Now.Millisecond);
            string[] sexo = { "M", "F", "" };
            for (int i = 1; i < 30; i++)
            {
                int numero = random.Next(0, 2);
                nombres.Add("Nombre " + i);
                apellido.Add("Apellido " + i);
                preguntas.Add("¿Algun tipo de pregunta relacionada con la habilidad establecida? " + i);
                edad.Add(random.Next());
                this.sexo.Add(sexo[numero]);      
                /*lista original*/
                componente.Nombre = "nombre componente"+i;
                componente.Puntaje = Convert.ToDouble(i);
                componente.Descripcion = "alguna descripcion";
                componente.Tipo = "un tipo" +i;
                Trabajador unTrabajador = new Trabajador("12.111.222-"+i,"trabajador "+i,"09/11/1990",perfil);
                trabajador.Add(unTrabajador);     
            }            
            
            int puntaje1 = random.Next(0,200);
            for (int i = 0; i < 6; i++ )
                puntajesHB.Add(puntaje1);

            int puntaje2 = random.Next(0, 200);
            for (int i = 0; i < 5; i++)
                puntajesHD.Add(puntaje2);

            int puntaje3 = random.Next(0, 200);
            for (int i = 0; i < 3; i++)
                puntajesCF.Add(puntaje3);

            int puntaje4 = random.Next(0, 100);
            for (int i = 0; i < 100; i++)
                puntajesGenerales.Add(puntaje4);
        }

        public List<string> Nombres
        {
            get { return nombres; }
            set { }
        }

        public List<string> Apellido
        {
            get { return apellido; }
            set { }
        }

        public List<int> Edad
        {
            get { return edad; }
            set { }
        }

        public List<string> Sexo
        {
            get { return sexo; }
            set { }
        }

        public List<string> Preguntas
        {
            get { return preguntas; }
            set { }
        }

        public List<Trabajador> Trabajadores
        {
            get { return trabajador; }
            set { }
        }

        public string[] HabilidadesDuras
        {
            get { return habilidadD; }
            set { }
        }

        public string[] HabilidadesBlandas
        {
            get { return habilidadB; }
            set { }
        }

        public string[] CaracteristicasFisicas
        {
            get { return caracteristicaF; }
            set { }
        }
        public List<double> PuntajesHB
        {
            get { return puntajesHB; }
        }
        public List<double> PuntajesHD
        {
            get { return puntajesHD; }
        }
        public List<double> PuntajesCF
        {
            get { return puntajesCF; }
        }
        public List<double> PuntajesGenerales
        {
            get { return puntajesGenerales; }
        } 
    }
}
